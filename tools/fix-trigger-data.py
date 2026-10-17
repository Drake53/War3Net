#!/usr/bin/env python3
import argparse
import difflib
import sys
from dataclasses import dataclass
from pathlib import Path

DEFAULT_PATH = Path(__file__).resolve().parent.parent / "src" / "War3Net.Build.Core" / "Resources" / "TriggerData.txt"

FUNCTION_SECTION_FIELDS = {
    "TriggerEvents": ["DisplayName", "Parameters", "Defaults", "Limits", "Category"],
    "TriggerConditions": ["DisplayName", "Parameters", "Defaults", "Category", "UseWithAI", "AIDefaults"],
    "TriggerActions": ["DisplayName", "Parameters", "Defaults", "Limits", "Category", "ScriptName"],
    "TriggerCalls": ["DisplayName", "Parameters", "Defaults", "Limits", "Category", "UseWithAI"],
}

NOTHING_TYPE = "nothing"
EDITOR_ARGUMENT_TYPES = ("Null",)
FLAG_VALUES = ("0", "1")
NO_VALUE = "_"


@dataclass(frozen=True)
class LinePatch:
    original: str
    replacement: str


LINE_PATCHES = [
    LinePatch("_BlzSetAbilityRealLevelArrayFieldBJ_Limits=_,_,0,_,0,_,_,_", "_BlzSetAbilityRealLevelArrayFieldBJ_Limits=_,_,_,_,0,_,0,_,_,_"),
    LinePatch("_BlzSetAbilityStringLevelArrayFieldBJ_Limits=_,_,0,_,0,_,_,_", "_BlzSetAbilityStringLevelArrayFieldBJ_Limits=_,_,_,_,0,_,0,_,_,_"),
    LinePatch("_BlzSetAbilityRealLevelArrayField_Limits=_,_,0,_,0,_,_,_", "_BlzSetAbilityRealLevelArrayField_Limits=_,_,_,_,0,_,0,_,_,_"),
    LinePatch("_BlzSetAbilityStringLevelArrayField_Limits=_,_,0,_,0,_,_,_", "_BlzSetAbilityStringLevelArrayField_Limits=_,_,_,_,0,_,0,_,_,_"),
    LinePatch("_BlzGetUnitAbility_Defaults=_,GetTriggerUnit,_", "_BlzGetUnitAbility_Defaults=GetTriggerUnit,_"),
    LinePatch("_SetSpecialEffectAnimationBlendTime_Limits=_,_,_,0,_", "_BlzSetSpecialEffectAnimationBlendTime_Limits=_,_,0,_"),
]

MISSING_FIELD_VALUES = {
    ("TriggerEvents", "TriggerRegisterCommonCommandEventBJ"): {
        "DisplayName": '"Common Button Pressed"',
        "Parameters": '"Button for common order ",~Order," pressed."',
    },
    ("TriggerActions", "CreateCommonCommandButtonEffectBJ"): {
        "DisplayName": '"Create Common Button Effect"',
        "Parameters": '"Create Common Button Effect for ",~Order',
    },
}


@dataclass
class FunctionBlock:
    section: str
    header_index: int
    field_indices: dict


class TriggerDataFixer:
    def __init__(self, lines):
        self.lines = lines
        self.fixes = []
        self.warnings = []

    def record(self, key, message):
        self.fixes.append(f"{key}: {message}")

    def run(self):
        self.apply_line_patches()
        self.fix_lines()
        self.remove_duplicate_keys()
        self.fix_function_blocks()
        return self.lines

    def apply_line_patches(self):
        for patch in LINE_PATCHES:
            matching_indices = [index for index, line in enumerate(self.lines) if line == patch.original]
            if not matching_indices:
                if patch.replacement not in self.lines:
                    self.warnings.append(f"Line patch no longer applies: {patch.original}")
                continue
            for index in matching_indices:
                self.lines[index] = patch.replacement
            self.record(patch.original.split("=", 1)[0], f"patched to '{patch.replacement.split('=', 1)[1]}'")

    def iterate_sections(self):
        current_section = None
        for index, line in enumerate(self.lines):
            stripped_line = line.strip()
            if stripped_line.startswith("[") and stripped_line.endswith("]"):
                current_section = stripped_line[1:-1]
                continue
            yield index, current_section, line

    def section_keys(self, section_name):
        keys = set()
        for _, section, line in self.iterate_sections():
            if section == section_name and is_entry(line) and "=" in line:
                keys.add(line.split("=", 1)[0].strip())
        return keys

    def function_names_by_section(self):
        return {section_name: {key for key in self.section_keys(section_name) if not key.startswith("_")} for section_name in FUNCTION_SECTION_FIELDS}

    def fix_lines(self):
        category_identifiers = self.section_keys("TriggerCategories")
        function_names_by_section = self.function_names_by_section()
        current_function_name = None
        for index, section, line in list(self.iterate_sections()):
            if is_comment(line):
                continue
            if line.strip() == "":
                self.lines[index] = ""
                continue
            if line != line.rstrip():
                line = line.rstrip()
                self.lines[index] = line
                self.record(line.split("=", 1)[0], "removed trailing whitespace")
            if section not in FUNCTION_SECTION_FIELDS:
                continue
            if not line.startswith("_"):
                current_function_name = line.split("=", 1)[0]
                continue
            if current_function_name is None:
                continue
            fixed_line = self.fix_field_line(line, section, current_function_name, category_identifiers, function_names_by_section[section])
            if fixed_line != line:
                self.lines[index] = fixed_line

    def fix_field_line(self, line, section, function_name, category_identifiers, function_names):
        allowed_fields = FUNCTION_SECTION_FIELDS[section]
        if "=" not in line:
            line = insert_missing_equals_sign(line, function_name, allowed_fields)
            if "=" not in line:
                return line
            self.record(line.split("=", 1)[0], "inserted missing '='")

        key, value = line.split("=", 1)
        owner_name, field_name = split_field_key(key)

        if field_name is None and value in category_identifiers:
            field_name = "Category"
            self.record(key, "added missing field name 'Category'")
        if field_name is None:
            return line

        canonical_field_name = canonicalize_field_name(field_name, allowed_fields)
        if canonical_field_name is not None and canonical_field_name != field_name:
            self.record(key, f"renamed field '{field_name}' to '{canonical_field_name}'")
            field_name = canonical_field_name
        if field_name not in allowed_fields:
            return line

        if owner_name != function_name and (owner_name.lower() == function_name.lower() or owner_name not in function_names):
            self.record(key, f"reassigned to preceding function '{function_name}'")
            owner_name = function_name

        return f"_{owner_name}_{field_name}={value}"

    def remove_duplicate_keys(self):
        first_index_by_section_key = {}
        removed_indices = set()
        for index, section, line in self.iterate_sections():
            if section in FUNCTION_SECTION_FIELDS or not is_entry(line) or "=" not in line:
                continue
            key, value = line.split("=", 1)
            section_key = (section, key)
            if section_key not in first_index_by_section_key:
                first_index_by_section_key[section_key] = index
                continue
            first_value = self.lines[first_index_by_section_key[section_key]].split("=", 1)[1]
            identical_hint = "identical " if first_value == value else ""
            self.record(key, f"removed {identical_hint}duplicate in [{section}], kept the first declaration '{first_value}'")
            removed_indices.add(index)
        self.lines = [line for index, line in enumerate(self.lines) if index not in removed_indices]

    def function_blocks(self):
        blocks = []
        current_block = None
        for index, section, line in self.iterate_sections():
            if section not in FUNCTION_SECTION_FIELDS:
                current_block = None
                continue
            if not is_entry(line) or "=" not in line:
                continue
            key = line.split("=", 1)[0]
            if not key.startswith("_"):
                current_block = FunctionBlock(section, index, {})
                blocks.append(current_block)
                continue
            if current_block is None:
                continue
            _, field_name = split_field_key(key)
            if field_name is not None:
                current_block.field_indices.setdefault(field_name, []).append(index)
        return blocks

    def fix_function_blocks(self):
        type_names = self.section_keys("TriggerTypes")
        removed_indices = set()
        insertions_after_index = {}
        used_missing_field_values = set()

        for block in self.function_blocks():
            function_name, argument_count = self.fix_function_header(block, type_names)

            for field_name, indices in block.field_indices.items():
                if len(indices) > 1:
                    kept_index = self.select_duplicate_field(field_name, indices, argument_count)
                    removed_indices.update(index for index in indices if index != kept_index)
                    block.field_indices[field_name] = [kept_index]
                    self.record(f"_{function_name}_{field_name}", f"removed duplicate field, kept '{self.field_value(kept_index)}'")

            self.fix_field(block, "DisplayName", function_name, fix_display_name)
            self.fix_field(block, "Parameters", function_name, repair_parameters)
            for field_name in ("Defaults", "AIDefaults"):
                self.fix_field(block, field_name, function_name, lambda value: fit_values(value, argument_count))
            self.fix_field(block, "Limits", function_name, lambda value: fit_values(value, argument_count * 2))

            missing_lines = []
            missing_field_values = MISSING_FIELD_VALUES.get((block.section, function_name), {})
            for field_name in ("DisplayName", "Parameters"):
                if field_name in block.field_indices:
                    if missing_field_values.get(field_name) == self.field_value(block.field_indices[field_name][0]):
                        used_missing_field_values.add((block.section, function_name, field_name))
                    continue
                if field_name in missing_field_values:
                    missing_lines.append(f"_{function_name}_{field_name}={missing_field_values[field_name]}")
                    used_missing_field_values.add((block.section, function_name, field_name))
                    self.record(function_name, f"added missing {field_name}")
                else:
                    self.warnings.append(f"{function_name} in [{block.section}] has no {field_name}, add it to MISSING_FIELD_VALUES")
            insertions_after_index.setdefault(block.header_index, []).extend(missing_lines)

            if argument_count > 0 and "Defaults" not in block.field_indices:
                anchor_index = block.field_indices["Parameters"][0] if "Parameters" in block.field_indices else block.header_index
                insertions_after_index.setdefault(anchor_index, []).append(f"_{function_name}_Defaults={','.join([NO_VALUE] * argument_count)}")
                self.record(function_name, "added missing Defaults")

        for (section, function_name), field_values in MISSING_FIELD_VALUES.items():
            for field_name in field_values:
                if (section, function_name, field_name) not in used_missing_field_values:
                    self.warnings.append(f"MISSING_FIELD_VALUES entry for {field_name} of {function_name} in [{section}] is no longer needed")

        fixed_lines = []
        for index, line in enumerate(self.lines):
            if index not in removed_indices:
                fixed_lines.append(line)
            fixed_lines.extend(insertions_after_index.get(index, []))
        self.lines = fixed_lines

    def fix_function_header(self, block, type_names):
        key, value = self.lines[block.header_index].split("=", 1)
        values = value.split(",")
        is_call = block.section == "TriggerCalls"
        leading_value_count = 3 if is_call else 1
        leading_values = values[:leading_value_count]
        argument_types = values[leading_value_count:]

        if is_call and len(leading_values) > 1 and leading_values[1] not in FLAG_VALUES and leading_values[1].isdigit():
            self.record(key, f"normalized usable-in-events flag '{leading_values[1]}' to '1'")
            leading_values[1] = "1"

        if "" in argument_types:
            argument_types = [argument_type for argument_type in argument_types if argument_type != ""]
            self.record(key, "removed empty argument types")

        for argument_index, argument_type in enumerate(argument_types):
            if argument_type == NOTHING_TYPE or argument_type in EDITOR_ARGUMENT_TYPES or argument_type in type_names:
                continue
            corrected_type = closest_type_name(argument_type, type_names)
            if corrected_type is None:
                self.warnings.append(f"{key} has unknown argument type '{argument_type}'")
                continue
            argument_types[argument_index] = corrected_type
            self.record(key, f"corrected argument type '{argument_type}' to '{corrected_type}'")

        if not is_call and argument_types == []:
            argument_types = [NOTHING_TYPE]
            self.record(key, f"added '{NOTHING_TYPE}' argument type")

        self.lines[block.header_index] = f"{key}={','.join(leading_values + argument_types)}"
        argument_count = len([argument_type for argument_type in argument_types if argument_type != NOTHING_TYPE])
        return key, argument_count

    def field_value(self, index):
        return self.lines[index].split("=", 1)[1]

    def select_duplicate_field(self, field_name, indices, argument_count):
        expected_value_count = {"Defaults": argument_count, "AIDefaults": argument_count, "Limits": argument_count * 2}.get(field_name)
        for index in indices:
            if expected_value_count is not None and len(split_respecting_quotes(self.field_value(index))) == expected_value_count:
                return index
        return indices[0]

    def fix_field(self, block, field_name, function_name, fix_value):
        if field_name not in block.field_indices:
            return
        index = block.field_indices[field_name][0]
        key, value = self.lines[index].split("=", 1)
        fixed_value = fix_value(value)
        if fixed_value == value:
            return
        self.lines[index] = f"{key}={fixed_value}"
        self.record(key, f"changed '{value}' to '{fixed_value}'")


def is_comment(line):
    return line.lstrip().startswith("//")


def is_entry(line):
    return line.strip() != "" and not is_comment(line) and not line.strip().startswith("[")


def split_field_key(key):
    parts = key[1:].rsplit("_", 1)
    if len(parts) == 1:
        return parts[0], None
    return parts[0], parts[1]


def insert_missing_equals_sign(line, function_name, allowed_fields):
    for field_name in allowed_fields:
        prefix = f"_{function_name}_{field_name}"
        if line.startswith(prefix) and len(line) > len(prefix):
            return f"{prefix}={line[len(prefix):]}"
    return line


def canonicalize_field_name(field_name, allowed_fields):
    if field_name in allowed_fields:
        return field_name
    case_insensitive_match = next((allowed for allowed in allowed_fields if allowed.lower() == field_name.lower()), None)
    if case_insensitive_match is not None:
        return case_insensitive_match
    close_matches = difflib.get_close_matches(field_name, allowed_fields, n=1, cutoff=0.8)
    return close_matches[0] if close_matches else None


def closest_type_name(type_name, type_names):
    candidates = [candidate for candidate in type_names if edit_distance(type_name, candidate) <= 1]
    return candidates[0] if len(candidates) == 1 else None


def edit_distance(first, second):
    previous_row = list(range(len(second) + 1))
    for first_index, first_character in enumerate(first, start=1):
        current_row = [first_index]
        for second_index, second_character in enumerate(second, start=1):
            substitution_cost = 0 if first_character == second_character else 1
            current_row.append(min(previous_row[second_index] + 1, current_row[second_index - 1] + 1, previous_row[second_index - 1] + substitution_cost))
        previous_row = current_row
    return previous_row[-1]


def fix_display_name(value):
    if value.startswith('"') and value.count('"') % 2 == 1:
        return f'{value}"'
    return value


def repair_parameters(value):
    tokens = []
    position = 0
    length = len(value)
    while position < length:
        character = value[position]
        if character == '"':
            closing_position = value.find('"', position + 1)
            if closing_position == -1:
                remainder = value[position + 1:]
                if remainder != "":
                    tokens.append(f'"{remainder}"')
                break
            tokens.append(value[position:closing_position + 1])
            position = closing_position + 1
        elif character == "~":
            end_position = position + 1
            while end_position < length and value[end_position] not in ',"`':
                end_position += 1
            tokens.append(value[position:end_position])
            position = end_position
            if position < length and value[position] == "`":
                position += 1
        elif character == ",":
            position += 1
            continue
        else:
            text_position = position
            while text_position < length and value[text_position].isspace():
                text_position += 1
            if text_position < length and value[text_position] in '"~':
                position = text_position
                continue
            separator_position = value.find(",", position)
            end_position = length if separator_position == -1 else separator_position
            text = value[position:end_position]
            if text.strip() != "":
                tokens.append(quote_unquoted_text(text))
            position = end_position
        if position < length and value[position] == ",":
            position += 1
    return ",".join(tokens)


def quote_unquoted_text(text):
    if text.count('"') == 1 and text.endswith('"'):
        return f'"{text}'
    if '"' in text or text.strip().startswith("WESTRING_"):
        return text
    leading_space = " " if text[0].isspace() else ""
    return f'"{leading_space}{text.strip()}"'


def split_respecting_quotes(value):
    if value == "":
        return []
    values = []
    current = []
    in_double_quotes = False
    in_backticks = False
    for character in value:
        if character == '"' and not in_backticks:
            in_double_quotes = not in_double_quotes
        elif character == "`" and not in_double_quotes:
            in_backticks = not in_backticks
        if character == "," and not in_double_quotes and not in_backticks:
            values.append("".join(current))
            current = []
            continue
        current.append(character)
    values.append("".join(current))
    return values


def fit_values(value, expected_value_count):
    values = split_respecting_quotes(value)[:expected_value_count]
    values += [NO_VALUE] * (expected_value_count - len(values))
    return ",".join(NO_VALUE if fitted_value == "" else fitted_value for fitted_value in values)


def main():
    argument_parser = argparse.ArgumentParser(description="Fix issues in a Warcraft III TriggerData.txt file.")
    argument_parser.add_argument("path", nargs="?", type=Path, default=DEFAULT_PATH)
    arguments = argument_parser.parse_args()

    if not arguments.path.is_file():
        print(f"File not found: {arguments.path}", file=sys.stderr)
        return 2

    raw_bytes = arguments.path.read_bytes()
    if raw_bytes.startswith(b"\xef\xbb\xbf"):
        raw_bytes = raw_bytes[3:]
    text = raw_bytes.decode("utf-8")
    has_trailing_newline = text.endswith("\n")

    fixer = TriggerDataFixer(text.splitlines())
    fixed_lines = fixer.run()

    output_text = "\n".join(fixed_lines) + ("\n" if has_trailing_newline else "")
    arguments.path.write_text(output_text, encoding="utf-8", newline="\n")

    for fix in fixer.fixes:
        print(fix)
    for warning in fixer.warnings:
        print(f"warning: {warning}", file=sys.stderr)
    return 1 if fixer.warnings else 0


if __name__ == "__main__":
    sys.exit(main())

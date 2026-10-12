#!/usr/bin/env python3
import argparse
import re
import sys
from dataclasses import dataclass, field
from pathlib import Path

DEFAULT_PATH = Path(__file__).resolve().parent.parent / "src" / "War3Net.Build.Core" / "Resources" / "TriggerData.txt"

KNOWN_SECTIONS = [
    "TriggerCategories",
    "TriggerTypes",
    "TriggerTypeDefaults",
    "TriggerParams",
    "TriggerEvents",
    "TriggerConditions",
    "TriggerActions",
    "TriggerCalls",
    "DefaultTriggerCategories",
    "DefaultTriggers",
]

FUNCTION_SECTION_FIELDS = {
    "TriggerEvents": ["DisplayName", "Parameters", "Defaults", "Limits", "Category"],
    "TriggerConditions": ["DisplayName", "Parameters", "Defaults", "Category", "UseWithAI", "AIDefaults"],
    "TriggerActions": ["DisplayName", "Parameters", "Defaults", "Limits", "Category", "ScriptName"],
    "TriggerCalls": ["DisplayName", "Parameters", "Defaults", "Limits", "Category", "UseWithAI"],
}

REQUIRED_FUNCTION_FIELDS = ["DisplayName", "Parameters", "Category"]

NOTHING_TYPE = "nothing"

INTEGER_PATTERN = re.compile(r"^-?\d+$")
NUMBER_PATTERN = re.compile(r"^-?(\d+(\.\d*)?|\.\d+)$")
FLAG_VALUES = ("0", "1")


@dataclass
class Entry:
    line_number: int
    key: str
    value: str


@dataclass
class FunctionDefinition:
    entry: Entry
    section: str
    fields: dict = field(default_factory=dict)

    @property
    def name(self):
        return self.entry.key


@dataclass
class Section:
    name: str
    line_number: int
    entries: list = field(default_factory=list)


@dataclass
class Issue:
    line_number: int
    check: str
    message: str


class TriggerDataChecker:
    def __init__(self, path):
        self.path = path
        self.issues = []
        self.sections = {}
        self.functions = {section_name: {} for section_name in FUNCTION_SECTION_FIELDS}
        self.commented_out_category_identifiers = set()

    def report(self, line_number, check, message):
        self.issues.append(Issue(line_number, check, message))

    def run(self):
        self.parse()
        self.check_duplicate_keys()
        self.check_trigger_categories()
        self.check_trigger_types()
        self.check_trigger_type_defaults()
        self.check_trigger_params()
        for section_name in FUNCTION_SECTION_FIELDS:
            self.check_function_section(section_name)
        self.check_default_trigger_categories()
        self.check_default_triggers()
        self.issues.sort(key=lambda issue: issue.line_number)
        return self.issues

    def parse(self):
        raw_bytes = self.path.read_bytes()
        if raw_bytes.startswith(b"\xef\xbb\xbf"):
            raw_bytes = raw_bytes[3:]
        try:
            text = raw_bytes.decode("utf-8")
        except UnicodeDecodeError as error:
            self.report(text_line_number(raw_bytes, error.start), "encoding", f"File is not valid UTF-8: {error.reason}")
            text = raw_bytes.decode("utf-8", errors="replace")

        lines = text.splitlines(keepends=True)
        self.check_line_endings(lines)

        current_section = None
        for index, raw_line in enumerate(lines):
            line_number = index + 1
            line = raw_line.rstrip("\r\n")

            if line.lstrip().startswith("//"):
                self.collect_commented_out_category(current_section, line)
                continue

            if line.strip() == "":
                if line != "":
                    self.report(line_number, "whitespace", "Blank line contains whitespace")
                continue

            if line != line.rstrip():
                self.report(line_number, "whitespace", "Trailing whitespace")
            if line != line.lstrip():
                self.report(line_number, "whitespace", "Leading whitespace")
            stripped_line = line.strip()

            if stripped_line.startswith("["):
                current_section = self.parse_section_heading(line_number, stripped_line)
                continue

            if "//" in stripped_line and stripped_line.count('"') % 2 == 0 and not is_inside_quotes(stripped_line, stripped_line.index("//")):
                self.report(line_number, "inline-comment", "Inline comments are not supported, the comment becomes part of the value")

            if "=" not in stripped_line:
                self.report(line_number, "syntax", f"Line is not a comment, section heading, or key=value pair: {stripped_line}")
                continue

            key, value = stripped_line.split("=", 1)
            if key == "":
                self.report(line_number, "syntax", "Empty key")
                continue
            if key != key.strip():
                self.report(line_number, "whitespace", f"Key '{key}' has whitespace around it")
            if value != value.strip():
                self.report(line_number, "whitespace", f"Value of '{key.strip()}' has whitespace around it")
            key = key.strip()
            value = value.strip()

            self.check_quote_balance(line_number, key, value)

            if current_section is None:
                self.report(line_number, "syntax", f"Key '{key}' appears before any section heading")
                continue

            current_section.entries.append(Entry(line_number, key, value))

        for section_name in KNOWN_SECTIONS:
            if section_name not in self.sections:
                self.report(1, "missing-section", f"Section [{section_name}] is missing")

    def check_line_endings(self, lines):
        crlf_line_numbers = [index + 1 for index, line in enumerate(lines) if line.endswith("\r\n")]
        lf_line_numbers = [index + 1 for index, line in enumerate(lines) if line.endswith("\n") and not line.endswith("\r\n")]
        if crlf_line_numbers and lf_line_numbers:
            minority_line_numbers, minority_name = (crlf_line_numbers, "CRLF") if len(crlf_line_numbers) < len(lf_line_numbers) else (lf_line_numbers, "LF")
            for line_number in minority_line_numbers:
                self.report(line_number, "line-ending", f"Mixed line endings, this line uses {minority_name}")
        for index, line in enumerate(lines):
            stripped_line = line.rstrip("\n")
            if "\r" in stripped_line.rstrip("\r") or stripped_line.endswith("\r\r"):
                self.report(index + 1, "line-ending", "Stray carriage return")

    def collect_commented_out_category(self, current_section, line):
        if current_section is None or current_section.name != "TriggerCategories":
            return
        match = re.match(r"^\s*//\s*(TC_[A-Za-z0-9_]+)=", line)
        if match:
            self.commented_out_category_identifiers.add(match.group(1))

    def parse_section_heading(self, line_number, stripped_line):
        match = re.fullmatch(r"\[([^\[\]]+)\]", stripped_line)
        if match is None:
            self.report(line_number, "syntax", f"Malformed section heading: {stripped_line}")
            return None
        section_name = match.group(1)
        if section_name not in KNOWN_SECTIONS:
            matching_section_name = next((known for known in KNOWN_SECTIONS if known.lower() == section_name.lower()), None)
            hint = f" (did you mean [{matching_section_name}]?)" if matching_section_name else ""
            self.report(line_number, "unknown-section", f"Unknown section [{section_name}]{hint}")
        if section_name in self.sections:
            self.report(line_number, "duplicate-section", f"Section [{section_name}] already declared on line {self.sections[section_name].line_number}")
            return self.sections[section_name]
        section = Section(section_name, line_number)
        self.sections[section_name] = section
        expected_index = len(self.sections) - 1
        if section_name in KNOWN_SECTIONS and expected_index < len(KNOWN_SECTIONS) and KNOWN_SECTIONS[expected_index] != section_name:
            self.report(line_number, "section-order", f"Section [{section_name}] is out of order, the parser expects [{KNOWN_SECTIONS[expected_index]}] here")
        return section

    def check_quote_balance(self, line_number, key, value):
        if value.count('"') % 2 != 0:
            self.report(line_number, "unbalanced-quotes", f"Value of '{key}' has an unbalanced double quote")
        if value.count("`") % 2 != 0:
            self.report(line_number, "unbalanced-quotes", f"Value of '{key}' has an unbalanced backtick")

    def section_entries(self, section_name):
        section = self.sections.get(section_name)
        return section.entries if section else []

    def check_duplicate_keys(self):
        for section in self.sections.values():
            first_entry_by_key = {}
            first_entry_by_lowercase_key = {}
            for entry in section.entries:
                if entry.key in first_entry_by_key:
                    first_entry = first_entry_by_key[entry.key]
                    identical_hint = " with an identical value" if first_entry.value == entry.value else ""
                    self.report(entry.line_number, "duplicate-key", f"Duplicate key '{entry.key}' in [{section.name}]{identical_hint}, first declared on line {first_entry.line_number}")
                    continue
                first_entry_by_key[entry.key] = entry
                lowercase_key = entry.key.lower()
                if lowercase_key in first_entry_by_lowercase_key:
                    first_entry = first_entry_by_lowercase_key[lowercase_key]
                    self.report(entry.line_number, "duplicate-key-case", f"Key '{entry.key}' in [{section.name}] differs only in casing from '{first_entry.key}' on line {first_entry.line_number}")
                    continue
                first_entry_by_lowercase_key[lowercase_key] = entry

    def category_identifiers(self):
        return {entry.key for entry in self.section_entries("TriggerCategories")}

    def type_names(self):
        return {entry.key for entry in self.section_entries("TriggerTypes")}

    def check_trigger_categories(self):
        for entry in self.section_entries("TriggerCategories"):
            values = entry.value.split(",")
            if len(values) < 2 or len(values) > 3:
                self.report(entry.line_number, "value-count", f"Category '{entry.key}' expects 2 or 3 values, found {len(values)}")
                continue
            if values[0] == "":
                self.report(entry.line_number, "empty-value", f"Category '{entry.key}' has an empty display text")
            if values[1] == "":
                self.report(entry.line_number, "empty-value", f"Category '{entry.key}' has an empty icon")
            if len(values) == 3 and values[2] not in FLAG_VALUES:
                self.report(entry.line_number, "invalid-flag", f"Category '{entry.key}' has an invalid hide-name flag '{values[2]}', expected 0 or 1")

    def check_trigger_types(self):
        type_names = self.type_names()
        for entry in self.section_entries("TriggerTypes"):
            values = entry.value.split(",")
            if len(values) < 4 or len(values) > 7:
                self.report(entry.line_number, "value-count", f"Type '{entry.key}' expects 4 to 7 values, found {len(values)}")
                if len(values) < 4:
                    continue
            self.check_game_version(entry, values[0])
            for index, description in ((1, "global-variable"), (2, "comparison")):
                if values[index] not in FLAG_VALUES:
                    self.report(entry.line_number, "invalid-flag", f"Type '{entry.key}' has an invalid {description} flag '{values[index]}', expected 0 or 1")
            if values[3] == "":
                self.report(entry.line_number, "empty-value", f"Type '{entry.key}' has an empty display text")
            if len(values) > 4 and values[4] != "" and values[4] not in type_names:
                self.report(entry.line_number, "unknown-type", f"Type '{entry.key}' has unknown base type '{values[4]}'")
            if len(values) > 6 and values[6] not in FLAG_VALUES:
                self.report(entry.line_number, "invalid-flag", f"Type '{entry.key}' has an invalid treat-as-base-type flag '{values[6]}', expected 0 or 1")
            if len(values) > 6 and values[6] == "1" and (len(values) < 5 or values[4] == ""):
                self.report(entry.line_number, "invalid-flag", f"Type '{entry.key}' is treated as its base type but has no base type")

    def check_trigger_type_defaults(self):
        type_names = self.type_names()
        for entry in self.section_entries("TriggerTypeDefaults"):
            if entry.key not in type_names:
                self.report(entry.line_number, "unknown-type", f"Default declared for unknown type '{entry.key}'")
            values = entry.value.split(",")
            if len(values) > 2:
                self.report(entry.line_number, "value-count", f"Type default '{entry.key}' expects 1 or 2 values, found {len(values)}")
            if values[0] == "":
                self.report(entry.line_number, "empty-value", f"Type default '{entry.key}' has an empty script text")

    def check_trigger_params(self):
        type_names = self.type_names()
        for entry in self.section_entries("TriggerParams"):
            values = entry.value.split(",")
            if len(values) < 4:
                self.report(entry.line_number, "value-count", f"Param '{entry.key}' expects 4 values, found {len(values)}")
                continue
            if len(values) > 4:
                self.report(entry.line_number, "value-count", f"Param '{entry.key}' expects 4 values, found {len(values)} (the parser merges the excess into the display text)")
            self.check_game_version(entry, values[0])
            if values[1] not in type_names:
                self.report(entry.line_number, "unknown-type", f"Param '{entry.key}' has unknown type '{values[1]}'")
            if values[2] == "":
                self.report(entry.line_number, "empty-value", f"Param '{entry.key}' has an empty script text")
            if values[3] == "":
                self.report(entry.line_number, "empty-value", f"Param '{entry.key}' has an empty display text")

    def check_game_version(self, entry, value):
        if not INTEGER_PATTERN.match(value) or int(value) < 0:
            self.report(entry.line_number, "invalid-version", f"'{entry.key}' has an invalid game version '{value}'")

    def check_function_section(self, section_name):
        allowed_fields = FUNCTION_SECTION_FIELDS[section_name]
        functions = self.functions[section_name]
        function_names = {entry.key for entry in self.section_entries(section_name) if not entry.key.startswith("_")}
        current_function = None

        for entry in self.section_entries(section_name):
            if not entry.key.startswith("_"):
                current_function = FunctionDefinition(entry, section_name)
                if entry.key not in functions:
                    functions[entry.key] = current_function
                continue

            if current_function is not None and entry.key.startswith(f"_{current_function.name}_"):
                field_name = entry.key[len(current_function.name) + 2:]
                self.register_function_field(current_function, field_name, entry, allowed_fields)
                continue

            owner_name = longest_matching_function_name(entry.key, function_names)
            if owner_name is None and current_function is not None and entry.key.lower().startswith(f"_{current_function.name.lower()}_"):
                self.report(entry.line_number, "orphan-field", f"'{entry.key}' does not match the casing of the preceding function '{current_function.name}'")
            elif owner_name is None:
                self.report(entry.line_number, "orphan-field", f"'{entry.key}' does not belong to any function in [{section_name}]")
            else:
                owner_line_number = functions[owner_name].entry.line_number if owner_name in functions else "?"
                current_name = current_function.name if current_function else "none"
                self.report(entry.line_number, "orphan-field", f"'{entry.key}' belongs to '{owner_name}' (line {owner_line_number}) but follows '{current_name}'")

        category_identifiers = self.category_identifiers()
        type_names = self.type_names()
        seen_definitions = set()
        for entry in self.section_entries(section_name):
            if entry.key.startswith("_") or entry.line_number in seen_definitions:
                continue
            seen_definitions.add(entry.line_number)
            definition = functions.get(entry.key)
            if definition is None or definition.entry is not entry:
                continue
            self.check_function_definition(definition, category_identifiers, type_names)

    def register_function_field(self, definition, field_name, entry, allowed_fields):
        if field_name not in allowed_fields:
            matching_field_name = next((allowed for allowed in allowed_fields if allowed.lower() == field_name.lower()), None)
            similar_field_name = matching_field_name or closest_field_name(field_name, allowed_fields)
            hint = f" (did you mean '{similar_field_name}'?)" if similar_field_name else ""
            self.report(entry.line_number, "unknown-field", f"Unknown field '{field_name}' for '{definition.name}' in [{definition.section}]{hint}")
            if similar_field_name is None:
                return
            field_name = similar_field_name
        if field_name in definition.fields:
            self.report(entry.line_number, "duplicate-field", f"Field '{field_name}' of '{definition.name}' already declared on line {definition.fields[field_name].line_number}")
            return
        definition.fields[field_name] = entry

    def check_function_definition(self, definition, category_identifiers, type_names):
        entry = definition.entry
        values = entry.value.split(",")
        minimum_value_count = 3 if definition.section == "TriggerCalls" else 1
        if len(values) < minimum_value_count:
            self.report(entry.line_number, "value-count", f"'{entry.key}' expects at least {minimum_value_count} values, found {len(values)}")
            return

        self.check_game_version(entry, values[0])
        if definition.section == "TriggerCalls":
            if values[1] not in FLAG_VALUES:
                self.report(entry.line_number, "invalid-flag", f"'{entry.key}' has an invalid usable-in-events flag '{values[1]}', expected 0 or 1")
            if values[2] not in type_names:
                self.report(entry.line_number, "unknown-type", f"'{entry.key}' has unknown return type '{values[2]}'")
            argument_types = values[3:]
        else:
            argument_types = values[1:]

        if NOTHING_TYPE in argument_types and argument_types != [NOTHING_TYPE]:
            self.report(entry.line_number, "invalid-arguments", f"'{entry.key}' mixes '{NOTHING_TYPE}' with other argument types")
        if definition.section != "TriggerCalls" and argument_types == []:
            self.report(entry.line_number, "invalid-arguments", f"'{entry.key}' declares no argument types, use '{NOTHING_TYPE}' for functions without arguments")
        for argument_type in argument_types:
            if argument_type == "":
                self.report(entry.line_number, "invalid-arguments", f"'{entry.key}' has an empty argument type")
            elif argument_type != NOTHING_TYPE and argument_type not in type_names:
                self.report(entry.line_number, "unknown-type", f"'{entry.key}' has unknown argument type '{argument_type}'")
        argument_count = len([argument_type for argument_type in argument_types if argument_type != NOTHING_TYPE])

        for field_name in REQUIRED_FUNCTION_FIELDS:
            if field_name not in definition.fields:
                self.report(entry.line_number, "missing-field", f"'{entry.key}' has no {field_name}")
        if argument_count > 0 and "Defaults" not in definition.fields:
            self.report(entry.line_number, "missing-field", f"'{entry.key}' has {argument_count} argument(s) but no Defaults")

        self.check_display_name(definition)
        self.check_parameters(definition, argument_count)
        self.check_defaults(definition, "Defaults", argument_count)
        self.check_defaults(definition, "AIDefaults", argument_count)
        self.check_limits(definition, argument_count)
        self.check_category(definition, category_identifiers)
        self.check_use_with_ai(definition)
        self.check_script_name(definition)

    def check_display_name(self, definition):
        display_name_entry = definition.fields.get("DisplayName")
        if display_name_entry is None:
            return
        if not (len(display_name_entry.value) >= 2 and display_name_entry.value.startswith('"') and display_name_entry.value.endswith('"')) and not display_name_entry.value.startswith("WESTRING_"):
            self.report(display_name_entry.line_number, "invalid-display-name", f"DisplayName of '{definition.name}' is not a quoted string")

    def check_parameters(self, definition, argument_count):
        parameters_entry = definition.fields.get("Parameters")
        if parameters_entry is None:
            return
        tokens, errors = tokenize_parameters(parameters_entry.value)
        for error in errors:
            self.report(parameters_entry.line_number, "invalid-parameters", f"Parameters of '{definition.name}': {error}")
        if errors:
            return
        placeholder_count = len([token for token in tokens if token.startswith("~")])
        if placeholder_count != argument_count:
            self.report(parameters_entry.line_number, "parameter-count", f"Parameters of '{definition.name}' has {placeholder_count} placeholder(s) but the function has {argument_count} argument(s)")

    def check_defaults(self, definition, field_name, argument_count):
        defaults_entry = definition.fields.get(field_name)
        if defaults_entry is None:
            return
        values, error = split_respecting_quotes(defaults_entry.value)
        if error:
            self.report(defaults_entry.line_number, "invalid-defaults", f"{field_name} of '{definition.name}': {error}")
            return
        if values == [""]:
            values = []
        naive_value_count = 0 if defaults_entry.value == "" else len(defaults_entry.value.split(","))
        if naive_value_count != len(values):
            self.report(defaults_entry.line_number, "invalid-defaults", f"{field_name} of '{definition.name}' contains a comma inside a quoted value, the parser splits it")
        if len(values) != argument_count:
            self.report(defaults_entry.line_number, "defaults-count", f"{field_name} of '{definition.name}' has {len(values)} value(s) but the function has {argument_count} argument(s)")
        for index, value in enumerate(values):
            if value == "":
                self.report(defaults_entry.line_number, "invalid-defaults", f"{field_name} of '{definition.name}' has an empty value at position {index + 1}, use '_' for no default")

    def check_limits(self, definition, argument_count):
        limits_entry = definition.fields.get("Limits")
        if limits_entry is None:
            return
        values = limits_entry.value.split(",")
        if len(values) != argument_count * 2:
            self.report(limits_entry.line_number, "limits-count", f"Limits of '{definition.name}' has {len(values)} value(s) but expects {argument_count * 2} (minimum and maximum per argument)")
        for value in values:
            if value != "_" and not NUMBER_PATTERN.match(value):
                self.report(limits_entry.line_number, "invalid-limits", f"Limits of '{definition.name}' has invalid value '{value}', expected a number or '_'")
        for index in range(0, len(values) - 1, 2):
            minimum, maximum = values[index], values[index + 1]
            if NUMBER_PATTERN.match(minimum) and NUMBER_PATTERN.match(maximum) and float(minimum) > float(maximum):
                self.report(limits_entry.line_number, "invalid-limits", f"Limits of '{definition.name}' has minimum {minimum} greater than maximum {maximum} for argument {index // 2 + 1}")

    def check_category(self, definition, category_identifiers):
        category_entry = definition.fields.get("Category")
        if category_entry is None:
            return
        if category_entry.value not in category_identifiers:
            hint = " (it is commented out in [TriggerCategories])" if category_entry.value in self.commented_out_category_identifiers else ""
            self.report(category_entry.line_number, "unknown-category", f"'{definition.name}' has unknown category '{category_entry.value}'{hint}")

    def check_use_with_ai(self, definition):
        use_with_ai_entry = definition.fields.get("UseWithAI")
        if use_with_ai_entry is not None and use_with_ai_entry.value not in FLAG_VALUES:
            self.report(use_with_ai_entry.line_number, "invalid-flag", f"UseWithAI of '{definition.name}' is '{use_with_ai_entry.value}', expected 0 or 1")
        if "AIDefaults" in definition.fields and (use_with_ai_entry is None or use_with_ai_entry.value != "1"):
            self.report(definition.fields["AIDefaults"].line_number, "invalid-flag", f"'{definition.name}' has AIDefaults but UseWithAI is not 1")

    def check_script_name(self, definition):
        script_name_entry = definition.fields.get("ScriptName")
        if script_name_entry is not None and not re.fullmatch(r"[A-Za-z_][A-Za-z0-9_]*", script_name_entry.value):
            self.report(script_name_entry.line_number, "invalid-script-name", f"ScriptName of '{definition.name}' is not a valid identifier: '{script_name_entry.value}'")

    def check_default_trigger_categories(self):
        entries = self.section_entries("DefaultTriggerCategories")
        declared_count = None
        category_numbers = []
        for entry in entries:
            if entry.key == "NumCategories":
                declared_count = self.parse_count(entry)
                continue
            match = re.fullmatch(r"Category(\d\d)", entry.key)
            if match is None:
                self.report(entry.line_number, "unknown-key", f"Unexpected key '{entry.key}' in [DefaultTriggerCategories]")
                continue
            category_numbers.append((int(match.group(1)), entry))
            if entry.value == "":
                self.report(entry.line_number, "empty-value", f"'{entry.key}' has an empty name")
        self.check_numbering(category_numbers, declared_count, "NumCategories", "DefaultTriggerCategories")

    def check_default_triggers(self):
        entries = self.section_entries("DefaultTriggers")
        declared_count = None
        fields_by_trigger_number = {}
        for entry in entries:
            if entry.key == "NumTriggers":
                declared_count = self.parse_count(entry)
                continue
            match = re.fullmatch(r"Trigger(\d\d)(Name|Comment|Category|Events|Conditions|Actions|Event\d\d|Condition\d\d|Action\d\d)", entry.key)
            if match is None:
                self.report(entry.line_number, "unknown-key", f"Unexpected key '{entry.key}' in [DefaultTriggers]")
                continue
            fields_by_trigger_number.setdefault(int(match.group(1)), {})[match.group(2)] = entry

        trigger_numbers = [(trigger_number, min(fields.values(), key=lambda field_entry: field_entry.line_number)) for trigger_number, fields in fields_by_trigger_number.items()]
        self.check_numbering(trigger_numbers, declared_count, "NumTriggers", "DefaultTriggers")

        default_category_count = len([entry for entry in self.section_entries("DefaultTriggerCategories") if entry.key.startswith("Category")])
        for trigger_number, fields in sorted(fields_by_trigger_number.items()):
            trigger_key = f"Trigger{trigger_number:02d}"
            first_line_number = min(field_entry.line_number for field_entry in fields.values())
            for required_field in ("Name", "Category", "Events", "Conditions", "Actions"):
                if required_field not in fields:
                    self.report(first_line_number, "missing-field", f"{trigger_key} has no {required_field}")
            category_entry = fields.get("Category")
            if category_entry is not None:
                category_number = self.parse_count(category_entry)
                if category_number is not None and not 1 <= category_number <= default_category_count:
                    self.report(category_entry.line_number, "unknown-category", f"{trigger_key}Category refers to category {category_number}, but only {default_category_count} default categories exist")
            for kind, section_name in (("Event", "TriggerEvents"), ("Condition", "TriggerConditions"), ("Action", "TriggerActions")):
                self.check_default_trigger_items(trigger_key, fields, kind, section_name)

    def check_default_trigger_items(self, trigger_key, fields, kind, section_name):
        count_entry = fields.get(f"{kind}s")
        declared_count = self.parse_count(count_entry) if count_entry is not None else None
        item_numbers = []
        for field_name, field_entry in fields.items():
            match = re.fullmatch(rf"{kind}(\d\d)", field_name)
            if match is None:
                continue
            item_numbers.append((int(match.group(1)), field_entry))
            function_name = field_entry.value.split(",")[0]
            if function_name not in self.functions[section_name]:
                self.report(field_entry.line_number, "unknown-function", f"{trigger_key}{field_name} refers to '{function_name}', which is not defined in [{section_name}]")
        self.check_numbering(item_numbers, declared_count, f"{trigger_key}{kind}s", "DefaultTriggers")

    def parse_count(self, entry):
        if not INTEGER_PATTERN.match(entry.value) or int(entry.value) < 0:
            self.report(entry.line_number, "invalid-number", f"'{entry.key}' is not a non-negative integer: '{entry.value}'")
            return None
        return int(entry.value)

    def check_numbering(self, numbered_entries, declared_count, count_key, section_name):
        numbers = sorted(number for number, _ in numbered_entries)
        expected_numbers = list(range(1, len(numbers) + 1))
        if numbers != expected_numbers:
            line_number = min((entry.line_number for _, entry in numbered_entries), default=self.sections[section_name].line_number if section_name in self.sections else 1)
            self.report(line_number, "numbering", f"Entries counted by {count_key} are not numbered consecutively from 01: {', '.join(f'{number:02d}' for number in numbers)}")
        if declared_count is not None and declared_count != len(numbers):
            line_number = min((entry.line_number for _, entry in numbered_entries), default=self.sections[section_name].line_number if section_name in self.sections else 1)
            self.report(line_number, "count-mismatch", f"{count_key} is {declared_count} but {len(numbers)} entries exist")


def text_line_number(raw_bytes, byte_offset):
    return raw_bytes.count(b"\n", 0, byte_offset) + 1


def is_inside_quotes(text, position):
    return text.count('"', 0, position) % 2 == 1 or text.count("`", 0, position) % 2 == 1


def longest_matching_function_name(key, function_names):
    matching_names = [name for name in function_names if key.startswith(f"_{name}_")]
    return max(matching_names, key=len) if matching_names else None


def closest_field_name(field_name, allowed_fields):
    lowercase_field_name = field_name.lower()
    for allowed in allowed_fields:
        lowercase_allowed = allowed.lower()
        if lowercase_allowed.startswith(lowercase_field_name[:4]) and abs(len(lowercase_allowed) - len(lowercase_field_name)) <= 2:
            return allowed
    return None


def split_respecting_quotes(value):
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
    if in_double_quotes or in_backticks:
        return values, "unterminated quoted value"
    return values, None


def tokenize_parameters(value):
    tokens = []
    errors = []
    position = 0
    length = len(value)
    if length == 0:
        return tokens, ["empty"]
    while position < length:
        character = value[position]
        if character == '"':
            closing_position = value.find('"', position + 1)
            if closing_position == -1:
                errors.append(f"unterminated string starting at column {position + 1}")
                return tokens, errors
            tokens.append(value[position:closing_position + 1])
            position = closing_position + 1
        elif character == "~":
            separator_position = value.find(",", position)
            end_position = length if separator_position == -1 else separator_position
            placeholder = value[position:end_position]
            if placeholder == "~":
                errors.append(f"empty placeholder at column {position + 1}")
            elif re.search(r'["`]', placeholder):
                errors.append(f"placeholder '{placeholder}' contains a quote or backtick")
            tokens.append(placeholder)
            position = end_position
        else:
            separator_position = value.find(",", position)
            end_position = length if separator_position == -1 else separator_position
            errors.append(f"unquoted text '{value[position:end_position]}' at column {position + 1}")
            position = end_position
        if position < length:
            if value[position] != ",":
                errors.append(f"expected ',' at column {position + 1} but found '{value[position]}'")
                separator_position = value.find(",", position)
                if separator_position == -1:
                    return tokens, errors
                position = separator_position
            position += 1
            if position == length:
                errors.append("trailing comma")
    return tokens, errors


def main():
    argument_parser = argparse.ArgumentParser(description="Report issues in a Warcraft III TriggerData.txt file.")
    argument_parser.add_argument("path", nargs="?", type=Path, default=DEFAULT_PATH)
    argument_parser.add_argument("--summary", action="store_true", help="Only print the number of issues per check")
    arguments = argument_parser.parse_args()

    if not arguments.path.is_file():
        print(f"File not found: {arguments.path}", file=sys.stderr)
        return 2

    issues = TriggerDataChecker(arguments.path).run()
    issue_count_by_check = {}
    for issue in issues:
        issue_count_by_check[issue.check] = issue_count_by_check.get(issue.check, 0) + 1
        if not arguments.summary:
            print(f"{issue.line_number}: [{issue.check}] {issue.message}")

    if issues:
        if not arguments.summary:
            print()
        for check, issue_count in sorted(issue_count_by_check.items(), key=lambda item: (-item[1], item[0])):
            print(f"{issue_count:6d}  {check}")
        print(f"{len(issues):6d}  total")
        return 1

    print("No issues found")
    return 0


if __name__ == "__main__":
    sys.exit(main())

// ------------------------------------------------------------------------------
// <copyright file="TranspileToEnumHandler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    // TODO: include a boolean that indicates which enums can have the [Flags] attribute.
    public static class TranspileToEnumHandler
    {
        private static readonly Dictionary<string, string> _enumTranspiledTypes;
        private static readonly Dictionary<string, string> _enumConvertFunctions;
        private static readonly Dictionary<string, EnumDeclarationSyntax> _enums;

        private static SyntaxKind _overrideFunctionAccessModifier;
        private static SyntaxKind _overrideDeclarationAccessModifier;

        static TranspileToEnumHandler()
        {
            _enumTranspiledTypes = new Dictionary<string, string>();
            _enumConvertFunctions = new Dictionary<string, string>();
            _enums = new Dictionary<string, EnumDeclarationSyntax>();

            _overrideFunctionAccessModifier = SyntaxKind.PublicKeyword;
            _overrideDeclarationAccessModifier = SyntaxKind.PublicKeyword;
        }

        /// <summary>
        /// Gets or sets the access modifier for a function, if it's used as a cast function for enums.
        /// </summary>
        public static SyntaxKind EnumCastFunctionAccessModifier
        {
            get => _overrideFunctionAccessModifier;
            set => _overrideFunctionAccessModifier = value;
        }

        /// <summary>
        /// Gets or sets the access modifier for a global declaration, if it's transpiled as an enum member.
        /// </summary>
        public static SyntaxKind EnumMemberDeclarationAccessModifier
        {
            get => _overrideDeclarationAccessModifier;
            set => _overrideDeclarationAccessModifier = value;
        }

        public static void DefineEnumTypes(IEnumerable<(string enumTypeName, string functionName)> enumPairs)
        {
            foreach (var (key, value) in enumPairs)
            {
                _enumTranspiledTypes.Add(key, value);
                _enumConvertFunctions.Add(value, key);
            }
        }

        internal static void Reset()
        {
            _enumTranspiledTypes.Clear();
            _enumConvertFunctions.Clear();
            _enums.Clear();
        }

        internal static void AddEnum(EnumDeclarationSyntax enumDeclaration)
        {
            _enums.Add(enumDeclaration.Identifier.Text, enumDeclaration);
        }

        internal static void AddEnumMember(EnumMemberDeclarationSyntax enumMemberDeclaration, string convertFunctionName)
        {
            var key = _enumConvertFunctions[convertFunctionName];
            _enums[key] = _enums[key].AddMembers(enumMemberDeclaration);
        }

        internal static IEnumerable<EnumDeclarationSyntax> GetEnums()
        {
            foreach (var @enum in _enums.Values)
            {
                yield return @enum;
            }
        }

        internal static bool IsTypeEnum(string typeName, out string convertFunction)
        {
            if (_enumTranspiledTypes.ContainsKey(typeName))
            {
                convertFunction = _enumTranspiledTypes[typeName];
                return true;
            }

            convertFunction = null;
            return false;
        }

        internal static bool IsFunctionEnumConverter(string convertFunction, out string typeName)
        {
            if (_enumConvertFunctions.ContainsKey(convertFunction))
            {
                typeName = _enumConvertFunctions[convertFunction];
                return true;
            }

            typeName = null;
            return false;
        }
    }
}
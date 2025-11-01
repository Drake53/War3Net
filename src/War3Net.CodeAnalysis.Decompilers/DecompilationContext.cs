// ------------------------------------------------------------------------------
// 
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using War3Net.Build.Info;
using War3Net.Build.Script;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Decompilers
{
    public class DecompilationContext
    {
        public DecompilationContext(JassCompilationUnitSyntax compilationUnit, DecompileOptions options = null, MapInfo mapInfo = null, TriggerData triggerData = null)
        {
            CompilationUnit = compilationUnit;
            Options = options ?? new DecompileOptions();
            MapInfo = mapInfo;

            TriggerData = new TriggerDataContext(triggerData);

            var comments = new List<JassCommentSyntax>();
            var functionDeclarationsBuilder = ImmutableDictionary.CreateBuilder<string, FunctionDeclarationContext>(StringComparer.Ordinal);
            var variableDeclarationsBuilder = ImmutableDictionary.CreateBuilder<string, VariableDeclarationContext>(StringComparer.Ordinal);

            foreach (var declaration in CompilationUnit.Declarations)
            {
                if (declaration is JassCommentSyntax comment)
                {
                    comments.Add(comment);
                }
                else
                {
                    if (declaration is JassFunctionDeclarationSyntax functionDeclaration)
                    {
                        functionDeclarationsBuilder.Add(functionDeclaration.FunctionDeclarator.IdentifierName.Name, new FunctionDeclarationContext(functionDeclaration, comments));
                    }
                    else if (declaration is JassGlobalDeclarationListSyntax globalDeclarationList)
                    {
                        foreach (var declaration2 in globalDeclarationList.Globals)
                        {
                            if (declaration2 is JassGlobalDeclarationSyntax globalDeclaration)
                            {
                                variableDeclarationsBuilder.Add(globalDeclaration.Declarator.IdentifierName.Name, new VariableDeclarationContext(globalDeclaration));
                            }
                        }
                    }

                    comments.Clear();
                }
            }

            FunctionDeclarations = functionDeclarationsBuilder.ToImmutable();
            VariableDeclarations = variableDeclarationsBuilder.ToImmutable();


            ImportedFileNames = new(StringComparer.OrdinalIgnoreCase);
            MaxPlayerSlots = mapInfo != null && mapInfo.EditorVersion >= EditorVersion.v6060 ? 24 : 12;
        }

        public TriggerDataContext TriggerData { get; }
        public ImmutableDictionary<string, FunctionDeclarationContext> FunctionDeclarations { get; }
        public ImmutableDictionary<string, VariableDeclarationContext> VariableDeclarations { get; }
        public HashSet<string> ImportedFileNames { get; }
        public int MaxPlayerSlots { get; }

        public HashSet<IStatementLineSyntax> HandledStatements = new HashSet<IStatementLineSyntax>();
        public JassCompilationUnitSyntax CompilationUnit { get; }
        public MapInfo MapInfo { get; }
        public DecompileOptions Options { get; }

        private readonly Dictionary<string, object> _variableNameToValueMapping = new();
        private readonly List<object> _values = new();
        private int _lastCreationNumber;

        public int GetNextCreationNumber()
        {
            return _lastCreationNumber++;
        }

        public string GetVariableName(object value)
        {
            return _variableNameToValueMapping.FirstOrDefault(x => x.Value == value).Key;
        }

        public void Add<T>(T value, string variableName = null) where T : class
        {
            if (variableName != null)
            {
                _variableNameToValueMapping[variableName] = value;
            }

            _values.Add(value);
        }

        public void Add_Struct<T>(T value, string variableName = null) where T : struct
        {
            Add(new Nullable_Class<T>(value), variableName);
        }

        public T Get<T>(string variableName) where T : class
        {
            if (variableName == null)
            {
                return default;
            }

            return _variableNameToValueMapping.GetValueOrDefault(variableName) as T;
        }

        public Nullable_Class<T> Get_Struct<T>(string variableName = null) where T : struct
        {
            return Get<Nullable_Class<T>>(variableName);
        }

        public T GetLastCreated<T>() where T : class
        {
            return _values.OfType<T>().LastOrDefault();
        }

        public Nullable_Class<T> GetLastCreated_Struct<T>() where T : struct
        {
            return GetLastCreated<Nullable_Class<T>>();
        }

        public IEnumerable<T> GetAll<T>() where T : class
        {
            return _values.OfType<T>();
        }

        public IEnumerable<Nullable_Class<T>> GetAll_Struct<T>() where T : struct
        {
            return GetAll<Nullable_Class<T>>();
        }

        private const string PSEUDO_VARIABLE_PREFIX = "##PSEUDO_VARIABLE_PREFIX##";
        internal string CreatePseudoVariableName(string type, string name = "")
        {
            return PSEUDO_VARIABLE_PREFIX + "_" + type.ToString() + "_" + name;
        }

    }
}
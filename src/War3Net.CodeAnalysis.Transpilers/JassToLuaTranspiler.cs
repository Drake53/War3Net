namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        private readonly Dictionary<string, JassTypeSyntax> _functionReturnTypes;
        private readonly Dictionary<string, JassTypeSyntax> _globalTypes;
        private readonly Dictionary<string, JassTypeSyntax> _localTypes;

        public JassToLuaTranspiler()
        {
            _functionReturnTypes = new(StringComparer.Ordinal);
            _globalTypes = new(StringComparer.Ordinal);
            _localTypes = new(StringComparer.Ordinal);
        }

        public bool IgnoreComments { get; set; }

        public bool IgnoreEmptyDeclarations { get; set; }

        public bool IgnoreEmptyStatements { get; set; }

        public bool KeepFunctionsSeparated { get; set; }

        public void RegisterJassFile(JassCompilationUnitSyntax compilationUnit)
        {
            foreach (var declaration in compilationUnit.Declarations)
            {
                if (declaration is JassGlobalsDeclarationSyntax globalsDeclaration)
                {
                    foreach (var globalDeclaration in globalsDeclaration.GlobalDeclarations)
                    {
                        switch (globalDeclaration)
                        {
                            case JassGlobalConstantDeclarationSyntax globalConstantDeclaration:
                                RegisterVariableType(globalConstantDeclaration);
                                break;

                            case JassGlobalVariableDeclarationSyntax globalVariableDeclaration:
                                RegisterVariableType(globalVariableDeclaration.Declarator, false);
                                break;
                        }
                    }
                }
                else if (declaration is JassNativeFunctionDeclarationSyntax nativeFunctionDeclaration)
                {
                    RegisterFunctionReturnType(nativeFunctionDeclaration);
                }
                else if (declaration is JassFunctionDeclarationSyntax functionDeclaration)
                {
                    RegisterFunctionReturnType(functionDeclaration.FunctionDeclarator);
                }
            }
        }

        public void RegisterFunctionReturnType(MethodInfo method)
        {
            if (method is null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            _functionReturnTypes.Add(method.Name, method.ReturnType.ToJassType());
        }

        public void RegisterGlobalVariableType(FieldInfo field)
        {
            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            _globalTypes.Add(field.Name, field.FieldType.ToJassType());
        }

        internal void ClearLocalTypes()
        {
            _localTypes.Clear();
        }

        internal void RegisterFunctionReturnType(JassFunctionDeclaratorSyntax functionDeclarator)
        {
            _functionReturnTypes.Add(functionDeclarator.IdentifierName.Token.Text, functionDeclarator.ReturnClause.ReturnType);
        }

        internal void RegisterFunctionReturnType(JassNativeFunctionDeclarationSyntax nativeFunctionDeclaration)
        {
            _functionReturnTypes.Add(nativeFunctionDeclaration.IdentifierName.Token.Text, nativeFunctionDeclaration.ReturnClause.ReturnType);
        }

        private void RegisterVariableType(JassVariableOrArrayDeclaratorSyntax declarator, bool isLocalDeclaration)
        {
            switch (declarator)
            {
                case JassArrayDeclaratorSyntax arrayDeclarator: (isLocalDeclaration ? _localTypes : _globalTypes).Add(arrayDeclarator.IdentifierName.Token.Text, arrayDeclarator.Type); break;
                case JassVariableDeclaratorSyntax variableDeclarator: (isLocalDeclaration ? _localTypes : _globalTypes).Add(variableDeclarator.IdentifierName.Token.Text, variableDeclarator.Type); break;
            }
        }

        private void RegisterVariableType(JassGlobalConstantDeclarationSyntax globalConstantDeclaration)
        {
            _globalTypes.Add(globalConstantDeclaration.IdentifierName.Token.Text, globalConstantDeclaration.Type);
        }

        private void RegisterLocalVariableType(JassParameterSyntax parameter)
        {
            _localTypes.Add(parameter.IdentifierName.Token.Text, parameter.Type);
        }

        private JassTypeSyntax GetFunctionReturnType(JassIdentifierNameSyntax functionName)
        {
            return _functionReturnTypes.TryGetValue(functionName.Token.Text, out var type)
                ? type
                : throw new KeyNotFoundException($"Function '{functionName}' could not be found.");
        }

        private JassTypeSyntax GetVariableType(JassIdentifierNameSyntax variableName)
        {
            return (_localTypes.TryGetValue(variableName.Token.Text, out var type) || _globalTypes.TryGetValue(variableName.Token.Text, out type))
                ? type
                : throw new KeyNotFoundException($"Variable '{variableName}' could not be found.");
        }
    }
}
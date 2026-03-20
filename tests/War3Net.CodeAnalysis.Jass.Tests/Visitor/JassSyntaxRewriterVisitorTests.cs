namespace War3Net.CodeAnalysis.Jass.Tests.Visitor
{
    /// <summary>
    /// Tests for using the visitor pattern to implement code fixes/rewrites.
    /// Demonstrates replacing method calls while preserving trivia (whitespace, comments).
    /// </summary>
    [TestClass]
    public class JassSyntaxRewriterVisitorTests
    {
        #region Rewriter Visitor Implementation

        /// <summary>
        /// A rewriter that replaces Blizzard "WithSkin" function calls with their non-skin equivalents.
        /// This demonstrates a practical code fix scenario where:
        /// - BlzCreateItemWithSkin(itemId, x, y, skinId) -> CreateItem(itemId, x, y)
        /// - BlzCreateUnitWithSkin(player, unitId, x, y, facing, skinId) -> CreateUnit(player, unitId, x, y, facing)
        /// etc.
        /// </summary>
        private sealed class BlzSkinFunctionRewriter : JassSyntaxVisitor<JassSyntaxNode?>
        {
            private static readonly Dictionary<string, (string ReplacementName, int ExpectedArgCount, int[] ArgsToKeep)> FunctionMappings = new()
            {
                ["BlzCreateItemWithSkin"] = ("CreateItem", 4, new[] { 0, 1, 2 }),
                ["BlzCreateUnitWithSkin"] = ("CreateUnit", 6, new[] { 0, 1, 2, 3, 4 }),
                ["BlzCreateDestructableWithSkin"] = ("CreateDestructable", 7, new[] { 0, 1, 2, 3, 4, 5 }),
                ["BlzCreateDestructableZWithSkin"] = ("CreateDestructableZ", 8, new[] { 0, 1, 2, 3, 4, 5, 6 }),
                ["BlzCreateDeadDestructableWithSkin"] = ("CreateDeadDestructable", 7, new[] { 0, 1, 2, 3, 4, 5 }),
                ["BlzCreateDeadDestructableZWithSkin"] = ("CreateDeadDestructableZ", 8, new[] { 0, 1, 2, 3, 4, 5, 6 }),
            };

            public int ReplacementCount { get; private set; }

            public override JassSyntaxNode? DefaultVisit(JassSyntaxNode node)
            {
                // For nodes we don't specifically handle, return null to indicate no change
                return null;
            }

            public override JassSyntaxNode? VisitCompilationUnit(JassCompilationUnitSyntax node)
            {
                var newDeclarations = ImmutableArray.CreateBuilder<JassTopLevelDeclarationSyntax>();
                var hasChanges = false;

                foreach (var declaration in node.Declarations)
                {
                    var rewritten = Visit(declaration);
                    if (rewritten is JassTopLevelDeclarationSyntax rewrittenDecl)
                    {
                        newDeclarations.Add(rewrittenDecl);
                        hasChanges = true;
                    }
                    else
                    {
                        newDeclarations.Add(declaration);
                    }
                }

                return hasChanges
                    ? new JassCompilationUnitSyntax(newDeclarations.ToImmutable(), node.EndOfFileToken)
                    : null;
            }

            public override JassSyntaxNode? VisitFunctionDeclaration(JassFunctionDeclarationSyntax node)
            {
                var newStatements = ImmutableArray.CreateBuilder<JassStatementSyntax>();
                var hasChanges = false;

                foreach (var statement in node.Statements)
                {
                    var rewritten = Visit(statement);
                    if (rewritten is JassStatementSyntax rewrittenStmt)
                    {
                        newStatements.Add(rewrittenStmt);
                        hasChanges = true;
                    }
                    else
                    {
                        newStatements.Add(statement);
                    }
                }

                return hasChanges
                    ? new JassFunctionDeclarationSyntax(node.FunctionDeclarator, newStatements.ToImmutable(), node.EndFunctionToken)
                    : null;
            }

            public override JassSyntaxNode? VisitCallStatement(JassCallStatementSyntax node)
            {
                var functionName = node.IdentifierName.Token.Text;

                if (FunctionMappings.TryGetValue(functionName, out var mapping))
                {
                    var args = node.ArgumentList.Arguments.Items;

                    if (args.Length == mapping.ExpectedArgCount)
                    {
                        ReplacementCount++;

                        // Create new identifier with new function name, preserving trivia from original
                        var originalToken = node.IdentifierName.Token;
                        var newIdentifierToken = new JassSyntaxToken(
                            originalToken.LeadingTrivia,
                            JassSyntaxKind.IdentifierToken,
                            mapping.ReplacementName,
                            originalToken.TrailingTrivia);
                        var newIdentifierName = new JassIdentifierNameSyntax(newIdentifierToken);

                        // Build new argument list with only the kept arguments, preserving trivia
                        var newArgs = mapping.ArgsToKeep.Select(i => args[i]).ToArray();
                        var newSeparators = new JassSyntaxToken[Math.Max(0, newArgs.Length - 1)];

                        // Preserve the original separators' trivia
                        var originalSeparators = node.ArgumentList.Arguments.Separators;
                        for (var i = 0; i < newSeparators.Length; i++)
                        {
                            if (i < originalSeparators.Length)
                            {
                                newSeparators[i] = originalSeparators[i];
                            }
                            else
                            {
                                // Create a new comma separator with standard spacing
                                newSeparators[i] = new JassSyntaxToken(
                                    JassSyntaxKind.CommaToken,
                                    JassSymbol.Comma,
                                    JassSyntaxTriviaList.SingleSpace);
                            }
                        }

                        var newArgumentList = new JassArgumentListSyntax(
                            node.ArgumentList.OpenParenToken,
                            SeparatedSyntaxList<JassExpressionSyntax, JassSyntaxToken>.Create(
                                newArgs.ToImmutableArray(),
                                newSeparators.ToImmutableArray()),
                            node.ArgumentList.CloseParenToken);

                        return new JassCallStatementSyntax(
                            node.CallToken,
                            newIdentifierName,
                            newArgumentList);
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// A simpler rewriter that renames all occurrences of a specific function call.
        /// </summary>
        private sealed class FunctionRenameRewriter : JassSyntaxVisitor<JassSyntaxNode?>
        {
            private readonly string _oldName;
            private readonly string _newName;

            public FunctionRenameRewriter(string oldName, string newName)
            {
                _oldName = oldName;
                _newName = newName;
            }

            public int RenameCount { get; private set; }

            public override JassSyntaxNode? DefaultVisit(JassSyntaxNode node)
            {
                return null;
            }

            public override JassSyntaxNode? VisitCompilationUnit(JassCompilationUnitSyntax node)
            {
                var newDeclarations = ImmutableArray.CreateBuilder<JassTopLevelDeclarationSyntax>();
                var hasChanges = false;

                foreach (var declaration in node.Declarations)
                {
                    var rewritten = Visit(declaration);
                    if (rewritten is JassTopLevelDeclarationSyntax rewrittenDecl)
                    {
                        newDeclarations.Add(rewrittenDecl);
                        hasChanges = true;
                    }
                    else
                    {
                        newDeclarations.Add(declaration);
                    }
                }

                return hasChanges
                    ? new JassCompilationUnitSyntax(newDeclarations.ToImmutable(), node.EndOfFileToken)
                    : null;
            }

            public override JassSyntaxNode? VisitFunctionDeclaration(JassFunctionDeclarationSyntax node)
            {
                var newStatements = ImmutableArray.CreateBuilder<JassStatementSyntax>();
                var hasChanges = false;

                foreach (var statement in node.Statements)
                {
                    var rewritten = Visit(statement);
                    if (rewritten is JassStatementSyntax rewrittenStmt)
                    {
                        newStatements.Add(rewrittenStmt);
                        hasChanges = true;
                    }
                    else
                    {
                        newStatements.Add(statement);
                    }
                }

                return hasChanges
                    ? new JassFunctionDeclarationSyntax(node.FunctionDeclarator, newStatements.ToImmutable(), node.EndFunctionToken)
                    : null;
            }

            public override JassSyntaxNode? VisitCallStatement(JassCallStatementSyntax node)
            {
                if (string.Equals(node.IdentifierName.Token.Text, _oldName, StringComparison.Ordinal))
                {
                    RenameCount++;

                    var originalToken = node.IdentifierName.Token;
                    var newToken = new JassSyntaxToken(
                        originalToken.LeadingTrivia,
                        JassSyntaxKind.IdentifierToken,
                        _newName,
                        originalToken.TrailingTrivia);

                    return new JassCallStatementSyntax(
                        node.CallToken,
                        new JassIdentifierNameSyntax(newToken),
                        node.ArgumentList);
                }

                return null;
            }

            public override JassSyntaxNode? VisitInvocationExpression(JassInvocationExpressionSyntax node)
            {
                if (string.Equals(node.IdentifierName.Token.Text, _oldName, StringComparison.Ordinal))
                {
                    RenameCount++;

                    var originalToken = node.IdentifierName.Token;
                    var newToken = new JassSyntaxToken(
                        originalToken.LeadingTrivia,
                        JassSyntaxKind.IdentifierToken,
                        _newName,
                        originalToken.TrailingTrivia);

                    return new JassInvocationExpressionSyntax(
                        new JassIdentifierNameSyntax(newToken),
                        node.ArgumentList);
                }

                return null;
            }
        }

        #endregion

        #region BlzSkinFunctionRewriter Tests

        [TestMethod]
        public void BlzSkinRewriter_ReplacesBlzCreateItemWithSkin()
        {
            const string input = @"
function Test takes nothing returns nothing
    call BlzCreateItemWithSkin('ratf', 0.0, 0.0, 'ratf')
endfunction
";
            const string expected = @"
function Test takes nothing returns nothing
    call CreateItem('ratf', 0.0, 0.0)
endfunction
";
            var compilationUnit = JassSyntaxFactory.ParseCompilationUnit(input);
            var rewriter = new BlzSkinFunctionRewriter();

            var result = rewriter.Visit(compilationUnit);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, rewriter.ReplacementCount);
            Assert.AreEqual(expected, ((JassCompilationUnitSyntax)result).ToFullString());
        }

        [TestMethod]
        public void BlzSkinRewriter_ReplacesBlzCreateUnitWithSkin()
        {
            const string input = @"
function Test takes nothing returns nothing
    call BlzCreateUnitWithSkin(Player(0), 'hfoo', 0.0, 0.0, 270.0, 'hfoo')
endfunction
";
            const string expected = @"
function Test takes nothing returns nothing
    call CreateUnit(Player(0), 'hfoo', 0.0, 0.0, 270.0)
endfunction
";
            var compilationUnit = JassSyntaxFactory.ParseCompilationUnit(input);
            var rewriter = new BlzSkinFunctionRewriter();

            var result = rewriter.Visit(compilationUnit);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, rewriter.ReplacementCount);
            Assert.AreEqual(expected, ((JassCompilationUnitSyntax)result).ToFullString());
        }

        [TestMethod]
        public void BlzSkinRewriter_PreservesTrivia()
        {
            const string input = @"
function Test takes nothing returns nothing
    call  BlzCreateItemWithSkin('ratf',  0.0,  0.0,  'ratf')
endfunction
";
            const string expected = @"
function Test takes nothing returns nothing
    call  CreateItem('ratf',  0.0,  0.0)
endfunction
";
            var compilationUnit = JassSyntaxFactory.ParseCompilationUnit(input);
            var rewriter = new BlzSkinFunctionRewriter();

            var result = rewriter.Visit(compilationUnit);

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, ((JassCompilationUnitSyntax)result).ToFullString());
        }

        [TestMethod]
        public void BlzSkinRewriter_RemovesLastArgument()
        {
            const string input = @"
function Test takes nothing returns nothing
    call BlzCreateItemWithSkin('ratf', 100.0, 200.0, 'skin')
endfunction
";
            const string expected = @"
function Test takes nothing returns nothing
    call CreateItem('ratf', 100.0, 200.0)
endfunction
";
            var compilationUnit = JassSyntaxFactory.ParseCompilationUnit(input);
            var rewriter = new BlzSkinFunctionRewriter();

            var result = rewriter.Visit(compilationUnit);

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, ((JassCompilationUnitSyntax)result).ToFullString());
        }

        [TestMethod]
        public void BlzSkinRewriter_HandlesMultipleReplacements()
        {
            const string input = @"
function Test takes nothing returns nothing
    call BlzCreateItemWithSkin('item', 0.0, 0.0, 'skin')
    call BlzCreateUnitWithSkin(Player(0), 'unit', 0.0, 0.0, 0.0, 'skin')
    call BlzCreateDestructableWithSkin('dest', 0.0, 0.0, 0.0, 1.0, 0, 'skin')
endfunction
";
            const string expected = @"
function Test takes nothing returns nothing
    call CreateItem('item', 0.0, 0.0)
    call CreateUnit(Player(0), 'unit', 0.0, 0.0, 0.0)
    call CreateDestructable('dest', 0.0, 0.0, 0.0, 1.0, 0)
endfunction
";
            var compilationUnit = JassSyntaxFactory.ParseCompilationUnit(input);
            var rewriter = new BlzSkinFunctionRewriter();

            var result = rewriter.Visit(compilationUnit);

            Assert.IsNotNull(result);
            Assert.AreEqual(3, rewriter.ReplacementCount);
            Assert.AreEqual(expected, ((JassCompilationUnitSyntax)result).ToFullString());
        }

        [TestMethod]
        public void BlzSkinRewriter_DoesNotModifyOtherCalls()
        {
            const string input = @"
function Test takes nothing returns nothing
    call DisplayText(""Hello"")
    call BlzCreateItemWithSkin('ratf', 0.0, 0.0, 'ratf')
    call DoNothing()
endfunction
";
            const string expected = @"
function Test takes nothing returns nothing
    call DisplayText(""Hello"")
    call CreateItem('ratf', 0.0, 0.0)
    call DoNothing()
endfunction
";
            var compilationUnit = JassSyntaxFactory.ParseCompilationUnit(input);
            var rewriter = new BlzSkinFunctionRewriter();

            var result = rewriter.Visit(compilationUnit);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, rewriter.ReplacementCount);
            Assert.AreEqual(expected, ((JassCompilationUnitSyntax)result).ToFullString());
        }

        [TestMethod]
        public void BlzSkinRewriter_ReturnsNullForNoChanges()
        {
            const string input = @"
function Test takes nothing returns nothing
    call DisplayText(""Hello"")
endfunction
";
            var compilationUnit = JassSyntaxFactory.ParseCompilationUnit(input);
            var rewriter = new BlzSkinFunctionRewriter();

            var result = rewriter.Visit(compilationUnit);

            Assert.IsNull(result, "Should return null when no changes are made");
            Assert.AreEqual(0, rewriter.ReplacementCount);
        }

        #endregion

        #region FunctionRenameRewriter Tests

        [TestMethod]
        public void FunctionRenameRewriter_RenamesCallStatements()
        {
            const string input = @"
function Test takes nothing returns nothing
    call OldFunction(1, 2, 3)
endfunction
";
            const string expected = @"
function Test takes nothing returns nothing
    call NewFunction(1, 2, 3)
endfunction
";
            var compilationUnit = JassSyntaxFactory.ParseCompilationUnit(input);
            var rewriter = new FunctionRenameRewriter("OldFunction", "NewFunction");

            var result = rewriter.Visit(compilationUnit);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, rewriter.RenameCount);
            Assert.AreEqual(expected, ((JassCompilationUnitSyntax)result).ToFullString());
        }

        [TestMethod]
        public void FunctionRenameRewriter_PreservesAllTrivia()
        {
            const string input = @"
function Test takes nothing returns nothing
    // This is a comment
    call  OldFunction(1,  2,  3)
endfunction
";
            const string expected = @"
function Test takes nothing returns nothing
    // This is a comment
    call  NewFunction(1,  2,  3)
endfunction
";
            var compilationUnit = JassSyntaxFactory.ParseCompilationUnit(input);
            var rewriter = new FunctionRenameRewriter("OldFunction", "NewFunction");

            var result = rewriter.Visit(compilationUnit);

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, ((JassCompilationUnitSyntax)result).ToFullString());
        }

        [TestMethod]
        public void FunctionRenameRewriter_PreservesArgumentTrivia()
        {
            const string input = @"
function Test takes nothing returns nothing
    call MyFunc( arg1 , arg2 , arg3 )
endfunction
";
            const string expected = @"
function Test takes nothing returns nothing
    call RenamedFunc( arg1 , arg2 , arg3 )
endfunction
";
            var compilationUnit = JassSyntaxFactory.ParseCompilationUnit(input);
            var rewriter = new FunctionRenameRewriter("MyFunc", "RenamedFunc");

            var result = rewriter.Visit(compilationUnit);

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, ((JassCompilationUnitSyntax)result).ToFullString());
        }

        [TestMethod]
        public void FunctionRenameRewriter_RenamesMultipleOccurrences()
        {
            const string input = @"
function Test takes nothing returns nothing
    call Foo()
    call Bar()
    call Foo()
    call Foo()
endfunction
";
            const string expected = @"
function Test takes nothing returns nothing
    call Baz()
    call Bar()
    call Baz()
    call Baz()
endfunction
";
            var compilationUnit = JassSyntaxFactory.ParseCompilationUnit(input);
            var rewriter = new FunctionRenameRewriter("Foo", "Baz");

            var result = rewriter.Visit(compilationUnit);

            Assert.IsNotNull(result);
            Assert.AreEqual(3, rewriter.RenameCount);
            Assert.AreEqual(expected, ((JassCompilationUnitSyntax)result).ToFullString());
        }

        [TestMethod]
        public void FunctionRenameRewriter_IsCaseSensitive()
        {
            const string input = @"
function Test takes nothing returns nothing
    call myFunction()
    call MyFunction()
    call MYFUNCTION()
endfunction
";
            const string expected = @"
function Test takes nothing returns nothing
    call myFunction()
    call Renamed()
    call MYFUNCTION()
endfunction
";
            var compilationUnit = JassSyntaxFactory.ParseCompilationUnit(input);
            var rewriter = new FunctionRenameRewriter("MyFunction", "Renamed");

            var result = rewriter.Visit(compilationUnit);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, rewriter.RenameCount, "Should only rename exact case matches");
            Assert.AreEqual(expected, ((JassCompilationUnitSyntax)result).ToFullString());
        }

        #endregion

        #region Trivia Preservation Tests

        [TestMethod]
        public void Rewriter_PreservesLeadingNewlines()
        {
            const string input = @"

function Test takes nothing returns nothing
    call OldFunc()
endfunction
";
            const string expected = @"

function Test takes nothing returns nothing
    call NewFunc()
endfunction
";
            var compilationUnit = JassSyntaxFactory.ParseCompilationUnit(input);
            var rewriter = new FunctionRenameRewriter("OldFunc", "NewFunc");

            var result = rewriter.Visit(compilationUnit);

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, ((JassCompilationUnitSyntax)result).ToFullString());
        }

        [TestMethod]
        public void Rewriter_PreservesInlineComments()
        {
            const string input = @"
function Test takes nothing returns nothing
    call OldFunc() // inline comment
endfunction
";
            const string expected = @"
function Test takes nothing returns nothing
    call NewFunc() // inline comment
endfunction
";
            var compilationUnit = JassSyntaxFactory.ParseCompilationUnit(input);
            var rewriter = new FunctionRenameRewriter("OldFunc", "NewFunc");

            var result = rewriter.Visit(compilationUnit);

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, ((JassCompilationUnitSyntax)result).ToFullString());
        }

        [TestMethod]
        public void Rewriter_DoesNotModifyExpressionContextCalls()
        {
            const string input = @"
function Test takes nothing returns nothing
    local item i = BlzCreateItemWithSkin('ratf', 100.0, 200.0, 'ratf')
    call DisplayText(""Item created"")
endfunction
";
            var compilationUnit = JassSyntaxFactory.ParseCompilationUnit(input);
            var rewriter = new BlzSkinFunctionRewriter();

            var result = rewriter.Visit(compilationUnit);

            // BlzCreateItemWithSkin is in an expression context (assignment), not a call statement.
            // The rewriter only handles call statements in this implementation.
            Assert.IsNull(result, "Assignment expressions aren't handled by this simple rewriter");
        }

        #endregion

        #region Edge Cases

        [TestMethod]
        public void BlzSkinRewriter_IgnoresWrongArgumentCount()
        {
            const string input = @"
function Test takes nothing returns nothing
    call BlzCreateItemWithSkin('ratf', 0.0)
endfunction
";
            var compilationUnit = JassSyntaxFactory.ParseCompilationUnit(input);
            var rewriter = new BlzSkinFunctionRewriter();

            var result = rewriter.Visit(compilationUnit);

            Assert.IsNull(result, "Should not rewrite when argument count doesn't match");
            Assert.AreEqual(0, rewriter.ReplacementCount);
        }

        [TestMethod]
        public void Rewriter_HandlesEmptyFunction()
        {
            const string input = @"
function Empty takes nothing returns nothing
endfunction
";
            var compilationUnit = JassSyntaxFactory.ParseCompilationUnit(input);
            var rewriter = new FunctionRenameRewriter("NonExistent", "Renamed");

            var result = rewriter.Visit(compilationUnit);

            Assert.IsNull(result, "Should return null for empty function with no matches");
        }

        [TestMethod]
        public void Rewriter_HandlesNestedCalls()
        {
            const string input = @"
function Test takes nothing returns nothing
    call Outer(Inner())
endfunction
";
            const string expected = @"
function Test takes nothing returns nothing
    call RenamedOuter(Inner())
endfunction
";
            var compilationUnit = JassSyntaxFactory.ParseCompilationUnit(input);
            var rewriter = new FunctionRenameRewriter("Outer", "RenamedOuter");

            var result = rewriter.Visit(compilationUnit);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, rewriter.RenameCount);
            Assert.AreEqual(expected, ((JassCompilationUnitSyntax)result).ToFullString());
        }

        #endregion
    }
}
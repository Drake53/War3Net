// ------------------------------------------------------------------------------
// <copyright file="JassSyntaxVisitorTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass.Tests.Visitor
{
    [TestClass]
    public class JassSyntaxVisitorTests
    {
        #region Test Visitors

        /// <summary>
        /// A visitor that counts nodes by type - demonstrates selective node visiting.
        /// </summary>
        private sealed class NodeCounterVisitor : JassSyntaxWalker
        {
            public int FunctionDeclarationCount { get; private set; }
            public int CallStatementCount { get; private set; }
            public int IdentifierCount { get; private set; }
            public int LiteralCount { get; private set; }
            public int BinaryExpressionCount { get; private set; }
            public int TotalNodeCount { get; private set; }

            public override void DefaultVisit(JassSyntaxNode node)
            {
                TotalNodeCount++;
                base.DefaultVisit(node);
            }

            public override void VisitFunctionDeclaration(JassFunctionDeclarationSyntax node)
            {
                FunctionDeclarationCount++;
                base.VisitFunctionDeclaration(node);
            }

            public override void VisitCallStatement(JassCallStatementSyntax node)
            {
                CallStatementCount++;
                base.VisitCallStatement(node);
            }

            public override void VisitIdentifierName(JassIdentifierNameSyntax node)
            {
                IdentifierCount++;
                base.VisitIdentifierName(node);
            }

            public override void VisitLiteralExpression(JassLiteralExpressionSyntax node)
            {
                LiteralCount++;
                base.VisitLiteralExpression(node);
            }

            public override void VisitBinaryExpression(JassBinaryExpressionSyntax node)
            {
                BinaryExpressionCount++;
                base.VisitBinaryExpression(node);
            }
        }

        /// <summary>
        /// A visitor that collects all function call names - demonstrates practical analysis.
        /// </summary>
        private sealed class FunctionCallCollector : JassSyntaxWalker
        {
            public List<string> FunctionCalls { get; } = new();

            public override void VisitCallStatement(JassCallStatementSyntax node)
            {
                FunctionCalls.Add(node.IdentifierName.Token.Text);
                base.VisitCallStatement(node);
            }

            public override void VisitInvocationExpression(JassInvocationExpressionSyntax node)
            {
                FunctionCalls.Add(node.IdentifierName.Token.Text);
                base.VisitInvocationExpression(node);
            }
        }

        /// <summary>
        /// A visitor that collects all identifier names - demonstrates identifier extraction.
        /// </summary>
        private sealed class IdentifierCollector : JassSyntaxWalker
        {
            public HashSet<string> Identifiers { get; } = new();

            public override void VisitIdentifierName(JassIdentifierNameSyntax node)
            {
                Identifiers.Add(node.Token.Text);
                base.VisitIdentifierName(node);
            }
        }

        /// <summary>
        /// A visitor that extracts all string literals - demonstrates literal analysis.
        /// </summary>
        private sealed class StringLiteralCollector : JassSyntaxWalker
        {
            public List<string> StringLiterals { get; } = new();

            public override void VisitLiteralExpression(JassLiteralExpressionSyntax node)
            {
                if (node.Token.SyntaxKind == JassSyntaxKind.StringLiteralToken)
                {
                    StringLiterals.Add(node.Token.Text);
                }
                base.VisitLiteralExpression(node);
            }
        }

        /// <summary>
        /// A visitor with a result - demonstrates IJassSyntaxVisitor&lt;TResult&gt; usage.
        /// </summary>
        private sealed class ExpressionEvaluator : JassSyntaxVisitor<int?>
        {
            public override int? VisitLiteralExpression(JassLiteralExpressionSyntax node)
            {
                if (node.Token.SyntaxKind == JassSyntaxKind.DecimalLiteralToken &&
                    int.TryParse(node.Token.Text, out var value))
                {
                    return value;
                }
                return null;
            }

            public override int? VisitBinaryExpression(JassBinaryExpressionSyntax node)
            {
                var left = Visit(node.Left);
                var right = Visit(node.Right);

                if (left == null || right == null)
                {
                    return null;
                }

                return node.OperatorToken.SyntaxKind switch
                {
                    JassSyntaxKind.PlusToken => left + right,
                    JassSyntaxKind.MinusToken => left - right,
                    JassSyntaxKind.AsteriskToken => left * right,
                    JassSyntaxKind.SlashToken when right != 0 => left / right,
                    _ => null,
                };
            }

            public override int? VisitParenthesizedExpression(JassParenthesizedExpressionSyntax node)
            {
                return Visit(node.Expression);
            }

            public override int? VisitUnaryExpression(JassUnaryExpressionSyntax node)
            {
                var operand = Visit(node.Operand);
                if (operand == null) return null;

                return node.OperatorToken.SyntaxKind switch
                {
                    JassSyntaxKind.PlusToken => operand,
                    JassSyntaxKind.MinusToken => -operand,
                    _ => null,
                };
            }
        }

        /// <summary>
        /// A visitor that generates a simple AST string representation.
        /// </summary>
        private sealed class AstPrinter : JassSyntaxVisitor<string>
        {
            public override string DefaultVisit(JassSyntaxNode node)
            {
                return node.GetType().Name.Replace("Jass", "").Replace("Syntax", "");
            }

            public override string VisitLiteralExpression(JassLiteralExpressionSyntax node)
            {
                return $"Literal({node.Token.Text})";
            }

            public override string VisitIdentifierName(JassIdentifierNameSyntax node)
            {
                return $"Id({node.Token.Text})";
            }

            public override string VisitBinaryExpression(JassBinaryExpressionSyntax node)
            {
                var left = Visit(node.Left) ?? "?";
                var right = Visit(node.Right) ?? "?";
                return $"Binary({left} {node.OperatorToken.Text} {right})";
            }

            public override string VisitInvocationExpression(JassInvocationExpressionSyntax node)
            {
                var args = string.Join(", ", node.ArgumentList.Arguments.Items.Select(a => Visit(a) ?? "?"));
                return $"Call({node.IdentifierName.Token.Text}({args}))";
            }
        }

        #endregion

        #region Basic Visitor Tests

        [TestMethod]
        public void Visit_NullNode_DoesNotThrow()
        {
            var visitor = new NodeCounterVisitor();
            visitor.Visit(null);
            Assert.AreEqual(0, visitor.TotalNodeCount);
        }

        [TestMethod]
        public void Visit_SingleExpression_CountsCorrectly()
        {
            var expression = JassSyntaxFactory.ParseExpression("42");
            var visitor = new NodeCounterVisitor();
            visitor.Visit(expression);

            Assert.AreEqual(1, visitor.LiteralCount);
            Assert.AreEqual(1, visitor.TotalNodeCount);
        }

        [TestMethod]
        public void Visit_BinaryExpression_CountsAllNodes()
        {
            // 2 + 3 * 4 creates: BinaryAdd(Literal(2), BinaryMul(Literal(3), Literal(4)))
            var expression = JassSyntaxFactory.ParseExpression("2 + 3 * 4");
            var visitor = new NodeCounterVisitor();
            visitor.Visit(expression);

            Assert.AreEqual(2, visitor.BinaryExpressionCount, "Should have 2 binary expressions (+ and *)");
            Assert.AreEqual(3, visitor.LiteralCount, "Should have 3 literals (2, 3, 4)");
        }

        #endregion

        #region Function Call Collection Tests

        [TestMethod]
        public void FunctionCallCollector_CollectsCallStatements()
        {
            var code = @"
function TestFunc takes nothing returns nothing
    call DisplayText(""Hello"")
    call DoSomething()
endfunction
";
            var compilationUnit = JassSyntaxFactory.ParseCompilationUnit(code);
            var collector = new FunctionCallCollector();
            collector.Visit(compilationUnit);

            Assert.AreEqual(2, collector.FunctionCalls.Count);
            CollectionAssert.Contains(collector.FunctionCalls, "DisplayText");
            CollectionAssert.Contains(collector.FunctionCalls, "DoSomething");
        }

        [TestMethod]
        public void FunctionCallCollector_CollectsInvocationExpressions()
        {
            var expression = JassSyntaxFactory.ParseExpression("GetUnitState(unit, UNIT_STATE_LIFE) > 0");
            var collector = new FunctionCallCollector();
            collector.Visit(expression);

            Assert.AreEqual(1, collector.FunctionCalls.Count);
            Assert.AreEqual("GetUnitState", collector.FunctionCalls[0]);
        }

        [TestMethod]
        public void FunctionCallCollector_CollectsNestedCalls()
        {
            var expression = JassSyntaxFactory.ParseExpression("Outer(Inner(Nested(x)))");
            var collector = new FunctionCallCollector();
            collector.Visit(expression);

            Assert.AreEqual(3, collector.FunctionCalls.Count);
            CollectionAssert.Contains(collector.FunctionCalls, "Outer");
            CollectionAssert.Contains(collector.FunctionCalls, "Inner");
            CollectionAssert.Contains(collector.FunctionCalls, "Nested");
        }

        #endregion

        #region Identifier Collection Tests

        [TestMethod]
        public void IdentifierCollector_CollectsUniqueIdentifiers()
        {
            var expression = JassSyntaxFactory.ParseExpression("a + b + a + c");
            var collector = new IdentifierCollector();
            collector.Visit(expression);

            Assert.AreEqual(3, collector.Identifiers.Count, "Should have 3 unique identifiers");
            Assert.IsTrue(collector.Identifiers.Contains("a"));
            Assert.IsTrue(collector.Identifiers.Contains("b"));
            Assert.IsTrue(collector.Identifiers.Contains("c"));
        }

        [TestMethod]
        public void IdentifierCollector_CollectsFromFunction()
        {
            var code = @"
function TestFunc takes integer x, real y returns integer
    local integer result = x + 1
    return result
endfunction
";
            var compilationUnit = JassSyntaxFactory.ParseCompilationUnit(code);
            var collector = new IdentifierCollector();
            collector.Visit(compilationUnit);

            Assert.IsTrue(collector.Identifiers.Contains("TestFunc"), "Should contain function name");
            Assert.IsTrue(collector.Identifiers.Contains("x"), "Should contain parameter x");
            Assert.IsTrue(collector.Identifiers.Contains("y"), "Should contain parameter y");
            Assert.IsTrue(collector.Identifiers.Contains("result"), "Should contain local variable");
        }

        #endregion

        #region String Literal Collection Tests

        [TestMethod]
        public void StringLiteralCollector_CollectsStringLiterals()
        {
            var code = @"
function TestFunc takes nothing returns nothing
    call DisplayText(""Hello World"")
    call DisplayText(""Goodbye"")
endfunction
";
            var compilationUnit = JassSyntaxFactory.ParseCompilationUnit(code);
            var collector = new StringLiteralCollector();
            collector.Visit(compilationUnit);

            Assert.AreEqual(2, collector.StringLiterals.Count);
        }

        #endregion

        #region Expression Evaluator Tests (Visitor with Result)

        [TestMethod]
        public void ExpressionEvaluator_EvaluatesSimpleLiteral()
        {
            var expression = JassSyntaxFactory.ParseExpression("42");
            var evaluator = new ExpressionEvaluator();
            var result = evaluator.Visit(expression);

            Assert.AreEqual(42, result);
        }

        [TestMethod]
        public void ExpressionEvaluator_EvaluatesAddition()
        {
            var expression = JassSyntaxFactory.ParseExpression("2 + 3");
            var evaluator = new ExpressionEvaluator();
            var result = evaluator.Visit(expression);

            Assert.AreEqual(5, result);
        }

        [TestMethod]
        public void ExpressionEvaluator_EvaluatesComplexExpression()
        {
            // 2 + 3 * 4 = 2 + 12 = 14 (respects operator precedence)
            var expression = JassSyntaxFactory.ParseExpression("2 + 3 * 4");
            var evaluator = new ExpressionEvaluator();
            var result = evaluator.Visit(expression);

            Assert.AreEqual(14, result);
        }

        [TestMethod]
        public void ExpressionEvaluator_EvaluatesParenthesized()
        {
            // (2 + 3) * 4 = 5 * 4 = 20
            var expression = JassSyntaxFactory.ParseExpression("(2 + 3) * 4");
            var evaluator = new ExpressionEvaluator();
            var result = evaluator.Visit(expression);

            Assert.AreEqual(20, result);
        }

        [TestMethod]
        public void ExpressionEvaluator_EvaluatesUnaryMinus()
        {
            var expression = JassSyntaxFactory.ParseExpression("-5");
            var evaluator = new ExpressionEvaluator();
            var result = evaluator.Visit(expression);

            Assert.AreEqual(-5, result);
        }

        [TestMethod]
        public void ExpressionEvaluator_ReturnsNullForNonConstant()
        {
            var expression = JassSyntaxFactory.ParseExpression("x + 1");
            var evaluator = new ExpressionEvaluator();
            var result = evaluator.Visit(expression);

            Assert.IsNull(result, "Should return null when expression contains variables");
        }

        #endregion

        #region AST Printer Tests (Visitor with String Result)

        [TestMethod]
        public void AstPrinter_PrintsLiteral()
        {
            var expression = JassSyntaxFactory.ParseExpression("42");
            var printer = new AstPrinter();
            var result = printer.Visit(expression);

            Assert.AreEqual("Literal(42)", result);
        }

        [TestMethod]
        public void AstPrinter_PrintsIdentifier()
        {
            var expression = JassSyntaxFactory.ParseExpression("myVar");
            var printer = new AstPrinter();
            var result = printer.Visit(expression);

            Assert.AreEqual("Id(myVar)", result);
        }

        [TestMethod]
        public void AstPrinter_PrintsBinaryExpression()
        {
            var expression = JassSyntaxFactory.ParseExpression("a + b");
            var printer = new AstPrinter();
            var result = printer.Visit(expression);

            Assert.AreEqual("Binary(Id(a) + Id(b))", result);
        }

        [TestMethod]
        public void AstPrinter_PrintsFunctionCall()
        {
            var expression = JassSyntaxFactory.ParseExpression("Foo(x, y)");
            var printer = new AstPrinter();
            var result = printer.Visit(expression);

            Assert.AreEqual("Call(Foo(Id(x), Id(y)))", result);
        }

        #endregion

        #region Full Program Analysis Tests

        [TestMethod]
        public void NodeCounter_AnalyzesFullProgram()
        {
            var code = @"
globals
    integer globalVar = 0
endglobals

function Init takes nothing returns nothing
    set globalVar = 1
endfunction

function Main takes nothing returns nothing
    call Init()
    call DisplayText(""Done"")
endfunction
";
            var compilationUnit = JassSyntaxFactory.ParseCompilationUnit(code);
            var counter = new NodeCounterVisitor();
            counter.Visit(compilationUnit);

            Assert.AreEqual(2, counter.FunctionDeclarationCount, "Should have 2 function declarations");
            Assert.AreEqual(2, counter.CallStatementCount, "Should have 2 call statements");
            Assert.IsTrue(counter.TotalNodeCount > 10, "Should have many nodes in total");
        }

        [TestMethod]
        public void MultipleVisitors_CanAnalyzeSameTree()
        {
            var code = @"
function Calculate takes integer x returns integer
    local integer result = x * 2 + 1
    call Log(result)
    return result
endfunction
";
            var compilationUnit = JassSyntaxFactory.ParseCompilationUnit(code);

            var counter = new NodeCounterVisitor();
            var identifiers = new IdentifierCollector();
            var calls = new FunctionCallCollector();

            counter.Visit(compilationUnit);
            identifiers.Visit(compilationUnit);
            calls.Visit(compilationUnit);

            Assert.AreEqual(1, counter.FunctionDeclarationCount);
            Assert.IsTrue(identifiers.Identifiers.Contains("Calculate"));
            Assert.IsTrue(identifiers.Identifiers.Contains("x"));
            Assert.IsTrue(identifiers.Identifiers.Contains("result"));
            Assert.AreEqual(1, calls.FunctionCalls.Count);
            Assert.AreEqual("Log", calls.FunctionCalls[0]);
        }

        #endregion

        #region Accept Method Tests

        [TestMethod]
        public void Accept_CallsCorrectVisitorMethod()
        {
            var expression = JassSyntaxFactory.ParseExpression("42");
            var visitedTypes = new List<string>();

            var visitor = new TrackingVisitor(visitedTypes);
            expression.Accept(visitor);

            Assert.AreEqual(1, visitedTypes.Count);
            Assert.AreEqual("LiteralExpression", visitedTypes[0]);
        }

        [TestMethod]
        public void Accept_GenericVersion_ReturnsResult()
        {
            var expression = JassSyntaxFactory.ParseExpression("42");
            var evaluator = new ExpressionEvaluator();

            // Using Accept directly
            var result = expression.Accept(evaluator);

            Assert.AreEqual(42, result);
        }

        private sealed class TrackingVisitor : JassSyntaxVisitor
        {
            private readonly List<string> _visitedTypes;

            public TrackingVisitor(List<string> visitedTypes)
            {
                _visitedTypes = visitedTypes;
            }

            public override void VisitLiteralExpression(JassLiteralExpressionSyntax node)
            {
                _visitedTypes.Add("LiteralExpression");
                base.VisitLiteralExpression(node);
            }

            public override void VisitBinaryExpression(JassBinaryExpressionSyntax node)
            {
                _visitedTypes.Add("BinaryExpression");
                base.VisitBinaryExpression(node);
            }

            public override void VisitIdentifierName(JassIdentifierNameSyntax node)
            {
                _visitedTypes.Add("IdentifierName");
                base.VisitIdentifierName(node);
            }
        }

        #endregion
    }
}
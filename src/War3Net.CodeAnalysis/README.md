# War3Net.CodeAnalysis

## About

War3Net.CodeAnalysis is a library providing helper methods and extension methods for [Pidgin](https://github.com/benjamin-hodgson/Pidgin) parsers, along with utility classes for code generation. It simplifies the creation of complex recursive parsers for domain-specific languages and provides tools for generating properly formatted source code output. It is part of the [War3Net](https://github.com/Drake53/War3Net) modding library.

## Key features

* Parse separated lists (e.g., comma-separated arguments) while preserving separator tokens
* Parse if/elseif/else/endif block structures with trivia preservation
* Parse block constructs with open/close tokens (e.g., loops, function bodies)
* Generate indented source code output with automatic indentation management
* Immutable data structures for representing parsed syntax lists

## How to Use

### Parse a comma-separated list

```csharp
using Pidgin;
using War3Net.CodeAnalysis;

// Define your item and separator parsers
Parser<char, Expression> expressionParser = /* your expression parser */;
Parser<char, char> commaParser = Parser.Char(',');

// Create a separated list parser
Parser<char, SeparatedSyntaxList<Expression, char>> argumentListParser =
    expressionParser.SeparatedList(commaParser);

// Parse input
var result = argumentListParser.Parse("a, b, c");
SeparatedSyntaxList<Expression, char> arguments = result.Value;

// Access items and separators
foreach (var item in arguments.Items)
{
    // Process each expression
}
```

### Parse block structures with UntilWithLeading

```csharp
using Pidgin;
using War3Net.CodeAnalysis;

// Parse items between open and close tokens (e.g., loop/endloop)
Parser<char, LoopStatement> loopParser = statementParser.UntilWithLeading(
    triviaParser,           // Parser for leading whitespace/comments
    loopKeywordParser,      // Open token parser
    endLoopKeywordParser,   // Close token parser
    (trivia, stmt) => stmt.WithLeadingTrivia(trivia),
    (loopToken, statements, trivia, endLoopToken) =>
        new LoopStatement(loopToken, statements, trivia, endLoopToken));
```

### Build a separated syntax list manually

```csharp
using War3Net.CodeAnalysis;

// Create a builder with the first item
var builder = SeparatedSyntaxList<string, char>.CreateBuilder("first");

// Add more items with separators
builder.Add(',', "second");
builder.Add(',', "third");

// Build the immutable list
SeparatedSyntaxList<string, char> list = builder.ToSeparatedSyntaxList();
```

### Generate indented source code

```csharp
using System.IO;
using War3Net.CodeAnalysis;

using var stringWriter = new StringWriter();
using var writer = new IndentedTextWriter(stringWriter);

writer.WriteLine("function Main()");
writer.Indent();
writer.WriteLine("local x = 1");
writer.WriteLine("call DoSomething(x)");
writer.Unindent();
writer.WriteLine("endfunction");

string output = stringWriter.ToString();
// Output:
// function Main()
//     local x = 1
//     call DoSomething(x)
// endfunction
```

> **Note:** For rendering JASS code, avoid writing strings manually as shown above. Instead, use `War3Net.CodeAnalysis.Jass.Extensions.IndentedTextWriterExtensions` which provides convenience extension methods that handle indentation for you automatically. See the [War3Net.CodeAnalysis.Jass](https://www.nuget.org/packages/War3Net.CodeAnalysis.Jass) package for details.

## Main Types

The main types provided by this library are:

* `War3Net.CodeAnalysis.ParserExtensions` - Extension methods for Pidgin parsers (SeparatedList, IfThenElse, UntilWithLeading)
* `War3Net.CodeAnalysis.SeparatedSyntaxList<TItem, TSeparator>` - Immutable list of items with separators between them
* `War3Net.CodeAnalysis.SeparatedSyntaxList<TItem, TSeparator>.Builder` - Builder for incrementally constructing separated syntax lists
* `War3Net.CodeAnalysis.IndentedTextWriter` - TextWriter that automatically handles indentation

## Related Packages

* [War3Net.CodeAnalysis.Jass](https://www.nuget.org/packages/War3Net.CodeAnalysis.Jass) - Parse and render JASS source files
* [War3Net.CodeAnalysis.Transpilers](https://www.nuget.org/packages/War3Net.CodeAnalysis.Transpilers) - Transpile JASS to C# or Lua
* [War3Net.Build](https://www.nuget.org/packages/War3Net.Build) - Generate JASS map scripts and compile maps

## Feedback and contributing

War3Net.CodeAnalysis is released as open source under the [MIT license](https://github.com/Drake53/War3Net/blob/master/LICENSE). Bug reports and contributions are welcome at [the GitHub repository](https://github.com/Drake53/War3Net).

* [File an issue](https://github.com/Drake53/War3Net/issues)
* [Submit a pull request](https://github.com/Drake53/War3Net/pulls)

## Disclaimer

This README was generated with the assistance of AI and may contain inaccuracies. Please verify the information and consult the source code for authoritative details.
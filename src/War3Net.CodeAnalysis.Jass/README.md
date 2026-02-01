# War3Net.CodeAnalysis.Jass

## About

War3Net.CodeAnalysis.Jass is a .NET library for parsing, analyzing, and generating JASS (Just Another Scripting Syntax) source code. JASS is the scripting language used in Warcraft III for game logic and triggers. It is part of the [War3Net](https://github.com/Drake53/War3Net) modding library.

## Key features

* Parse JASS source code into a complete immutable syntax tree
* Generate JASS source code from syntax nodes with formatting control
* Syntax tree traversal and analysis with child/descendant node enumeration
* Code transformation via the syntax rewriter pattern
* Rename identifiers across a compilation unit with scope awareness
* Normalize and pretty-print JASS code with configurable indentation
* Support for all JASS language constructs including FourCC literals
* Fluent builder API for constructing syntax trees programmatically

## How to Use

### Parse JASS source code

```csharp
using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Syntax;

// Parse a complete JASS file
string jassCode = @"
function HelloWorld takes nothing returns nothing
    call BJDebugMsg(""Hello, World!"")
endfunction
";

JassCompilationUnitSyntax compilationUnit = JassSyntaxFactory.ParseCompilationUnit(jassCode);

// Iterate over declarations
foreach (var declaration in compilationUnit.Declarations)
{
    if (declaration is JassFunctionDeclarationSyntax function)
    {
        string functionName = function.FunctionDeclarator.IdentifierName.Name;
        Console.WriteLine($"Found function: {functionName}");
    }
}
```

### Parse with error handling

```csharp
using War3Net.CodeAnalysis.Jass;

string expression = "x + y * 2";

if (JassSyntaxFactory.TryParseExpression(expression, out var result))
{
    Console.WriteLine("Parsed successfully!");
}
else
{
    Console.WriteLine("Parse failed.");
}
```

### Generate JASS source code

```csharp
using System.IO;
using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Syntax;

// Parse and regenerate code
var unit = JassSyntaxFactory.ParseCompilationUnit(jassCode);

// Write to a TextWriter
using var writer = new StringWriter();
unit.WriteTo(writer);
string output = writer.ToString();
```

### Generate JASS code with IndentedTextWriter

```csharp
using System.IO;
using War3Net.CodeAnalysis;
using War3Net.CodeAnalysis.Jass.Extensions;

// Create a writer for generating formatted JASS code
using var stringWriter = new StringWriter();
using var writer = new IndentedTextWriter(stringWriter);

// Write a function with local variables and control flow
writer.WriteFunction("InitTrigger");
writer.WriteLocal("trigger", "t", "CreateTrigger()");
writer.WriteLocal("integer", "i");
writer.WriteCall("TriggerRegisterTimerEvent", "t", "1.0", "true");
writer.WriteSet("i", "0");

writer.WriteLoop();
writer.WriteExitWhen("i >= 10");
writer.WriteCall("BJDebugMsg", "I2S(i)");
writer.WriteSet("i", "i + 1");
writer.WriteEndLoop();

writer.WriteIf("i == 10");
writer.WriteCall("BJDebugMsg", "\"Done!\"");
writer.WriteElse();
writer.WriteCall("BJDebugMsg", "\"Error\"");
writer.WriteEndIf();

writer.EndFunction();

string jassCode = stringWriter.ToString();
// Output:
// function InitTrigger takes nothing returns nothing
//     local trigger t = CreateTrigger()
//     local integer i
//     call TriggerRegisterTimerEvent( t, 1.0, true )
//     set i = 0
//     loop
//         exitwhen i >= 10
//         call BJDebugMsg( I2S(i) )
//         set i = i + 1
//     endloop
//     if i == 10 then
//         call BJDebugMsg( "Done!" )
//     else
//         call BJDebugMsg( "Error" )
//     endif
// endfunction
```

### Build expressions programmatically

```csharp
using War3Net.CodeAnalysis.Jass;

// Create literal expressions
var intLiteral = JassLiteral.Int(42);
var realLiteral = JassLiteral.Real(3.14f);
var stringLiteral = JassLiteral.String("Hello");
var fourccLiteral = JassLiteral.FourCC("hpea"); // Warcraft 3 unit rawcode

// Create compound expressions
var andExpr = JassExpression.And("conditionA", "conditionB");
var notExpr = JassExpression.Not("isEnabled");
```

### Rename identifiers

```csharp
using War3Net.CodeAnalysis.Jass;

var functionRenames = new Dictionary<string, string>
{
    { "OldFunctionName", "NewFunctionName" }
};

var globalRenames = new Dictionary<string, string>
{
    { "udg_OldVariable", "udg_NewVariable" }
};

var renamer = new JassRenamer(functionRenames, globalRenames);
var renamedUnit = renamer.Rename(compilationUnit);
```

## Main Types

The main types provided by this library are:

* `War3Net.CodeAnalysis.Jass.JassSyntaxFactory` - Parse JASS source strings into syntax trees
* `War3Net.CodeAnalysis.Jass.JassRenamer` - Rename function and variable identifiers with scope tracking
* `War3Net.CodeAnalysis.Jass.JassSyntaxFacts` - Character and identifier validation utilities
* `War3Net.CodeAnalysis.Jass.JassLiteral` - Create literal value expressions (int, real, string, FourCC)
* `War3Net.CodeAnalysis.Jass.JassExpression` - Helper methods for creating compound expressions
* `War3Net.CodeAnalysis.Jass.Syntax.JassCompilationUnitSyntax` - Root syntax node containing all declarations
* `War3Net.CodeAnalysis.Jass.Syntax.JassFunctionDeclarationSyntax` - Function declaration syntax node
* `War3Net.CodeAnalysis.Jass.Syntax.JassGlobalsDeclarationSyntax` - Global variable block syntax node
* `War3Net.CodeAnalysis.Jass.Syntax.JassSyntaxNode` - Abstract base class for all syntax nodes

## Related Packages

* [War3Net.Build](https://www.nuget.org/packages/War3Net.Build) - Generate JASS map scripts and compile maps
* [War3Net.CodeAnalysis.Transpilers](https://www.nuget.org/packages/War3Net.CodeAnalysis.Transpilers) - Transpile JASS to C# or Lua
* [War3Net.Build.Core](https://www.nuget.org/packages/War3Net.Build.Core) - Parsers and serializers for war3map files

## Feedback and contributing

War3Net.CodeAnalysis.Jass is released as open source under the [MIT license](https://github.com/Drake53/War3Net/blob/master/LICENSE). Bug reports and contributions are welcome at [the GitHub repository](https://github.com/Drake53/War3Net).

* [File an issue](https://github.com/Drake53/War3Net/issues)
* [Submit a pull request](https://github.com/Drake53/War3Net/pulls)

## Disclaimer

This README was generated with the assistance of AI and may contain inaccuracies. Please verify the information and consult the source code for authoritative details.
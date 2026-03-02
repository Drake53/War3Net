namespace War3Net.CodeAnalysis.Jass.Extensions
{
    public static class IndentedTextWriterExtensions
    {
        public static void WriteComment(this IndentedTextWriter writer, string comment)
        {
            writer.Write("// ");
            writer.WriteLine(comment);
        }

        public static void WriteFunction(this IndentedTextWriter writer, string functionName)
        {
            writer.Write(JassKeyword.Function);
            writer.Write(' ');
            writer.Write(functionName);
            writer.Write(' ');
            writer.Write(JassKeyword.Takes);
            writer.Write(' ');
            writer.Write(JassKeyword.Nothing);
            writer.Write(' ');
            writer.Write(JassKeyword.Returns);
            writer.Write(' ');
            writer.Write(JassKeyword.Nothing);
            writer.WriteLine();
            writer.Indent();
        }

        public static void WriteFilterFunction(this IndentedTextWriter writer, string functionName)
        {
            writer.Write(JassKeyword.Function);
            writer.Write(' ');
            writer.Write(functionName);
            writer.Write(' ');
            writer.Write(JassKeyword.Takes);
            writer.Write(' ');
            writer.Write(JassKeyword.Nothing);
            writer.Write(' ');
            writer.Write(JassKeyword.Returns);
            writer.Write(' ');
            writer.Write(JassKeyword.Boolean);
            writer.WriteLine();
            writer.Indent();
        }

        public static void EndFunction(this IndentedTextWriter writer)
        {
            writer.Unindent();
            writer.WriteLine(JassKeyword.EndFunction);
        }

        public static void WriteLocal(this IndentedTextWriter writer, string typeName, string variableName)
        {
            writer.Write(JassKeyword.Local);
            writer.Write(' ');
            writer.Write(typeName);
            writer.Write(' ');
            writer.WriteLine(variableName);
        }

        public static void WriteLocal(this IndentedTextWriter writer, string typeName, string variableName, string initialValue)
        {
            writer.Write(JassKeyword.Local);
            writer.Write(' ');
            writer.Write(typeName);
            writer.Write(' ');
            writer.Write(variableName);
            writer.Write(" = ");
            writer.WriteLine(initialValue);
        }

        public static void WriteAlignedLocal(
            this IndentedTextWriter writer,
            string typeName,
            string variableName,
            string initialValue,
            int typeColumnWidth = 8,
            int variableNameColumnWidth = 11)
        {
            writer.Write(JassKeyword.Local);
            writer.Write(' ');
            writer.Write(typeName);
            var typePadding = typeColumnWidth - typeName.Length;
            if (typePadding > 0)
            {
                writer.Write(new string(' ', typePadding));
            }
            else
            {
                writer.Write(' ');
            }

            writer.Write(variableName);

            var namePadding = variableNameColumnWidth - variableName.Length;
            if (namePadding > 0)
            {
                writer.Write(new string(' ', namePadding));
            }
            else
            {
                writer.Write(' ');
            }

            writer.Write("= ");
            writer.WriteLine(initialValue);
        }

        public static void WriteAlignedGlobal(
            this IndentedTextWriter writer,
            string typeName,
            string variableName,
            int typeColumnWidth = 24)
        {
            writer.Write(typeName);
            var typePadding = typeColumnWidth - typeName.Length;
            if (typePadding > 0)
            {
                writer.Write(new string(' ', typePadding));
            }
            else
            {
                writer.Write(' ');
            }

            writer.WriteLine(variableName);
        }

        public static void WriteAlignedGlobal(
            this IndentedTextWriter writer,
            string typeName,
            string variableName,
            string value,
            int typeColumnWidth = 24,
            int variableNameColumnWidth = 27)
        {
            writer.Write(typeName);
            var typePadding = typeColumnWidth - typeName.Length;
            if (typePadding > 0)
            {
                writer.Write(new string(' ', typePadding));
            }
            else
            {
                writer.Write(' ');
            }

            writer.Write(variableName);

            var namePadding = variableNameColumnWidth - variableName.Length;
            if (namePadding > 0)
            {
                writer.Write(new string(' ', namePadding));
            }
            else
            {
                writer.Write(' ');
            }

            writer.Write("= ");
            writer.WriteLine(value);
        }

        public static void WriteCall(this IndentedTextWriter writer, string functionName)
        {
            writer.Write(JassKeyword.Call);
            writer.Write(' ');
            writer.Write(functionName);
            writer.Write("(  )");
            writer.WriteLine();
        }

        public static void WriteCall(this IndentedTextWriter writer, string functionName, params string[] arguments)
        {
            writer.Write(JassKeyword.Call);
            writer.Write(' ');
            writer.Write(functionName);
            writer.Write("( ");
            for (var i = 0; i < arguments.Length; i++)
            {
                if (i > 0)
                {
                    writer.Write(", ");
                }

                writer.Write(arguments[i]);
            }

            writer.Write(" )");
            writer.WriteLine();
        }

        public static void WriteCallCompact(this IndentedTextWriter writer, string functionName, params string[] arguments)
        {
            writer.Write(JassKeyword.Call);
            writer.Write(' ');
            writer.Write(functionName);
            writer.Write('(');
            for (var i = 0; i < arguments.Length; i++)
            {
                if (i > 0)
                {
                    writer.Write(", ");
                }

                writer.Write(arguments[i]);
            }

            writer.Write(')');
            writer.WriteLine();
        }

        public static void WriteSet(this IndentedTextWriter writer, string variableName, string valueExpression)
        {
            writer.Write(JassKeyword.Set);
            writer.Write(' ');
            writer.Write(variableName);
            writer.Write(" = ");
            writer.WriteLine(valueExpression);
        }

        public static void WriteIf(this IndentedTextWriter writer, string condition)
        {
            writer.Write(JassKeyword.If);
            writer.Write(' ');
            writer.Write(condition);
            writer.Write(' ');
            writer.Write(JassKeyword.Then);
            writer.WriteLine();
            writer.Indent();
        }

        public static void WriteElseIf(this IndentedTextWriter writer, string condition)
        {
            writer.Unindent();
            writer.Write(JassKeyword.ElseIf);
            writer.Write(' ');
            writer.Write(condition);
            writer.Write(' ');
            writer.Write(JassKeyword.Then);
            writer.WriteLine();
            writer.Indent();
        }

        public static void WriteElse(this IndentedTextWriter writer)
        {
            writer.Unindent();
            writer.WriteLine(JassKeyword.Else);
            writer.Indent();
        }

        public static void WriteEndIf(this IndentedTextWriter writer)
        {
            writer.Unindent();
            writer.Write(JassKeyword.EndIf);
            writer.WriteLine();
        }

        public static void WriteLoop(this IndentedTextWriter writer)
        {
            writer.WriteLine(JassKeyword.Loop);
            writer.Indent();
        }

        public static void WriteExitWhen(this IndentedTextWriter writer, string condition)
        {
            writer.Write(JassKeyword.ExitWhen);
            writer.Write(' ');
            writer.WriteLine(condition);
        }

        public static void WriteEndLoop(this IndentedTextWriter writer)
        {
            writer.Unindent();
            writer.WriteLine(JassKeyword.EndLoop);
        }

        public static void WriteReturn(this IndentedTextWriter writer)
        {
            writer.WriteLine(JassKeyword.Return);
        }

        public static void WriteReturn(this IndentedTextWriter writer, string expression)
        {
            writer.Write(JassKeyword.Return);
            writer.Write(' ');
            writer.WriteLine(expression);
        }
    }
}
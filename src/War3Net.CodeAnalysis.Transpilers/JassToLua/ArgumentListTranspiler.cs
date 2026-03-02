namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public IEnumerable<LuaExpressionSyntax> Transpile(JassArgumentListSyntax argumentList)
        {
            return argumentList.Arguments.Items.Select(argument => Transpile(argument, out _));
        }
    }
}
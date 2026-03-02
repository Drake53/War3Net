using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenderer
    {
        public void Render(JassParameterListSyntax parameterList)
        {
            Render(parameterList.TakesToken);
            WriteSpace();

            Render(parameterList.ParameterList.Items[0]);
            for (var i = 1; i < parameterList.ParameterList.Items.Length; i++)
            {
                Render(parameterList.ParameterList.Separators[i - 1]);
                WriteSpace();
                Render(parameterList.ParameterList.Items[i]);
            }
        }
    }
}
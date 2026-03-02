using System;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenderer
    {
        public void Render(JassParameterListOrEmptyParameterListSyntax parameterListOrEmptyParameterList)
        {
            switch (parameterListOrEmptyParameterList)
            {
                case JassParameterListSyntax parameterList: Render(parameterList); break;
                case JassEmptyParameterListSyntax emptyParameterList: Render(emptyParameterList); break;

                default: throw new NotSupportedException();
            }
        }
    }
}
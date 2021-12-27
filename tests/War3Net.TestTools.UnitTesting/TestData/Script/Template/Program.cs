using static War3Api.Common;

namespace War3Net.Build.Tests.TestData.Script.Template
{
    internal static class Program
    {
        private static void Main()
        {
            DisplayTextToPlayer(GetLocalPlayer(), 0, 0, "Hello World!");
        }
    }
}
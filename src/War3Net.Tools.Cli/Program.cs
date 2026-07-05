namespace War3Net.Tools.Cli
{
    internal static class Program
    {
        private static Task<int> Main(string[] args)
        {
            var rootCommand = new RootCommand("War3Net command-line tools.");
            rootCommand.Subcommands.Add(MpqCommand.Build());

            return rootCommand.Parse(args).InvokeAsync();
        }
    }
}
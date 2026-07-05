namespace War3Net.Tools.Cli.Mpq
{
    public static partial class MpqCommand
    {
        public static Command Build()
        {
            var command = new Command("mpq", "List and extract files from MPQ archives.");
            command.Subcommands.Add(BuildListCommand());
            command.Subcommands.Add(BuildExtractCommand());
            command.Subcommands.Add(BuildExtractAllCommand());

            return command;
        }
    }
}
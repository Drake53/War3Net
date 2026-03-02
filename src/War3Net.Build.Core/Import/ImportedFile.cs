namespace War3Net.Build.Import
{
    public sealed partial class ImportedFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImportedFile"/> class.
        /// </summary>
        public ImportedFile()
        {
        }

        public ImportedFileFlags Flags { get; set; }

        public string FullPath { get; set; }

        public override string ToString() => FullPath;
    }
}
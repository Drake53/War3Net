using System;
using System.Collections.Generic;
using War3Net.IO.Mpq;

namespace War3Net.Build
{
    public sealed class ArchiveBuildingEventArgs : EventArgs
    {
        public ArchiveBuildingEventArgs(ICollection<MpqFile> mpqFiles)
        {
            MpqFiles = mpqFiles;
        }

        public ICollection<MpqFile> MpqFiles { get; set; }
    }
}
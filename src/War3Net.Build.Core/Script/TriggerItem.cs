// ------------------------------------------------------------------------------
// <copyright file="TriggerItem.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

namespace War3Net.Build.Script
{
    public abstract class TriggerItem
    {
        private readonly TriggerItemType _type;

        internal TriggerItem(TriggerItemType type)
        {
            _type = type;
        }

        public TriggerItemType ItemType => _type;

        public abstract string Name { get; set; }

        public abstract int Id { get; set; }

        public abstract int ParentId { get; set; }

        public abstract void WriteTo(BinaryWriter writer, MapTriggersFormatVersion formatVersion, bool useNewFormat);

        public override string ToString() => Name;
    }
}
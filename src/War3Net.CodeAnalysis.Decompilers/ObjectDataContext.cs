using System.Collections.Immutable;

using War3Net.Build;

namespace War3Net.CodeAnalysis.Decompilers
{
    internal class ObjectDataContext
    {
        public ObjectDataContext(Map map, Campaign? campaign)
        {
            // TODO
        }

        public ImmutableHashSet<int> KnownUnitIds { get; set; }

        public ImmutableHashSet<int> KnownItemIds { get; set; }

        public ImmutableHashSet<int> KnownDestructableIds { get; set; }

        public ImmutableHashSet<int> KnownDoodadIds { get; set; }

        public ImmutableHashSet<int> KnownAbilityIds { get; set; }

        public ImmutableHashSet<int> KnownBuffIds { get; set; }

        public ImmutableHashSet<int> KnownUpgradeIds { get; set; }

        public ImmutableHashSet<int> KnownTechIds { get; set; }

        public ImmutableHashSet<int> KnownObjectIds { get; set; }
    }
}
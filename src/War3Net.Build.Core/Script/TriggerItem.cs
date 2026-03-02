namespace War3Net.Build.Script
{
    public abstract partial class TriggerItem
    {
        internal TriggerItem(TriggerItemType triggerItemType)
        {
            Type = triggerItemType;
        }

        public TriggerItemType Type { get; private init; }

        public string Name { get; set; }

        public int Id { get; set; }

        public int ParentId { get; set; }

        public override string ToString() => Name;
    }
}
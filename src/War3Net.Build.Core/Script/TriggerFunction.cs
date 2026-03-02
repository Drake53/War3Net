namespace War3Net.Build.Script
{
    public sealed partial class TriggerFunction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TriggerFunction"/> class.
        /// </summary>
        public TriggerFunction()
        {
        }

        public TriggerFunctionType Type { get; set; }

        public int? Branch { get; set; }

        public string Name { get; set; }

        public bool IsEnabled { get; set; }

        public List<TriggerFunctionParameter> Parameters { get; init; } = new();

        public List<TriggerFunction> ChildFunctions { get; init; } = new();

        public override string ToString()
        {
            return $"{Name}({string.Join(", ", Parameters)})";
        }
    }
}
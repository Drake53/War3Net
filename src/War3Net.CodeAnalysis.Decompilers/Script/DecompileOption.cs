using War3Net.Build.Script;

namespace War3Net.CodeAnalysis.Decompilers
{
    internal sealed class DecompileOption
    {
        public string Type { get; set; }

        public TriggerFunctionParameter Parameter { get; set; }

        public override string ToString() => $"{Type}: {Parameter}";
    }
}
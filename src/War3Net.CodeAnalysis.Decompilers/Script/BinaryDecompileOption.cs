namespace War3Net.CodeAnalysis.Decompilers
{
    internal sealed class BinaryDecompileOption
    {
        public string Type { get; set; }

        public TriggerFunctionParameter LeftParameter { get; set; }

        public TriggerFunctionParameter RightParameter { get; set; }

        public override string ToString() => $"{Type}: [{LeftParameter}] [{RightParameter}]";
    }
}
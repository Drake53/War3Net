using System;

namespace War3Net.Modeling.Enums
{
    [Flags]
    public enum MaterialRenderMode
    {
        ConstantColor = 1,
        SortPrismFarZ = 16,
        FullResolution = 32,
    }
}
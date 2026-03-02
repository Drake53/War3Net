using System;

namespace War3Net.Modeling.Enums
{
    [Flags]
    public enum LayerShading
    {
        Unshaded = 1,
        SphereEnvMap = 2,
        TwoSided = 16,
        Unfogged = 32,
        NoDepthTest = 64,
        NoDepthSet = 128,
    }
}
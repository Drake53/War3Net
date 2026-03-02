using War3Net.Build.Environment;

namespace War3Net.Build.Extensions
{
    public static class CameraExtensions
    {
        public static string GetVariableName(this Camera camera)
        {
            return $"gg_cam_{camera.Name.Replace(' ', '_')}";
        }
    }
}
using System;
using War3Net.Build.Extensions;
using War3Net.CodeAnalysis;
using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Extensions;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected internal virtual void GenerateCameraVariables(Map map, IndentedTextWriter writer)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            var mapCameras = map.Cameras;
            if (mapCameras is null)
            {
                return;
            }

            foreach (var camera in mapCameras.Cameras)
            {
                writer.WriteAlignedGlobal(
                    TypeName.CameraSetup,
                    camera.GetVariableName(),
                    JassKeyword.Null);
            }
        }

        protected internal virtual bool ShouldGenerateCameraVariables(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return map.Cameras is not null
                && map.Cameras.Cameras.Count > 0;
        }
    }
}
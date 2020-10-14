// ------------------------------------------------------------------------------
// <copyright file="GraphicsProvider.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;

using Veldrid;

namespace War3Net.Rendering
{
    public static class GraphicsProvider
    {
        public static GraphicsDevice GraphicsDevice { get; set; }

        public static ResourceFactory ResourceFactory { get; set; }

        public static DeviceBuffer WorldBuffer { get; set; }

        public static DeviceBuffer ProjectionBuffer { get; set; }

        public static DeviceBuffer ViewBuffer { get; set; }

        public static DeviceBuffer FullResolutionWorldBuffer { get; set; }

        public static DeviceBuffer FullResolutionProjectionBuffer { get; set; }

        public static DeviceBuffer FullResolutionViewBuffer { get; set; }

        public static DeviceBuffer UIWorldBuffer { get; set; }

        public static DeviceBuffer UIProjectionBuffer { get; set; }

        public static DeviceBuffer UIViewBuffer { get; set; }

        public static Func<string, Stream> Path2ModelStream { get; set; }

        public static Func<string, Stream> Path2TextureStream { get; set; }
    }
}
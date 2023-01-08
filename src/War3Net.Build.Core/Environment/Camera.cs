// ------------------------------------------------------------------------------
// <copyright file="Camera.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Numerics;

namespace War3Net.Build.Environment
{
    public sealed partial class Camera
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Camera"/> class.
        /// </summary>
        public Camera()
        {
        }

        public Vector2 TargetPosition { get; set; }

        public float ZOffset { get; set; }

        public float Rotation { get; set; }

        public float AngleOfAttack { get; set; }

        public float TargetDistance { get; set; }

        public float Roll { get; set; }

        public float FieldOfView { get; set; }

        public float FarClippingPlane { get; set; }

        public float NearClippingPlane { get; set; }

        public float LocalPitch { get; set; }

        public float LocalYaw { get; set; }

        public float LocalRoll { get; set; }

        public string Name { get; set; }

        public override string ToString() => Name;
    }
}
// ------------------------------------------------------------------------------
// <copyright file="AnimationChannel.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.Modeling.Enums;

namespace War3Net.Modeling.DataStructures
{
    public struct AnimationChannel<T>
        where T : struct
    {
        public InterpolationType InterpolationType { get; set; }

        public uint GlobalSequenceId { get; set; }

        public Key[] Keys { get; set; }

        public struct Key
        {
            public int Frame { get; set; }

            public T Value { get; set; }

            public T TanIn { get; set; }

            public T TanOut { get; set; }
        }
    }
}
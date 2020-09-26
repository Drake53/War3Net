// ------------------------------------------------------------------------------
// <copyright file="IVirtualMachine.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Reflection;

namespace War3Net.Runtime
{
    public interface IVirtualMachine
    {
        public void InjectField(FieldInfo fieldInfo);

        public void InjectMethod(MethodInfo methodInfo);

        public void Start();
    }
}
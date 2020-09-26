// ------------------------------------------------------------------------------
// <copyright file="VirtualMachineExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Reflection;

using War3Net.Build.Common;

namespace War3Net.Runtime.Api.Common
{
    public static class VirtualMachineExtensions
    {
        public static void InjectCommonApi(this IVirtualMachine virtualMachine, GamePatch gamePatch)
        {
            foreach (var type in typeof(VirtualMachineExtensions).Assembly.GetTypes())
            {
                if (type.Name.EndsWith("Api", StringComparison.Ordinal))
                {
                    virtualMachine.InjectMembers(gamePatch, type);
                }
            }
        }

        public static void InjectMembers(this IVirtualMachine virtualMachine, GamePatch gamePatch, Type type)
        {
            if (((AvailableSinceAttribute)Attribute.GetCustomAttribute(type, typeof(AvailableSinceAttribute)))?.AvailableSince > gamePatch)
            {
                return;
            }

            foreach (var member in type.GetMembers())
            {
                if (((AvailableSinceAttribute)Attribute.GetCustomAttribute(member, typeof(AvailableSinceAttribute)))?.AvailableSince > gamePatch)
                {
                    continue;
                }

                if (member.DeclaringType == type)
                {
                    if (member is FieldInfo field)
                    {
                        virtualMachine.InjectField(field);
                    }
                    else if (member is MethodInfo method)
                    {
                        virtualMachine.InjectMethod(method);
                    }
                }
            }
        }
    }
}
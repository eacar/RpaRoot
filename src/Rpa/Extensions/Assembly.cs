using System;
using System.Collections.Generic;

namespace Rpa.Extensions
{
    public static class AssemblyExtensions
    {
        public static IEnumerable<Type> GetFromAllAssemblies(this Type subclassType)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var aType in assembly.GetTypes())
                {
                    if (aType.IsClass && !aType.IsAbstract && (subclassType == null || aType.IsSubclassOf(subclassType)))
                        yield return aType;
                }
            }
        }
    }
}
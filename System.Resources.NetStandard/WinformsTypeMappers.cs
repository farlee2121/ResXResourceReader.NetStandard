using System;
using System.Collections.Generic;
using System.Text;

namespace System.Resources.NetStandard
{
    internal static class WinformsTypeMappers
    {
        public static Func<Type, string> InterceptWinformsTypes(Func<Type,string> typeNameConverter)
        {
            return (Type type) =>
            {
                if (type.AssemblyQualifiedName == typeof(ResXFileRef).AssemblyQualifiedName)
                {
                    return NetStandard.ResXConstants.ResxFileRefTypeInfo;
                }
                else
                {
                    if (typeNameConverter != null) return typeNameConverter(type);
                    else return null;
                }
            };
        }
        
    }
}

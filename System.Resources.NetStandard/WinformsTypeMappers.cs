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
                    return ResXConstants.ResxFileRefTypeInfo;
                }
                else if (type.AssemblyQualifiedName == typeof(ResXNullRef).AssemblyQualifiedName)
                {
                    return ResXConstants.ResxNullRefTypeInfo;
                }
                else if (typeNameConverter != null) return typeNameConverter(type);
                else return null;
            };
        }
        
    }
}

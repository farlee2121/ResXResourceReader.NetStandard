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
                
                if (type.AssemblyQualifiedName == typeof(ResXNullRef).AssemblyQualifiedName)
                {
                    return NetStandard.ResXConstants.ResxNullRefTypeInfo;
                }
                
                
                if (typeNameConverter != null) 
                    return typeNameConverter(type);
                
                return null;
            };
        }
        
    }
}

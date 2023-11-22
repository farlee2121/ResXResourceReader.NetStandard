namespace System.Resources.NetStandard{
    internal class ResXConstants
    {
        public const string ResHeaderReader = "System.Resources.ResXResourceReader, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
        public const string ResHeaderWriter = "System.Resources.ResXResourceWriter, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";

        public static string ResHeaderReaderTypeName => ResHeaderReader.Split(',')[0].Trim();
        public static string ResHeaderWriterTypeName => ResHeaderWriter.Split(',')[0].Trim();


        public const string ResxFileRefTypeInfo = "System.Resources.ResXFileRef, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
        public const string ResxFileRef_TypeNameAndAssembly = "System.Resources.ResXFileRef, System.Windows.Forms";
    }
}
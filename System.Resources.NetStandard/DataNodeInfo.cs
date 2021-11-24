using System.Drawing;

namespace System.Resources.NetStandard
{
    internal class DataNodeInfo
    {
        internal string Name;
        internal string Comment;
        internal string TypeName;
        internal string MimeType;
        internal string ValueData;
        internal Point  ReaderPosition; //only used to track position in the reader

        internal DataNodeInfo Clone()
        {
            return new DataNodeInfo
            {
                Name           = Name,
                Comment        = Comment,
                TypeName       = TypeName,
                MimeType       = MimeType,
                ValueData      = ValueData,
                ReaderPosition = new Point(ReaderPosition.X, ReaderPosition.Y)
            };
        }
    }
}

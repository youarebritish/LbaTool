using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace LbaTool
{
    [DebuggerDisplay("x = {X}, y = {Y}, z = {Z}, w = {W}")]
    public class Vector4 : IXmlSerializable
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float W { get; set; }
        
        public virtual void Read(BinaryReader reader)
        {
            X = reader.ReadSingle();
            Y = reader.ReadSingle();
            Z = reader.ReadSingle();
            W = reader.ReadSingle();
        }

        public virtual void Write(BinaryWriter writer)
        {
            writer.Write(X);
            writer.Write(Y);
            writer.Write(Z);
            writer.Write(W);
        }

        public virtual void ReadXml(XmlReader reader)
        {
            X = float.Parse(reader["x"]);
            Y = float.Parse(reader["y"]);
            Z = float.Parse(reader["z"]);
            W = float.Parse(reader["w"]);
        }

        public virtual void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("x", X.ToString(CultureInfo.InvariantCulture));
            writer.WriteAttributeString("y", Y.ToString(CultureInfo.InvariantCulture));
            writer.WriteAttributeString("z", Z.ToString(CultureInfo.InvariantCulture));
            writer.WriteAttributeString("w", W.ToString(CultureInfo.InvariantCulture));
        }

        public override string ToString()
        {
            return string.Format("X: {1}, Y: {2}, Z: {3}, W: {4}", X, Y, Z, W);
        }

        public XmlSchema GetSchema()
        {
            return null;
        }
    }
}

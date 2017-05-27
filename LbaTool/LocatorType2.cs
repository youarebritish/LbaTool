using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace LbaTool
{
    public class LocatorType2 : ILocator
    {
        public Vector4 Translation { get; set; }
        public Vector4 Rotation { get; set; }
        public FoxHash LocatorName { get; set; }
        public FoxHash DataSet { get; set; }

        public bool HasFooter => true;

        public void Read(BinaryReader reader, Dictionary<uint, string> hashLookupTable, HashIdentifiedDelegate hashIdentifiedCallback)
        {
            Translation = new Vector4();
            Translation.Read(reader);

            Rotation = new Vector4();
            Rotation.Read(reader);
        }

        public void Write(BinaryWriter writer)
        {
            Translation.Write(writer);
            Rotation.Write(writer);
        }

        public void ReadFooter(BinaryReader reader, Dictionary<uint, string> hashLookupTable, HashIdentifiedDelegate hashIdentifiedCallback)
        {
            LocatorName = new FoxHash();
            LocatorName.Read(reader, hashLookupTable, hashIdentifiedCallback);

            DataSet = new FoxHash();
            DataSet.Read(reader, hashLookupTable, hashIdentifiedCallback);
        }

        public void WriteFooter(BinaryWriter writer)
        {
            LocatorName.Write(writer);
            DataSet.Write(writer);
        }

        public void ReadXml(XmlReader reader)
        {
            LocatorName = new FoxHash();
            LocatorName.ReadXml(reader, "name");

            DataSet = new FoxHash();
            DataSet.ReadXml(reader, "dataSet");

            reader.ReadStartElement("locator");

            Translation = new Vector4();
            Translation.ReadXml(reader);
            reader.Read();
            
            Rotation = new Vector4();
            Rotation.ReadXml(reader);
            reader.Read();
        }

        public void WriteXml(XmlWriter writer)
        {
            LocatorName.WriteXml(writer, "name");
            DataSet.WriteXml(writer, "dataSet");

            writer.WriteStartElement("translation");
            Translation.WriteXml(writer);
            writer.WriteEndElement();

            writer.WriteStartElement("rotation");
            Rotation.WriteXml(writer);
            writer.WriteEndElement();
        }

        public XmlSchema GetSchema()
        {
            return null;
        }
    }
}

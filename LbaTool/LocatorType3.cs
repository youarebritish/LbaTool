using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace LbaTool
{
    public class LocatorType3 : ILocator
    {
        public Vector4 Translation { get; set; }
        public Vector4 Rotation { get; set; }
        public FoxHash LocatorName { get; set; }
        public FoxHash DataSet { get; set; }

        // Still not sure what these values are.
        public FoxHash Unknown30 { get; set; }
        public FoxHash Unknown31 { get; set; }
        public FoxHash Unknown32 { get; set; }
        public FoxHash Unknown33 { get; set; }

        public bool HasFooter => true;

        public void Read(BinaryReader reader, Dictionary<uint, string> hashLookupTable, HashIdentifiedDelegate hashIdentifiedCallback)
        {
            Translation = new Vector4();
            Translation.Read(reader);

            Rotation = new Vector4();
            Rotation.Read(reader);

            Unknown30 = new FoxHash();
            Unknown30.Read(reader, hashLookupTable, hashIdentifiedCallback);

            Unknown31 = new FoxHash();
            Unknown31.Read(reader, hashLookupTable, hashIdentifiedCallback);

            Unknown32 = new FoxHash();
            Unknown32.Read(reader, hashLookupTable, hashIdentifiedCallback);

            Unknown33 = new FoxHash();
            Unknown33.Read(reader, hashLookupTable, hashIdentifiedCallback);
        }

        public void Write(BinaryWriter writer)
        {
            Translation.Write(writer);
            Rotation.Write(writer);

            Unknown30.Write(writer);
            Unknown31.Write(writer);
            Unknown32.Write(writer);
            Unknown33.Write(writer);
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

            Unknown30 = new FoxHash();
            Unknown30.ReadXml(reader, "u30");

            Unknown31 = new FoxHash();
            Unknown31.ReadXml(reader, "u31");

            Unknown32 = new FoxHash();
            Unknown32.ReadXml(reader, "u32");

            Unknown33 = new FoxHash();
            Unknown33.ReadXml(reader, "u33");

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
            Unknown30.WriteXml(writer, "u30");
            Unknown31.WriteXml(writer, "u31");
            Unknown32.WriteXml(writer, "u32");
            Unknown33.WriteXml(writer, "u33");

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

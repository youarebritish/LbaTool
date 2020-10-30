﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LbaTool
{
    internal static class Program
    {
        private const string DefaultNameDictionaryFileName = "lba name dictionary.txt";
        private const string DefaultDataSetDictionaryFileName = "lba dataset dictionary.txt";
        private const string DefaultHashMatchOutputFileName = "lba hash matches.txt";

        private static void Main(string[] args)
        {
            var hashManager = new HashManager();

            // Read hash dictionaries
            if (File.Exists(DefaultNameDictionaryFileName))
            {
                hashManager.StrCode32LookupTable = MakeHashLookupTableFromFile(DefaultNameDictionaryFileName, FoxHash.Type.StrCode32);
                hashManager.PathCode32LookupTable = MakeHashLookupTableFromFile(DefaultDataSetDictionaryFileName, FoxHash.Type.PathCode32);
            }

            foreach (var lbaPath in args)
            {
                if (File.Exists(lbaPath))
                {
                    // Read input file
                    string fileExtension = Path.GetExtension(lbaPath);
                    if (fileExtension.Equals(".xml", StringComparison.OrdinalIgnoreCase))
                    {
                        LbaFile lba = ReadFromXml(lbaPath);
                        WriteToBinary(lba, Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(lbaPath)) + ".lba");
                    }
                    else if (fileExtension.Equals(".lba", StringComparison.OrdinalIgnoreCase))
                    {
                        LbaFile lba = ReadFromBinary(lbaPath, hashManager);
                        WriteToXml(lba, Path.GetFileNameWithoutExtension(lbaPath) + ".lba.xml");
                    }
                    else
                    {
                        throw new IOException("Unrecognized input type.");
                    }
                }
            }

            // Write hash matches output
            WriteHashMatchesToFile(DefaultHashMatchOutputFileName, hashManager);
        }

        public static void WriteToBinary(LbaFile lba, string path)
        {
            using (BinaryWriter writer = new BinaryWriter(new FileStream(path, FileMode.Create)))
            {
                lba.Write(writer);
            }
        }

        public static LbaFile ReadFromBinary(string path, HashManager hashManager)
        {
            LbaFile lba = new LbaFile();
            using (BinaryReader reader = new BinaryReader(new FileStream(path, FileMode.Open)))
            {
                lba.Read(reader, hashManager);
            }
            return lba;
        }

        public static void WriteToXml(LbaFile lba, string path)
        {
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings()
            {
                Encoding = Encoding.UTF8,
                Indent = true
            };
            using (var writer = XmlWriter.Create(path, xmlWriterSettings))
            {
                lba.WriteXml(writer);
            }
        }

        public static LbaFile ReadFromXml(string path)
        {
            XmlReaderSettings xmlReaderSettings = new XmlReaderSettings
            {
                IgnoreWhitespace = true
            };

            LbaFile lba = new LbaFile();
            using (var reader = XmlReader.Create(path, xmlReaderSettings))
            {
                lba.ReadXml(reader);
            }
            return lba;
        }

        /// <summary>
        /// Opens a file containing one string per line, hashes each string, and adds each pair to a lookup table.
        /// </summary>
        private static Dictionary<uint, string> MakeHashLookupTableFromFile(string path, FoxHash.Type hashType)
        {
            ConcurrentDictionary<uint, string> table = new ConcurrentDictionary<uint, string>();

            // Read file
            List<string> stringLiterals = new List<string>();
            using (StreamReader file = new StreamReader(path))
            {
                // TODO multi-thread
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    stringLiterals.Add(line);
                }
            }

            // Hash entries
            Parallel.ForEach(stringLiterals, (string entry) =>
            {
                if (hashType == FoxHash.Type.StrCode32)
                {
                    uint hash = HashManager.StrCode32(entry);
                    table.TryAdd(hash, entry);
                }
                else
                {
                    uint hash = HashManager.PathCode32(entry);
                    table.TryAdd(hash, entry);
                }
            });

            return new Dictionary<uint, string>(table);
        }

        /// <summary>
        /// Outputs all hash matched strings to a file.
        /// </summary>
        private static void WriteHashMatchesToFile(string path, HashManager hashManager)
        {
            using (StreamWriter file = new StreamWriter(path))
            {
                foreach (var entry in hashManager.UsedHashes)
                {
                    file.WriteLine(entry.Value);
                }
            }
        }
    }
}

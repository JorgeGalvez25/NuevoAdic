using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace AdicionalWeb.Code
{
    public class AdicionalUtils
    {
        public static JavaScriptSerializer JSSerializer = new JavaScriptSerializer();

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static List<int> Compress(string uncompressed)
        {
            // build the dictionary
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            for (int i = 0; i < 256; i++)
                dictionary.Add(((char)i).ToString(), i);

            string w = string.Empty;
            List<int> compressed = new List<int>();

            foreach (char c in uncompressed)
            {
                string wc = w + c;
                if (dictionary.ContainsKey(wc))
                {
                    w = wc;
                }
                else
                {
                    // write w to output
                    compressed.Add(dictionary[w]);
                    // wc is a new sequence; add it to the dictionary
                    dictionary.Add(wc, dictionary.Count);
                    w = c.ToString();
                }
            }

            // write remaining output if necessary
            if (!string.IsNullOrEmpty(w))
                compressed.Add(dictionary[w]);

            return compressed;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static string Decompress(List<int> compressed)
        {
            // build the dictionary
            Dictionary<int, string> dictionary = new Dictionary<int, string>();
            for (int i = 0; i < 256; i++)
                dictionary.Add(i, ((char)i).ToString());

            string w = dictionary[compressed[0]];
            compressed.RemoveAt(0);
            StringBuilder decompressed = new StringBuilder(w);

            foreach (int k in compressed)
            {
                string entry = null;
                if (dictionary.ContainsKey(k))
                    entry = dictionary[k];
                else if (k == dictionary.Count)
                    entry = w + w[0];

                decompressed.Append(entry);


                // new sequence; add it to the dictionary
                dictionary.Add(dictionary.Count, w + entry[0]);

                w = entry;
            }

            return decompressed.ToString();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static byte[] CompressToBytes(string uncompressed)
        {
            List<int> lstCompress = Compress(uncompressed);

            IFormatter formatter = new BinaryFormatter();
            MemoryStream strCompressed = new MemoryStream();
            formatter.Serialize(strCompressed, lstCompress);
            return strCompressed.ToArray();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static string DecompressFromBytes(byte[] compressed)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            formatter.Serialize(stream, compressed);
            stream.Seek(0, SeekOrigin.Begin);
            return Decompress((List<int>)formatter.Deserialize(stream));
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static string CombinePaths(params string[] path)
        {
            List<string> paths = new List<string>();

            if (path[0].StartsWith("//")) { paths.Add("file:"); }

            char[] splt = new char[] { '/' };
            for (int i = 0; i < path.Length; i++)
            {
                paths.AddRange(path[i].Split(splt, System.StringSplitOptions.RemoveEmptyEntries));
            }

            int count = 0;
            Regex reg = new Regex("^(http(s)?|ftp(s)?|file)", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);
            return paths.Aggregate((x, y) => { return x + ((count++ == 0 && reg.IsMatch(x)) ? "//" : "/") + y; });
        }


        private const int NumBytesPerCode = 2;

        static int ReadCode(BinaryReader reader)
        {
            int code = 0;
            int shift = 0;

            for (int i = 0; i < NumBytesPerCode; i++)
            {
                byte nextByte = reader.ReadByte();
                code += nextByte << shift;
                shift += 8;
            }

            return code;
        }

        static void WriteCode(BinaryWriter writer, int code)
        {
            int shift = 0;
            int mask = 0xFF;

            for (int i = 0; i < NumBytesPerCode; i++)
            {
                byte nextByte = (byte)((code >> shift) & mask);
                writer.Write(nextByte);
                shift += 8;
            }
        }

        static void Compress(StreamReader input, BinaryWriter output)
        {
            LzwStringTable table = new LzwStringTable(NumBytesPerCode);

            char firstChar = (char)input.Read();
            string match = firstChar.ToString();

            while (input.Peek() != -1)
            {
                char nextChar = (char)input.Read();
                string nextMatch = match + nextChar;

                if (table.Contains(nextMatch))
                {
                    match = nextMatch;
                }
                else
                {
                    WriteCode(output, table.GetCode(match));
                    table.AddCode(nextMatch);
                    match = nextChar.ToString();
                }
            }

            WriteCode(output, table.GetCode(match));
        }

        static void Decompress(BinaryReader input, StreamWriter output)
        {
            List<string> table = new List<string>();

            for (int i = 0; i < 256; i++)
            {
                char ch = (char)i;
                table.Add(ch.ToString());
            }

            int firstCode = ReadCode(input);
            char matchChar = (char)firstCode;
            string match = matchChar.ToString();

            output.Write(match);

            while (input.PeekChar() != -1)
            {
                int nextCode = ReadCode(input);

                string nextMatch;
                if (nextCode < table.Count)
                    nextMatch = table[nextCode];
                else
                    nextMatch = match + match[0];

                output.Write(nextMatch);

                table.Add(match + nextMatch[0]);
                match = nextMatch;
            }
        }

        private class LzwStringTable
        {
            public LzwStringTable(int numBytesPerCode)
            {
                maxCode = (1 << (8 * numBytesPerCode)) - 1;
            }

            public void AddCode(string s)
            {
                if (nextAvailableCode <= maxCode)
                {
                    if (s.Length != 1 && !table.ContainsKey(s))
                        table[s] = nextAvailableCode++;
                }
                else
                {
                    throw new Exception("LZW string table overflow");
                }
            }

            public int GetCode(string s)
            {
                if (s.Length == 1)
                    return (int)s[0];
                else
                    return table[s];
            }

            public bool Contains(string s)
            {
                return s.Length == 1 || table.ContainsKey(s);
            }

            private Dictionary<string, int> table = new Dictionary<string, int>();
            private int nextAvailableCode = 256;
            private int maxCode;
        }
    }
}

using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdicionalWeb.Code
{
    public class AdicionalUtils
    {
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

        public static byte[] CompressToBytes(string uncompressed)
        {
            List<int> lstCompress = Compress(uncompressed);

            IFormatter formatter = new BinaryFormatter();
            MemoryStream strCompressed = new MemoryStream();
            formatter.Serialize(strCompressed, lstCompress);
            return strCompressed.ToArray();
        }

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

        public static string DecompressFromBytes(byte[] compressed)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            formatter.Serialize(stream, compressed);
            stream.Seek(0, SeekOrigin.Begin);
            return Decompress((List<int>)formatter.Deserialize(stream));
        }

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
    }
}

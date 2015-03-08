using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MoreLinq;

namespace JapaneseCrossword.FileReaderUtils
{
    public class FileReader
    {
        private readonly TextReader fileReader;
        private readonly Queue<string> words; 
        public FileReader(string file)
        {
            this.fileReader = new StreamReader(file);
            this.words = new Queue<string>(ReadWords());
        }

        public int? ReadNextInt()
        {
            if (words.Count == 0)
                return null;

            int result;
            var word = words.Dequeue();
            if (!int.TryParse(word, out result))
                return null;
            return result;
        }

        public IEnumerable<int?> ReadNextInts(int count)
        {
            for (var i = 0; i < count; ++i)
            {
                yield return ReadNextInt();
            }
        } 

        public string ReadNextWord()
        {
            if (words.Count == 0)
                return null;

            return words.Dequeue();
        }

        private IEnumerable<string> ReadWords()
        {
            var buffer = new char[1024];
            while ((fileReader.ReadBlock(buffer, 0, 1024)) != 0)
            {
                var result =  buffer.ToList()
                                    .Split(Char.IsWhiteSpace)
                                    .Select(x => x.ToDelimitedString(""))
                                    .Where(s => s != "");
                
                foreach (var word in result)
                {
                    yield return word;
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace gyak1
{
    public class DocumentStatistics : IDocumentStatistics
    {
        private string _filePath;

        public string FileContent { get; set; }
        public Dictionary<string, int> DistinctWordCount { get; set; }
        public DocumentStatistics(string filePath)
        {
            _filePath = filePath;
            DistinctWordCount = new Dictionary<string, int>();
        }

        public void Load()
        {
            FileContent = File.ReadAllText(_filePath);
            DistinctWordCount.Clear();
            CountDistinctWords();
        }

        private void CountDistinctWords()
        {
            string[] words = FileContent.Split();

            words = words.Where(word => !string.IsNullOrWhiteSpace(word)).ToArray();

            for (int i = 0; i < words.Length; i++)
            {
                words[i] = string.Concat(
                words[i]
                    .SkipWhile(c => !char.IsLetter(c))
                    .Reverse()
                    .SkipWhile(c => !char.IsLetter(c))
                    .Reverse());
            }

            words = words.Where(word => !string.IsNullOrWhiteSpace(word)).ToArray();

            foreach (string word in words)
            {
                if (DistinctWordCount.ContainsKey(word))
                {
                    DistinctWordCount[word]++;
                }
                else
                {
                    DistinctWordCount[word] = 1;
                    //DistinctWordCount.Add(word, 1);
                }
            }
        }
    }
}

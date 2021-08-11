using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoryBookApi.Entities
{
    public class StatVowels
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public int SingleVowelCount { get; set; }
        public int PairVowelCount { get; set; }
        public int TotalWordCount { get; set; }
    }
}

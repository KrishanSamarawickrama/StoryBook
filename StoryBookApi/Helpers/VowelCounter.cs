using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoryBookApi.Helpers
{
    public static class VowelCounter
    {
        public static (int,int) VowelProcessor(string post)
        {
            int single = 0, vowelPairs = 0;

            for (int i = 0; i < post.Length - 1; i++)
            {
                if (IsVowel(post[i]))
                {
                    if (IsVowel(post[i + 1]))
                        vowelPairs++;
                    else
                        single++;
                }                    
            }

            return (single, vowelPairs);
        }

        private static bool IsVowel(char ch)
        {
            switch (ch)
            {
                case 'a':
                case 'e':
                case 'i':
                case 'o':
                case 'u':
                    return true;
                default:
                    return false;
            }
        }

    }
}

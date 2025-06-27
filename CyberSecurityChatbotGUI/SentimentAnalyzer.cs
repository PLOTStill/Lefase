using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSecurityChatbotGUI
{
    internal class SentimentAnalyzer
    {
        //Dictionary of sentiment keywords
        private Dictionary<string, List<string>> sentimentKeywords = new Dictionary<string, List<string>>
        {
            { "worried", new List<string> { "worried", "concern", "scared", "afraid", "fear", "anxious", "nervous", "trouble", "risk", "dangerous" } },
            { "curious", new List<string> { "curious", "interested", "wonder", "learn", "know more", "tell me", "explain", "how does", "what is" } },
            { "frustrated", new List<string> { "frustrated", "annoying", "difficult", "hard", "struggle", "problem", "can't figure", "too complicated", "confusing" } },
            { "confused", new List<string> { "confused", "don't understand", "unclear", "lost", "what do you mean", "not making sense" } }
        };

        public string DetectSentiment(string input)
        {
            string inputLower = input.ToLower();

            foreach (var sentiment in sentimentKeywords)
            {
                foreach (var keyword in sentiment.Value)
                {
                    if (inputLower.Contains(keyword))
                    {
                        return sentiment.Key;
                    }
                }
            }
            return null;
        }
    }

}

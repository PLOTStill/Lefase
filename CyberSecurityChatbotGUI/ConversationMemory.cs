using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSecurityChatbotGUI
{
    internal class ConversationMemory
    {
        //Store general information about the user
        private Dictionary<string, string> userInfo = new Dictionary<string, string>();

        //List of user inputs to track the conversation
        private List<string> userInputHistory = new List<string>();

        //Track topics discussed
        private Queue<string> topicsDiscussed = new Queue<string>();
        private int maxTopicsToRemember = 5;

        public void AddInfo(string key, string value)
        {
            userInfo[key] = value;
        }

        public string GetInfo(string key)
        {
            return userInfo.ContainsKey(key) ? userInfo[key] : null;
        }

        public void AddUserInput(string input)
        {
            userInputHistory.Add(input);

            //Keep history manageable - only keeps last 20 inputs
            if (userInputHistory.Count > 20)
            {
                userInputHistory.RemoveAt(0);
            }
        }

        public List<string> GetRecentInputs(int count)
        {
            return userInputHistory.Skip(Math.Max(0, userInputHistory.Count - count)).ToList();
        }

        public void AddTopicDiscussed(string topic)
        {
            //Check if the topic is already in the queue
            if (!topicsDiscussed.Contains(topic))
            {
                topicsDiscussed.Enqueue(topic);

                //Keep only most recent topics
                if (topicsDiscussed.Count > maxTopicsToRemember)
                {
                    topicsDiscussed.Dequeue();
                }
            }
        }

        public string GetLastTopicDiscussed()
        {
            return topicsDiscussed.Count > 0 ? topicsDiscussed.Last() : null;
        }
    }

}

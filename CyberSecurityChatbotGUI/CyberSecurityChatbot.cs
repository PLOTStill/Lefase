using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSecurityChatbotGUI
{
    internal class CyberSecurityChatbot
    {
        public string UserName { get; set; }

        public string ProcessUserInput(string userInput)
        {
            if (string.IsNullOrWhiteSpace(userInput))
            {
                return "Please ask me something about online safety.";
            }

            memory.AddUserInput(userInput);
            string inputLower = userInput.ToLower();

            //Task 1 & 4: NLP for Tasks and Activity Logging
            if (inputLower.StartsWith("add task"))
            {
                string taskTitle = userInput.Substring("add task".Length).Trim();
                Tasks.Add(new CyberTask { Title = taskTitle });
                ActivityLog.Add($"[{DateTime.Now:T}] Task added: '{taskTitle}'");
                return $"Task added: '{taskTitle}'.";
            }
            if (inputLower.Equals("view tasks"))
            {
                if (Tasks.Count == 0) return "You have no tasks.";
                string taskList = "Here are your tasks:\n" + string.Join("\n", Tasks);
                return taskList;
            }
            if (inputLower.Equals("show activity log"))
            {
                if(ActivityLog.Count == 0) return "No activity yet.";
                int startIndex = Math.Max(0, ActivityLog.Count - 5);
                var lastFiveItems = ActivityLog.GetRange(startIndex, Math.Min(5, ActivityLog.Count));
                string log = "Here is your recent activity:\n" + string.Join("\n", lastFiveItems);
                return log;
            }
            //Task 2: NLP for Quiz (simple version)
            if (inputLower.Equals("start quiz"))
            {
                ActivityLog.Add($"[{DateTime.Now:T}] Quiz started.");
                return "Quiz feature is under construction! Ask me about passwords instead.";
            }

            //Original Logic
            CheckForInterests(userInput);
            string response = GetResponse(userInput);
            return response;
        }

        //Speech synthesizer for audio playback output
        private string userName = "";
        private ConversationMemory memory = new ConversationMemory();
        private SentimentAnalyzer sentimentAnalyzer = new SentimentAnalyzer();
        private Random random = new Random();

        public List<CyberTask> Tasks { get; private set; } = new List<CyberTask>();
        public List<string> ActivityLog { get; private set; } = new List<string>();

        //Topics of interest remembered by the chatbot
        private List<string> userInterests = new List<string>();

        //Dictionary to store keyword responses
        private Dictionary<string, List<string>> keywordResponses = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

        //Dictionary for sentiment-based responses
        private Dictionary<string, List<string>> sentimentResponses = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

        //Default responses when no keyword is matched
        private List<string> defaultResponses = new List<string>
        {
            "I can help you with cybersecurity advice. Would you like to know more about safe browsing or phishing prevention?",
            "I'm focused on helping you stay safe online. Ask me about phishing, secure browsing, or online protection.",
            "Cybersecurity is crucial in today's digital world. What specific concerns do you have?",
            "I don't quite understand. Can you try rephrasing your question about cybersecurity?"
        };

        public CyberSecurityChatbot()
        {
            InitializeResponses();
            InitializeSentimentResponses();
        }

        private void InitializeResponses()
        {
            //Initialize phishing responses
            keywordResponses["phishing"] = new List<string>
            {
                "Phishing is a cybercrime where attackers try to trick you into revealing sensitive information like passwords or credit card details by posing as a trustworthy entity.",
                "Phishing attacks often use fake emails or websites that look legitimate. Always verify the sender and check URLs carefully.",
                "A common phishing tactic is creating urgency. Be suspicious of messages claiming your account will be closed unless you act immediately."
            };

            keywordResponses["identify phishing"] = new List<string>
            {
                "To identify phishing attempts:\n1. Check the sender's email address carefully\n2. Be wary of urgent or threatening language\n3. Hover over links before clicking\n4. Look for spelling or grammatical errors\n5. Don't click on unexpected attachments",
                "Phishing emails often have telltale signs like generic greetings, suspicious links, and requests for personal information. Always double-check before responding."
            };

            //Initialize password security responses
            keywordResponses["password"] = new List<string>
            {
                "Create strong passwords by:\n1. Using at least 12 characters\n2. Mixing uppercase, lowercase, numbers, and symbols\n3. Avoiding personal information\n4. Using a password manager\n5. Not reusing passwords across sites",
                "Password managers are a great tool for generating and storing strong, unique passwords. They help you avoid the dangerous habit of password reuse.",
                "Consider using passphrases instead of passwords. They're longer, more memorable, and often more secure than complex passwords."
            };

            //Initialize safe browsing responses
            keywordResponses["safe browsing"] = new List<string>
            {
                "Safe browsing involves protecting yourself online by being cautious about the websites you visit and the information you share.",
                "Always look for HTTPS in the URL bar and a padlock icon to ensure the website connection is encrypted.",
                "Be cautious when downloading files. Only download from trusted sources and scan files with antivirus software before opening."
            };

            //Initialize social engineering responses
            keywordResponses["social engineering"] = new List<string>
            {
                "Social engineering is the psychological manipulation of people into performing actions or divulging confidential information.",
                "Social engineers exploit human psychology and trust rather than technical hacking techniques. Always verify unusual requests, even from seemingly trusted sources.",
                "Be wary of oversharing on social media. Hackers can use personal information to craft targeted attacks or answer your security questions."
            };

            //Initialize privacy responses
            keywordResponses["privacy"] = new List<string>
            {
                "Protecting your online privacy involves controlling what personal information you share and who has access to it.",
                "Regularly review privacy settings on your social media accounts and limit the personal information you share publicly.",
                "Consider using a VPN (Virtual Private Network) when connecting to public Wi-Fi to encrypt your connection and protect your data."
            };

            //Initialize two-factor authentication responses
            keywordResponses["two-factor"] = new List<string>
            {
                "Two-factor authentication adds an extra layer of security by requiring a second form of verification, like a code sent to your phone, in addition to your password.",
                "Even if someone steals your password, two-factor authentication can prevent unauthorized access to your accounts.",
                "Consider using an authenticator app instead of SMS for two-factor authentication when possible, as it's more secure against SIM-swapping attacks."
            };

            //Response for when users ask what the bot can do
            keywordResponses["what can you do"] = new List<string>
            {
                "I can provide information about various cybersecurity topics like phishing, passwords, safe browsing, privacy, social engineering, and two-factor authentication. What would you like to learn about?",
                "I'm here to help with cybersecurity awareness! Ask me about phishing attacks, password security, safe browsing practices, privacy protection, or two-factor authentication."
            };

            //Response for greetings
            keywordResponses["hello"] = new List<string>
            {
                "Hello! How can I help you with cybersecurity today?",
                "Hi there! I'm ready to chat about staying safe online. What would you like to know?"
            };
        }

        private void InitializeSentimentResponses()
        {
            //Responses for worried sentiment
            sentimentResponses["worried"] = new List<string>
            {
                "It's natural to feel concerned about online threats. Let me help you understand how to protect yourself.",
                "I understand your concern. Cybersecurity can seem overwhelming, but taking small steps can significantly improve your safety online.",
                "Many people share your concerns about online security. Let's focus on practical steps you can take."
            };

            //Responses for curious sentiment
            sentimentResponses["curious"] = new List<string>
            {
                "It's great that you're curious about cybersecurity! Learning more is the first step to staying safe online.",
                "Your curiosity will serve you well! Understanding cyber threats helps you recognize and avoid them."
            };

            //Responses for frustrated sentiment
            sentimentResponses["frustrated"] = new List<string>
            {
                "I understand cybersecurity can be frustrating at times. Let's break it down into simpler steps.",
                "It's okay to feel frustrated. Cybersecurity concepts can be complex, but I'm here to help explain them clearly."
            };

            //Responses for confused sentiment
            sentimentResponses["confused"] = new List<string>
            {
                "I see you might be a bit confused. Let me try to explain this in a clearer way.",
                "Cybersecurity terminology can be confusing! Let me break this down into simpler terms."
            };
        }


        private void CheckForInterests(string input)
        {
            //Check for expressions of interest in various cybersecurity topics
            string inputLower = input.ToLower();

            //List of topics to check for interest
            Dictionary<string, string> topics = new Dictionary<string, string>
            {
                { "password", "password security" },
                { "phishing", "phishing prevention" },
                { "privacy", "privacy protection" },
                { "social engineering", "social engineering" },
                { "two-factor", "two-factor authentication" },
                { "2fa", "two-factor authentication" },
                { "browsing", "safe browsing" }
            };

            //Check for interest expressions
            bool hasInterestExpression = inputLower.Contains("interested in") ||
                                        inputLower.Contains("want to learn about") ||
                                        inputLower.Contains("tell me about") ||
                                        inputLower.Contains("curious about");

            if (hasInterestExpression)
            {
                foreach (var topic in topics)
                {
                    if (inputLower.Contains(topic.Key) && !userInterests.Contains(topic.Value))
                    {
                        userInterests.Add(topic.Value);
                        memory.AddInfo("interest", topic.Value);
                        break;
                    }
                }
            }
        }

        private string GetResponse(string input)
        {
            //Detect sentiment in user input
            string detectedSentiment = sentimentAnalyzer.DetectSentiment(input);
            string response = null;

            //If a sentiment is detected, we might modify the response
            if (!string.IsNullOrEmpty(detectedSentiment) && sentimentResponses.ContainsKey(detectedSentiment))
            {
                //Get a random sentiment-based response
                List<string> sentimentBasedResponses = sentimentResponses[detectedSentiment];
                response = sentimentBasedResponses[random.Next(sentimentBasedResponses.Count)] + " ";
            }

            //Try to find a matching keyword response
            string contentResponse = FindKeywordResponse(input);

            //If we have both a sentiment response and content response, combine them
            if (response != null)
            {
                return response + contentResponse;
            }

            //Otherwise just return the content response
            return contentResponse;
        }

        private string FindKeywordResponse(string input)
        {
            string inputLower = input.ToLower();

            //First, check for exact keyword matches
            foreach (var entry in keywordResponses)
            {
                if (inputLower.Contains(entry.Key.ToLower()))
                {
                    //Get a random response for this keyword
                    List<string> responses = entry.Value;
                    string response = responses[random.Next(responses.Count)];

                    //Add personalization if user has shown interest in this topic before
                    return PersonalizeResponse(response, entry.Key);
                }
            }

            //If no exact match, try to be a bit smarter with partial matches
            foreach (var entry in keywordResponses)
            {
                string[] keywords = entry.Key.Split(' ');

                //If any word in the keyword phrase matches, consider it a match
                foreach (string keyword in keywords)
                {
                    if (keyword.Length > 3 && inputLower.Contains(keyword.ToLower()))
                    {
                        List<string> responses = entry.Value;
                        string response = responses[random.Next(responses.Count)];
                        return PersonalizeResponse(response, entry.Key);
                    }
                }
            }

            //If still no match, check for common variations
            if (inputLower.Contains("safe") || inputLower.Contains("security") || inputLower.Contains("protect"))
            {
                return "I can help you with online safety. Would you like to know about secure browsing, phishing prevention, or password protection?";
            }

            //Check if this is a follow-up question
            if (IsFollowUpQuestion(inputLower))
            {
                string lastTopic = memory.GetLastTopicDiscussed();
                if (!string.IsNullOrEmpty(lastTopic) && keywordResponses.ContainsKey(lastTopic))
                {
                    List<string> responses = keywordResponses[lastTopic];
                    string response = "Regarding " + lastTopic + ", " + responses[random.Next(responses.Count)];
                    return response;
                }
            }

            //If no match is found, return a random default response
            return defaultResponses[random.Next(defaultResponses.Count)];
        }

        private bool IsFollowUpQuestion(string input)
        {
            //Detect if this is likely a follow-up question/request for more information
            string[] followUpPhrases = new string[]
            {
                "tell me more",
                "more information",
                "elaborate",
                "explain further",
                "what else",
                "how does that work",
                "why is that important",
                "can you explain",
                "what about",
                "?",
                "give me an example"
            };

            return followUpPhrases.Any(phrase => input.Contains(phrase));
        }

        private string PersonalizeResponse(string response, string topic)
        {
            //Record the topic in memory
            memory.AddTopicDiscussed(topic);

            //If user has expressed interest in this topic before, personalize the response
            if (userInterests.Contains(topic))
            {
                return $"Since you're interested in {topic}, here's some information: {response}";
            }

            //If there are other interests, sometimes reference them
            if (userInterests.Count > 0 && random.Next(100) < 30)
            {
                string randomInterest = userInterests[random.Next(userInterests.Count)];
                if (randomInterest != topic)
                {
                    return $"{response}\n\nBy the way, since you were interested in {randomInterest}, you might also want to know that they're related. Good {topic} practices can help with {randomInterest}.";
                }
            }
            return response;
        }
    }
}

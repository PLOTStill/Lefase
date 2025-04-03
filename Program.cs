using System;
using System.Collections.Generic;
using System.Speech.Synthesis;
using System.Linq;

class CyberSecurityChatbot
{
    //Speech synthesizer for audio playback output
    private static SpeechSynthesizer synthesizer = new SpeechSynthesizer();

    private static Dictionary<string, string> responses = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        //Phishing responses
        { "phishing", "Phishing is a cybercrime where attackers try to trick you into revealing sensitive information like passwords or credit card details by posing as a trustworthy entity." },
        { "how to identify phishing", "To identify phishing attempts:\n1. Check the sender's email address carefully\n2. Be wary of urgent or threatening language\n3. Hover over links before clicking\n4. Look for spelling or grammatical errors\n5. Don't click on unexpected attachments" },
        { "phishing email", "Phishing emails often appear to be from legitimate companies. They may ask you to 'verify' your account, claim there's a problem, or offer unexpected rewards." },
        
        //Safe browsing responses
        { "safe browsing", "Safe browsing involves protecting yourself online by being cautious about the websites you visit and the information you share." },
        { "protect online", "Protect yourself online by:\n1. Using strong, unique passwords\n2. Enabling two-factor authentication\n3. Keeping software updated\n4. Using a reputable antivirus\n5. Being cautious about public Wi-Fi" },
        { "secure website", "A secure website typically:\n1. Starts with 'https://'\n2. Has a padlock icon in the address bar\n3. Is from a trusted, verified domain\n4. Doesn't ask for unnecessary personal information" },
        
        //Specific security responses
        { "password", "Create strong passwords by:\n1. Using at least 12 characters\n2. Mixing uppercase, lowercase, numbers, and symbols\n3. Avoiding personal information\n4. Using a password manager\n5. Not reusing passwords across sites" },
        { "two-factor", "Two-factor authentication adds an extra layer of security by requiring a second form of verification, like a code sent to your phone, in addition to your password." }
    };

    private static List<string> defaultResponses = new List<string>
    {
        "I can help you with cybersecurity advice. Would you like to know more about safe browsing or phishing prevention?",
        "I'm focused on helping you stay safe online. Ask me about phishing, secure browsing, or online protection.",
        "Cybersecurity is crucial in today's digital world. What specific concerns do you have?"
    };

    //Prints username 
    private static string userName = "";

    static void Main(string[] args)
    {
        //ASCII code logo
        char A = (char)95; // "_"
        char B = (char)47; // "/" " " * 7
        char C = (char)92; // "\"
        char D = (char)124; // "|"
        char E = (char)40; // "("
        char F = (char)41; // ")"
        char G = (char)111; // "o"

        //ASCII code colour
        Console.ForegroundColor = ConsoleColor.Cyan;

        //Prints ASCII code characters
        Console.WriteLine("    " + A + "" + B + "       " + C + "" + A);
        Console.WriteLine("   " + B + " " + D + "       " + D + " " + C);
        Console.WriteLine("  " + B + "  " + D + A + A + "   " + A + A + D + "  " + C);
        Console.WriteLine(" " + D + "" + A + "" + A + "" + B + E + E + G + D + " " + D + G + F + F + C + A + A + D);
        Console.WriteLine(" " + D + "      " + D + " " + D + "      " + D);
        Console.WriteLine(" " + D + "" + C + "     " + D + A + D + "     " + B + D);
        Console.WriteLine(" " + D + " " + C + "           " + B + " " + D);
        Console.WriteLine("  " + C + D + " " + B + "  " + A + A + A + "  " + C + " " + D + B);
        Console.WriteLine("   " + C + " " + D + " " + B + " " + A + " " + C + " " + D + " " + B);
        Console.WriteLine("    " + C + A + A + A + A + A + A + A + A + A + B);
        Console.WriteLine("     " + A + D + A + A + A + A + A + D + A);
        Console.WriteLine(" " + A + "" + A + A + D + "         " + D + A + A + A);
        Console.WriteLine(B + "                 " + C);
        Console.ResetColor();

        try
        {
            //Configure speech synthesizer
            synthesizer.Volume = 100;  //0-100
            synthesizer.Rate = -2;     //Speed of speech (-10 to 10)

            //Initial greeting with speech
            string welcomeMessage = "Welcome to the Cybersecurity Awareness Chatbot!";
            Console.WriteLine(welcomeMessage);
            synthesizer.Speak(welcomeMessage);

            //Program asks user for name
            while (string.IsNullOrWhiteSpace(userName))
            {
                Console.Write("What's your name? ");
                synthesizer.Speak("What is your name?");
                userName = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(userName))
                {
                    string namePrompt = "I'm sorry, I didn't catch that. Could you please tell me your name?";
                    Console.WriteLine(namePrompt);
                    synthesizer.Speak(namePrompt);
                }
            }

            //Personalized user greeting
            string personalGreeting = $"Hello, {userName}! I'm here to help you stay safe online.";
            Console.WriteLine(personalGreeting);
            synthesizer.Speak(personalGreeting);

            string instructionMessage = "You can ask me about phishing, safe browsing, passwords, and more. Type 'bye' to end our conversation.";
            Console.WriteLine(instructionMessage);
            synthesizer.Speak(instructionMessage);

            while (true)
            {
                Console.Write($"{userName}, what would you like to know? ");
                synthesizer.Speak($"{userName}, what would you like to know?");
                string userInput = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(userInput))
                {
                    string emptyInputResponse = "Please ask me something about online safety.";
                    Console.WriteLine(emptyInputResponse);
                    synthesizer.Speak(emptyInputResponse);
                    continue;
                }

                //Checks for exit command
                if (userInput.Equals("bye", StringComparison.OrdinalIgnoreCase))
                {
                    string goodbyeMessage = $"Goodbye, {userName}! Stay safe online!";
                    Console.WriteLine(goodbyeMessage);
                    synthesizer.Speak(goodbyeMessage);
                    break;
                }

                //Responds to user input
                string response = GetResponse(userInput);
                Console.WriteLine($"Bot: {response}");
                synthesizer.Speak(response);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
        finally
        {
            //Dispose of the speech synthesizer
            synthesizer?.Dispose();
        }
    }

    static string GetResponse(string input)
    {
        //Try to find a matching response
        foreach (var entry in responses)
        {
            if (input.Contains(entry.Key))
            {
                return entry.Value;
            }
        }

        //Checks for some common variations
        if (input.Contains("safe") || input.Contains("security"))
        {
            return "I can help you with online safety. Would you like to know about secure browsing, phishing prevention, or password protection?";
        }

        //If no match is found, program returns a random default response
        return defaultResponses[new Random().Next(defaultResponses.Count)];
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Speech.Synthesis;
using Microsoft.VisualBasic;

namespace CyberSecurityChatbotGUI
{
    public partial class MainWindow : Window
    {
        private CyberSecurityChatbot chatbot;
        private SpeechSynthesizer synthesizer;

        public MainWindow()
        {
            InitializeComponent();
            chatbot = new CyberSecurityChatbot();
            synthesizer = new SpeechSynthesizer();
            synthesizer.SetOutputToDefaultAudioDevice();

            // Start the conversation
            DisplayBotMessage("Hello! Welcome to the Cybersecurity Awareness Bot. What's your name?");
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            ProcessUserInput();
        }

        private void UserInputBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                ProcessUserInput();
            }
        }

        private void ProcessUserInput()
        {
            string userInput = UserInputBox.Text;
            if (string.IsNullOrWhiteSpace(userInput)) return;

            // Display the user's message
            AppendMessage($"You: {userInput}");
            UserInputBox.Clear();

            // If the chatbot doesn't have a name for the user yet, this is the name.
            if (string.IsNullOrEmpty(chatbot.UserName))
            {
                chatbot.UserName = userInput;
                DisplayBotMessage($"Nice to meet you, {chatbot.UserName}! You can ask me about phishing, passwords, and more. Type 'bye' to exit.");
            }
            else
            {
                // Otherwise, process the input normally.
                if (userInput.Equals("bye", StringComparison.OrdinalIgnoreCase))
                {
                    DisplayBotMessage($"Goodbye, {chatbot.UserName}! Stay safe online!");
                    // Optionally close the app after a delay
                    // Application.Current.Shutdown(); 
                }
                else
                {
                    string botResponse = chatbot.ProcessUserInput(userInput);
                    DisplayBotMessage(botResponse);
                }
            }
        }

        private void DisplayBotMessage(string message)
        {
            AppendMessage($"Bot: {message}");
            synthesizer.SpeakAsync(message); // Use SpeakAsync to not freeze the UI
        }

        private void AppendMessage(string message)
        {
            ConversationDisplay.AppendText(message + "\n");
            ConversationDisplay.ScrollToEnd();
        }
    }
}

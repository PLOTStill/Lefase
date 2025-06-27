using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSecurityChatbotGUI
{
    internal class CyberTask
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? ReminderDate { get; set; }
        public bool IsCompleted { get; set; }

        public override string ToString()
        {
            return $"{(IsCompleted ? "[Done] " : "")}{Title}{(ReminderDate.HasValue ? $" (Reminder: {ReminderDate.Value:d})" : "")}";
        }
    }
}

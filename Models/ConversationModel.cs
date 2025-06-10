using System;

namespace TLGames.Models
{
    internal class ConversationModel
    {
        public int ConversationId { get; private set; }
        public int FirstUserId { get; private set; }
        public int SecondUserId { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime LastMessageTime { get; private set; }
        public string Status { get; private set; }

        public ConversationModel() { }

        public ConversationModel(int conversationId, int firstUserId, int secondUserId, DateTime startTime, DateTime lastMessageTime, string status)
        {
            ConversationId = conversationId;
            FirstUserId = firstUserId;
            SecondUserId = secondUserId;
            StartTime = startTime;
            LastMessageTime = lastMessageTime;
            Status = status;
        }

        public void SetConversationId(int id) { ConversationId = id; }
        public void SetFirstUserId(int id) { FirstUserId = id; }
        public void SetSecondUserId(int id) { SecondUserId = id; }
        public void SetStartTime(DateTime time) { StartTime = time; }
        public void SetLastMessageTime(DateTime time) { LastMessageTime = time; }
        public void SetStatus(string status) { Status = status; }
    }
}

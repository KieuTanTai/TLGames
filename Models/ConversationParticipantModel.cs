using System;

namespace TLGames.Models
{
    internal class ConversationParticipantModel
    {
        public int Id { get; private set; }
        public int ConversationId { get; private set; }
        public int UserId { get; private set; }
        public DateTime LastReadTime { get; private set; }
        public DateTime JoinTime { get; private set; }

        public ConversationParticipantModel() { }

        public ConversationParticipantModel(int id, int conversationId, int userId, DateTime lastReadTime, DateTime joinTime)
        {
            Id = id;
            ConversationId = conversationId;
            UserId = userId;
            LastReadTime = lastReadTime;
            JoinTime = joinTime;
        }

        public void SetId(int id) { Id = id; }
        public void SetConversationId(int id) { ConversationId = id; }
        public void SetUserId(int id) { UserId = id; }
        public void SetLastReadTime(DateTime time) { LastReadTime = time; }
        public void SetJoinTime(DateTime time) { JoinTime = time; }
    }
}

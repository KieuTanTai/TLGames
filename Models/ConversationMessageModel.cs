using System;

namespace TLGames.Models
{
    internal class ConversationMessageModel
    {
        public int MessageId { get; private set; }
        public int RepliedMessageId { get; private set; }
        public int SendUserId { get; private set; }
        public int ConversationId { get; private set; }
        public string Content { get; private set; }
        public DateTime SendTime { get; private set; }
        public string MessageType { get; private set; }
        public string AttachmentURL { get; private set; }

        public ConversationMessageModel() { }

        public ConversationMessageModel(int messageId, int repliedMessageId, int sendUserId, int conversationId,
                            string content, DateTime sendTime, string messageType, string attachmentURL)
        {
            MessageId = messageId;
            RepliedMessageId = repliedMessageId;
            SendUserId = sendUserId;
            ConversationId = conversationId;
            Content = content;
            SendTime = sendTime;
            MessageType = messageType;
            AttachmentURL = attachmentURL;
        }

        public void SetMessageId(int id) { MessageId = id; }
        public void SetRepliedMessageId(int id) { RepliedMessageId = id; }
        public void SetSendUserId(int id) { SendUserId = id; }
        public void SetConversationId(int id) { ConversationId = id; }
        public void SetContent(string content) { Content = content; }
        public void SetSendTime(DateTime time) { SendTime = time; }
        public void SetMessageType(string type) { MessageType = type; }
        public void SetAttachmentURL(string url) { AttachmentURL = url; }
    }
}

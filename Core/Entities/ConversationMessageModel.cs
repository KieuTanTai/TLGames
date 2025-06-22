using System;
using TLGames.Core.Enums;

namespace TLGames.Core.Entities
{
    internal class ConversationMessageModel
    {
        public int MessageId { get; private set; }
        public int? RepliedMessageId { get; private set; }
        public int SendUserId { get; private set; }
        public int ConversationId { get; private set; }
        public string Content { get; private set; }
        public DateTime SendTime { get; private set; }
        public EMessageType MessageType { get; private set; }
        public string AttachmentUrl { get; private set; }

        public ConversationMessageModel() { }

        public ConversationMessageModel(int messageId, int repliedMessageId, int sendUserId, int conversationId,
                            string content, DateTime sendTime, EMessageType messageType, string attachmentUrl)
        {
            MessageId = messageId;
            RepliedMessageId = repliedMessageId;
            SendUserId = sendUserId;
            ConversationId = conversationId;
            Content = content;
            SendTime = sendTime;
            MessageType = messageType;
            AttachmentUrl = attachmentUrl;
        }

        public void SetMessageId(int id) { MessageId = id; }
        public void SetRepliedMessageId(int id) { RepliedMessageId = id; }
        public void SetSendUserId(int id) { SendUserId = id; }
        public void SetConversationId(int id) { ConversationId = id; }
        public void SetContent(string content) { Content = content; }
        public void SetSendTime(DateTime time) { SendTime = time; }
        public void SetMessageType(EMessageType type) { MessageType = type; }
        public void SetAttachmentUrl(string url) { AttachmentUrl = url; }
    }
}

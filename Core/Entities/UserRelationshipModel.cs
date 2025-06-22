using System;
using TLGames.Core.Enums;

namespace TLGames.Core.Entities
{
    internal class UserRelationshipModel
    {
        public int RequesterId { get; private set; }
        public int ReceiverId { get; private set; }
        public DateTime RequestDate { get; private set; }
        public DateTime? AcceptDate { get; private set; }
        public EUserRelationshipStatus Status { get; private set; }

        public UserRelationshipModel() { }

        public UserRelationshipModel(int requesterId, int receiverId, DateTime requestDate, DateTime? acceptDate, EUserRelationshipStatus status)
        {
            RequesterId = requesterId;
            ReceiverId = receiverId;
            RequestDate = requestDate;
            AcceptDate = acceptDate;
            Status = status;
        }

        public void SetRequesterId(int id) { RequesterId = id; }
        public void SetReceiverId(int id) { ReceiverId = id; }
        public void SetRequestDate(DateTime date) { RequestDate = date; }
        public void SetAcceptDate(DateTime? date) { AcceptDate = date; }
        public void SetStatus(EUserRelationshipStatus status) { Status = status; }
    }
}

using System;

namespace TLGames.Models
{
    internal class UserPaymentMethodModel
    {
        public int UserId { get; private set; }
        public int PaymentMethodId { get; private set; }
        public DateTime AddedDate { get; private set; }
        public DateTime LastUpdatedDate { get; private set; }
        public bool Status { get; private set; }

        public UserPaymentMethodModel() { }

        public UserPaymentMethodModel(int userId, int paymentMethodId, DateTime addedDate, DateTime lastUpdatedDate, bool status)
        {
            UserId = userId;
            PaymentMethodId = paymentMethodId;
            AddedDate = addedDate;
            LastUpdatedDate = lastUpdatedDate;
            Status = status;
        }

        public void SetUserId(int id) { UserId = id; }
        public void SetPaymentMethodId(int id) { PaymentMethodId = id; }
        public void SetAddedDate(DateTime date) { AddedDate = date; }
        public void SetLastUpdatedDate(DateTime date) { LastUpdatedDate = date; }
        public void SetStatus(bool status) { Status = status; }
    }
}

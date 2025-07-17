using System;
using TLGames.Core.Enums;

namespace TLGames.Core.Entities
{
    public class UserPaymentMethodModel
    {
        public int UserPaymentMethodId { get; private set; }
        public int? BankId { get; private set; }
        public ETypePaymentMethod PaymentMethodType { get; private set; }
        public string DisplayName { get; private set; }
        public string LastFourDigit { get; private set; }
        public int ExpiryYear { get; private set; }
        public int ExpiryMonth { get; private set; }
        public string Token { get; private set; }
        public int UserId { get; private set; }
        public DateTime AddedDate { get; private set; }
        public DateTime LastUpdatedDate { get; private set; }
        public EActiveStatus Status { get; private set; }

        public UserPaymentMethodModel() { }

        public UserPaymentMethodModel(int userPaymentMethodId, int bankId, ETypePaymentMethod paymentMethodType, string displayName,
                                  string lastFourDigit, int expiryYear, int expiryMonth, string token, int userId, DateTime addedDate, DateTime lastUpdatedDate, EActiveStatus status)
        {
            UserPaymentMethodId = userPaymentMethodId;
            BankId = bankId;
            UserId = userId;
            PaymentMethodType = paymentMethodType;
            DisplayName = displayName;
            LastFourDigit = lastFourDigit;
            ExpiryYear = expiryYear;
            ExpiryMonth = expiryMonth;
            Token = token;
            AddedDate = addedDate;
            LastUpdatedDate = lastUpdatedDate;
            Status = status;
        }

        public void SetUserPaymentMethodId(int id) { UserPaymentMethodId = id; }
        public void SetBankId(int id) { BankId = id; }
        public void SetPaymentMethodType(ETypePaymentMethod type) { PaymentMethodType = type; }
        public void SetDisplayName(string name) { DisplayName = name; }
        public void SetLastFourDigit(string digits) { LastFourDigit = digits; }
        public void SetExpiryYear(int year) { ExpiryYear = year; }
        public void SetExpiryMonth(int month) { ExpiryMonth = month; }
        public void SetToken(string token) { Token = token; }
        public void SetUserId(int id) { UserId = id; }
        public void SetAddedDate(DateTime date) { AddedDate = date; }
        public void SetLastUpdatedDate(DateTime date) { LastUpdatedDate = date; }
        public void SetStatus(EActiveStatus status) { Status = status; }
    }
}

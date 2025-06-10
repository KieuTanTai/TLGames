namespace TLGames.Models
{
    internal class PaymentMethodModel
    {
        public int PaymentMethodId { get; private set; }
        public int BankId { get; private set; }
        public string PaymentMethodType { get; private set; }
        public string DisplayName { get; private set; }
        public string LastFourDigit { get; private set; }
        public int ExpiryYear { get; private set; }
        public int ExpiryMonth { get; private set; }
        public string Token { get; private set; }

        public PaymentMethodModel() { }

        public PaymentMethodModel(int paymentMethodId, int bankId, string paymentMethodType, string displayName,
                                  string lastFourDigit, int expiryYear, int expiryMonth, string token)
        {
            PaymentMethodId = paymentMethodId;
            BankId = bankId;
            PaymentMethodType = paymentMethodType;
            DisplayName = displayName;
            LastFourDigit = lastFourDigit;
            ExpiryYear = expiryYear;
            ExpiryMonth = expiryMonth;
            Token = token;
        }

        public void SetPaymentMethodId(int id) { PaymentMethodId = id; }
        public void SetBankId(int id) { BankId = id; }
        public void SetPaymentMethodType(string type) { PaymentMethodType = type; }
        public void SetDisplayName(string name) { DisplayName = name; }
        public void SetLastFourDigit(string digits) { LastFourDigit = digits; }
        public void SetExpiryYear(int year) { ExpiryYear = year; }
        public void SetExpiryMonth(int month) { ExpiryMonth = month; }
        public void SetToken(string token) { Token = token; }
    }
}

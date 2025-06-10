using System;

namespace TLGames.Models
{
    internal class TransactionModel
    {
        public int TransactionId { get; private set; }
        public int WalletId { get; private set; }
        public int InvoiceId { get; private set; }
        public int PlatformRuleId { get; private set; } // Assuming this is an FK to a PlatformRule table
        public DateTime TransactionDate { get; private set; }
        public string TransactionType { get; private set; }
        public decimal CurrentBalance { get; private set; }
        public string Status { get; private set; }

        public TransactionModel() { }

        public TransactionModel(int transactionId, int walletId, int invoiceId, int platformRuleId,
                                DateTime transactionDate, string transactionType, decimal currentBalance, string status)
        {
            TransactionId = transactionId;
            WalletId = walletId;
            InvoiceId = invoiceId;
            PlatformRuleId = platformRuleId;
            TransactionDate = transactionDate;
            TransactionType = transactionType;
            CurrentBalance = currentBalance;
            Status = status;
        }

        public void SetTransactionId(int id) { TransactionId = id; }
        public void SetWalletId(int id) { WalletId = id; }
        public void SetInvoiceId(int id) { InvoiceId = id; }
        public void SetPlatformRuleId(int id) { PlatformRuleId = id; }
        public void SetTransactionDate(DateTime date) { TransactionDate = date; }
        public void SetTransactionType(string type) { TransactionType = type; }
        public void SetCurrentBalance(decimal balance) { CurrentBalance = balance; }
        public void SetStatus(string status) { Status = status; }
    }
}

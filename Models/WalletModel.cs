using System;

namespace TLGames.Models
{
    internal class WalletModel
    {
        public int WalletId { get; private set; }
        public int UserId { get; private set; }
        public decimal Balance { get; private set; }
        public string Currency { get; private set; }
        public DateTime LastUpdateBalanceDate { get; private set; }

        public WalletModel() { }

        public WalletModel(int walletId, int userId, decimal balance, string currency, DateTime lastUpdateBalanceDate)
        {
            WalletId = walletId;
            UserId = userId;
            Balance = balance;
            Currency = currency;
            LastUpdateBalanceDate = lastUpdateBalanceDate;
        }

        public void SetWalletId(int id) { WalletId = id; }
        public void SetUserId(int id) { UserId = id; }
        public void SetBalance(decimal balance) { Balance = balance; }
        public void SetCurrency(string currency) { Currency = currency; }
        public void SetLastUpdateBalanceDate(DateTime date) { LastUpdateBalanceDate = date; }
    }
}

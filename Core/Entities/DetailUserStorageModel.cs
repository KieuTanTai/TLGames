using System;
using TLGames.Core.Enums;

namespace TLGames.Core.Entities
{
    public class DetailUserStorageModel
    {
        public int ProductId { get; private set; }
        public int UserStorageId { get; private set; }
        public DateTime? LastPlayed { get; private set; }
        public TimeSpan? PlayTime { get; private set; }
        public bool IsFavored { get; private set; }
        public DateTime PurchaseDate { get; private set; }
        public bool IsInstalled { get; private set; }
        public DateTime? InstalledDate { get; private set; }
        public EActiveStatus Status { get; private set; }

        public DetailUserStorageModel() { }

        public DetailUserStorageModel(int productId, int userStorageId, DateTime lastPlayed, TimeSpan playTime,
                                      bool isFavored, DateTime purchaseDate, bool isInstalled, DateTime installedDate, EActiveStatus status)
        {
            ProductId = productId;
            UserStorageId = userStorageId;
            LastPlayed = lastPlayed;
            PlayTime = playTime;
            IsFavored = isFavored;
            PurchaseDate = purchaseDate;
            IsInstalled = isInstalled;
            InstalledDate = installedDate;
            Status = status;
        }

        public void SetProductId(int id) { ProductId = id; }
        public void SetStatus(EActiveStatus status) { Status = status; }
        public void SetUserStorageId(int id) { UserStorageId = id; }
        public void SetLastPlayed(DateTime date) { LastPlayed = date; }
        public void SetPlayTime(TimeSpan time) { PlayTime = time; }
        public void SetIsFavored(bool favored) { IsFavored = favored; }
        public void SetPurchaseDate(DateTime date) { PurchaseDate = date; }
        public void SetIsInstalled(bool installed) { IsInstalled = installed; }
        public void SetInstalledDate(DateTime date) { InstalledDate = date; }
    }
}

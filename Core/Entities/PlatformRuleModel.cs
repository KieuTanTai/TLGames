using System;

namespace TLGames.Core.Entities
{
    internal class PlatformRuleModel
    {
        public int PlatformRuleId { get; private set; }
        public decimal Fee { get; private set; }
        public TimeSpan PendingTime { get; private set; }

        public PlatformRuleModel() { }

        public PlatformRuleModel(int platformRuleId, decimal fee, TimeSpan pendingTime)
        {
            PlatformRuleId = platformRuleId;
            Fee = fee;
            PendingTime = pendingTime;
        }

        public void SetPlatformRuleId(int id) { PlatformRuleId = id; }
        public void SetFee(decimal fee) { Fee = fee; }
        public void SetPendingTime(TimeSpan time) { PendingTime = time; }
    }
}

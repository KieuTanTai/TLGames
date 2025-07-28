using System;
using TLGames.Core.Enums;

namespace TLGames.Core.Entities
{
    public class PlatformRuleModel
    {
        public int PlatformRuleId { get; private set; }
        public decimal Fee { get; private set; }
        public TimeSpan PendingTime { get; private set; }
        public EActiveStatus Status { get; private set; }

        public PlatformRuleModel() { }

        public PlatformRuleModel(int platformRuleId, decimal fee, TimeSpan pendingTime, EActiveStatus status)
        {
            PlatformRuleId = platformRuleId;
            Fee = fee;
            PendingTime = pendingTime;
            Status = status;
        }

        public void SetPlatformRuleId(int id) { PlatformRuleId = id; }
        public void SetStatus(EActiveStatus status) { Status = status; }
        public void SetFee(decimal fee) { Fee = fee; }
        public void SetPendingTime(TimeSpan time) { PendingTime = time; }
    }
}

using System;

namespace TLGames.Core.Entities
{
    public class FollowerOfDeveloperModel
    {
        public int DeveloperId { get; private set; }
        public int FollowerId { get; private set; }
        public DateTime StartFollowDate { get; private set; }

        public FollowerOfDeveloperModel() { }

        public FollowerOfDeveloperModel(int developerId, int followerId, DateTime startFollowDate)
        {
            DeveloperId = developerId;
            FollowerId = followerId;
            StartFollowDate = startFollowDate;
        }

        public void SetDeveloperId(int id) { DeveloperId = id; }
        public void SetFollowerId(int id) { FollowerId = id; }
        public void SetStartFollowDate(DateTime date) { StartFollowDate = date; }
    }
}

using System;

namespace TLGames.Models
{
    internal class FollowerOfPublisherModel
    {
        public int PublisherId { get; private set; }
        public int FollowerUserId { get; private set; }
        public DateTime StartFollowDate { get; private set; }

        public FollowerOfPublisherModel() { }

        public FollowerOfPublisherModel(int publisherId, int followerId, DateTime startFollowDate)
        {
            PublisherId = publisherId;
            FollowerUserId = followerId;
            StartFollowDate = startFollowDate;
        }

        public void SetPublisherId(int id) { PublisherId = id; }
        public void SetFollowerUserId(int id) { FollowerUserId = id; }
        public void SetStartFollowDate(DateTime date) { StartFollowDate = date; }
    }
}

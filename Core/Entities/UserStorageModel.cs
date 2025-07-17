namespace TLGames.Core.Entities
{
    public class UserStorageModel
    {
        public int UserStorageId { get; private set; }
        public int UserId { get; private set; }

        public UserStorageModel() { }
        public UserStorageModel(int userStorageId, int userId)
        {
            UserStorageId = userStorageId;
            UserId = userId;
        }

        public void SetUserStorageId(int id) { UserStorageId = id; }
        public void SetUserId(int id) { UserId = id; }
    }
}

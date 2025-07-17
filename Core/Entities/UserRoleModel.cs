using System;

namespace TLGames.Core.Entities
{
    public class UserRoleModel
    {
        public int UserId { get; private set; }
        public int RoleId { get; private set; }
        public DateTime CreateDate { get; private set; }

        public UserRoleModel() { }

        public UserRoleModel(int userId, int roleId, DateTime createDate)
        {
            UserId = userId;
            RoleId = roleId;
            CreateDate = createDate;
        }

        public void SetUserId(int id) { UserId = id; }
        public void SetRoleId(int id) { RoleId = id; }
        public void SetCreateDate(DateTime date) { CreateDate = date; }
    }
}

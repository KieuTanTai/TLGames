using System;
using TLGames.Core.Enums;

namespace TLGames.Core.Entities
{
    internal class UserModel
    {
        public int UserId { get; private set; }
        public string UserName { get; private set; }
        public string Password { get; private set; }
        public EUserStatus Status { get; private set; }
        public DateTime? CreateDate { get; private set; }

        public UserModel() { }

        public UserModel(int userId, string userName, string password, EUserStatus status, DateTime? createDate)
        {
            UserId = userId;
            UserName = userName;
            Password = password;
            Status = status;
            CreateDate = createDate;
        }

        public void SetUserId(int id)
        {
            UserId = id;
        }

        public void SetUserName(string name)
        {
            UserName = name;
        }

        public void SetPassword(string password)
        {
            Password = password;
        }

        public void SetStatus(EUserStatus status)
        {
            Status = status;
        }

        public void SetCreateDate(DateTime? date)
        {
            CreateDate = date;
        }
    }
}

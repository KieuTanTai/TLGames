using System;

namespace TLGames.Models
{
    internal class UserModel
    {
        public int UserId { get; private set; }
        public string UserName { get; private set; }
        public string Password { get; private set; }
        public DateTime? CreateDate { get; private set; }

        public UserModel() { }

        public UserModel(int userId, string userName, string password, DateTime? createDate)
        {
            UserId = userId;
            UserName = userName;
            Password = password;
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

        public void SetCreateDate(DateTime? date)
        {
            CreateDate = date;
        }
    }
}

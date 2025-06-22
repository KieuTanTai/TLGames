using System;
using TLGames.Core.Enums;

namespace TLGames.Core.Entities
{
    internal class CustomerModel
    {
        public int CustomerId { get; private set; }
        public int UserId { get; private set; }
        public DateTime? BirthDay { get; private set; }
        public string PersonalPhone { get; private set; }
        public string PersonalName { get; private set; }
        public string PersonalAddress { get; private set; }
        public string AvatarUrl { get; private set; }
        public string BackgroundUrl { get; private set; }
        public EGenderType Gender { get; private set; }
        public EUserStatus Status { get; private set; }
        public CustomerModel() { }

        public CustomerModel(int customerId, int userId, DateTime birthday, string personalPhone, string personalName,
                                string personalAddress, string avatarUrl, string backgroundUrl, EGenderType gender, EUserStatus status)
        {
            CustomerId = customerId;
            UserId = userId;
            BirthDay = birthday;
            PersonalPhone = personalPhone;
            PersonalName = personalName;
            PersonalAddress = personalAddress;
            AvatarUrl = avatarUrl;
            BackgroundUrl = backgroundUrl;
            Gender = gender;
            Status = status;
        }

        public void SetCustomerId(int id) { CustomerId = id; }
        public void SetUserId(int id) { UserId = id; }
        public void SetBirthDay(DateTime birthday) { BirthDay = birthday; }
        public void SetPersonalPhone(string phone) { PersonalPhone = phone; }
        public void SetPersonalName(string name) { PersonalName = name; }
        public void SetPersonalAddress(string address) { PersonalAddress = address; }
        public void SetAvatarUrl(string url) { AvatarUrl = url; }
        public void SetBackgroundUrl(string url) { BackgroundUrl = url; }
        public void SetGender(EGenderType gender) { Gender = gender; }

        public void SetStatus(EUserStatus status) { Status = status; }
    }
}

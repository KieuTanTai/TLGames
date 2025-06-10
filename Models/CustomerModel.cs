namespace TLGames.Models
{
    internal class CustomerModel
    {
        public int CustomerId { get; private set; }
        public int UserId { get; private set; }
        public int Age { get; private set; }
        public string PersonalPhone { get; private set; }
        public string PersonalName { get; private set; }
        public string PersonalAddress { get; private set; }
        public string AvatarURL { get; private set; }
        public string BackgroundURL { get; private set; }
        public string Biology { get; private set; }

        public CustomerModel() { }

        public CustomerModel(int customerId, int userId, int age, string personalPhone, string personalName,
                                string personalAddress, string avatarURL, string backgroundURL, string biology)
        {
            CustomerId = customerId;
            UserId = userId;
            Age = age;
            PersonalPhone = personalPhone;
            PersonalName = personalName;
            PersonalAddress = personalAddress;
            AvatarURL = avatarURL;
            BackgroundURL = backgroundURL;
            Biology = biology;
        }

        public void SetCustomerId(int id) { CustomerId = id; }
        public void SetUserId(int id) { UserId = id; }
        public void SetAge(int age) { Age = age; }
        public void SetPersonalPhone(string phone) { PersonalPhone = phone; }
        public void SetPersonalName(string name) { PersonalName = name; }
        public void SetPersonalAddress(string address) { PersonalAddress = address; }
        public void SetAvatarURL(string url) { AvatarURL = url; }
        public void SetBackgroundURL(string url) { BackgroundURL = url; }
        public void SetBiology(string bio) { Biology = bio; }
    }
}

using System;

namespace TLGames.Models
{
    internal class DeveloperModel
    {
        public int DeveloperId { get; private set; }
        public int UserId { get; private set; }
        public string DeveloperName { get; private set; }
        public DateTime BecameDeveloperDate { get; private set; }
        public string Description { get; private set; }
        public string WebsiteURL { get; private set; }
        public string StudioPhone { get; private set; }
        public string StudioAddress { get; private set; }
        public string StudioEmail { get; private set; }

        public DeveloperModel() { }

        public DeveloperModel(int developerId, int userId, string developerName, DateTime becameDeveloperDate,
                              string description, string websiteURL, string studioPhone, string studioAddress, string studioEmail)
        {
            DeveloperId = developerId;
            UserId = userId;
            DeveloperName = developerName;
            BecameDeveloperDate = becameDeveloperDate;
            Description = description;
            WebsiteURL = websiteURL;
            StudioPhone = studioPhone;
            StudioAddress = studioAddress;
            StudioEmail = studioEmail;
        }

        public void SetDeveloperId(int id) { DeveloperId = id; }
        public void SetUserId(int id) { UserId = id; }
        public void SetDeveloperName(string name) { DeveloperName = name; }
        public void SetBecameDeveloperDate(DateTime date) { BecameDeveloperDate = date; }
        public void SetDescription(string description) { Description = description; }
        public void SetWebsiteURL(string url) { WebsiteURL = url; }
        public void SetStudioPhone(string phone) { StudioPhone = phone; }
        public void SetStudioAddress(string address) { StudioAddress = address; }
        public void SetStudioEmail(string email) { StudioEmail = email; }
    }
}

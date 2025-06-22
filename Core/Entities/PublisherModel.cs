using System;
using TLGames.Core.Enums;

namespace TLGames.Core.Entities
{
    internal class PublisherModel
    {
        public int PublisherId { get; private set; }
        public int UserId { get; private set; }
        public string PublisherName { get; private set; }
        public DateTime BecamePublisherDate { get; private set; }
        public string Description { get; private set; }
        public string WebsiteUrl { get; private set; }
        public string BusinessPhone { get; private set; }
        public string BusinessAddress { get; private set; }
        public string BusinessEmail { get; private set; }
        public EUserStatus Status { get; private set; }

        public PublisherModel() { }

        public PublisherModel(int publisherId, int userId, string publisherName, DateTime becamePublisherDate,
                              string description, string websiteURL, string businessPhone, string businessAddress, string businessEmail, EUserStatus status)
        {
            PublisherId = publisherId;
            UserId = userId;
            PublisherName = publisherName;
            BecamePublisherDate = becamePublisherDate;
            Description = description;
            WebsiteUrl = websiteURL;
            BusinessPhone = businessPhone;
            BusinessAddress = businessAddress;
            BusinessEmail = businessEmail;
            Status = status;
        }

        public void SetPublisherId(int id) { PublisherId = id; }
        public void SetUserId(int id) { UserId = id; }
        public void SetPublisherName(string name) { PublisherName = name; }
        public void SetBecamePublisherDate(DateTime date) { BecamePublisherDate = date; }
        public void SetDescription(string description) { Description = description; }
        public void SetWebsiteUrl(string url) { WebsiteUrl = url; }
        public void SetBusinessPhone(string phone) { BusinessPhone = phone; }
        public void SetBusinessAddress(string address) { BusinessAddress = address; }
        public void SetBusinessEmail(string email) { BusinessEmail = email; }
        public void SetStatus(EUserStatus status) { Status = status; }
    }
}

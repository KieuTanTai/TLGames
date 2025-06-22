using System;
using TLGames.Core.Enums;

namespace TLGames.Core.Entities
{
    internal class SaleEventModel
    {
        public int SaleEventId { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime? EndDate { get; private set; }
        public string Name { get; private set; }
        public EActiveStatus Status { get; private set; }
        public string Description { get; private set; }
        public string DiscountCode { get; private set; }

        public SaleEventModel() { }

        public SaleEventModel(int saleEventId, DateTime startDate, DateTime endDate, string name, EActiveStatus status, string description, string discountCode)
        {
            SaleEventId = saleEventId;
            StartDate = startDate;
            EndDate = endDate;
            Name = name;
            Status = status;
            Description = description;
            DiscountCode = discountCode;
        }

        public void SetSaleEventId(int id) { SaleEventId = id; }
        public void SetStartDate(DateTime date) { StartDate = date; }
        public void SetEndDate(DateTime date) { EndDate = date; }
        public void SetName(string name) { Name = name; }
        public void SetStatus(EActiveStatus status) { Status = status; }
        public void SetDescription(string description) { Description = description; }
        public void SetDiscountCode(string code) { DiscountCode = code; }
    }
}

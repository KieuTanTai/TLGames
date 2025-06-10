using System;

namespace TLGames.Models
{
    internal class InvoiceModel
    {
        public int InvoiceId { get; private set; }
        public int CustomerId { get; private set; }
        public int PaymentMethodId { get; private set; }
        public decimal TotalPrice { get; private set; }
        public DateTime InvoiceDate { get; private set; }
        public string Status { get; private set; }

        public InvoiceModel() { }

        public InvoiceModel(int invoiceId, int customerId, int paymentMethodId, decimal totalPrice, DateTime invoiceDate, string status)
        {
            InvoiceId = invoiceId;
            CustomerId = customerId;
            PaymentMethodId = paymentMethodId;
            TotalPrice = totalPrice;
            InvoiceDate = invoiceDate;
            Status = status;
        }

        public void SetInvoiceId(int id) { InvoiceId = id; }
        public void SetCustomerId(int id) { CustomerId = id; }
        public void SetPaymentMethodId(int id) { PaymentMethodId = id; }
        public void SetTotalPrice(decimal price) { TotalPrice = price; }
        public void SetInvoiceDate(DateTime date) { InvoiceDate = date; }
        public void SetStatus(string status) { Status = status; }
    }
}

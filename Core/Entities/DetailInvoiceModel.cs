using TLGames.Core.Enums;

namespace TLGames.Core.Entities
{
    internal class DetailInvoiceModel
    {
        public int InvoiceId { get; private set; }
        public int ProductId { get; private set; }
        public int Quantity { get; private set; }
        public decimal Price { get; private set; }
        public EDetailStatusInvoice Status { get; private set; }

        public DetailInvoiceModel() { }

        public DetailInvoiceModel(int invoiceId, int productId, int quantity, decimal price, EDetailStatusInvoice status)
        {
            InvoiceId = invoiceId;
            ProductId = productId;
            Quantity = quantity;
            Price = price;
            Status = status;
        }

        public void SetInvoiceId(int id) { InvoiceId = id; }
        public void SetProductId(int id) { ProductId = id; }
        public void SetQuantity(int quantity) { Quantity = quantity; }
        public void SetPrice(decimal price) { Price = price; }
        public void SetStatus(EDetailStatusInvoice status) { Status = status; }
    }
}

namespace TLGames.Models
{
    internal class DetailInvoiceModel
    {
        public int InvoiceId { get; private set; }
        public string DiscountCode { get; private set; }
        public int ProductId { get; private set; }
        public int Quantity { get; private set; }
        public decimal Price { get; private set; }
        public string Type { get; private set; }

        public DetailInvoiceModel() { }

        public DetailInvoiceModel(int invoiceId, string discountCode, int productId, int quantity, decimal price, string type)
        {
            InvoiceId = invoiceId;
            DiscountCode = discountCode;
            ProductId = productId;
            Quantity = quantity;
            Price = price;
            Type = type;
        }

        public void SetInvoiceId(int id) { InvoiceId = id; }
        public void SetDiscountCode(string code) { DiscountCode = code; }
        public void SetProductId(int id) { ProductId = id; }
        public void SetQuantity(int quantity) { Quantity = quantity; }
        public void SetPrice(decimal price) { Price = price; }
        public void SetType(string type) { Type = type; }
    }
}

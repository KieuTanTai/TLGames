namespace TLGames.Core.Entities
{
    public class InvoiceDiscountCodeModel
    {
        public int InvoiceId { get; private set; }
        public string DiscountCode { get; private set; }

        public InvoiceDiscountCodeModel() { }
        public InvoiceDiscountCodeModel(int invoiceId, string discountCode)
        {
            InvoiceId = invoiceId;
            DiscountCode = discountCode;
        }

        public void SetDiscountCode(string code) { DiscountCode = code; }
        public void SetInvoiceId(int id) { InvoiceId = id; }

    }
}

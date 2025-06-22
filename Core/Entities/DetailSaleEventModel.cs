using TLGames.Core.Enums;

namespace TLGames.Core.Entities
{
    internal class DetailSaleEventModel
    {
        public int SaleEventId { get; private set; }
        public int ProductId { get; private set; }
        public EDiscountType DiscountType { get; private set; }
        public decimal DiscountPercent { get; private set; }
        public decimal DiscountAmount { get; private set; }
        public decimal MaxDiscountPrice { get; private set; }
        public decimal MinPriceToUse { get; private set; }

        public DetailSaleEventModel() { }

        public DetailSaleEventModel(int saleEventId, int productId, EDiscountType discountType,
                                    decimal discountPercent, decimal discountAmount, decimal maxDiscountPrice, decimal minPriceToUse)
        {
            SaleEventId = saleEventId;
            ProductId = productId;
            DiscountType = discountType;
            DiscountPercent = discountPercent;
            DiscountAmount = discountAmount;
            MaxDiscountPrice = maxDiscountPrice;
            MinPriceToUse = minPriceToUse;
        }

        public void SetSaleEventId(int id) { SaleEventId = id; }
        public void SetProductId(int id) { ProductId = id; }
        public void SetDiscountType(EDiscountType type) { DiscountType = type; }
        public void SetDiscountPercent(decimal percent) { DiscountPercent = percent; }
        public void SetDiscountAmount(decimal amount) { DiscountAmount = amount; }
        public void SetMaxDiscountPrice(decimal price) { MaxDiscountPrice = price; }
        public void SetMinPriceToUse(decimal price) { MinPriceToUse = price; }
    }
}

using System;
using TLGames.Core.Enums;

namespace TLGames.Core.Entities
{
    public class DetailCartModel
    {
        public int CartId { get; private set; }
        public int ProductId { get; private set; }
        public DateTime AddDate { get; private set; }
        public decimal Price { get; private set; }
        public ETypeItemCart Type { get; private set; }

        public DetailCartModel() { }

        public DetailCartModel(int cartId, int productId, DateTime addDate, decimal price, ETypeItemCart type)
        {
            CartId = cartId;
            ProductId = productId;
            AddDate = addDate;
            Price = price;
            Type = type;
        }

        public void SetCartId(int id) { CartId = id; }
        public void SetProductId(int id) { ProductId = id; }
        public void SetAddDate(DateTime date) { AddDate = date; }
        public void SetPrice(decimal price) { Price = price; }
        public void SetType(ETypeItemCart type) { Type = type; }
    }
}

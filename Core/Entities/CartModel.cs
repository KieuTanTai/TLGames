namespace TLGames.Core.Entities
{
    public class CartModel
    {
        public int CartId { get; private set; }
        public int CustomerId { get; private set; }
        public decimal TotalPrice { get; private set; }

        public CartModel() { }

        public CartModel(int cartId, int customerId, decimal totalPrice)
        {
            CartId = cartId;
            CustomerId = customerId;
            TotalPrice = totalPrice;
        }

        public void SetCartId(int id) { CartId = id; }
        public void SetCustomerId(int id) { CustomerId = id; }
        public void SetTotalPrice(decimal price) { TotalPrice = price; }
    }
}

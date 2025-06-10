namespace TLGames.Models
{
    internal class CartModel
    {
        public int CartId { get; private set; }
        public int UserId { get; private set; }
        public decimal TotalPrice { get; private set; }

        public CartModel() { }

        public CartModel(int cartId, int userId, decimal totalPrice)
        {
            CartId = cartId;
            UserId = userId;
            TotalPrice = totalPrice;
        }

        public void SetCartId(int id) { CartId = id; }
        public void SetUserId(int id) { UserId = id; }
        public void SetTotalPrice(decimal price) { TotalPrice = price; }
    }
}

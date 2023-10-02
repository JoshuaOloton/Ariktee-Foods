namespace ArikteeFoods.Web.Services
{
    public class ShoppingCartEventArgs : EventArgs
    {
        public readonly int TotalQuantity;

        public ShoppingCartEventArgs(int totalQuantity)
        {
            TotalQuantity = totalQuantity;
        }
    }
}

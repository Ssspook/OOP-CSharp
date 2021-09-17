namespace Shops.Entities
{
    public class CommoditySet
    {
        public CommoditySet()
        {
        }

        public CommoditySet(Commodity commodity, uint quantity, uint price)
        {
            Commodity = commodity;
            Quantity = quantity;
            Price = price;
        }

        public Commodity Commodity { get; }
        public uint Quantity { get; private set; }
        public uint Price { get; private set; }

        public void ChangePrice(uint newPrice)
        {
            Price = newPrice;
        }

        public void IncreaseQuantity(uint increaser)
        {
            Quantity += increaser;
        }

        public void DecreaseQuantity(uint decreasor)
        {
            Quantity -= decreasor;
        }
    }
}

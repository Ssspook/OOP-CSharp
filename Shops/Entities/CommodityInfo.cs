namespace Shops.Entities
{
    public class CommodityInfo
    {
        public CommodityInfo()
        {
        }

        public CommodityInfo(uint quantity, uint price)
        {
            Quantity = quantity;
            Price = price;
        }

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

using System.Collections.Generic;
using Shops.Tools;

namespace Shops.Entities
{
    public class Customer
    {
        private Dictionary<Commodity, CommodityInfo> _shoppingBasket = new Dictionary<Commodity, CommodityInfo>();

        public Customer(string name, uint money)
        {
            if (name == null)
                throw new ShopsException("Customer's name cannot be null");

            Money = money;
            Name = name;
        }

        public uint Money { get; private set; }
        public string Name { get; }

        public void AddToShoppingBasket(Commodity commodity, uint items, Shop shop)
        {
            if (commodity == null)
                throw new ShopsException("Commodity cannot be null");

            CommodityInfo commodityInfo = shop.FindCommoditySet(commodity);

            if (Money - (commodityInfo.Price * items) < 0)
                throw new ShopsException($"Customer {Name} doesn't have enough money to buy {commodity.Name} {commodity.Id}");

            _shoppingBasket.Add(commodity, commodityInfo);
            Money -= commodityInfo.Price * items;
        }
    }
}

using System.Collections.Generic;
using Shops.Tools;

namespace Shops.Entities
{
    public class Customer
    {
        private List<CommoditySet> _shoppingBasket = new List<CommoditySet>();

        public Customer(string name, uint money)
        {
            Money = money;
            Name = name;
        }

        public uint Money { get; private set; }
        public string Name { get; }

        public void AddToShoppingBasket(Commodity commodity, uint items, Shop shop)
        {
            CommoditySet commoditySet = shop.FindCommoditySet(commodity.Id);

            if (Money - (commoditySet.Price * items) < 0)
                throw new ShopsException($"Customer {Name} doesn't have enough money to buy {commodity.Name} {commodity.Id}");

            _shoppingBasket.Add(commoditySet);
            Money -= commoditySet.Price * items;
        }
    }
}

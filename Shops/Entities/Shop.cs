using System.Collections.Generic;
using System.Linq;
using Shops.Tools;

namespace Shops.Entities
{
    public class Shop
    {
        private static uint _shopsCount = 0;
        private List<CommoditySet> _commodities = new List<CommoditySet>();

        public Shop(string shopName, string address)
        {
            _shopsCount++;
            ShopId = _shopsCount;
            ShopName = shopName;
            ShopAddress = address;
        }

        public uint ShopId { get; }
        public string ShopName { get; }
        public string ShopAddress { get; }
        public uint ShopMoney { get; private set; }

        public void AddCommodity(uint id, uint quantity, uint price)
        {
            CommoditySet commoditySet = _commodities.SingleOrDefault(item => item.Commodity.Id == id);

            if (commoditySet == null)
                throw new ShopsException($"Commodity {id} is not registered yet");

            commoditySet.IncreaseQuantity(quantity);
            commoditySet.ChangePrice(price);
        }

        public CommoditySet BuyItem(Customer customer, Commodity commodity, uint positionsBought)
        {
            ItemCheck(commodity, this);

            CommoditySet commoditySet = _commodities.SingleOrDefault(item => item.Commodity.Id == commodity.Id);

            if (commoditySet.Quantity < positionsBought)
                throw new ShopsException($"Shop {ShopName} on {ShopAddress} doesn't have enough items of {commodity.Name} {commodity.Id} id");

            if (commoditySet.Price * positionsBought > customer.Money)
                throw new ShopsException($"Customer {customer.Name} doesn't have enough money to buy {commodity.Name} {commodity.Id} id of {positionsBought} items");

            commoditySet.DecreaseQuantity(positionsBought);
            customer.AddToShoppingBasket(commodity, positionsBought, this);

            ShopMoney += commoditySet.Price * positionsBought;

            return commoditySet;
        }

        public void ChangeItemPrice(Commodity commodity, uint newPrice)
        {
            ItemCheck(commodity, this);

            CommoditySet commoditySet = _commodities.SingleOrDefault(item => item.Commodity.Id == commodity.Id);

            commoditySet.ChangePrice(newPrice);
        }

        public void RegisterCommodity(Commodity commodity)
        {
            CommoditySet item = _commodities.SingleOrDefault(item => item.Commodity.Id == commodity.Id);

            if (item != null)
                throw new ShopsException($"Commodity {commodity.Name} with {commodity.Id} id is already registered in {ShopName} shop");

            var commoditySet = new CommoditySet(commodity, 0, 0);

            _commodities.Add(commoditySet);
        }

        public CommoditySet RegisterAndAddCommodity(Commodity commodity, uint quantity, uint price)
        {
            RegisterCommodity(commodity);
            AddCommodity(commodity.Id, quantity, price);

            return FindCommoditySet(commodity.Id);
        }

        public CommoditySet FindCommoditySet(uint id)
        {
            CommoditySet commoditySet = _commodities.SingleOrDefault(item => item.Commodity.Id == id);

            if (commoditySet == null)
                throw new ShopsException($"No commodity with {id} id found in the shop {ShopName}");

            return commoditySet;
        }

        private void ItemCheck(Commodity commodity, Shop shop)
        {
            CommoditySet commoditySet = _commodities.SingleOrDefault(item => item.Commodity.Id == commodity.Id);

            if (commoditySet.Quantity == 0)
                throw new ShopsException($"There is not enough of {commodity.Name} {commodity.Id} in {shop.ShopName} on {shop.ShopAddress}");

            if (!_commodities.Contains(commoditySet))
                throw new ShopsException($"There is no of {commodity.Name} {commodity.Id} registered in {shop.ShopName} on {shop.ShopAddress}");
        }
    }
}

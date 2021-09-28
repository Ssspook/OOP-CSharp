using System.Collections.Generic;
using Shops.Tools;

namespace Shops.Entities
{
    public class Shop
    {
        private static uint _shopsCount = 0;
        private Dictionary<Commodity, CommodityInfo> _commodities = new Dictionary<Commodity, CommodityInfo>();

        public Shop(string shopName, string address)
        {
            if (shopName == null)
                throw new ShopsException("Shop name cannot be null");

            _shopsCount++;
            ShopId = _shopsCount;
            ShopName = shopName;
            ShopAddress = address;
        }

        public uint ShopId { get; }
        public string ShopName { get; }
        public string ShopAddress { get; }
        public uint ShopMoney { get; private set; }

        public void AddCommodity(Commodity commodity, uint quantity, uint price)
        {
            if (!_commodities.ContainsKey(commodity))
                throw new ShopsException($"Commodity {commodity.Id} is not registered yet");

            _commodities[commodity].IncreaseQuantity(quantity);
            _commodities[commodity].ChangePrice(price);
        }

        public CommodityInfo BuyItem(Customer customer, Commodity commodity, uint positionsBought)
        {
            if (customer == null)
                throw new ShopsException("Customer cannot be null");

            CommodityNullCheck(commodity);

            CommodityAvailabilityCheck(commodity, this);

            CommodityInfo commodityInfo = _commodities[commodity];

            if (commodityInfo.Quantity < positionsBought)
                throw new ShopsException($"Shop {ShopName} on {ShopAddress} doesn't have enough items of {commodity.Name} {commodity.Id} id");

            if (commodityInfo.Price * positionsBought > customer.Money)
                throw new ShopsException($"Customer {customer.Name} doesn't have enough money to buy {commodity.Name} {commodity.Id} id of {positionsBought} items");

            commodityInfo.DecreaseQuantity(positionsBought);

            customer.AddToShoppingBasket(commodity, positionsBought, this);

            ShopMoney += commodityInfo.Price * positionsBought;

            return commodityInfo;
        }

        public void ChangeItemPrice(Commodity commodity, uint newPrice)
        {
            CommodityNullCheck(commodity);

            CommodityAvailabilityCheck(commodity, this);

            _commodities[commodity].ChangePrice(newPrice);
        }

        public void RegisterCommodity(Commodity commodity)
        {
            CommodityNullCheck(commodity);

            if (_commodities.ContainsKey(commodity))
                throw new ShopsException($"Commodity {commodity.Name} with {commodity.Id} id is already registered in {ShopName} shop");

            _commodities.Add(commodity, new CommodityInfo(0, 0));
        }

        public CommodityInfo RegisterAndAddCommodity(Commodity commodity, uint quantity, uint price)
        {
            CommodityNullCheck(commodity);

            RegisterCommodity(commodity);
            AddCommodity(commodity, quantity, price);

            return FindCommoditySet(commodity);
        }

        public CommodityInfo FindCommoditySet(Commodity commodity)
        {
            if (!_commodities.ContainsKey(commodity))
                return null;

            return _commodities[commodity];
        }

        private void CommodityAvailabilityCheck(Commodity commodity, Shop shop)
        {
            if (!_commodities.ContainsKey(commodity))
                throw new ShopsException($"There is no of {commodity.Name} {commodity.Id} registered in {shop.ShopName} on {shop.ShopAddress}");

            if (_commodities[commodity].Quantity == 0)
                throw new ShopsException($"There is not enough of {commodity.Name} {commodity.Id} in {shop.ShopName} on {shop.ShopAddress}");
        }

        private void CommodityNullCheck(Commodity commodity)
        {
            if (commodity == null)
                throw new ShopsException("Commodity cannot be null");
        }
    }
}
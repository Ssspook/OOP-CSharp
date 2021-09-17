using System.Collections.Generic;
using System.Linq;
using Shops.Entities;
using Shops.Tools;

namespace Shops.EntitiesManagement
{
    public class ShopsManager
    {
        public List<Shop> Shops { get; } = new List<Shop>();

        public Shop CreateShop(string name, string address)
        {
            var shop = new Shop(name, address);
            Shops.Add(shop);

            return shop;
        }

        public Shop FindShopWithTheMostPrifotableSet(Commodity commodity, uint itemsToBuy)
        {
            var shops = Shops.OrderBy(shop => shop.FindCommoditySet(commodity.Id).Price * itemsToBuy).ToList();

            if (shops[0] == null)
                throw new ShopsException($"Commodity {commodity.Name} is not registered in any of the shops");

            if (shops[0].FindCommoditySet(commodity.Id).Quantity < itemsToBuy)
                throw new ShopsException($"Not enough of {commodity.Name} of {commodity.Id} id in any shops");

            return shops[0];
        }
    }
}

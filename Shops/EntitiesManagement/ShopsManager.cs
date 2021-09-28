using System.Collections.Generic;
using System.Linq;
using Shops.Entities;
using Shops.Tools;

namespace Shops.EntitiesManagement
{
    public class ShopsManager
    {
        private List<Shop> _shops = new List<Shop>();

        public Shop RegisterShop(Shop shop)
        {
            if (shop == null)
                throw new ShopsException("Shop cannot be null");

            _shops.Add(shop);

            return shop;
        }

        public Shop FindShopWithTheMostPrifotableSet(Commodity commodity, uint itemsToBuy)
        {
            if (commodity == null)
                throw new ShopsException("Commodity cannot be null");

            Shop shop = _shops.Where(shop => shop.FindCommoditySet(commodity) != null && shop.FindCommoditySet(commodity).Quantity >= itemsToBuy).OrderBy(shop => shop.FindCommoditySet(commodity).Price * itemsToBuy).FirstOrDefault();

            if (shop == null)
                return null;

            return shop;
        }
    }
}
using NUnit.Framework;
using Shops.Entities;
using Shops.EntitiesManagement;
using Shops.Tools;

namespace Shops.Tests
{
    [TestFixture]
    public class ShopsTests
    {
        private ShopsManager _shopManager;

        private Shop okayShop = new Shop("O'Kay", "Street 1, building 1");
        private Shop prizmaShop1 = new Shop("Prizma", "Street 2, building 2");
        private Shop prizmaShop2 = new Shop("Prizma", "Street 3, building 3");

        [SetUp]
        public void Setup()
        {
            _shopManager = new ShopsManager();
            okayShop = _shopManager.RegisterShop(okayShop);
            prizmaShop1 = _shopManager.RegisterShop(prizmaShop1);
            prizmaShop2 = _shopManager.RegisterShop(prizmaShop2);
        }
    
        [Test]
        public void RegisterAndAddCommodity_CommoditiesAreAvailableToBuy()
        {
            var whiteRice = new Commodity("White Rice");

            var customer = new Customer("John Doe", 1000);

            uint initialWhiteRiceQuantity = 7;

            uint positionsToBuy = 3;

            okayShop.RegisterAndAddCommodity(whiteRice, 7, 200);

            CommodityInfo commodityInfo = okayShop.BuyItem(customer, whiteRice, positionsToBuy);

            Assert.AreEqual(commodityInfo.Quantity, initialWhiteRiceQuantity - positionsToBuy);
        }

        [Test]
        public void SetAndChangePrice_PriceSuccessfullySetAndChanged()
        { 
            var whiteRice = new Commodity("White Rice");

            uint initialWhiteRicePrice = 100;
            uint newWhiteRicePrice = 500;
           
            CommodityInfo commodityInfo = okayShop.RegisterAndAddCommodity(whiteRice, 5, initialWhiteRicePrice);

            commodityInfo.ChangePrice(newWhiteRicePrice);

            Assert.AreNotEqual(initialWhiteRicePrice, commodityInfo.Price);
        }

        [Test]
        public void BuySetOfCommoditiesWithMaximumPofit_ShopFound()
        {
            var donut = new Commodity("Donut");

            okayShop.RegisterAndAddCommodity(donut, 7, 100);
            prizmaShop1.RegisterAndAddCommodity(donut, 5, 100);
            prizmaShop2.RegisterAndAddCommodity(donut, 10, 80);

            Assert.AreEqual(prizmaShop2, _shopManager.FindShopWithTheMostPrifotableSet(donut, 7));
        }

        [Test]
        public void CommoditySetNotRegistered_ReturnNull()
        {
            var brownRice = new Commodity("Brown Rice");

            Assert.AreEqual(_shopManager.FindShopWithTheMostPrifotableSet(brownRice, 39), null);
        }

        [Test]
        public void NotEnoughCommoditiesInAnyShops_ReturnNull()
        {
            var whiteRice = new Commodity("White Rice");

            okayShop.RegisterAndAddCommodity(whiteRice, 7, 100);
            prizmaShop1.RegisterAndAddCommodity(whiteRice, 5, 100);
            prizmaShop2.RegisterAndAddCommodity(whiteRice, 10, 80);

            Assert.AreEqual(_shopManager.FindShopWithTheMostPrifotableSet(whiteRice, 13), null);
        }


        [Test]
        public void BuySetOfCommodities_MoneyTrasfered()
        {
            var customer = new Customer("John Doe", 1000);

            uint initialCustomersMoney = customer.Money;
            uint initialShopMoney = okayShop.ShopMoney;
            
            var peanutButter = new Commodity("Peanut Butter");
            okayShop.RegisterAndAddCommodity(peanutButter, 20, 70);

            uint initialCommodityQuantity = okayShop.FindCommoditySet(peanutButter).Quantity;

            okayShop.BuyItem(customer, peanutButter, 1);

            Assert.AreEqual(customer.Money + 70, initialCustomersMoney);
            Assert.AreEqual(okayShop.ShopMoney - 70, initialShopMoney);
            Assert.AreEqual(okayShop.FindCommoditySet(peanutButter).Quantity + 1, initialCommodityQuantity);
        }

        [Test]
        public void NotEnoughItems_ThrowException()
        {
            var customer = new Customer("John Doe", 1000);
            var donut = new Commodity("Donut");

            okayShop.RegisterAndAddCommodity(donut, 7, 100);

            Assert.Catch<ShopsException>(() =>
            {
                okayShop.BuyItem(customer, donut, 20);
            });
            
        }

        [Test]
        public void NotEnoughMoney_ThrowException()
        {
            var customer = new Customer("John Doe", 10);
            var donut = new Commodity("Donut");

            okayShop.RegisterAndAddCommodity(donut, 7, 100);

            Assert.Catch<ShopsException>(() =>
            {
                okayShop.BuyItem(customer, donut, 7);
            });
            
        }
    }
}
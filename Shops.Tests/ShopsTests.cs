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

        private Shop okayShop;
        private Shop prizmaShop1;
        private Shop prizmaShop2;

        [SetUp]
        public void Setup()
        {
            _shopManager = new ShopsManager();
            okayShop = _shopManager.CreateShop("Okay", "Street 1, Building 1");
            prizmaShop1 = _shopManager.CreateShop("Prizma", "Street 2, Building 2");
            prizmaShop2 = _shopManager.CreateShop("Prizma", "Street 3, Building 3");
        }
    
        [Test]
        public void RegisterAndAddCommodity_CommoditiesAreAvailableToBuy()
        {
            var whiteRice = new Commodity("White Rice");

            var customer = new Customer("John Doe", 1000);

            uint initialWhiteRiceQuantity = 7;

            uint positionsToBuy = 3;

            okayShop.RegisterAndAddCommodity(whiteRice, 7, 200);

            CommoditySet commoditySet = okayShop.BuyItem(customer, whiteRice, positionsToBuy);

            Assert.AreEqual(commoditySet.Quantity, initialWhiteRiceQuantity - positionsToBuy);
        }

        [Test]
        public void SetAndChangePrice_PriceSuccessfullySetAndChanged()
        { 
            var whiteRice = new Commodity("White Rice");

            uint initialWhiteRicePrice = 100;
            uint newWhiteRicePrice = 500;
           
            CommoditySet commoditySet = okayShop.RegisterAndAddCommodity(whiteRice, 5, initialWhiteRicePrice);

            commoditySet.ChangePrice(newWhiteRicePrice);

            Assert.AreNotEqual(initialWhiteRicePrice, commoditySet.Price);
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
        public void CommoditySetNotRegistered_TrowException()
        {
            Assert.Catch<ShopsException>(() =>
            {
                var brownRice = new Commodity("Brown Rice");

                _shopManager.FindShopWithTheMostPrifotableSet(brownRice, 39);
            });
        }

        [Test]
        public void NotEnoughCommoditiesInAnyShops_ThrowException()
        {
            var whiteRice = new Commodity("White Rice");

            okayShop.RegisterAndAddCommodity(whiteRice, 7, 100);
            prizmaShop1.RegisterAndAddCommodity(whiteRice, 5, 100);
            prizmaShop2.RegisterAndAddCommodity(whiteRice, 10, 80);

            Assert.Catch<ShopsException>(() =>
            {
                _shopManager.FindShopWithTheMostPrifotableSet(whiteRice, 13);
            });
        }


        [Test]
        public void BuySetOfCommodities_MoneyTrasfered()
        {
            var customer = new Customer("John Doe", 1000);

            uint initialCustomersMoney = customer.Money;
            uint initialShopMoney = okayShop.ShopMoney;
            
            var peanutButter = new Commodity("Peanut Butter");
            okayShop.RegisterAndAddCommodity(peanutButter, 20, 70);

            uint initialCommodityQuantity = okayShop.FindCommoditySet(peanutButter.Id).Quantity;

            okayShop.BuyItem(customer, peanutButter, 1);

            Assert.AreEqual(customer.Money + 70, initialCustomersMoney);
            Assert.AreEqual(okayShop.ShopMoney - 70, initialShopMoney);
            Assert.AreEqual(okayShop.FindCommoditySet(peanutButter.Id).Quantity + 1, initialCommodityQuantity);
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

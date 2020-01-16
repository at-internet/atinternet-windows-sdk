using ATInternet;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Linq;

namespace TrackerTests
{
    [TestClass]
    public class CartTest : AbstractTest
    {
        Cart cart;

        [TestInitialize]
        public new void setUp()
        {
            base.setUp();
            cart = new Cart(tracker);
        }

        [TestMethod]
        public void initTest()
        {
            Assert.IsNull(cart.CartId);
        }

        [TestMethod]
        public void setEvent()
        {
            cart.CartId = "898";
            Product p = cart.Products.Add("prod");
            p.Category1 = "1";
            p.Category2 = "2";
            p.Category3 = "3";
            p.Category4 = "4";
            p.Category5 = "5";
            p.Category6 = "6";
            p.Quantity = 1;
            p.UnitPriceTaxFree = 4;
            p.UnitPriceTaxIncluded = 5;
            p.DiscountTaxFree = 1;
            p.DiscountTaxIncluded = 1.56;
            p.PromotionalCode = "promo";

            int index = 0;

            cart.SetEvent();
            Assert.AreEqual(8, tracker.buffer.volatileParameters.Count);

            Assert.AreEqual("idcart", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("898", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("pdt1", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("1::2::3::4::5::6::prod", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("qte1", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("1", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("mtht1", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("4", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("mt1", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("5", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("dscht1", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("1", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("dsc1", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("1.56", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("pcode1", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("promo", tracker.buffer.volatileParameters[index++].value());

        }
    }
}

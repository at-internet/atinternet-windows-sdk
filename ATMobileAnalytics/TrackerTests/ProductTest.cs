using ATInternet;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Linq;

namespace TrackerTests
{
    [TestClass]
    public class ProductTest : AbstractTest
    {
        Product p;
        Products psTracker;
        Products psCart;

        [TestInitialize]
        public new void setUp()
        {
            base.setUp();
            p = new Product(tracker);
            psTracker = new Products(tracker);
            psCart = new Products(tracker.Cart);
        }

        [TestMethod]
        public void initTest()
        {
            Assert.AreEqual(string.Empty, p.ProductId);
        }

        [TestMethod]
        public void setEventTest()
        {
            p.ProductId = "ID";
            p.Category5 = "5";
            p.SetEvent();
            Assert.AreEqual(2, tracker.buffer.volatileParameters.Count);

            Assert.AreEqual("type", tracker.buffer.volatileParameters[0].key);
            Assert.AreEqual("pdt", tracker.buffer.volatileParameters[0].value());

            Assert.AreEqual("pdtl", tracker.buffer.volatileParameters[1].key);
            Assert.AreEqual("5::ID", tracker.buffer.volatileParameters[1].value());
        }

        [TestMethod]
        public void addTest()
        {
            Product p = new Product(tracker);
            p.ProductId = "test";
            Assert.AreEqual(0, tracker.businessObjects.Count);
            Assert.AreEqual(0, tracker.Cart.productsList.Count);

            psTracker.Add(p);

            Assert.AreEqual(1, tracker.businessObjects.Count);
            Assert.AreEqual(0, tracker.Cart.productsList.Count);

            psCart.Add(p);

            Assert.AreEqual(1, tracker.businessObjects.Count);
            Assert.AreEqual(1, tracker.Cart.productsList.Count);
        }

        [TestMethod]
        public void removeTest()
        {
            Product p = new Product(tracker);
            p.ProductId = "test";
            Product p2 = new Product(tracker);
            p2.ProductId = "tutu";

            psCart.Add(p);
            psTracker.Add(p);
            psCart.Add(p2);
            psTracker.Add(p2);

            Assert.AreEqual(2, tracker.businessObjects.Count);
            Assert.AreEqual(2, tracker.Cart.productsList.Count);

            psTracker.Remove("test");
            psTracker.Remove("totp");
            psCart.Remove("tutu");

            Assert.AreEqual(1, tracker.businessObjects.Count);
            Assert.AreEqual(1, tracker.Cart.productsList.Count);
            Assert.AreEqual("tutu", (tracker.businessObjects[p2.id] as Product).ProductId);
            Assert.AreEqual("test", tracker.Cart.productsList[0].ProductId);
        }

        [TestMethod]
        public void removeAllTest()
        {
            Product p = new Product(tracker);
            p.ProductId = "test";
            Product p2 = new Product(tracker);
            p2.ProductId = "tutu";

            psCart.Add(p);
            psTracker.Add(p);
            psCart.Add(p2);
            psTracker.Add(p2);

            Assert.AreEqual(2, tracker.businessObjects.Count);
            Assert.AreEqual(2, tracker.Cart.productsList.Count);

            psTracker.RemoveAll();
            psCart.RemoveAll();

            Assert.AreEqual(0, tracker.businessObjects.Count);
            Assert.AreEqual(0, tracker.Cart.productsList.Count);
        }
    }
}

using ATInternet;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Linq;

namespace TrackerTests
{
    [TestClass]
    public class OrderTest : AbstractTest
    {
        Order o;
        Orders os;

        [TestInitialize]
        public new void setUp()
        {
            base.setUp();
            o = new Order(tracker);
            os = new Orders(tracker);
        }

        [TestMethod]
        public void initTest()
        {
            Assert.AreEqual(string.Empty, o.OrderId);
            Assert.AreEqual(-1, o.Turnover);
            Assert.AreEqual(-1, o.Status);
            Assert.AreEqual(-1, o.PaymentMethod);
            Assert.IsFalse(o.NewCustomer);
            Assert.IsFalse(o.ConfirmationRequired);
        }

        [TestMethod]
        public void amountTest()
        {
            OrderAmount am= o.Amount;
            Assert.AreEqual(am, o.Amount);
        }

        [TestMethod]
        public void discountTest()
        {
            OrderDiscount dsc = o.Discount;
            Assert.AreEqual(dsc, o.Discount);
        }

        [TestMethod]
        public void deliveryTest()
        {
            OrderDelivery dl = o.Delivery;
            Assert.AreEqual(dl, o.Delivery);
        }

        [TestMethod]
        public void customVarsTest()
        {
            OrderCustomVars ocvs = o.CustomVars;
            Assert.AreEqual(ocvs, o.CustomVars);
        }

        [TestMethod]
        public void setEventTest()
        {
            o.OrderId = "444";
            o.Amount.Set(55.2, 63.2, 8).CustomVars.Add(2, "cv");
            o.SetEvent();
            int index = 0;

            Assert.AreEqual(6, tracker.buffer.volatileParameters.Count);

            Assert.AreEqual("cmd", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("444", tracker.buffer.volatileParameters[index++].value());
            Assert.AreEqual("newcus", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("0", tracker.buffer.volatileParameters[index++].value());
            Assert.AreEqual("mtht", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("55.2", tracker.buffer.volatileParameters[index++].value());
            Assert.AreEqual("mtttc", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("63.2", tracker.buffer.volatileParameters[index++].value());
            Assert.AreEqual("tax", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("8", tracker.buffer.volatileParameters[index++].value());
            Assert.AreEqual("o2", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("cv", tracker.buffer.volatileParameters[index++].value());
        }

        [TestMethod]
        public void addTest()
        {
            o = os.Add("guid", 89.59);
            Assert.AreEqual(1, tracker.businessObjects.Count);

            Assert.AreEqual("guid", (tracker.businessObjects[o.id] as Order).OrderId);
            Assert.AreEqual(89.59, (tracker.businessObjects[o.id] as Order).Turnover);
        }
    }
}

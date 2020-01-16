using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using ATInternet;

namespace TrackerTests
{
    [TestClass]
    public class ParameterTests
    {
        [TestMethod]
        public void multiInstanceTest()
        {
            Param page = new Param("p", (() => "Home"), Param.Type.String);
            Param stc = new Param("stc", (() => "toto"), Param.Type.JSON);
            Assert.AreNotEqual(page, stc);
        }

        [TestMethod]
        public void defaultInitTest()
        {
            Param page = new Param();
            Assert.AreEqual(Param.Type.Unknown, page.type);
            Assert.AreEqual(string.Empty, page.key);
            Assert.IsNull(page.value);
            Assert.IsNull(page.paramOptions);
        }

        [TestMethod]
        public void initWithKeyValueTest()
        {
            Param page = new Param("p", (() => "home"), Param.Type.String);
            Assert.AreEqual(Param.Type.String, page.type);
            Assert.AreEqual("p", page.key);
            Assert.AreEqual("home", page.value());
        }

        [TestMethod]
        public void initFirstPositionTest()
        {
            Param page = new Param("p", (() => "home"), Param.Type.String, new ParamOption() { RelativePosition = RelativePosition.First});
            Assert.AreEqual(RelativePosition.First, page.paramOptions.RelativePosition);
            Assert.AreEqual(string.Empty, page.paramOptions.RelativeParameterKey);
        }

        [TestMethod]
        public void initLastPositionTest()
        {
            Param page = new Param("p", (() => "home"), Param.Type.String, new ParamOption() { RelativePosition = RelativePosition.Last });
            Assert.AreEqual(RelativePosition.Last, page.paramOptions.RelativePosition);
            Assert.AreEqual(string.Empty, page.paramOptions.RelativeParameterKey);
        }

        [TestMethod]
        public void initBeforeStcPositionTest()
        {
            Param page = new Param("p", (() => "home"), Param.Type.String, new ParamOption() { RelativePosition = RelativePosition.Before, RelativeParameterKey = "stc" });
            Assert.AreEqual(RelativePosition.Before, page.paramOptions.RelativePosition);
            Assert.AreEqual("stc", page.paramOptions.RelativeParameterKey);
        }

        [TestMethod]
        public void initAfterStcPositionTest()
        {
            Param page = new Param("p", (() => "home"), Param.Type.String, new ParamOption() { RelativePosition = RelativePosition.After, RelativeParameterKey = "stc" });
            Assert.AreEqual(RelativePosition.After, page.paramOptions.RelativePosition);
            Assert.AreEqual("stc", page.paramOptions.RelativeParameterKey);
        }

        [TestMethod]
        public void sliceReadyListTest()
        {
            Assert.AreEqual(4, SliceReadyParam.list.Count);
            Assert.IsTrue(SliceReadyParam.list.Contains("stc"));
            Assert.IsTrue(SliceReadyParam.list.Contains("ati"));
            Assert.IsTrue(SliceReadyParam.list.Contains("atc"));
            Assert.IsTrue(SliceReadyParam.list.Contains("pdtl"));
        }

        [TestMethod]
        public void readOnlyListTest()
        {
            Assert.AreEqual(12, ReadOnlyParam.list.Count);
            Assert.IsTrue(ReadOnlyParam.list.Contains("vtag"));
            Assert.IsTrue(ReadOnlyParam.list.Contains("lng"));
            Assert.IsTrue(ReadOnlyParam.list.Contains("mfmd"));
            Assert.IsTrue(ReadOnlyParam.list.Contains("manufacturer"));
            Assert.IsTrue(ReadOnlyParam.list.Contains("model"));
            Assert.IsTrue(ReadOnlyParam.list.Contains("os"));
            Assert.IsTrue(ReadOnlyParam.list.Contains("apvr"));
            Assert.IsTrue(ReadOnlyParam.list.Contains("hl"));
            Assert.IsTrue(ReadOnlyParam.list.Contains("car"));
            Assert.IsTrue(ReadOnlyParam.list.Contains("cn"));
            Assert.IsTrue(ReadOnlyParam.list.Contains("ts"));
            Assert.IsTrue(ReadOnlyParam.list.Contains("olt"));
        }
    }
}

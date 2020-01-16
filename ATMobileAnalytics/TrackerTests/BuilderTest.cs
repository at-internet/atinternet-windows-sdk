using ATInternet;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Collections.Generic;

namespace TrackerTests
{
    [TestClass]
    public class BuilderTest : AbstractTest
    {
        Builder builder;

        Param intP;
        Param floatP;
        Param boolP;
        Param stringP;
        Param dicoP;
        Param listP;

        [TestInitialize]
        public new void setUp()
        {
            base.setUp();
            tracker.buffer.persistentParameters.Clear();
            tracker.buffer.volatileParameters.Add(new Param()
            {
                key = "ref",
                value = (() => "www.atinternet.com?test1=1&test2=2&test3=<script></script>")
            });

            stringP = new Param("p", (() => "Home"), Param.Type.String);
            floatP = new Param("float", (() => 3.154.ToString()), Param.Type.Float);
            intP = new Param("int", (() => 56.ToString()), Param.Type.Integer);
            boolP = new Param("bool", (() => false.ToString()), Param.Type.Boolean);
            dicoP = new Param("dico", (() => Tool.ConvertToString(new Dictionary<string, object>() {
                { "voiture","citroen"}
            })), Param.Type.JSON);
            listP = new Param("list", (() => Tool.ConvertToString(new List<object>() { 1, true, "test" })), Param.Type.Array);
        }

        [TestMethod]
        public void buildConfigTest()
        {
            string config = new Builder(tracker).BuildConfiguration();
            Assert.AreEqual("http://logp.xiti.com/hit.xiti?s=564360", config);
        }

        [TestMethod]
        public void buildFloatTest()
        {
            tracker.buffer.volatileParameters.Add(floatP);
            string hit = (new Builder(tracker).Build()[0] as List<string>)[0];

            Assert.AreEqual("float=3.154", hit.Split('&')[1]);
            Assert.AreEqual("ref=www.atinternet.com?test1=1$test2=2$test3=script/script", hit.Split('&')[2]);
        }

        [TestMethod]
        public void buildIntTest()
        {
            tracker.buffer.volatileParameters.Add(intP);
            string hit = (new Builder(tracker).Build()[0] as List<string>)[0];

            Assert.AreEqual("int=56", hit.Split('&')[1]);
            Assert.AreEqual("ref=www.atinternet.com?test1=1$test2=2$test3=script/script", hit.Split('&')[2]);
        }

        [TestMethod]
        public void buildBoolTest()
        {
            tracker.buffer.volatileParameters.Add(boolP);
            string hit = (new Builder(tracker).Build()[0] as List<string>)[0];

            Assert.AreEqual("bool=False", hit.Split('&')[1]);
            Assert.AreEqual("ref=www.atinternet.com?test1=1$test2=2$test3=script/script", hit.Split('&')[2]);
        }

        [TestMethod]
        public void buildDicoTest()
        {
            tracker.buffer.volatileParameters.Add(dicoP);
            string hit = (new Builder(tracker).Build()[0] as List<string>)[0];

            Assert.AreEqual("dico={\"voiture\":\"citroen\"}", hit.Split('&')[1]);
        }

        [TestMethod]
        public void buildListTest()
        {
            tracker.buffer.volatileParameters.Add(listP);
            string hit = (new Builder(tracker).Build()[0] as List<string>)[0];

            Assert.AreEqual("list=1,True,test", hit.Split('&')[1]);
            Assert.AreEqual("ref=www.atinternet.com?test1=1$test2=2$test3=script/script", hit.Split('&')[2]);
        }

        [TestMethod]
        public void buildListWithOptionsTest()
        {
            listP = new Param("list", (() => Tool.ConvertToString(new List<object>() { 1, true, "test" }, "#")), Param.Type.Array);
            tracker.buffer.volatileParameters.Add(listP);
            string hit = (new Builder(tracker).Build()[0] as List<string>)[0];

            Assert.AreEqual("list=1#True#test", hit.Split('&')[1]);
            Assert.AreEqual("ref=www.atinternet.com?test1=1$test2=2$test3=script/script", hit.Split('&')[2]);
        }

        [TestMethod]
        public void multiHitsFailedNotSplittableParamTest()
        {
            List<string> array = new List<string>();
            for (int i = 1; i <= 150; i++)
            {
                array.Add("verybigvalue" + i);
            }
            tracker.buffer.volatileParameters.Add(new Param("toto", (() => Tool.ConvertToString(array)), Param.Type.Array));
            builder = new Builder(tracker);

            List<string> hits = builder.Build()[0] as List<string>;
            string hit = hits[0];
            Assert.IsTrue(hit.Contains("mherr=1"));
            Assert.AreEqual(1, hits.Count);
        }

        [TestMethod]
        public void multiHitsFailedNotSplittableValueTest()
        {
            string s = "";
            for (int i = 1; i <= 130; i++)
            {
                s += "verybigvalue";
            }
            tracker.buffer.volatileParameters.Add(new Param("stc", (() => s), Param.Type.String));
            builder = new Builder(tracker);

            List<string> hits = builder.Build()[0] as List<string>;
            string hit = hits[0];
            Assert.IsTrue(hit.Contains("mherr=1"));
            Assert.AreEqual(1, hits.Count);
        }

        [TestMethod]
        public void multiHitsOkSplittableParamTest()
        {
            List<string> array = new List<string>();
            for (int i = 1; i <= 300; i++)
            {
                array.Add("verybigvalue" + i);
            }
            tracker.buffer.volatileParameters.Add(new Param("stc", (() => Tool.ConvertToString(array, "#")), Param.Type.Array, new ParamOption() { Separator = "#" }));
            builder = new Builder(tracker);

            List<string> hits = builder.Build()[0] as List<string>;
            Assert.IsTrue(hits.Count == 4);
            Assert.IsTrue(hits[0].Contains("&mh=1-4-") && !hits[0].Contains("mherr=1"));
            Assert.IsTrue(hits[1].Contains("&mh=2-4-") && !hits[1].Contains("mherr=1"));
            Assert.IsTrue(hits[2].Contains("&mh=3-4-") && !hits[2].Contains("mherr=1"));
            Assert.IsTrue(hits[3].Contains("&mh=4-4-") && !hits[3].Contains("mherr=1"));
        }

        [TestMethod]
        public void multiHitsOkSplittableHitTest()
        {
            List<string> array = new List<string>();
            for (int i = 1; i <= 200; i++)
            {
                tracker.buffer.volatileParameters.Add(new Param("verybigkey" + i, (() => "verybigvalue" + i), Param.Type.String));
            }
            builder = new Builder(tracker);

            List<string> hits = builder.Build()[0] as List<string>;
            Assert.IsTrue(hits.Count == 5);
            Assert.IsTrue(hits[0].Contains("&mh=1-5-") && !hits[0].Contains("mherr=1"));
            Assert.IsTrue(hits[1].Contains("&mh=2-5-") && !hits[1].Contains("mherr=1"));
            Assert.IsTrue(hits[2].Contains("&mh=3-5-") && !hits[2].Contains("mherr=1"));
            Assert.IsTrue(hits[3].Contains("&mh=4-5-") && !hits[3].Contains("mherr=1"));
            Assert.IsTrue(hits[4].Contains("&mh=5-5-") && !hits[4].Contains("mherr=1"));
        }

        [TestMethod]
        public void makeSubQueryTest()
        {
            Assert.AreEqual("&p=toto", new Builder(tracker).MakeSubQuery("p", "toto"));
        }

        [TestMethod]
        public void organizeParametersTest()
        {
            stringP = new Param("p", (() => "Home"), Param.Type.String, new ParamOption() { RelativePosition = RelativePosition.Last });
            floatP = new Param("float", (() => 3.154.ToString()), Param.Type.Float, new ParamOption() { RelativePosition = RelativePosition.First });
            intP = new Param("int", (() => 56.ToString()), Param.Type.Integer, new ParamOption() { RelativePosition = RelativePosition.Before, RelativeParameterKey = "bool" });
            boolP = new Param("bool", (() => false.ToString()), Param.Type.Boolean);
            listP = new Param("list", (() => Tool.ConvertToString(new List<object>() { 1, true, "test" })), Param.Type.Array, new ParamOption() { RelativePosition = RelativePosition.After, RelativeParameterKey = "int" });

            tracker.buffer.volatileParameters.Add(stringP);
            tracker.buffer.volatileParameters.Add(floatP);
            tracker.buffer.volatileParameters.Add(intP);
            tracker.buffer.volatileParameters.Add(boolP);
            tracker.buffer.volatileParameters.Add(listP);

            List<Param> organizedParams = new Builder(tracker).OrganizeParameters(tracker.buffer.volatileParameters);

            Assert.IsTrue(organizedParams.Count == 6);
            Assert.AreEqual("float", organizedParams[0].key);
            Assert.AreEqual("int", organizedParams[1].key);
            Assert.AreEqual("list", organizedParams[2].key);
            Assert.AreEqual("bool", organizedParams[3].key);
            Assert.AreEqual("p", organizedParams[4].key);
            Assert.AreEqual("ref", organizedParams[5].key);
        }
    }
}

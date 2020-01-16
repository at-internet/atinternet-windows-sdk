using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using ATInternet;

namespace TrackerTests
{
    [TestClass]
    public class ScreenTest : AbstractTest
    {
        Screen screen;
        DynamicScreen dynamicScreen;

        Screens screens;
        DynamicScreens dynamicScreens;

        [TestInitialize]
        public new void setUp()
        {
            base.setUp();
            screen = new Screen(tracker);
            dynamicScreen = new DynamicScreen(tracker);
            screens = tracker.Screens;
            dynamicScreens = tracker.DynamicScreens;
        }

        [TestMethod]
        public void initScreenTest()
        {
            Assert.AreEqual(ScreenAction.View, screen.Action);
            Assert.AreEqual(0, screen.Level2);
            Assert.AreEqual(string.Empty, screen.Name);
            Assert.IsNull(screen.Chapter1);
            Assert.IsNull(screen.Chapter2);
            Assert.IsNull(screen.Chapter3);
            Assert.IsFalse(screen.IsBasketScreen);

        }

        [TestMethod]
        public void initDynamicScreenTest()
        {
            Assert.AreEqual(ScreenAction.View, dynamicScreen.Action);
            Assert.AreEqual(0, dynamicScreen.Level2);
            Assert.AreEqual(string.Empty, dynamicScreen.Name);
            Assert.IsNull(dynamicScreen.Chapter1);
            Assert.IsNull(dynamicScreen.Chapter2);
            Assert.IsNull(dynamicScreen.Chapter3);
            Assert.IsFalse(dynamicScreen.IsBasketScreen);

            Assert.IsNotNull(dynamicScreen.Update);
            Assert.AreEqual(string.Empty, dynamicScreen.ScreenId);

        }

        [TestMethod]
        public void setEventScreenTest()
        {
            int index = 0;
            screen.Name = "home";
            screen.Chapter2 = "test";
            screen.Chapter3 = null;
            screen.Level2 = 69;
            screen.SetEvent();

            Assert.AreEqual(5, tracker.buffer.volatileParameters.Count);

            Assert.AreEqual("s2", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("69", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("type", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("screen", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("action", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("view", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("p", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("test::home", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("stc", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("{}", tracker.buffer.volatileParameters[index++].value());

        }

        [TestMethod]
        public void setEventDynamicScreenTest()
        {
            int index = 0;
            dynamicScreen.Name = "home";
            dynamicScreen.Chapter1 = "test";
            dynamicScreen.ScreenId = "testId";
            dynamicScreen.SetEvent();

            Assert.AreEqual(7, tracker.buffer.volatileParameters.Count);

            Assert.AreEqual("pid", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("testId", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("pchap", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("test", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("pidt", tracker.buffer.volatileParameters[index].key);
            Assert.IsNotNull(tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("type", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("screen", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("action", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("view", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("p", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("home", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("stc", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("{}", tracker.buffer.volatileParameters[index++].value());

        }

        [TestMethod]
        public void setEventDynamicScreenTooLongScreenIdTest()
        {
            string id = "";
            for (int i = 0; i < 256; i++)
            {
                id += i;
            }
            int index = 0;
            dynamicScreen.Name = "home";
            dynamicScreen.Chapter1 = "test";
            dynamicScreen.ScreenId = id;
            dynamicScreen.SetEvent();

            Assert.AreEqual(7, tracker.buffer.volatileParameters.Count);

            Assert.AreEqual("pid", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("pchap", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("test", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("pidt", tracker.buffer.volatileParameters[index].key);
            Assert.IsNotNull(tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("type", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("screen", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("action", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("view", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("p", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("home", tracker.buffer.volatileParameters[index++].value());

            Assert.AreEqual("stc", tracker.buffer.volatileParameters[index].key);
            Assert.AreEqual("{}", tracker.buffer.volatileParameters[index++].value());

        }

        [TestMethod]
        public void propertiesTest()
        {
            Assert.AreEqual(screens, tracker.Screens);
            Assert.AreEqual(dynamicScreens, tracker.DynamicScreens);
        }

        [TestMethod]
        public void addScreenTest()
        {
            Screen scr = screens.Add();
            Assert.AreEqual(1, tracker.businessObjects.Count);
            Assert.AreEqual(string.Empty, (tracker.businessObjects[scr.id] as Screen).Name);
        }

        [TestMethod]
        public void addScreenWithNameAndLevel2Test()
        {
            Screen scr = screens.Add("Home", "test");
            scr.Level2 = 8;
            Assert.AreEqual(1, tracker.businessObjects.Count);
            Assert.AreEqual("Home", (tracker.businessObjects[scr.id] as Screen).Name);
            Assert.AreEqual("test", (tracker.businessObjects[scr.id] as Screen).Chapter1);
            Assert.AreEqual(8, (tracker.businessObjects[scr.id] as Screen).Level2);
        }

        [TestMethod]
        public void addDynamicScreenTest()
        {
            DynamicScreen dynScr = dynamicScreens.Add(56, "page", DateTimeOffset.Now, "chap1");
            Assert.AreEqual(1, tracker.businessObjects.Count);
            Assert.AreEqual("56", (tracker.businessObjects[dynScr.id] as DynamicScreen).ScreenId);
            Assert.AreEqual("page", (tracker.businessObjects[dynScr.id] as DynamicScreen).Name);
            Assert.AreEqual("chap1", (tracker.businessObjects[dynScr.id] as DynamicScreen).Chapter1);
            Assert.IsNotNull((tracker.businessObjects[dynScr.id] as DynamicScreen).Update);
        }

        [TestMethod]
        public void addDynamicScreenWithScreenIdStringTest()
        {
            DynamicScreen dynScr = dynamicScreens.Add("testId", "page", DateTimeOffset.Now, "chap1");
            Assert.AreEqual(1, tracker.businessObjects.Count);
            Assert.AreEqual("testId", (tracker.businessObjects[dynScr.id] as DynamicScreen).ScreenId);
            Assert.AreEqual("page", (tracker.businessObjects[dynScr.id] as DynamicScreen).Name);
            Assert.AreEqual("chap1", (tracker.businessObjects[dynScr.id] as DynamicScreen).Chapter1);
            Assert.IsNotNull((tracker.businessObjects[dynScr.id] as DynamicScreen).Update);
        }

    }
}

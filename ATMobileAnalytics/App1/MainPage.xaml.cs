using ATInternet;
using System;
using System.Collections.Generic;
using Windows.Data.Json;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, TrackerDelegate, TrackerReadyHandler
    {
        Tracker _tracker;
        //Action[] _allTests;
        List<Action> _allTests;
        List<string> _allTestNames;

        private void TestScreen()
        {
            // simple
            _tracker.Screens.Add().SendView();
            // named
            _tracker.Screens.Add("test screen").SendView();
            // named + chapters
            _tracker.Screens.Add("testChapters", "chpt1", "chapter 2", "%ch3").SendView();
        }

        private void TestGesture()
        {
            _tracker.Gestures.Add().SendTouch();
            _tracker.Gestures.Add("testGesture++", "chpt_1", "chpt 2", "chpt#3").SendNavigation();
            _tracker.Gestures.Add("searchTest", "chapitre=>1").SendSearch();
        }

        private void TestCustomVar()
        {
            _tracker.CustomVars.Add(1, "customVarAppValue", CustomVarType.App);
            _tracker.Screens.Add("AppValueScreen").SendView();
            _tracker.CustomVars.Add(2, "customVarScreenValue", CustomVarType.Screen);
            _tracker.Screens.Add("ScreenValueScreen").SendView();
        }

        private void TestCustomObject()
        {
            JsonObject kv = new JsonObject();
            kv.Add("jsonkey", JsonValue.CreateStringValue("jsonValue"));
            _tracker.CustomObjects.Add(kv.Stringify());
            _tracker.Gestures.Add("customObject+gesture").SendTouch();

            IDictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("key", "value");
            dict.Add("ugly_key|", "Clean Value");

            IDictionary<string, object> dict2 = new Dictionary<string, object>();
            dict2.Add("obj",dict);
            _tracker.Screens.Add("complexeCustomObject").SendView();
        }

        private void TestRichMedia()
        {
            MediaPlayer player = _tracker.MediaPlayers.Add(1);
            Video video = player.Videos.Add("video", "chapitre 1", 123);
            video.SendPlay(0);
            video.SendPause();
            video.SendMove();
            video.SendStop();
        }

        private void TestSearch()
        {
            _tracker.InternalSearches.Add("keyWordLabel", 1, 2);
            _tracker.Screens.Add("internal search").SendView();
        }

        private void TestDynScreen()
        {
            _tracker.DynamicScreens.Add(1, "dynamicScreen#1", DateTime.Now).SendView();
            _tracker.DynamicScreens.Add(1, "dynamicScreen#2", DateTime.Now, "chapt1", "chapt2", "chapt3").SendView();
        }

        private void TestCustomTree()
        {
            _tracker.customTreeStructures.Add(1, 2, 3);
            _tracker.Screens.Add("custom tree").SendView();
        }

        private void TestUserId()
        {
            //value allowed : deviceId
            _tracker.SetConfig("identifier", "deviceId", this);
        }

        private void TestIdentifiedVisitor()
        {
            IdentifiedVisitor visitor = _tracker.IdentifiedVisitor;
            visitor.Set(1, 2);
            _tracker.Screens.Add().SendView();
            // verfify that it's persistent
            _tracker.Screens.Add("screen1").SendView();
        }

        private void TestLocation()
        {
            _tracker.Locations.Add(44.8372128, -0.6870637);
            _tracker.Screens.Add("map").SendView();
        }

        private void TestPrivate()
        {
            Tracker.DoNotTrack = true;
            _tracker.Screens.Add("do not track me").SendView();
            Tracker.DoNotTrack = false;
        }

        private void TestCampaigns()
        {
            _tracker.Campaigns.Add("AD-3030-[ad_Version_7]-[without_text]-[468]-[www.site.com]-[GT]-[top_page]");
            _tracker.Screens.Add("Ad").SendView();

            _tracker.Campaigns.Add("AL - 3030 - 1[comparison_shopper] - 34253 -[468] - 4[cars_advertisement] - 6[blue_version] | 233[customer_name] - 3425[id_contract]");
            _tracker.Screens.Add("Affiliation").SendView();

            _tracker.Campaigns.Add("SEC-300-GOO-[group_1]-[Var_1]-{ifContent:C}{ifSearch:S}-[{keyword}]&xts=1111111");
            _tracker.Screens.Add("Sponsored Link").SendView();

            _tracker.Campaigns.Add("EPR - 300 -[Presentation_service] - 20070304 -[link2] - 1435@1 - 20070304130405");
            _tracker.Screens.Add("eMailing").SendView();
        }

        private void TestAds()
        {
            _tracker.Publishers.Add("[ad1]").SendImpression();
            _tracker.SelfPromotions.Add(1).SendImpression();
            _tracker.SelfPromotions.Add(2).SendTouch();
        }

        private void TestOrders()
        {
            _tracker.Orders.Add("4655", 3978.65);
            _tracker.Screens.Add("order1").SendView();


            // salestracker
            Order order = _tracker.Orders.Add("112", 0.22);
            order.NewCustomer = true;
            order.Amount.AmountTaxFree = 23.1;
            order.Amount.TaxAmount = 10.7;
            order.Amount.AmountTaxIncluded = 2.8;
            order.Delivery.DeliveryMethod = "pied";
            order.Delivery.ShippingFeesTaxIncluded = 1.1;
            order.Discount.PromotionalCode = "OVZER";
            _tracker.Screens.Add("bigorder").SendView();

            // cart

            _tracker.Cart.Set("3");
            Product p1 = _tracker.Cart.Products.Add("Product#1", "cat1");
            p1.Category1 = "shoes";
            p1.Quantity = 1;
            p1.UnitPriceTaxFree = 29.9;
            p1.UnitPriceTaxIncluded = 35;
            p1.PromotionalCode = "ERT51";
            p1.DiscountTaxFree = 0;
            p1.DiscountTaxIncluded = 0;

            Product p2 = _tracker.Cart.Products.Add("Product#2", "cat7");
            p2.Category1 = "socks";
            p2.Quantity = 1;
            p2.UnitPriceTaxFree = 2;
            p2.UnitPriceTaxIncluded = 3.5;

            _tracker.Orders.Add("ord12", 45.2);
            _tracker.Screens.Add("order and cart").SendView();

            _tracker.Cart.Unset();

            // marquage avec custom var + before paying on external site
            order = _tracker.Orders.Add("cmd1", 94.3, 1);
            order.NewCustomer = true;
            order.ConfirmationRequired = true;
            order.Amount.TaxAmount = 14;
            order.Amount.AmountTaxFree = 80;
            order.Amount.AmountTaxIncluded = 94.5;
            order.Delivery.DeliveryMethod = "Plane";
            order.Delivery.ShippingFeesTaxFree = 10.0;
            order.Delivery.ShippingFeesTaxIncluded = 15.0;
            order.Discount.PromotionalCode = "SUMMER";
            order.CustomVars.Add(1, "fr");

            _tracker.Screens.Add("Order info before payment").SendView();
        }

        private void TestCart()
        {
            // Enable cart and set an identifier
            _tracker.Cart.Set("5");
            Product p1 = _tracker.Cart.Products.Add("ID[p1]");
            p1.Category1 = "10Shoes";
            p1.Quantity = 1;
            p1.UnitPriceTaxFree = 70;
            p1.UnitPriceTaxIncluded = 85;

            Product p2 = _tracker.Cart.Products.Add("ID[P2]");
            p2.Category1 = "20Socks";
            p2.Quantity = 2;
            p2.UnitPriceTaxFree = 7;
            p2.UnitPriceTaxIncluded = 10;

            _tracker.Cart.Unset();
            // If isBasketScreen is not set to true, Cart info won't be added to Screen hit
            Screen s = _tracker.Screens.Add("order confirm");
            s.IsBasketScreen = true;
            s.SendView();
        }

        private void TestVisitedAisle()
        {
            _tracker.Aisles.Add("10[high_tech]", "20[Computers_network]", "30[Computers]", "40[laptops]");
            _tracker.Screens.Add("High Tech").SendView();
        }

        private void TestMultiHit()
        {
            for (int x = 0; x < 100; x++)
            {
                SelfPromotion sp = _tracker.SelfPromotions.Add(x);
                sp.Format = "[120x40]";
                sp.ProductId = "p" + x;
                System.Diagnostics.Debug.WriteLine("add promotion");
            }
            System.Diagnostics.Debug.WriteLine("sendImpression()");
            _tracker.SelfPromotions.SendImpressions();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var buttonNames = new List<string>();
            for (int i = 0; i < _allTests.Count; i++)
            {
                buttonNames.Add("" + i);
            }

            // Parse the XML, Fill the list..
            // Note: You could do it the way you prefer, it is just a sample

            foreach (var buttonName in buttonNames)
            {
                //Create the button
                var newButton = new Button() {
                    Name = buttonName,
                    Content = _allTestNames[int.Parse(buttonName)],
                    Width = 150,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(1),
                };

                newButton.Click += new RoutedEventHandler(button_clicked);

                //Add it to the xaml/stackPanel
                this.stackPanel.Children.Add(newButton);
            }
        }

        // Name is the index of the button in the testList
        private void button_clicked(object button, RoutedEventArgs e)
        {
            _allTests[ int.Parse((button as Button).Name) ]();
        }

        public MainPage()
        {
            this.InitializeComponent();
            _tracker = SmartTag.Instance.defaultTracker;
            _tracker.Delegate = this;
            _allTests = new List<Action>() {
                new Action(TestScreen),
                new Action(TestGesture),
                new Action(TestCustomVar),
                new Action(TestCustomObject),
                new Action(TestRichMedia),
                new Action(TestSearch),
                new Action(TestDynScreen),
                new Action(TestCustomTree),
                new Action(TestUserId),
                new Action(TestIdentifiedVisitor),
                new Action(TestLocation),
                new Action(TestPrivate),
                new Action(TestCampaigns),
                new Action(TestAds),
                new Action(TestOrders),
                new Action(TestCart),
                new Action(TestVisitedAisle),
                new Action(TestMultiHit),
            };

            _allTestNames = new List<string>() {
                "Screens",
                "Gestures",
                "Custom vars",
                "Custom obj",
                "Rich Media",
                "Internal search",
                "Dynamic screens",
                "Custom tree",
                "Print userId",
                "Identified visitor",
                "Location",
                "Pricacy",
                "Campaigns",
                "Adverts",
                "Orders",
                "Carts",
                "Visited aisle",
                "MultiHit",
            };
        }

        private void PlayTest(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            foreach (Action test in _allTests)
            {
                test();
            }
        }

        public void TrackerReady()
        {
            string uid = _tracker.GetUserId();
            System.Diagnostics.Debug.WriteLine("user id :" + uid);
        }

        void TrackerDelegate.TrackerNeedsFirstLaunchApproval(string message)
        {
            //System.Diagnostics.Debug.WriteLine("errorDidOccur :" + message);
        }

        void TrackerDelegate.BuildDidEnd(HitStatus status, string message)
        {
            System.Diagnostics.Debug.WriteLine("buildDidEnd :" + message);
        }

        void TrackerDelegate.SendDidEnd(HitStatus status, string message)
        {
            System.Diagnostics.Debug.WriteLine("sendDidEnd :" + message);
        }

        void TrackerDelegate.DidCallPartner(string response)
        {
            System.Diagnostics.Debug.WriteLine("didCallPartner :" + response);
        }

        void TrackerDelegate.WarningDidOccur(string message)
        {
            System.Diagnostics.Debug.WriteLine("warningDidOccur :" + message);
        }

        void TrackerDelegate.SaveDidEnd(string message)
        {
            System.Diagnostics.Debug.WriteLine("saveDidEnd :" + message);
        }

        void TrackerDelegate.ErrorDidOccur(string message)
        {
            System.Diagnostics.Debug.WriteLine("errorDidOccur :" + message);
        }
    }
}

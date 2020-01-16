using System.Collections.Generic;

namespace ATInternet
{
    #region Product
    public class Product : BusinessObject
    {
        #region Members 

        public string ProductId { get; set; }

        public string Category1 { get; set; }

        public string Category2 { get; set; }

        public string Category3 { get; set; }

        public string Category4 { get; set; }

        public string Category5 { get; set; }

        public string Category6 { get; set; }

        public int Quantity { get; set; }

        public double UnitPriceTaxIncluded { get; set; }

        public double UnitPriceTaxFree { get; set; }

        public double DiscountTaxIncluded { get; set; }

        public double DiscountTaxFree { get; set; }

        public string PromotionalCode { get; set; }

        #endregion

        #region Constructor

        internal Product(Tracker tracker) : base(tracker)
        {
            ProductId = string.Empty;
        }

        #endregion

        #region Methods

        public void SendView()
        {
            tracker.dispatcher.Dispatch(this);
        }

        internal string BuildProductName()
        {
            string productName = Category1 == null ? string.Empty : Category1 + "::";
            productName += Category2 == null ? string.Empty : Category2 + "::";
            productName += Category3 == null ? string.Empty : Category3 + "::";
            productName += Category4 == null ? string.Empty : Category4 + "::";
            productName += Category5 == null ? string.Empty : Category5 + "::";
            productName += Category6 == null ? string.Empty : Category6 + "::";
            return productName += ProductId;
        }

        internal override void SetEvent()
        {
            tracker.SetParam("type", "pdt")
                .SetParam("pdtl", BuildProductName(), new ParamOption() { Append = true, Encode = true,Separator = "|"});

        }

        #endregion
    }

    #endregion

    #region Products

    public class Products
    {
        #region Members

        private Tracker tracker;

        private Cart cart;

        #endregion

        #region Constructors

        internal Products(Tracker tracker)
        {
            this.tracker = tracker;
        }

        internal Products(Cart cart)
        {
            this.cart = cart;
        }

        #endregion

        #region Methods

        public Product Add(Product product)
        {
            if(cart != null)
            {
                cart.productsList.Add(product);
            }
            else
            {
                tracker.businessObjects.Add(product.id, product);
                tracker.objectIndex++;
            }

            return product;
        }

        [Windows.Foundation.Metadata.DefaultOverloadAttribute]
        public Product Add(string productId)
        {
            Product p;
            if (cart != null)
            {
                p = new Product(cart.tracker);
                p.ProductId = productId;
                cart.productsList.Add(p);
            }
            else
            {
                p = new Product(tracker);
                p.ProductId = productId;
                tracker.businessObjects.Add(p.id, p);
                tracker.objectIndex++;
            }

            return p;
        }

        public Product Add(string productId, string category1)
        {
            Product p = Add(productId);
            p.Category1 = category1;
            return p;
        }

        public Product Add(string productId, string category1, string category2)
        {
            Product p = Add(productId, category1);
            p.Category2 = category2;
            return p;
        }

        public Product Add(string productId, string category1, string category2, string category3)
        {
            Product p = Add(productId, category1, category2);
            p.Category3 = category3;
            return p;
        }

        public Product Add(string productId, string category1, string category2, string category3, string category4)
        {
            Product p = Add(productId, category1, category2, category3);
            p.Category4 = category4;
            return p;
        }

        public Product Add(string productId, string category1, string category2, string category3, string category4, string category5)
        {
            Product p = Add(productId, category1, category2, category3, category4);
            p.Category5 = category5;
            return p;
        }

        public Product Add(string productId, string category1, string category2, string category3, string category4, string category5, string category6)
        {
            Product p = Add(productId, category1, category2, category3, category4, category5);
            p.Category6 = category6;
            return p;
        }

        public void Remove(string productId)
        {
            if(cart != null)
            {
                int index = -1;
                for(int i = 0; i < cart.productsList.Count; i++)
                {
                    if (cart.productsList[i].ProductId.Equals(productId))
                    {
                        index = i;
                        break;
                    }
                }
                if(index > -1)
                {
                    cart.productsList.RemoveAt(index);
                }
            }
            else
            {
                List<BusinessObject> businessObjects = new List<BusinessObject>();
                businessObjects.AddRange(tracker.businessObjects.Values);
                for(int i = 0; i < businessObjects.Count; i++)
                {
                    if(businessObjects[i] is Product && (businessObjects[i] as Product).ProductId.Equals(productId))
                    {
                        tracker.businessObjects.Remove(businessObjects[i].id);
                        break;
                    }
                }
            }
        }

        public void RemoveAll()
        {
            if(cart != null)
            {
                cart.productsList.Clear();
            }
            else
            {
                List<BusinessObject> objs = new List<BusinessObject>();
                objs.AddRange(tracker.businessObjects.Values);

                foreach(BusinessObject obj in objs)
                {
                    if(obj is Product)
                    {
                        tracker.businessObjects.Remove(obj.id);
                    }
                }
            }
        }

        public void SendViews()
        {
            List<BusinessObject> views = new List<BusinessObject>();
            foreach(BusinessObject obj in tracker.businessObjects.Values)
            {
                if(obj is Product)
                {
                    views.Add(obj);
                }
            }

            tracker.dispatcher.Dispatch(views.ToArray());
        }

        #endregion


    }

#endregion
}

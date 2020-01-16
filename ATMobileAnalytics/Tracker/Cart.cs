using System.Collections.Generic;
using System.Linq;

namespace ATInternet
{
    #region Cart
    public class Cart : BusinessObject
    {

        #region Members

        public string CartId { get; set; }

        private Products _products;
        public Products Products { get { return _products ?? (_products = new Products(this)); } }

        internal List<Product> productsList { get; set; }

        #endregion

        #region Constructor

        internal Cart(Tracker tracker) : base(tracker)
        {
            productsList = new List<Product>();
        }

        #endregion

        #region Methods

        public Cart Set(string cartId)
        {
            if (CartId != cartId && _products != null)
            {
                _products.RemoveAll();
            }
            tracker.businessObjects[id] = this;
            CartId = cartId;

            return this;
        }

        public Cart Unset()
        {
            CartId = null;
            if (_products != null)
            {
                _products.RemoveAll();
            }
            tracker.businessObjects.Remove(id);

            return this;
        }

        internal override void SetEvent()
        {
            if (CartId != null)
            {
                tracker.SetParam("idcart", CartId);
            }

            if (_products != null)
            {
                ParamOption encoding = new ParamOption() { Encode = true };
                for (int i = 0; i < productsList.Count(); i++)
                {
                    Product p = productsList[i];
                    tracker.SetParam("pdt" + (i + 1), p.BuildProductName(), encoding);
                    if (p.Quantity > -1)
                    {
                        tracker.SetParam("qte" + (i + 1), p.Quantity);
                    }
                    if (p.UnitPriceTaxFree > -1)
                    {
                        tracker.SetParam("mtht" + (i + 1), p.UnitPriceTaxFree);
                    }
                    if (p.UnitPriceTaxIncluded > -1)
                    {
                        tracker.SetParam("mt" + (i + 1), p.UnitPriceTaxIncluded);
                    }
                    if (p.DiscountTaxFree > -1)
                    {
                        tracker.SetParam("dscht" + (i + 1), p.DiscountTaxFree);
                    }
                    if (p.Quantity > -1)
                    {
                        tracker.SetParam("dsc" + (i + 1), p.DiscountTaxIncluded);
                    }
                    if (p.PromotionalCode != null)
                    {
                        tracker.SetParam("pcode" + (i + 1), p.PromotionalCode);
                    }
                }
            }
        }

        #endregion
    }

    #endregion
}

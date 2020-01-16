using System.Collections.Generic;

namespace ATInternet
{
    #region OrderCustomVar

    public class OrderCustomVar
    {
        #region Members

        internal int VarId { get; set; }

        internal string Value{ get; set; }

        #endregion

        #region Constructor

        internal OrderCustomVar(int varId, string value)
        {
            VarId = varId;
            Value = value;
        }

        #endregion
    }

    #endregion

    #region OrderCustomVars

    public class OrderCustomVars
    {
        #region Members

        internal List<OrderCustomVar> list = new List<OrderCustomVar>();

        #endregion

        #region Constructor

        internal OrderCustomVars() { }

        #endregion

        public OrderCustomVar Add(int varId, string customVarValue)
        {
            OrderCustomVar ocv = new OrderCustomVar(varId, customVarValue);
            list.Add(ocv);
            return ocv;
        }
    }

    #endregion

    #region OrderAmount

    public class OrderAmount
    {
        #region Members

        private Order order;

        public double AmountTaxFree { get; set; }
        public double AmountTaxIncluded { get; set; }
        public double TaxAmount { get; set; }

        #endregion

        #region Constructor

        internal OrderAmount(Order order)
        {
            this.order = order;
            AmountTaxFree = -1;
            AmountTaxIncluded = -1;
            TaxAmount = -1;
        }

        #endregion

        #region Methods

        public Order Set(double amountTaxFree, double amountTaxIncluded, double taxAmount)
        {
            AmountTaxFree = amountTaxFree;
            AmountTaxIncluded = amountTaxIncluded;
            TaxAmount = taxAmount;

            return order;
        }

        #endregion
    }

    #endregion

    #region OrderDiscount

    public class OrderDiscount
    {
        #region Members

        private Order order;

        public double DiscountTaxFree { get; set; }
        public double DiscountTaxIncluded { get; set; }
        public string PromotionalCode { get; set; }

        #endregion

        #region Constructor

        internal OrderDiscount(Order order)
        {
            this.order = order;
            DiscountTaxFree = -1;
            DiscountTaxIncluded = -1;
        }

        #endregion

        #region Methods

        public Order Set(double discountTaxFree, double discountTaxIncluded, string promotionalCode)
        {
            DiscountTaxFree = discountTaxFree;
            DiscountTaxIncluded = discountTaxIncluded;
            PromotionalCode = promotionalCode;

            return order;
        }

        #endregion
    }

    #endregion

    #region OrderDelivery

    public class OrderDelivery
    {
        #region Members

        private Order order;

        public double ShippingFeesTaxFree { get; set; }
        public double ShippingFeesTaxIncluded { get; set; }
        public string DeliveryMethod { get; set; }

        #endregion

        #region Constructor

        internal OrderDelivery(Order order)
        {
            this.order = order;
            ShippingFeesTaxFree = -1;
            ShippingFeesTaxIncluded = -1;
        }

        #endregion

        #region Methods

        public Order Set(double shippingFeesTaxFree, double shippingFeesTaxIncluded, string deliveryMethod)
        {
            ShippingFeesTaxFree = shippingFeesTaxFree;
            ShippingFeesTaxIncluded = shippingFeesTaxIncluded;
            DeliveryMethod = deliveryMethod;

            return order;
        }

        #endregion
    }

    #endregion

    #region Order

    public class Order : BusinessObject
    {
        #region Members 

        public string OrderId { get; set; }
        public double Turnover { get; set; }
        public int Status { get; set; }
        public int PaymentMethod { get; set; }
        public bool NewCustomer { get; set; }
        public bool ConfirmationRequired { get; set; }

        private OrderDiscount _orderDiscount;
        public OrderDiscount Discount { get { return _orderDiscount ?? (_orderDiscount = new OrderDiscount(this)); } }

        private OrderAmount _orderAmount;
        public OrderAmount Amount { get { return _orderAmount ?? (_orderAmount = new OrderAmount(this)); } }

        private OrderDelivery _orderDelivery;
        public OrderDelivery Delivery { get { return _orderDelivery ?? (_orderDelivery = new OrderDelivery(this)); } }

        private OrderCustomVars _customVars;
        public OrderCustomVars CustomVars { get { return _customVars ?? (_customVars = new OrderCustomVars()); } }

        #endregion

        #region Constructor

        internal Order(Tracker tracker) : base(tracker)
        {
            OrderId = string.Empty;
            Turnover = -1;
            Status = -1;
            PaymentMethod = -1;
            NewCustomer = false;
            ConfirmationRequired = false;
        }

        #endregion

        #region Methods

        internal override void SetEvent()
        {
            ParamOption encode = new ParamOption() { Encode = true };

            tracker.SetParam("cmd", OrderId);

            if(Turnover > 0)
            {
                tracker.SetParam("roimt", Turnover);
            }

            if (Status > -1)
            {
                tracker.SetParam("st", Status);
            }

            tracker.SetParam("newcus", NewCustomer ? "1" : "0");

            if(_orderDiscount != null)
            {
                if(_orderDiscount.DiscountTaxFree > 0)
                {
                    tracker.SetParam("dscht", _orderDiscount.DiscountTaxFree);
                }
                if (_orderDiscount.DiscountTaxIncluded > 0)
                {
                    tracker.SetParam("dsc", _orderDiscount.DiscountTaxIncluded);
                }
                if (_orderDiscount.PromotionalCode != null)
                {
                    tracker.SetParam("pcd", _orderDiscount.PromotionalCode);
                }
            }

            if (_orderAmount != null)
            {
                if (_orderAmount.AmountTaxFree > 0)
                {
                    tracker.SetParam("mtht", _orderAmount.AmountTaxFree);
                }
                if (_orderAmount.AmountTaxIncluded > 0)
                {
                    tracker.SetParam("mtttc", _orderAmount.AmountTaxIncluded);
                }
                if (_orderAmount.TaxAmount > 0)
                {
                    tracker.SetParam("tax", _orderAmount.TaxAmount);
                }
            }

            if (_orderDelivery != null)
            {
                if (_orderDelivery.ShippingFeesTaxFree > 0)
                {
                    tracker.SetParam("fpht", _orderDelivery.ShippingFeesTaxFree);
                }
                if (_orderDelivery.ShippingFeesTaxIncluded > 0)
                {
                    tracker.SetParam("fp", _orderDelivery.ShippingFeesTaxIncluded);
                }
                if (_orderDelivery.DeliveryMethod != null)
                {
                    tracker.SetParam("dl", _orderDelivery.DeliveryMethod, encode);
                }
            }

            if(_customVars != null)
            {
                foreach(OrderCustomVar cv in _customVars.list)
                {
                    tracker.SetParam("o" + cv.VarId, cv.Value);
                }
            }

            if(PaymentMethod > -1)
            {
                tracker.SetParam("mp", PaymentMethod);
            }

            if (ConfirmationRequired)
            {
                tracker.SetParam("tp", "pre1");
            }
        }

        #endregion
    }
    #endregion

    #region Orders

    public class Orders
    {
        #region Members

        private Tracker tracker;

        #endregion

        #region Constructor

        internal Orders(Tracker tracker)
        {
            this.tracker = tracker;
        }

        #endregion

        #region Methods 

        public Order Add(string orderId, double turnover)
        {
            Order order = new Order(tracker);
            order.OrderId = orderId;
            order.Turnover = turnover;
            tracker.businessObjects.Add(order.id, order);
            tracker.objectIndex++;

            return order;
        }

        public Order Add(string orderId, double turnover, int status)
        {
            Order order = Add(orderId, turnover);
            order.Status = status;
            return order;
        }

        #endregion
    }

    #endregion
}

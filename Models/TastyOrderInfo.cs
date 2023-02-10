namespace TastyBot.Models
{

    public class TastyOrderInfo
    {
        public TastyOrderData data { get; set; }
        public string context { get; set; }
    }

    public class TastyOrderData
    {
        public TastyOrder order { get; set; }
        public Warning[] warnings { get; set; }
        public BuyingPowerEffect buyingpowereffect { get; set; }
        public FeeCalculation feecalculation { get; set; }
    }

    public class TastyOrder
    {
        public string accountnumber { get; set; }
        public string timeinforce { get; set; }
        public string ordertype { get; set; }
        public int size { get; set; }
        public string underlyingsymbol { get; set; }
        public string underlyinginstrumenttype { get; set; }
        public string price { get; set; }
        public string priceeffect { get; set; }
        public string status { get; set; }
        public bool cancellable { get; set; }
        public bool editable { get; set; }
        public bool edited { get; set; }
        public int updatedat { get; set; }
        public OrderLeg[] legs { get; set; }
    }

    public class OrderLeg
    {
        public string instrumenttype { get; set; }
        public string symbol { get; set; }
        public int quantity { get; set; }
        public int remainingquantity { get; set; }
        public string action { get; set; }
        public object[] fills { get; set; }
    }

    public class BuyingPowerEffect
    {
        public string changeinmarginrequirement { get; set; }
        public string changeinmarginrequirementeffect { get; set; }
        public string changeinbuyingpower { get; set; }
        public string changeinbuyingpowereffect { get; set; }
        public string currentbuyingpower { get; set; }
        public string currentbuyingpowereffect { get; set; }
        public string newbuyingpower { get; set; }
        public string newbuyingpowereffect { get; set; }
        public string isolatedordermarginrequirement { get; set; }
        public string isolatedordermarginrequirementeffect { get; set; }
        public bool isspread { get; set; }
        public string impact { get; set; }
        public string effect { get; set; }
    }

    public class FeeCalculation
    {
        public string regulatoryfees { get; set; }
        public string regulatoryfeeseffect { get; set; }
        public string clearingfees { get; set; }
        public string clearingfeeseffect { get; set; }
        public string commission { get; set; }
        public string commissioneffect { get; set; }
        public string proprietaryindexoptionfees { get; set; }
        public string proprietaryindexoptionfeeseffect { get; set; }
        public string totalfees { get; set; }
        public string totalfeeseffect { get; set; }
    }

    public class Warning
    {
        public string code { get; set; }
        public string message { get; set; }
    }
}
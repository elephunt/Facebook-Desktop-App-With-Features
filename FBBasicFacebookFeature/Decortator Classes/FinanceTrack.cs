using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace FBBasicFacebookFeature
{
    [Serializable]
    public class FinanceTrack : IFinance
    {
        [XmlElement("Reason")]
        public string Reason { get; set; }

        [XmlElement("Date")]
        public DateTime Date { get; set; }

        [XmlElement("Amount")]
        public double Amount { get; set; }

        public FinanceTrack()
        {
        }

        public override string ToString()
        {
            return string.Format("{0:0.00}   {1}    {2}", Amount, Reason, Date);
        }

        public FinanceTrack(double i_Amount, string i_Reason, DateTime i_Date)
        {
            Amount = i_Amount;
            Reason = i_Reason;
            Date = i_Date;
        }
    }
}

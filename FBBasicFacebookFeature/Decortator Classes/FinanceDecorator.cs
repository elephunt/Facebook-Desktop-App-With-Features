using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FBBasicFacebookFeature.Observers_Class;

namespace FBBasicFacebookFeature
{
    public class FinanceDecorator : IDecorator, IObserverFinance
    {
        private static List<IUpdateObserver> m_Observer = new List<IUpdateObserver>();

        public void Add(IUpdateObserver i_TextBoxToUpdate)
        {
            m_Observer.Add(i_TextBoxToUpdate);
        }

        public void Remove(IUpdateObserver i_TextBoxToRemove)
        {
            m_Observer.Remove(i_TextBoxToRemove);
        }

        public void Notify()
        {
            foreach (IUpdateObserver Listener in m_Observer)
            {
                Listener.Update(Amount);
            }
        }

        private IFinance m_Finance;

        public double Amount { get; set; }

        public FinanceDecorator(IFinance i_Finace)
        {
            m_Finance = i_Finace;
        }

        public void AddNewTrack()
        {
            List<FinanceTrack> list = new List<FinanceTrack>();
            LoadFinanceDocument(ref list);
            list.Add(m_Finance as FinanceTrack);
            SaveFinanceTrack(list);
        }

        public IFinance Finance
        {
            get
            {
                return m_Finance;
            }

            set
            {
                m_Finance = value;
            }
        }

        public FinanceDecorator()
        {
        }

        private void calculateTotalExpenditure(List<FinanceTrack> i_ListToCalculate)
        {
            Amount = 0;
            foreach(FinanceTrack Track in i_ListToCalculate)
            {
                Amount += Track.Amount;
            }

            Notify();
        }

        public void SaveFinanceTrack(List<FinanceTrack> i_List)
        {
            calculateTotalExpenditure(i_List);
            SaveAndReadXmlFile.SaveToXml("finance.xml", i_List, FileMode.Create);
        }

        public void LoadFinanceDocument(ref List<FinanceTrack> i_List)
        {
            SaveAndReadXmlFile.ReadFromXml("finance.xml", typeof(List<FinanceTrack>), ref i_List);
            calculateTotalExpenditure(i_List);
        }

        public override string ToString()
        {
            return m_Finance.ToString();
        }     
    }
}

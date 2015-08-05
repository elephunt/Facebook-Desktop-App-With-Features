using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FBBasicFacebookFeature.Observers_Class
{
    public interface IObserverFinance
    {
        void Add(IUpdateObserver i_TextBoxToUpdate);

        void Remove(IUpdateObserver i_TextBoxToRemove);
       
        void Notify();
    }
}

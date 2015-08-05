using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FBBasicFacebookFeature.Observers_Class;

namespace FBBasicFacebookFeature
{
    public class ObserverTextBox : TextBox, IUpdateObserver
    {
        public void Update(double Sum)
        {
            this.Text = string.Format("{0:0.00}", Sum);
        }
    }
}

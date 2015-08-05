using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FBBasicFacebookFeature
{
    public partial class EditFormForFinance : Form
    {
        public double Amount { get; set; }

        public string Comment { get; set; }

        public EditFormForFinance(double i_Amount, string i_Comment)
        {
            InitializeComponent();
            Amount = i_Amount;
            Comment = i_Comment;
            textBoxAmount.Text = i_Amount.ToString();
            textBoxComment.Text = i_Comment;
        }

        private void buttonAccept_Click(object sender, EventArgs e)
        {
            float amount;
            if (float.TryParse(textBoxAmount.Text, out amount))
            {
                Amount = amount;
                Comment = textBoxComment.Text;
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Please Enter Amount Only With Numbers");
            }   
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}

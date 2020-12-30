using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ScreenerWFP;

namespace ScreenerWFP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //for(int i = 0; i < 10; i++)
            //{
            Entry test = new Entry("Test", "Update1", DateTime.Now, DateTime.Now, new Entry.ScreeningQuestions("NNNN"),
               36.0f, 35.9f, "Jack", "Murray", "");
            //    button1.Text = ScreenerData.AddEntry(test).ToString();

            //}
            ScreenerData.UpdateEntry("30-12-2020_SHData.txt;1", test);

            //button1.Text = ScreenerData.GetEntryByID(0, "29-12-2020_SHData.txt").ToString();
        }
    }
}

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
            Entry test = new Entry("Jack", "Murray", DateTime.Now, DateTime.Now, new Entry.ScreeningQuestions("NNNN"),
                36.0f, 35.9f, "Jack", "Murray", "");
            Entry test2 = new Entry("Jack2", "Murray2", DateTime.Now, DateTime.Now, new Entry.ScreeningQuestions("NNNN"),
                36.0f, 35.9f, "Jack", "Murray", "");
            button1.Text = ScreenerData.AddEntry(test).ToString();
            button1.Text = ScreenerData.AddEntry(test2).ToString();
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        //Initial load; do startup here
        private void MainForm_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists("Active"))
            {
                Directory.CreateDirectory("Active");
            }
            if (!Directory.Exists("Archive"))
            {
                Directory.CreateDirectory("Archive");
            }
            EntryTypeComboBox.SelectedIndex = 0;
        }

        /// <summary>
        /// When the EntryTypeComboBox's value is changed, update the Table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EntryTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Format the EntryTable
            if(EntryTypeComboBox.SelectedIndex == 0) //All
            {

            } else if(EntryTypeComboBox.SelectedIndex == 1) //Employees
            {

            }else if(EntryTypeComboBox.SelectedIndex == 2) //Essential Service Providers
            {

            }else if(EntryTypeComboBox.SelectedIndex == 3) //Essential Caregivers
            {

            }else if(EntryTypeComboBox.SelectedIndex == 4) //Essential Visitors
            {

            }
            

            
        }

        /// <summary>
        /// Update the EntryTable object with the passed entries.
        /// </summary>
        /// <param name="entries"></param>
        private void UpdateEntryTable(List<Entry> entries)
        {

        }
    }
}

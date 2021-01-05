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
using System.Media;
using controls = System.Windows.Controls;
using System.Windows.Forms.Design;
using ScreenerWFP;

namespace ScreenerWFP
{
    public partial class MainForm : Form
    {
        private enum TableFormat
        {
            ALL,
            EMPLOYEE,
            ESP,
            CAREGIVER,
            VISITOR
        }
        public MainForm()
        {
            InitializeComponent();
        }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams handleParam = base.CreateParams;
                handleParam.ExStyle |= 0x02000000;   // WS_EX_COMPOSITED       
                return handleParam;
            }
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
            EntryTable.AutoSize = true;

            //Testing
            
        }

        /// <summary>
        /// Update the EntryTable object with the passed entries.
        /// </summary>
        /// <param name="entries">The list of entries to display</param>
        /// <param name="format">The tables format</param>
        /// <param name="sortby">The property to sort by. If invalid, will sort by last name</param>
        private void UpdateEntryTable(List<Entry> entries, TableFormat format, string sortby)
        {
            //First, sort the list
            entries = SortList(entries, sortby).ToList();
            //Next, format the table with its headers and such.
            //Reset the table
            EntryTable.Controls.Clear();

            //Enforce auto-size on all rows
            TableLayoutRowStyleCollection styles =
                EntryTable.RowStyles;


            List<string> headers = new List<string>() { "First Name", "Last Name",
                "Time Arrived", "Time Left",
                "Temp In", "Temp Out", "Screener", "Screening Questions", "Notes", "", ""};
            EntryTable.ColumnCount = 11;
            if(format == TableFormat.ALL)
            {
                //13 columns.
                EntryTable.ColumnCount = 14;
                //Set the headers
                headers.Insert(6, "Resident First Name");
                headers.Insert(7, "Resident Last Name");
                headers.Insert(headers.Count -2, "Company");


            } else if(format == TableFormat.EMPLOYEE)
            {

            }
            else if(format == TableFormat.CAREGIVER)
            {

            }else if(format == TableFormat.VISITOR)
            {

            }
            else if(format == TableFormat.ESP)
            {

            }
            foreach(var header in headers)
            {
                System.Windows.Forms.Label newLabel = new Label();
                newLabel.Text = header;
                EntryTable.Controls.Add(newLabel);
            }
            foreach(var entry in entries)
            {
                if(headers.Contains("First Name"))
                {
                    TextBox newtb = new TextBox();
                    newtb.Text = entry.fname;
                    newtb.Dock = DockStyle.Fill;
                    EntryTable.Controls.Add(newtb);
                }
                if(headers.Contains("Last Name"))
                {
                    TextBox newtb = new TextBox();
                    newtb.Text = entry.lname;
                    EntryTable.Controls.Add(newtb);
                }
                if (headers.Contains("Time Arrived"))
                {
                    TextBox newtb = new TextBox();
                    newtb.Text = entry.timeIn.ToShortTimeString();
                    EntryTable.Controls.Add(newtb);
                }
                if (headers.Contains("Time Left"))
                {
                    TextBox newtb = new TextBox();
                    if(entry.timeOut == DateTime.MinValue)
                    {
                        newtb.Text = "";
                    }
                    else
                    {
                        newtb.Text = entry.timeOut.ToShortTimeString();
                    }
                    EntryTable.Controls.Add(newtb);
                }
                if (headers.Contains("Temp In"))
                {
                    TextBox newtb = new TextBox();
                    newtb.Text = entry.temperatureIn.ToString();
                    EntryTable.Controls.Add(newtb);
                }
                if (headers.Contains("Temp Out"))
                {
                    TextBox newtb = new TextBox();
                    newtb.Text = entry.temperatureOut.ToString();
                    EntryTable.Controls.Add(newtb);
                }
                if(headers.Contains("Resident First Name"))
                {
                    TextBox newtb = new TextBox();
                    newtb.Text = entry.resident_fname;
                    EntryTable.Controls.Add(newtb);
                }if(headers.Contains("Resident Last Name"))
                {
                    TextBox newtb = new TextBox();
                    newtb.Text = entry.resident_lname;
                    EntryTable.Controls.Add(newtb);
                }
                if (headers.Contains("Screener"))
                {
                    TextBox newtb = new TextBox();
                    newtb.Text = entry.screener_fname + " " + entry.screener_lname;
                    EntryTable.Controls.Add(newtb);
                }
                if(headers.Contains("Screening Questions"))
                {
                    TextBox newtb = new TextBox();
                    newtb.Text = entry.sq;
                    EntryTable.Controls.Add(newtb);
                }
                if (headers.Contains("Company"))
                {
                    TextBox newtb = new TextBox();
                    newtb.Text = entry.company;
                    EntryTable.Controls.Add(newtb);
                }
                if (headers.Contains("Notes"))
                {
                    TextBox newtb = new TextBox();
                    newtb.Text = entry.notes;
                    EntryTable.Controls.Add(newtb);
                }
                Button editBtn = new Button();
                editBtn.Text = "Edit";
                EntryTable.Controls.Add(editBtn);

                Button leaveBtn = new Button();
                leaveBtn.Text = "Leave";
                EntryTable.Controls.Add(leaveBtn);
                //Each LeaveBtn will have access to this method, which will fill in the exit time
                //and ask for a confirmation that the visit has ended
                leaveBtn.Click += (sender, e) => LeaveBtn_Click(sender, e, EntryTable.GetRow(leaveBtn));

            }

            foreach(Control cell in EntryTable.Controls)
            {
                cell.Dock = DockStyle.Fill;
                cell.Margin = new Padding(0);
                if(EntryTable.GetPositionFromControl(cell).Row % 2 == 0)
                {
                    cell.BackColor = Color.LightGray;
                }
            }
            foreach (RowStyle row in styles)
            {
                row.SizeType = SizeType.AutoSize;
            }

            EntryTable.Refresh();   
        }
        private IEnumerable<Entry> SortList(List<Entry> entries, string sortby)
        {
            switch (sortby)
            {
                case "fname":
                    return entries.OrderBy(e => e.fname);
                case "lname":
                    return entries.OrderBy(e => e.lname);
            }
            return entries;
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void AddEntryBtn_Click(object sender, EventArgs e)
        {

        }

        private void LeaveBtn_Click(object sender, EventArgs e, int row)
        {

        }


        private void EntryTable_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            
            if ((e.Row) % 2 == 0)
                e.Graphics.FillRectangle(Brushes.LightGray, e.CellBounds);
        }

        private void SearchBtn_Click(object sender, EventArgs e)
        {
            TableFormat tf = TableFormat.ALL;
            if(EntryTypeComboBox.SelectedIndex == 0)
            {
                tf = TableFormat.ALL;
            }else if(EntryTypeComboBox.SelectedIndex == 1)
            {
                tf = TableFormat.EMPLOYEE;
            }else if(EntryTypeComboBox.SelectedIndex == 2)
            {
                tf = TableFormat.ESP;
            }else if(EntryTypeComboBox.SelectedIndex == 3)
            {
                tf = TableFormat.CAREGIVER;
            }else if(EntryTypeComboBox.SelectedIndex == 4)
            {
                tf = TableFormat.VISITOR;
            }

            SearchTerm[] st = { };
           
            UpdateEntryTable(ScreenerData.SearchAllEntries(
                st), tf, "lname");
        }
    }
}

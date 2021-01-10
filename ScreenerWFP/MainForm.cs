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

        private List<string> LeaveBtn_IDs = new List<string>();

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
                headers.Insert(8, "Company");


            } else if(format == TableFormat.EMPLOYEE)
            {
                //No further columns to add
            }
            else if(format == TableFormat.CAREGIVER)
            {
                EntryTable.ColumnCount = 13;
                headers.Insert(6, "Resident First Name");
                headers.Insert(7, "Resident Last Name");
            }
            else if(format == TableFormat.VISITOR)
            {
                EntryTable.ColumnCount = 12;

                headers.Insert(6, "Resident First Name");
                headers.Insert(7, "Resident Last Name");
                headers.RemoveAt(9);
            }
            else if(format == TableFormat.ESP)
            {
                EntryTable.ColumnCount = 12;
                headers.Insert(6, "Company");

            }
            foreach(var header in headers)
            {
                System.Windows.Forms.Label newLabel = new Label();
                newLabel.Text = header;
                EntryTable.Controls.Add(newLabel);
                //Need to make them sortable
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
                if (headers.Contains("Company"))
                {
                    TextBox newtb = new TextBox();
                    newtb.Text = entry.company;
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
                if (headers.Contains("Notes"))
                {
                    TextBox newtb = new TextBox();
                    newtb.Text = entry.notes;
                    EntryTable.Controls.Add(newtb);
                }
                Button editBtn = new Button();
                editBtn.Text = "Edit";
                EntryTable.Controls.Add(editBtn);


                LeaveBtn_IDs.Add(entry.location);
                Button leaveBtn = new Button();
                leaveBtn.Text = "Leave";
                EntryTable.Controls.Add(leaveBtn);
                //Each LeaveBtn will have access to this method, which will fill in the exit time
                //and ask for a confirmation that the visit has ended
                leaveBtn.Click += (sender, e) => LeaveBtn_Click(sender, e, 
                    EntryTable.GetRow(leaveBtn),
                    LeaveBtn_IDs[EntryTable.GetRow(leaveBtn)]);

            }

            foreach(Control cell in EntryTable.Controls)
            {
                cell.Dock = DockStyle.Fill;
                cell.Margin = new Padding(0);
                cell.Font = new Font("Arial", 12);
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

        private void AddEntryBtn_Click(object sender, EventArgs e)
        {
            //hide the entry buttons 
            AddEmployeeBtn.Hide();
            AddESPBtn.Hide();
            AddEssentialCaregiverBtn.Hide();
            AddEssentialVisitorBtn.Hide();
            //Add the basic items
            Label FNameLabel = new Label();
            FNameLabel.Text = "First Name: ";
            TextBox FNameTxt = new TextBox();

            Label LNameLabel = new Label();
            LNameLabel.Text = "Last Name: ";
            TextBox LNameTxt = new TextBox();

            Label TimeInLabel = new Label();
            TimeInLabel.Text = "Time Arrived: ";
            TextBox TimeInTxt = new TextBox();
            TimeInTxt.Text = DateTime.Now.ToShortTimeString();

            Label TempInLabel = new Label();
            TempInLabel.Text = "Temperature: ";
            TextBox TempInTxt = new TextBox();

            AddEntryPanel.Controls.AddRange(new Control[] {
                FNameLabel, FNameTxt,
                LNameLabel, LNameTxt,
                TimeInLabel, TimeInTxt,
                TempInLabel, TempInTxt
            });
            //Depending on which EntryBtn was clicked (Figure it out with Sender)
            //Add different items to the panel
            if (((Button)sender).Text == "NEW EMPLOYEE") {
                
            }else if (((Button)sender).Text == "NEW ESSENTIAL SERVICE PROVIDER")
            {
                Label CompanyLbl = new Label();
                CompanyLbl.Text = "Company: ";
                TextBox CompanyTxt = new TextBox();
                AddEntryPanel.Controls.AddRange(new Control[] { CompanyLbl, CompanyTxt });
            }
            else if (((Button)sender).Text == "NEW ESSENTIAL CAREGIVER" ||
                ((Button)sender).Text == "NEW ESSENTIAL VISITOR")
            {
                Label ResidentFNameLbl = new Label();
                ResidentFNameLbl.Text = "Residents First Name";
                TextBox ResidentFNameTxt = new TextBox();
                
                Label ResidentLNameLbl = new Label();
                ResidentLNameLbl.Text = "Residents Last Name";
                TextBox ResidentLnameTxt = new TextBox();
                AddEntryPanel.Controls.AddRange(new Control[] {
                ResidentFNameLbl, ResidentFNameTxt,
                ResidentLNameLbl, ResidentLnameTxt
                });
            }
            else
            {
                throw new Exception("Unknown Entry Button");
            }

            if(((Button)sender).Text != "NEW ESSENTIAL VISITOR")
            {
                Button SymptomBtn = new Button();
                SymptomBtn.Text = "Has Symptoms";
                Button TravelBtn = new Button();
                TravelBtn.Text = "Has traveled within 2 wks";
                Button ContactBtn = new Button();
                ContactBtn.Text = "Has had contact covid in 2 wks";
                Button PPEBtn = new Button();
                PPEBtn.Text = "Didnt Wear appropriate PPE";
                AddEntryPanel.Controls.AddRange(new Control[] {
                SymptomBtn, TravelBtn, ContactBtn, PPEBtn
                });
            }
            //and fill the panel with the information for a new
            //entry, and a Confirm button. 


            Button ConfirmBtn = new Button();
            ConfirmBtn.Text = "Confirm";
            AddEntryPanel.Controls.AddRange(new Control[] {
                ConfirmBtn 
            });
            
            
            //Once the confirm button is clicked, add the
            //information to a new Entry object, and add it to the database.
            //Then, update the table and revert the panel back to the various add entry buttons
        }

        private void LeaveBtn_Click(object sender, EventArgs e, int row, string entryID)
        {

            //TODO:
            //Set the leaving time to now  
            
            //screenerdata.updateentry(entryid,
            //    entryfromrow(row));


        }

        //private entry entryfromrow(int row)
        //{
        //    //create a 'mock-up' entry with default values
        //    string fname = "";
        //    string lname = "";
        //    datetime timein = datetime.now;
        //    datetime timout = datetime.minvalue;
        //    string sq = "nnnn";
        //    string company = "";
        //    string resident_fname = "";
        //    string resident_lanem = "";
        //    float tempin = 0.0f;
        //    float tempout = 0.0f;
        //    string screener_fname = "";
        //    string screener_lname = "";
        //    string notes = "";
        //}


        private void EntryTable_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            
            if ((e.Row) % 2 == 0)
                e.Graphics.FillRectangle(Brushes.LightGray, e.CellBounds);
        }

        private void SearchBtn_Click(object sender, EventArgs e)
        {
            List<SearchTerm> st = new List<SearchTerm>();
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

            if (ShowCurrentVisitorsChk.Checked)
            {
                st.Add(new SearchTerm(SearchTerm.Fields.ISACTIVE, ""));
            }

            
           
            UpdateEntryTable(ScreenerData.SearchAllEntries(
                st), tf, "lname");
        }
    }
}

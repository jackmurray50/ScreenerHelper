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
            ScreenerDropdown.SelectedIndex = 0;

            //Testing
            
        }

        private List<string> LeaveBtn_IDs = new List<string>();


        private List<Entry> WorkingSet = new List<Entry>();
        /// <summary>
        /// Update the EntryTable object with the passed entries.
        /// </summary>
        /// <param name="entries">The list of entries to display</param>
        /// <param name="format">The tables format</param>
        /// <param name="sortby">The property to sort by. If invalid, will sort by last name</param>
        private void UpdateEntryTable(TableFormat format, string sortby)
        {
            //First, sort the list
            WorkingSet = SortList(WorkingSet, sortby).ToList();
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
            foreach(var entry in WorkingSet)
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
                    WorkingSet[EntryTable.GetRow(leaveBtn)].location);

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
                CompanyTxt.Name = "Company";
                AddEntryPanel.Controls.AddRange(new Control[] { CompanyLbl, CompanyTxt });
            }
            else if (((Button)sender).Text == "NEW ESSENTIAL CAREGIVER" ||
                ((Button)sender).Text == "NEW ESSENTIAL VISITOR")
            {
                Label ResidentFNameLbl = new Label();
                ResidentFNameLbl.Text = "Residents First Name";
                TextBox ResidentFNameTxt = new TextBox();
                ResidentFNameTxt.Name = "ResidentFNameTxt";


                Label ResidentLNameLbl = new Label();
                ResidentLNameLbl.Text = "Residents Last Name";
                TextBox ResidentLNameTxt = new TextBox();
                ResidentLNameTxt.Name = "ResidentLNameTxt";

                AddEntryPanel.Controls.AddRange(new Control[] {
                    ResidentFNameLbl, ResidentFNameTxt,
                    ResidentLNameLbl, ResidentLNameTxt
                });
            }
            else
            {
                throw new Exception("Unknown Entry Button");
            }

            if(((Button)sender).Text != "NEW ESSENTIAL VISITOR")
            {
                CheckBox SymptomBtn = new CheckBox();
                SymptomBtn.Text = "Has Symptoms";
                SymptomBtn.Name = "SymptomBtn";
                CheckBox TravelBtn = new CheckBox();
                TravelBtn.Text = "Has traveled within 2 wks";
                TravelBtn.Name = "TravelBtn";
                CheckBox ContactBtn = new CheckBox();
                ContactBtn.Text = "Has had contact covid in 2 wks";
                ContactBtn.Name = "ContactBtn";
                CheckBox PPEBtn = new CheckBox();
                PPEBtn.Text = "Didnt Wear appropriate PPE";
                PPEBtn.Name  = "PPEBtn";
                AddEntryPanel.Controls.AddRange(new Control[] {
                SymptomBtn, TravelBtn, ContactBtn, PPEBtn
                });
            }
            //and fill the panel with the information for a new
            //entry, and a Confirm button. 


            Button ConfirmBtn = new Button();
            ConfirmBtn.Text = "Confirm";

            ConfirmBtn.Click += new System.EventHandler(ConfirmBtn_Click);

            Button CancelBtn = new Button();
            CancelBtn.Text = "Cancel";
            CancelBtn.Click += new System.EventHandler(Reset);

            AddEntryPanel.Controls.AddRange(new Control[] {
                ConfirmBtn,
                CancelBtn
            });
            
            
            //Once the confirm button is clicked, add the
            //information to a new Entry object, and add it to the database.
            //Then, update the table and revert the panel back to the various add entry buttons
            
            //Local function to be added to the ConfirmBtn.
            //Made as a local function so it has access to this functions local variables, making
            //coding a bit cleaner
            void ConfirmBtn_Click(object local_sender, EventArgs local_e)
            {
                //Putting it here to create less calls
                string senderName = ((Button)sender).Name;
                string sq = "";
                if(senderName != "AddEssentialVisitorBtn")
                {
                    if (((CheckBox)AddEntryPanel.Controls[AddEntryPanel.Controls.IndexOfKey("SymptomBtn")]).Checked)
                    {
                        sq += "Y";
                    }
                    else
                    {
                        sq += "N";
                    }
                    if (((CheckBox)AddEntryPanel.Controls[AddEntryPanel.Controls.IndexOfKey("TravelBtn")]).Checked)
                    {
                        sq += "Y";
                    }
                    else
                    {
                        sq += "N";
                    }
                    if (((CheckBox)AddEntryPanel.Controls[AddEntryPanel.Controls.IndexOfKey("ContactBtn")]).Checked)
                    {
                        sq += "Y";
                    }
                    else
                    {
                        sq += "N";
                    }
                    if (((CheckBox)AddEntryPanel.Controls[AddEntryPanel.Controls.IndexOfKey("PPEBtn")]).Checked)
                    {
                        sq += "Y";
                    }
                    else
                    {
                        sq += "N";
                    }
                }

                try
                {
                    if (senderName == "AddEssentialVisitorBtn")
                    {
                        ScreenerData.AddEntry(new Entry("",
                            FNameTxt.Text, LNameTxt.Text,
                            DateTime.Now, DateTime.MinValue,
                            //This mess of a line of code means "Grab the residents first name"
                            ((TextBox)AddEntryPanel.Controls[AddEntryPanel.Controls.IndexOfKey("ResidentFNameTxt")]).Text,
                            ((TextBox)AddEntryPanel.Controls[AddEntryPanel.Controls.IndexOfKey("ResidentLNameTxt")]).Text,
                            float.Parse(TempInTxt.Text), 0.00f,
                            ScreenerDropdown.Text.Split()[0], ScreenerDropdown.Text.Split()[1],
                            ""//empty notes
                            ));
                    }
                    else if (senderName == "AddESPBtn")
                    {
                        ScreenerData.AddEntry(new Entry("",
                            FNameTxt.Text, LNameTxt.Text,
                            DateTime.Now, DateTime.MinValue,
                            new Entry.ScreeningQuestions(sq),
                            //this mess of a line means "Get the company name"
                            ((TextBox)AddEntryPanel.Controls[AddEntryPanel.Controls.IndexOfKey("Company")]).Text,
                            float.Parse(TempInTxt.Text), 0.00f,
                            ScreenerDropdown.Text.Split()[0], ScreenerDropdown.Text.Split()[1],
                            ""//empty notes
                            ));
                    }
                    else if (senderName == "AddEssentialCaregiverBtn")
                    {
                        ScreenerData.AddEntry(new Entry("",
                            FNameTxt.Text, LNameTxt.Text,
                            DateTime.Now, DateTime.MinValue,
                            new Entry.ScreeningQuestions(sq),
                            //This mess of a line of code means "Grab the residents first name"
                            ((TextBox)AddEntryPanel.Controls[AddEntryPanel.Controls.IndexOfKey("ResidentFNameTxt")]).Text,
                            ((TextBox)AddEntryPanel.Controls[AddEntryPanel.Controls.IndexOfKey("ResidentLNameTxt")]).Text,
                            float.Parse(TempInTxt.Text), 0.00f,
                            ScreenerDropdown.Text.Split()[0], ScreenerDropdown.Text.Split()[1],
                            ""//empty notes
                            ));
                    }
                    else if (senderName == "AddEmployeeBtn")
                    {
                        ScreenerData.AddEntry(new Entry("",
                            FNameTxt.Text, LNameTxt.Text,
                            DateTime.Now, DateTime.MinValue,
                            new Entry.ScreeningQuestions(sq),
                            float.Parse(TempInTxt.Text), 0.00f,
                            ScreenerDropdown.Text.Split()[0], ScreenerDropdown.Text.Split()[1],
                            ""//empty notes
                            ));
                    }


                    Reset(local_sender, local_e);
                } catch (FormatException)
                {

                }

            }

            //Resets the AddEntryPanel to having the 4 buttons. 
            void Reset(object local_sender, EventArgs local_e)
            {

                List<string> KeepNames = new List<string>() {
                "AddEssentialVisitorBtn", "AddESPBtn",
                "AddEssentialCaregiverBtn", "AddEmployeeBtn"
                };
                List<Control> ToDelete = new List<Control>();
                foreach (Control control in AddEntryPanel.Controls)
                {
                    if (!KeepNames.Contains(control.Name))
                    {
                        ToDelete.Add(control);
                    }
                }
                foreach (Control control in ToDelete)
                {
                    AddEntryPanel.Controls.Remove(control);
                }

                AddEssentialCaregiverBtn.Show();
                AddESPBtn.Show();
                AddEmployeeBtn.Show();
                AddEssentialVisitorBtn.Show();

            }
        }

        /// <summary>
        /// Open the row for editing, then ask for confirmation that the entry is to be saved
        /// then the row deleted.
        /// </summary>
        /// <param name="sender">Triggering object</param>
        /// <param name="e">Triggering event</param>
        /// <param name="location">entries location to edit</param>
        private void LeaveBtn_Click(object sender, EventArgs e, string location)
        {
            //1. Set the sender's row to Edit.
            //2. Change the sender's text to 'Confirm' and give it the event handler
                //ConfirmChangesBtn_Click
            //3. Change the TimeOut text box's text to the current time
            //4. Set the focus to the Temp In


        }

        private void ConfirmChangesBtn_Click(object sender, EventArgs e, string location)
        {
            //1. Tell the Database to update the entry, using the location parameter
            //2. Remove this entry from the Working Set
            //3. Tell the table to update itself from the Working Set

        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            //1. Enable all the text fields on the Row
            //2. 
        }


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


            WorkingSet = ScreenerData.SearchAllEntries(st);
            UpdateEntryTable(tf, "lname");
        }
    }
}

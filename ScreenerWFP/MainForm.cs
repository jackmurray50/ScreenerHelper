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
using System.Reflection;
//testing
using CsvHelper;
using System.Globalization;
using System.Diagnostics;
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
            if (!System.Windows.Forms.SystemInformation.TerminalServerSession)
                //Set Double buffering on the Grid using reflection and the bindingflags enum.
                typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic |
                BindingFlags.Instance | BindingFlags.SetProperty, null,
                EntryTable, new object[] { true });
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
        }

        private List<string> LeaveBtn_IDs = new List<string>();


        private List<Entry> WorkingSet = new List<Entry>();
        /// <summary>
        /// Update the EntryTable object with the current working set
        /// </summary>
        /// <param name="format">The tables format</param>
        /// <param name="sortby">The property to sort by. If invalid, will sort by last name</param>
        private void UpdateEntryTable()
        {
            EntryTable.SuspendLayout();
            EntryTable.Columns.Clear();
            #region UpdateEntryTable_createcolumns
            //First, create the headers depending on what types of entry are selected.
            //The ones that are able to be usefully sorted will be set up to do so
            //location is never needed
            //fname is always needed
            EntryTable.Columns.Add("fname", "First Name");
            EntryTable.Columns["fname"].SortMode = DataGridViewColumnSortMode.Automatic;
            EntryTable.Columns["fname"].MinimumWidth = 100;
            EntryTable.Columns["fname"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            EntryTable.Columns["fname"].Name = "fname";

            //lname,
            EntryTable.Columns.Add("lname", "Last Name");
            EntryTable.Columns["lname"].SortMode = DataGridViewColumnSortMode.Automatic;
            EntryTable.Columns["lname"].MinimumWidth = 100;
            EntryTable.Columns["lname"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            EntryTable.Columns["lname"].Name = "lname";

            //timeIn,
            EntryTable.Columns.Add("timeIn", "Time Arrived");
            EntryTable.Columns["timeIn"].SortMode = DataGridViewColumnSortMode.Automatic;
            EntryTable.Columns["timeIn"].MinimumWidth = 60;
            EntryTable.Columns["timeIn"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            EntryTable.Columns["timeIn"].Name = "timeIn";

            //timeOut is sometimes needed, if we're doing a Search. Add conditionally
            if (!ShowCurrentVisitorsChk.Checked)
            {
                EntryTable.Columns.Add("timeOut", "Time Left");
                EntryTable.Columns["timeOut"].SortMode = DataGridViewColumnSortMode.Automatic;
                EntryTable.Columns["timeOut"].MinimumWidth = 60;
                EntryTable.Columns["timeOut"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                EntryTable.Columns["timeOut"].Name = "timeOut";
                
            }
            //sq is sometimes needed, depending on the type of entry we're looking for
            if (EntryTypeComboBox.SelectedItem.ToString() != "Essential Visitors")
            {
                EntryTable.Columns.Add("sq", "Screening Questions");
                EntryTable.Columns["sq"].SortMode = DataGridViewColumnSortMode.NotSortable;
                EntryTable.Columns["sq"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
                EntryTable.Columns["sq"].Name = "sq";

            }
            //company sometimes needed
            if (EntryTypeComboBox.SelectedItem.ToString() == "Essential Service Providers" ||
                EntryTypeComboBox.SelectedItem.ToString() == "All")
            {
                EntryTable.Columns.Add("company", "Company");
                EntryTable.Columns["company"].SortMode = DataGridViewColumnSortMode.Automatic;
                EntryTable.Columns["company"].MinimumWidth = 80;
                EntryTable.Columns["company"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                EntryTable.Columns["company"].Name = "company";

            }
            //resident_fname sometimes needed
            //resident_lname sometimes needed          
            if (EntryTypeComboBox.SelectedItem.ToString() == "Essential Visitors" ||
                EntryTypeComboBox.SelectedItem.ToString() == "Essential Caregivers" ||
                EntryTypeComboBox.SelectedItem.ToString() == "All")
            {
                EntryTable.Columns.Add("resident_fname", "Residents First Name");
                EntryTable.Columns["resident_fname"].SortMode = DataGridViewColumnSortMode.Automatic;
                EntryTable.Columns["resident_fname"].MinimumWidth = 180;
                EntryTable.Columns["resident_fname"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                EntryTable.Columns["resident_fname"].Name = "resident_fname";
                EntryTable.Columns.Add("resident_lname", "Residents Last Name");
                EntryTable.Columns["resident_lname"].SortMode = DataGridViewColumnSortMode.Automatic;
                EntryTable.Columns["resident_lname"].MinimumWidth = 180;
                EntryTable.Columns["resident_fname"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            }
            //temperatureIn always needed
            EntryTable.Columns.Add("tempIn", "Temp In");
            EntryTable.Columns["tempIn"].SortMode = DataGridViewColumnSortMode.NotSortable;
            EntryTable.Columns["tempIn"].MinimumWidth = 50;
            EntryTable.Columns["tempIn"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            //temperatureOut sometimes needed
            if (!ShowCurrentVisitorsChk.Checked)
            {
                EntryTable.Columns.Add("tempOut", "Temp Out");
                EntryTable.Columns["tempOut"].SortMode = DataGridViewColumnSortMode.NotSortable;
                EntryTable.Columns["tempOut"].MinimumWidth = 50;
                EntryTable.Columns["tempOut"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            }
            EntryTable.Columns.Add("screenerName", "Screener Name");
            EntryTable.Columns["screenerName"].SortMode = DataGridViewColumnSortMode.Automatic;
            EntryTable.Columns["screenerName"].MinimumWidth = 100;
            EntryTable.Columns["screenerName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

            //notes always needed
            EntryTable.Columns.Add("notes", "Notes");
            EntryTable.Columns["notes"].SortMode = DataGridViewColumnSortMode.NotSortable;
            
            
            EntryTable.Columns.Add(new DataGridViewButtonColumn());
            EntryTable.Columns[EntryTable.Columns.Count - 1].Name = "editBtn";
            EntryTable.Columns[EntryTable.Columns.Count - 1].HeaderText = "";
            EntryTable.Columns.Add(new DataGridViewButtonColumn());
            EntryTable.Columns[EntryTable.Columns.Count - 1].Name = "exitBtn";
            EntryTable.Columns[EntryTable.Columns.Count - 1].HeaderText = "";

            //Hidden column, essentially used as metadata
            EntryTable.Columns.Add("location", "location");
            EntryTable.Columns["location"].Visible = false;
            EntryTable.Columns["location"].Name = "location";
            EntryTable.Columns["location"].HeaderText = "";

            #endregion UpdateEntryTable_createcolumns
            //Add all info to the entries. 
            List<DataGridViewRow> rows = new List<DataGridViewRow>();
            foreach(Entry e in WorkingSet)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(EntryTable);
                //Attempt to name each cell
                //why am i doing this, its starting to take more cpu time than just a host of if/else
                    
                for(int i = 0; i < row.Cells.Count; i++)
                {
                    string nextValue = "";
                    switch (EntryTable.Columns[i].Name) {
                        case "fname":
                            nextValue = e.fname;
                            break;
                        case "lname":
                            nextValue = e.lname;
                            break;
                        case "timeIn":
                            nextValue = e.timeIn.ToShortTimeString();
                            break;
                        case "timeOut":
                            nextValue = e.timeOut.ToShortTimeString();
                            break;
                        case "resident_fname":
                            nextValue = e.resident_fname;
                            break;
                        case "resident_lname":
                            nextValue = e.resident_lname;
                            break;
                        case "company":
                            nextValue = e.company;
                            break;
                        case "tempIn":
                            nextValue = e.temperatureIn.ToString();
                            break;
                        case "tempOut":
                            nextValue = e.temperatureOut.ToString();
                            break;
                        case "screenerName":
                            nextValue = e.screener_fname + " " + e.screener_lname;
                            break;
                        case "sq":
                            nextValue = e.sq.ToString();
                            break;
                        case "location":
                            nextValue = e.location;
                            break;
                        case "notes":
                            nextValue = e.notes;
                            break;
                        case "editBtn":
                            nextValue = "Edit";
                            break;
                        case "exitBtn":
                            nextValue = "Leave";
                            break;
                        default:
                            break;
                    }
                    row.Cells[i].Value = nextValue;
                }
                row.Cells[row.Cells.Count-1].Value = e.location;
                //Check if each Column exists. If it does, add it to the table.
                rows.Add(row);

            }
            StyleTable(EntryTable);
            EntryTable.Rows.AddRange(rows.ToArray());

            EntryTable.ResumeLayout();

        } 
        private void StyleTable(DataGridView table)
        {
            foreach(var c in table.Columns)
            {
                //Set all the text columns to disable entry
                if (!(c is DataGridViewButtonColumn))
                {
                    ((DataGridViewColumn)c).ReadOnly = true;
                }
                //Set the font size and style
                ((DataGridViewColumn)c).DefaultCellStyle.Font = new Font("Arial", 16f, GraphicsUnit.Pixel);
            }
            //Colour the rows alternatingly, to make for easier visibility
            int row = 0;
            foreach(DataGridViewRow r in table.Rows)
            {
                row++;
                if(row % 2 == 0)
                {
                    r.DefaultCellStyle.BackColor = Color.LightGray;
                }
            }
        }
        private void AddEntryBtn_Click(object sender, EventArgs e)
        {
            AddEntryPanel.SuspendLayout();
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
            AddEntryPanel.ResumeLayout();
            UpdateEntryTable();
        }

        /// <summary>
        /// Open the row for editing, then ask for confirmation that the entry is to be saved
        /// then the row deleted.
        /// </summary>
        /// <param name="sender">Triggering object</param>
        /// <param name="e">Triggering event</param>
        /// <param name="location">entries location to edit</param>
        private void LeaveBtn_Click(object sender, DataGridViewCellEventArgs e) 
        {
            DataGridView senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                //Create a new panel that's alwaysontop, with a background to keep it
                //separate from the rest of the application
                FlowLayoutPanel popup = new FlowLayoutPanel();
                popup.SuspendLayout();
                popup.Show();
                panel1.Controls.Add(popup);

                //Populate FlowLayoutPanel with the entries information
                foreach (DataGridViewCell entry in senderGrid.Rows[e.RowIndex].Cells)
                {
                    if (entry.OwningColumn.HeaderText != "") {
                        FlowLayoutPanel flp = new FlowLayoutPanel();
                        Label lbl = new Label();
                        lbl.Text = entry.OwningColumn.HeaderText;
                        TextBox tb = new TextBox();
                        tb.Text = entry.Value.ToString();
                        flp.Controls.Add(lbl);
                        flp.Controls.Add(tb);
                        flp.Name = entry.OwningColumn.Name;
                        flp.AutoSize = true;
                        flp.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                        popup.Controls.Add(flp);
                    }


                }
                //Populate FlowLayoutPanel with the leaving fields (out screening, timeout,
                //tempout)
                FlowLayoutPanel tempOutFLP = new FlowLayoutPanel();
                Label templbl = new Label();
                templbl.Text = "Temp Out";
                tempOutFLP.Controls.Add(templbl);
                TextBox temptxt = new TextBox();
                tempOutFLP.Controls.Add(temptxt);

                FlowLayoutPanel sqOutFLP = new FlowLayoutPanel();
                CheckBox sq_symptomschk = new CheckBox();
                sq_symptomschk.Text = "Any symptoms?";
                CheckBox sq_travelchk = new CheckBox();
                sq_travelchk.Text = "Any travel outside the country?";
                CheckBox sq_contactchk = new CheckBox();
                sq_contactchk.Text = "Any contact with covid?";

                sqOutFLP.Controls.AddRange(new Control[]
                {
                    sq_travelchk, sq_symptomschk, sq_contactchk
                });

                FlowLayoutPanel timeOutFLP = new FlowLayoutPanel();
                Label timeoutlbl = new Label();
                timeoutlbl.Text = "Time Out: ";
                TextBox timeouttxt = new TextBox();
                timeouttxt.Text = DateTime.Now.ToShortTimeString();
                timeOutFLP.Controls.AddRange(new Control[]
                {
                    timeoutlbl, timeouttxt
                });


                popup.Controls.AddRange(new Control[] { tempOutFLP, sqOutFLP, timeOutFLP});

                Label locationLabel = new Label();
                locationLabel.Visible = false;
                locationLabel.Text = senderGrid.Rows[e.RowIndex]
                    .Cells[senderGrid.Rows[e.RowIndex].Cells.Count -1]
                    .Value.ToString();
                locationLabel.Name = "location";
                popup.Controls.Add(locationLabel);

                //Add a 'confirm' and 'cancel' button
                Button confirmBtn = new Button();
                confirmBtn.Text = "Confirm";
                confirmBtn.Click += new System.EventHandler(this.ConfirmChangesBtn_Click);


                Button cancelBtn = new Button();
                cancelBtn.Text = "Cancel";
                cancelBtn.Click += new System.EventHandler(this.CancelPopupBtnClick);
                popup.Controls.Add(confirmBtn);
                popup.Controls.Add(cancelBtn);
                popup.Name = "Popup";
                StylePopup(popup);
                popup.ResumeLayout();
            }
        }
        private void CancelPopupBtnClick(object sender, EventArgs e)
        {
            Panel senderPanel = (Panel)((Button)sender).Parent;
            senderPanel.Dispose();
        }
        private void EditBtn_Click(object sender, EventArgs e)
        {
            //1. Enable all the text fields on the Row
            //2. 
        }
        private void StylePopup(Panel popup)
        {
            popup.BackColor = Color.Gray;
            popup.BorderStyle = BorderStyle.FixedSingle;
            popup.Location = new Point(this.panel1.Width/4, this.panel1.Height/3);
            popup.AutoSize = true;
            popup.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            popup.MaximumSize = new Size(this.panel1.Width / 2, 200);
            popup.BringToFront();
        }

        private void ConfirmChangesBtn_Click(object sender, EventArgs e)
        {

            Panel senderPanel = (Panel)((Button)sender).Parent;
            //1. Work out which entry to remove
            Entry toRemove = WorkingSet.Where(n => n.location ==
                            senderPanel.Controls["location"].Text).First();
            //2. Tell the Database to update the entry, using the location parameter
            //2.1 Create a new entry
            string fname = "";
            if (senderPanel.Controls.ContainsKey("fname"))
            {
                fname = senderPanel.Controls["fname"].Text;
            }
            string lname = "";
            if (senderPanel.Controls.ContainsKey("lname"))
            {
                lname = senderPanel.Controls["lname"].Text;
            }
            DateTime timein = toRemove.timeIn;
            if (senderPanel.Controls.ContainsKey("timeIn"))
            {
                //TODO: Figure this out? 
                //Keep the updated time, but the old date.
                //Maybe take the datetime from the working set, then use a DatePicker control?
                //timein = senderPanel.Controls["timeIn"].Text;
            }
            DateTime timeout = DateTime.MinValue;
            //if(senderPanel.Controls.ContainsKey("time"))
            float tempin = 0.0f;
            float tempout = 0.0f;
            string company = "";
            string resident_fname = "";
            string resident_lname = "";
            Entry.ScreeningQuestions sq = null;
            string screener_fname = "";
            string screener_lname = "";
            string notes = "";
            ScreenerData.UpdateEntry(toRemove.location, new Entry(
                toRemove.location, //location
                fname, //first name
                lname,
                timein,
                timeout,
                sq,
                company,
                resident_fname,
                resident_lname,
                tempin,
                tempout,
                screener_fname,
                screener_lname,
                notes));

            //3. Remove the entry from the working set
            WorkingSet.Remove(toRemove);
            //4. Close the 'window'
            //CancelPopupBtnClick(sender, e);
            //5. Tell the table to update itself from the Working Set
            UpdateEntryTable();

        }


        private void SearchBtn_Click(object sender, EventArgs e)
        {
            Trace.WriteLine("Start Searching at " + DateTime.Now);
            List<SearchTerm> st = new List<SearchTerm>();
            if(EntryTypeComboBox.SelectedIndex == 0)
            {
            }else if(EntryTypeComboBox.SelectedIndex == 1)
            {

            }else if(EntryTypeComboBox.SelectedIndex == 2)
            {

            }else if(EntryTypeComboBox.SelectedIndex == 3)
            {

            }else if(EntryTypeComboBox.SelectedIndex == 4)
            {
            }

            if (ShowCurrentVisitorsChk.Checked)
            {
                st.Add(new SearchTerm(SearchTerm.Fields.ISACTIVE, ""));
            }


            WorkingSet = ScreenerData.SearchAllEntries(st);
            Trace.WriteLine("Finished searching at " + DateTime.Now);
            UpdateEntryTable();
        }
    }
}
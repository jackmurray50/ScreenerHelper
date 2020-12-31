using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using System.Text.RegularExpressions;


namespace ScreenerWFP
{
    /// <summary>
    /// Class meant to allow retrieval of data from various CSVs. Note: Database in this case isn't a dbms, its a collection of csvs
    /// Will support Creation, Retrieval and Update, but not Deletion. 
    /// </summary>
    public static class ScreenerData
    {
        //These two are the paths to the relevant folders.
        private static string activeFolderPath = "Active";
        private static string archiveFolderPath = "Archive";

        ///<summary>
        ///Adds a new entry to the database.
        ///</summary>
        ///<returns>The new entries id</returns>
        public static int AddEntry(Entry entry)
        {
            string curFile = "";
            //First, figure out which file to write to.
            //Go through all active folders and find the most recent one
            if (!Directory.Exists(activeFolderPath)) {
                Directory.CreateDirectory(activeFolderPath);
            }

            foreach(string file in Directory.GetFiles(activeFolderPath))
            {
                //Check if the files creation date is within the past day
                if(File.GetCreationTime(file) >= DateTime.Now.AddHours(-24))

                {
                    //If true, check if its newer than curFile
                    if (File.Exists(curFile))
                    {
                        if (File.GetCreationTime(file) > File.GetCreationTime(curFile))
                        {
                            curFile = file;
                        }
                    }
                    else
                    {
                        curFile = file;
                    }
                }
            }
            //Now, either the newest file within 24hrs has been chosen or the curFile string is empty.
            //If its empty, create a new file and replace curFile with it
            if(curFile == "")
            {
                curFile = $"{activeFolderPath}/{DateTime.Today.Day.ToString()}-{DateTime.Today.Month.ToString()}-{DateTime.Today.Year.ToString()}" +
                    $"_SHData.csv";
                FileStream fs = File.Create(curFile );
                fs.Close();
                //Create the header
                using (var writer = new StreamWriter(curFile))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {

                    csv.WriteHeader<Entry>();
                    csv.Configuration.RegisterClassMap<EntryMap>();
                    csv.NextRecord();
                }
            }

            //The new record will be at the bottom, so its ID will be the line number
            int count = 0;
            using (var reader = new StreamReader(curFile))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Configuration.HasHeaderRecord = true;
                //Set the entries location
                
                count = csv.GetRecords<Entry>().Count();
            }
            //At this point its a simple matter to append the entry to the file
            using (var stream = File.Open(curFile, FileMode.Append))
            using (var writer = new StreamWriter(stream))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                entry.location = curFile + ";" + count.ToString();
                csv.WriteRecord(entry);
                csv.NextRecord();
            }
            return count;

            
        }
        /// <summary>
        /// Updates an entry. Requires an EntryID to know what to update.
        /// </summary>
        /// <param name="location">A string representing the entries location, in format "filename.extension;index"</param>
        /// <param name="newEntry">The new entry</param>
        public static void UpdateEntry(string location, Entry newEntry)
        {
            //First, parse location to find out which file and index to look at
            Regex rgx = new Regex("(?<file>.*);(?<index>..*)");
            Match match = rgx.Match(location);

            //Best way to update a csv is to read in the whole thing, update it in memory, then overwrite the old csv.
            //This hurts my soul, but here we are.
            //Note: This means 'In Active Folder', not 'inactive' as in 'not active'
            bool inActive = true;
            //Check if the file is in Active. If its not, then the file must be in Archive.
            if (!File.Exists(activeFolderPath + "/" + match.Groups["file"].Value))
            {
                inActive = false;
            }
            List<Entry> records;
            using (var reader = new StreamReader((inActive ? activeFolderPath : archiveFolderPath) + "/"+ match.Groups["file"].Value))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                records = csv.GetRecords<Entry>().ToList();

            }
            records[Int32.Parse(match.Groups["index"].Value)] = newEntry;

            using(var writer = new StreamWriter((inActive ? activeFolderPath : archiveFolderPath) + "/" + match.Groups["file"].Value))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords<Entry>(records);
            }

        }

        /// <summary>
        /// Get an entry from a specific CSV file based on its ID, mostly a helper function for the update  
        /// </summary>
        /// <param name="EntryID">The Entry's ID</param>
        /// <returns></returns>
        public static Entry GetEntryByID(int EntryID, string path)
        {
            //prevent an out of bounds exception
            if (EntryID < 0)
            {
                return null;
            }
            string newpath = "";
            //Check the active and archive folders, starting with the active folder since it'll be smaller.
            if (File.Exists(activeFolderPath + "/" + path))
            {
                newpath = activeFolderPath + "/" + path;
            }
            else if (File.Exists(archiveFolderPath + "/" + path))
            {
                newpath = archiveFolderPath + "/" + path;
            }
            else //If its not in either of the two above folders, the file doesnt exist.
            {
                return null;
            }

            using (var reader = new StreamReader(newpath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                List<Entry> records = csv.GetRecords<Entry>().ToList();
                //prevent an outofbounds exception
                if (EntryID > records.Count())
                {
                    return null;
                }
                return records[EntryID];
            }
        }
    
        public static List<Entry> SearchActiveEntries(params SearchTerm[] searchTerms)
        {
            return SearchEntries(false, searchTerms);
        }
        public static List<Entry> SearchAllEntries(params SearchTerm[] searchTerms)
        {
            return SearchEntries(true, searchTerms);
        }
        private static List<Entry> SearchEntries(bool searchArchived, SearchTerm[] searchTerms)
        {
            List<string> files = new List<string>();
            Regex rgx = new Regex("\\\\(?<date>.*)_");
            
            //Check the dates we're going to be looking at, to facilitate searching
            foreach (string file in Directory.GetFiles(activeFolderPath))
            {
                //Get the files creation date
                Match match = rgx.Match(file);
                if (CheckIfDateIsInDateRange(searchTerms, match.Groups["date"].Value))
                {
                    files.Add(activeFolderPath + "/" + match.Groups["date"].Value + "_SHData.csv");
                }
                
            }

            if (searchArchived)
            {
                foreach(string file in Directory.GetFiles(archiveFolderPath))
                {
                    //Get the files creation date
                    Match match = rgx.Match(activeFolderPath + file);
                    if (CheckIfDateIsInDateRange(searchTerms, match.Groups["date"].Value))
                    {
                        files.Add(archiveFolderPath + "/" + match.Groups["date"].Value + "_SHData.csv");
                    }
                }
            }

            //Create a list of all the records to search through
            List<Entry> records = new List<Entry>();
            foreach (string path in files)
            {
                using (var reader = new StreamReader(path))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    List<Entry> toAdd = csv.GetRecords<Entry>().ToList();
                    records.AddRange(toAdd);
                }
            }
            List<Entry> output = new List<Entry>();
            
            foreach(Entry record in records)
            {
                bool valid = true;
                foreach(SearchTerm st in searchTerms)
                {
                    if (!st.Validate(record))
                    {
                        valid = false;
                        break;
                    }
                }
                if (valid == true)
                {
                    output.Add(record);
                }
            }

            return output;
        }
        /// <summary>
        /// Check if a date falls within the dates within the SearchTerms
        /// </summary>
        /// <param name="st">SearchTerms</param>
        /// <param name="date">The date to compare</param>
        /// <returns>A bool. True if the date is within the date range</returns>
        private static bool CheckIfDateIsInDateRange(SearchTerm[] st, string date)
        {
            //TODO: Implement this. For now it'll always return true
            DateTime dt = DateTime.Now;
            try
            {
                dt = DateTime.ParseExact(date, "dd-MM-yyyy", null);
            }
            catch (FormatException)
            {
                //This should happen if its an invalid file, so its good to
                //not read it.
                return false;
            }
            bool accepted = true;
            foreach (var term in st)
            {
                DateTime parsedKey;
                if (!DateTime.TryParseExact(term.Key, "dd-MM-yyyy", null, DateTimeStyles.None, out parsedKey))
                {
                    continue;
                }

                if (term.Field == SearchTerm.Fields.ENTER_AFTER_DATE ||
                    term.Field == SearchTerm.Fields.EXIT_AFTER_DATE)
                {
                    if (parsedKey < dt)
                    {
                        accepted = false;
                    }
                } else if (term.Field == SearchTerm.Fields.ENTER_BEFORE_DATE)
                {
                    if (parsedKey > dt)
                    {
                        accepted = false;
                    }
                }
                else if (term.Field == SearchTerm.Fields.EXIT_BEFORE_DATE)
                {
                    if (parsedKey > dt.AddDays(1))
                    {
                        accepted = false;
                    }
                }
                else if (term.Field == SearchTerm.Fields.ENTER_ON_DATE) { 
                    if(parsedKey != dt)
                    {
                        accepted = false;
                    }
                
                }
                else if(term.Field == SearchTerm.Fields.EXIT_ON_DATE)
                {
                    //Accept the day of
                    if (parsedKey != dt || parsedKey != dt.AddDays(1))
                    {
                        //No need to do more checks if its on the date
                        accepted = false;
                    }
                }

            }

            return accepted;

        }
    }


    /// <summary>
    /// A term used for searching through entries
    /// </summary>
    public class SearchTerm
    {
        //The fields name to be searched
        public Fields Field { get; }
        public string Key { get; }
        public SearchTerm(Fields field, string key)
        {
            Field = field;
            Key = key;
        }
        public enum Fields
        {
            FIRSTNAME,
            LASTNAME,
            RESIDENT_FIRSTNAME,
            RESIDENT_LASTNAME,
            COMPANY,
            ENTER_BEFORE_DATE,
            EXIT_BEFORE_DATE,
            ENTER_AFTER_DATE,
            EXIT_AFTER_DATE,
            ENTER_ON_DATE,
            EXIT_ON_DATE,
            NOTE,
            TEMP_ABOVE,
            TEMP_BELOW,
            SCREENINGQUESTIONS,
            SCREENER_FIRSTNAME,
            SCREENER_LASTNAME,
            ANY
        }

        /// <summary>
        /// Check if the searchterm is True.
        /// </summary>
        /// <param name="entry">The entry to validate</param>
        /// <returns>True if the entry has the searchterm</returns>
        public bool Validate(Entry entry)
        {
            switch (Field) {
                case Fields.COMPANY:
                    return entry.company.ToLower().Contains(Key.ToLower());
                case Fields.ENTER_AFTER_DATE:
                    return true;
                case Fields.ENTER_BEFORE_DATE:
                    return true;
                case Fields.ENTER_ON_DATE:
                    return true;
                case Fields.EXIT_AFTER_DATE:
                    return true;
                case Fields.EXIT_BEFORE_DATE:
                    return true;
                case Fields.EXIT_ON_DATE:
                    return true;
                case Fields.FIRSTNAME:
                    return entry.fname.ToLower().Contains(Key.ToLower());
                case Fields.LASTNAME:
                    return entry.lname.ToLower().Contains(Key.ToLower());
                case Fields.NOTE:
                    return entry.notes.ToLower().Contains(Key.ToLower());
                case Fields.RESIDENT_FIRSTNAME:
                    return entry.resident_fname.ToLower().Contains(Key.ToLower());
                case Fields.RESIDENT_LASTNAME:
                    return entry.resident_lname.ToLower().Contains(Key.ToLower());
                case Fields.SCREENER_FIRSTNAME:
                    return entry.screener_fname.ToLower().Contains(Key.ToLower());
                case Fields.SCREENER_LASTNAME:
                    return entry.screener_lname.ToLower().Contains(Key.ToLower());
                case Fields.SCREENINGQUESTIONS:
                    return entry.sq.Contains(Key.ToUpper()); //sq is toupper instead of tolower like the others because screening questions are saved in all caps
                case Fields.TEMP_ABOVE:
                    return entry.temperatureIn >= float.Parse(Key) &&
                        entry.temperatureOut >= float.Parse(Key);
                case Fields.TEMP_BELOW:
                    return entry.temperatureIn <= float.Parse(Key) &&
                        entry.temperatureOut <= float.Parse(Key);
                case Fields.ANY: //General searech in all fields
                    //First, figure out if its fits a date, a datetime
                    break;
            }
            return false;
            
        }
    }

    /// <summary>
    /// Class meant to hold the entry's data.
    /// </summary>
    public class Entry
    {
        //Location will be the file name followed by its index, to ease finding.
        //Ex: "29-12-20_SHData.txt;1"
        public string location { get; set; }
        public string fname { get; }
        public string lname { get; }
        public DateTime timeIn { get; }
        public DateTime timeOut { get; }
        private ScreeningQuestions _sq;
        public string sq { get => _sq.ToString(); }
        
        public string company { get; }
        public string resident_fname { get; }
        public string resident_lname { get; }
        public float temperatureIn { get; }
        public float temperatureOut { get; }
        public string screener_fname { get; }
        public string screener_lname { get; }
        public string notes { get; }
        /// <summary>
        /// Base entry constructor
        /// </summary>
        /// <param name="fname">Visitors first name</param>
        /// <param name="lname">Visitors last name</param>
        /// <param name="timeIn">Time the visitor came in</param>
        /// <param name="timeOut">Time the visitor left</param>
        /// <param name="sq">Screening questions results</param>
        /// <param name="company">Visitors company</param>
        /// <param name="resident_fname">Visiting residents first name</param>
        /// <param name="resident_lname">Visiting residents last name</param>
        /// <param name="temperatureIn">Visitors temperature upon arrival</param>
        /// <param name="temperatureOut">Visitors temperature upon departure</param>
        /// <param name="screener_fname">Screeners first name</param>
        /// <param name="screener_lname">Screener last name</param>
        /// <param name="notes">Additional notes</param>
        private Entry(string fname, string lname,
            DateTime timeIn, DateTime timeOut, 
            ScreeningQuestions sq,
            string company,
            string resident_fname, string resident_lname,
            float temperatureIn, float temperatureOut,
            string screener_fname, string screener_lname, string notes
            )
        { 
            this.fname = fname;
            this.lname = lname;
            this.timeIn = timeIn;
            this.timeOut = timeOut;
            this._sq = sq;
            this.resident_fname = resident_fname;
            this.resident_lname = resident_lname;
            this.temperatureIn = temperatureIn;
            this.temperatureOut = temperatureOut;
            this.screener_fname = screener_fname;
            this.screener_lname = screener_lname;
            this.notes = notes;
        }

        /// <summary>
        /// Employee Entry constructor
        /// </summary>
        /// <param name="fname">Visitors first name</param>
        /// <param name="lname">Visitors last name</param>
        /// <param name="timeIn">Time the visitor came in</param>
        /// <param name="timeOut">Time the visitor left</param>
        /// <param name="sq">Screening questions results</param>
        /// <param name="temperatureIn">Visitors temperature upon arrival</param>
        /// <param name="temperatureOut">Visitors temperature upon departure</param>
        /// <param name="screener_fname">Screeners first name</param>
        /// <param name="screener_lname">Screener last name</param>
        public Entry(string fname, string lname,
            DateTime timeIn, DateTime timeOut,
            ScreeningQuestions sq,
            float temperatureIn, float temperatureOut,
            string screener_fname, string screener_lname, string notes) : this(fname, lname, timeIn, timeOut, sq, "", "", "", temperatureIn, temperatureOut, screener_fname, screener_lname, notes) 
        { }

        /// <summary>
        /// Essential service provider Entry constructor
        /// </summary>
        /// <param name="fname">Visitors first name</param>
        /// <param name="lname">Visitors last name</param>
        /// <param name="timeIn">Time the visitor came in</param>
        /// <param name="timeOut">Time the visitor left</param>
        /// <param name="sq">Screening questions results</param>
        /// <param name="company">Visitors employer</param>
        /// <param name="temperatureIn">Visitors temperature upon arrival</param>
        /// <param name="temperatureOut">Visitors temperature upon departure</param>
        /// <param name="screener_fname">Screeners first name</param>
        /// <param name="screener_lname">Screener last name</param>
        public Entry(string fname, string lname,
            DateTime timeIn, DateTime timeOut,
            ScreeningQuestions sq,
            string company,
            float temperatureIn, float temperatureOut,
            string screener_fname, string screener_lname, string notes) : this(fname, lname, timeIn, timeOut, sq, company, "", "", temperatureIn, temperatureOut, screener_fname, screener_lname, notes)
        { }

        /// <summary>
        /// Essential caregiver entry constructor
        /// </summary>
        /// <param name="fname"></param>
        /// <param name="lname"></param>
        /// <param name="timeIn"></param>
        /// <param name="timeOut"></param>
        /// <param name="sq"></param>
        /// <param name="resident_fname"></param>
        /// <param name="resident_lname"></param>
        /// <param name="temperatureIn"></param>
        /// <param name="temperatureOut"></param>
        /// <param name="screener_fname"></param>
        /// <param name="screener_lname"></param>
        public Entry(string fname, string lname,
            DateTime timeIn, DateTime timeOut,
            string resident_fname, string resident_lname,
            float temperatureIn, float temperatureOut,
            string screener_fname, string screener_lname, string notes) : this(fname, lname, timeIn, timeOut, null, "", resident_fname, resident_lname, temperatureIn, temperatureOut, screener_fname, screener_lname, notes) 
        { }

        public override string ToString()
        {
            return $"{this.fname}, {this.lname}, {this.timeIn.ToString()}, {this.timeOut.ToString()}, " +
                $"{this.sq.ToString()}, {this.company}, {this.temperatureIn.ToString()}, {this.temperatureOut.ToString()}, " +
                $"{this.screener_fname}, {this.screener_lname}, {this.notes}";
        }

        /// <summary>
        /// Class representing screening questions. Mostly used to clean up code
        /// </summary>
        public class ScreeningQuestions
        {

            public bool Symptoms { get; }
            public bool Travel { get; }
            public bool Contact { get; }
            public bool PPE { get; }
            
            /// <summary>
            /// A constructor that fills in the parameters using a string. Mostly used to allow ScreeningQuestions 
            /// to be stored in a csv cell.
            /// </summary>
            /// <param name="sq">A string representing the answers to the screening questions</param>
            public ScreeningQuestions(string sq)
            {
                //A lazy hack to keep the code readable. The CsvHelper Header expects a field called 'sq' not 'answers' so to keep
                //this method easily readable I just change the name here.
                string answers = sq;
                for(int i = 0; i < answers.Length; i++){
                    if(i == 0)
                    {
                        if(answers[i] == 'Y' || answers[i] == 'y')
                        {
                            this.Symptoms = true;
                        }
                        if(answers[i] == 'N' || answers[i] == 'n')
                        {
                            this.Symptoms = false;
                        }
                    }
                    if(i == 1)
                    {
                        if(answers[i] == 'Y' || answers[i] == 'y')
                        {
                            this.Travel = true;
                        }
                        if (answers[i] == 'N' || answers[i] == 'n')
                        {
                            this.Travel = false;
                        }

                    }
                    if(i == 2)
                    {
                        if(answers[i] == 'Y' || answers[i] == 'y')
                        {
                            this.Contact = true;
                        }
                        if (answers[i] == 'N' || answers[i] == 'n')
                        {
                            this.Contact = false;
                        }
                    }
                    if(i == 3)
                    {
                        if(answers[i] == 'Y' || answers[i] == 'y')
                        {
                            this.PPE = true;
                        }
                        if (answers[i] == 'N' || answers[i] == 'n')
                        {
                            this.PPE = false;
                        }
                    }
                }
            }

            /// <summary>
            /// Returns a set of characters that represent the answers to the various screening questions
            /// </summary>
            /// <returns>A set of characters that represents the answers to the screening questions. Ex: "YYNY"</returns>
            public override string ToString()
            {
                string output = "";
                List<bool> answers = new List<bool> {Symptoms, Travel, Contact, PPE };
                foreach(bool answer in answers)
                {
                    if(answer == true)
                    {
                        output += 'Y';
                    }
                    else if(answer == false)
                    {
                        output += 'N';
                    }
                }
                return output;
            }

        }

    }
    public class EntryMap : ClassMap<Entry>
    {
        public EntryMap()
        {
            AutoMap(CultureInfo.InvariantCulture);
        }
    }
}

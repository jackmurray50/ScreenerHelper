using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenerWFP
{
    /// <summary>
    /// Class meant to allow retrieval of data from various CSVs. Note: Database in this case isn't a dbms, its a collection of csvs
    /// </summary>
    public static class ScreenerData
    {
        ///<summary>
        ///Adds a new entry to the database.
        ///</summary>
        ///<returns>Returns true if the operation succeeded</returns>
        public static bool AddEntry()
        {
            return true;
        }
        /// <summary>
        /// Updates an entry. Requires an EntryID to know what to update.
        /// </summary>
        /// <param name="EntryID"></param>
        /// <returns></returns>
        public static bool UpdateEntry(int EntryID)
        {
            return true;
        }
    }
    /// <summary>
    /// Class meant to hold the entry's data.
    /// </summary>
    public class Entry
    {
        public string fname { get; }
        public string lname { get; }
        public DateTime timeIn { get; }
        public DateTime timeOut { get; }
        public ScreeningQuestions sq { get; }
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
            this.sq = sq;
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
            return $"{this.fname}, {this.lname}, {this.timeIn.ToString()}, {this.timeOut.ToString()}" +
                $"{this.sq.ToString()}, {this.company}, {this.temperatureIn.ToString()}, {this.temperatureOut.ToString()}" +
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
            /// <param name="answers">A string representing the answers to the screening questions</param>
            public ScreeningQuestions(string answers)
            {
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

    
}

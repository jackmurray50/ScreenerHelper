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
        { }

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

        /// <summary>
        /// Class representing screening questions. Mostly used to clean up code
        /// </summary>
        public class ScreeningQuestions
        {
            public ScreeningQuestions()
            {

            }

        }

    }

    
}

using System;
using UtleiraTidtaker.Lib.Utilities;

namespace UtleiraTidtaker.Lib.Model
{
    public class Race
    {
        /*
         * {
         *   "Key":"K2",
         *   "status":"1",
         *   "name":"Damer 2km",
         *   "fStartTime":"Sat Sep 12 2015 11:15:00 GMTpluss0100 (CET)",
         *   "intervalTime":"0",
         *   "noStart":"1",
         *   "sortRunTime":"False"
         * }
         * */

        /*
         * Klasseinndeling Stærkløpet
         * Følgende klasser på 5 og 10 km:
         * 11 - 12 år
         * 13 - 14 år
         * 15 - 16 år
         * 17 - 34 år
         * 35 - 54 år
         * 55 - 99 år
         * 
         * Barneløp 2 km:
         * 7 - 12 år - Tidtakning men ingen rangering.
         * Premie til samtlige deltakere
         * 
         * Barneløp 500m:
         * 3-6 år - Uten tidtaking.
         * Premie til samtlige deltakere.
         * 
         * */

        private DateTime _raceTime;
        private int raceLength = -1;

        public Race(string name, int year, bool male, DateTime raceday)
        {
            var age = DateTime.Now.Year - year;
            var raceage = "";
            var raceagestring = "";
            key = male ? "G" : "J";
            var gender = male ? "Gutter" : "Jenter";

            if (age < 13)
            {
                raceage = "11";
                raceagestring = "11 - 12";
            }
            else if (age < 15)
            {
                raceage = "13";
                raceagestring = "13 - 14";
            }
            else if (age < 17)
            {
                raceage = "15";
                raceagestring = "15 - 16";
            }
            else if (age < 23)
            {
                raceage = "17";
                raceagestring = "17 - 22";
                gender = male ? "Junior menn" : "Junior kvinner";
            }
            else if (age < 35)
            {
                raceage = "23";
                raceagestring = "23 - 34";
                gender = male ? "Senior menn" : "Senior kvinner";
            }
            else if (age < 50)
            {
                raceage = "35";
                raceagestring = "35 - 49";
                gender = male ? "Veteran menn" : "Veteran kvinner";
            }
            else
            {
                raceage = "50";
                raceagestring = "50+";
                gender = male ? "Eldre veteran menn" : "Eldre veteran kvinner";
            }

            this.name = name;
            switch (this.name.ToLowerInvariant())
            {
                case "10 km":
                case "10 km tineansatt":
                case "10 km uil håndball g15":
                    key = $"10KM{key}";
                    _raceTime = new DateTime(raceday.Year, raceday.Month, raceday.Day, raceday.Hour + 1, raceday.Minute + 15, raceday.Second);
                    this.name = male ? "Menn 10km" : "Kvinner 10km";
                    raceLength = 10000;
                    break;
                case "5 km":
                case "5 km tineansatt":
                case "5 km uil håndball g15":
                    key = $"5KM{key}{raceage}";
                    _raceTime = new DateTime(raceday.Year, raceday.Month, raceday.Day, raceday.Hour + 1, raceday.Minute + 30, raceday.Second);
                    this.name = $"{gender} 5km {raceagestring} år";
                    raceLength = 5000;
                    break;
                case "5 km trim":
                    key = "TRIM";
                    _raceTime = new DateTime(raceday.Year, raceday.Month, raceday.Day, raceday.Hour + 1, raceday.Minute + 30, raceday.Second);
                    this.name = "Trimklasse uten tidtakning";
                    raceLength = 4999;
                    break;
                case "2 km barneløp 7-12":
                    key = "2KM";
                    _raceTime = new DateTime(raceday.Year, raceday.Month, raceday.Day, raceday.Hour, raceday.Minute + 15, raceday.Second);
                    this.name = "Barneløp 2 km, 7 - 12 år";
                    raceLength = 2000;
                    break;
                case "500m barneløp 4-6":
                case "600m barneløp 3-6":
                    key = "600M";
                    _raceTime = new DateTime(raceday.Year, raceday.Month, raceday.Day, raceday.Hour, raceday.Minute, raceday.Second);
                    this.name = "Barneløp 600 meter, 3 - 6 år";
                    raceLength = 600;
                    break;
                default:
                    throw new Exception($"Illegal string: {this.name}");
            }
        }

        public int GetLength()
        {
            return Length;
        }

        public string key { get; private set; }

        public string status
        {
            get { return "0"; }
        }

        public string name { get; private set; }

        private int Length
        {
            get { return raceLength; }
        }

        public string fStartTime
        {
            get { return RaceTimeParser.GetTimestring(_raceTime); }
        }

        public string intervalTime
        {
            get { return "0"; }
        }

        public string noStart
        {
            get { return "1"; }
        }

        public string sortRunTime
        {
            get
            {
                return Length < 3000 ? "False" : "True";
            }
        }
    }
}

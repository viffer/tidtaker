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
                raceage = "1";
                raceagestring = "11 - 12";
            }
            else if (age < 15)
            {
                raceage = "2";
                raceagestring = "13 - 14";
            }
            else if (age < 17)
            {
                raceage = "3";
                raceagestring = "15 - 16";
            }
            else if (age < 23)
            {
                raceage = "4";
                raceagestring = "17 - 22";
                gender = male ? "Junior menn" : "Junior kvinner";
            }
            else if (age < 35)
            {
                raceage = "4";
                raceagestring = "23 - 34";
                gender = male ? "Senior menn" : "Senior kvinner";
            }
            else if (age < 50)
            {
                raceage = "5";
                raceagestring = "35 - 49";
                gender = male ? "Veteran menn" : "Veteran kvinner";
            }
            else
            {
                raceage = "6";
                raceagestring = "50+";
                gender = male ? "Eldre veteran menn" : "Eldre veteran kvinner";
            }

            this.name = name;
            switch (this.name.ToLowerInvariant())
            {
                case "10 km":
                case "10 km tineansatt":
                case "10 km uil håndball g15":
                    key = string.Format("10KM{0}", key);
                    _raceTime = new DateTime(raceday.Year, raceday.Month, raceday.Day, raceday.Hour + 1, raceday.Minute + 15, raceday.Second);
                    this.name = male ? "Menn 10km" : "Kvinner 10km";
                    raceLength = 10000;
                    break;
                case "5 km":
                case "5 km tineansatt":
                case "5 km uil håndball g15":
                    key = string.Format("5KM{0}{1}", key, raceage);
                    _raceTime = new DateTime(raceday.Year, raceday.Month, raceday.Day, raceday.Hour + 1, raceday.Minute + 30, raceday.Second);
                    this.name = string.Format("{0} 5km {1} år", gender, raceagestring);
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
            get
            {
                //switch (key.ToLowerInvariant())
                //{
                //    case "d10km":
                //    case "m10km":
                //    case "1d10km":
                //    case "2d10km":
                //    case "3d10km":
                //    case "4d10km":
                //    case "5d10km":
                //    case "6d10km":
                //    case "1m10km":
                //    case "2m10km":
                //    case "3m10km":
                //    case "4m10km":
                //    case "5m10km":
                //    case "6m10km":
                //    case "j10km":
                //    case "g10km":
                //    case "1j10km":
                //    case "2j10km":
                //    case "3j10km":
                //    case "4j10km":
                //    case "5j10km":
                //    case "6j10km":
                //    case "1g10km":
                //    case "2g10km":
                //    case "3g10km":
                //    case "4g10km":
                //    case "5g10km":
                //    case "6g10km":
                //    case "10km d":
                //    case "10km d1":
                //    case "10km d2":
                //    case "10km d3":
                //    case "10km d4":
                //    case "10km d5":
                //    case "10km d6":
                //    case "10km j":
                //    case "10km j1":
                //    case "10km j2":
                //    case "10km j3":
                //    case "10km j4":
                //    case "10km j5":
                //    case "10km j6":
                //    case "10km m":
                //    case "10km m1":
                //    case "10km m2":
                //    case "10km m3":
                //    case "10km m4":
                //    case "10km m5":
                //    case "10km m6":
                //    case "10km g":
                //    case "10km g1":
                //    case "10km g2":
                //    case "10km g3":
                //    case "10km g4":
                //    case "10km g5":
                //    case "10km g6":
                //        return 10000;
                //    case "1d5km":
                //    case "2d5km":
                //    case "3d5km":
                //    case "4d5km":
                //    case "5d5km":
                //    case "6d5km":
                //    case "1m5km":
                //    case "2m5km":
                //    case "3m5km":
                //    case "4m5km":
                //    case "5m5km":
                //    case "6m5km":
                //    case "1j5km":
                //    case "2j5km":
                //    case "3j5km":
                //    case "4j5km":
                //    case "5j5km":
                //    case "6j5km":
                //    case "1g5km":
                //    case "2g5km":
                //    case "3g5km":
                //    case "4g5km":
                //    case "5g5km":
                //    case "6g5km":
                //    case "5km d1":
                //    case "5km d2":
                //    case "5km d3":
                //    case "5km d4":
                //    case "5km d5":
                //    case "5km d6":
                //    case "5km j1":
                //    case "5km j2":
                //    case "5km j3":
                //    case "5km j4":
                //    case "5km j5":
                //    case "5km j6":
                //    case "5km g1":
                //    case "5km g2":
                //    case "5km g3":
                //    case "5km g4":
                //    case "5km g5":
                //    case "5km g6":
                //    case "5km m1":
                //    case "5km m2":
                //    case "5km m3":
                //    case "5km m4":
                //    case "5km m5":
                //    case "5km m6":
                //        return 5000;
                //    case "trim":
                //        return 4999;
                //    case "2km":
                //        return 2000;
                //    case "500m":
                //    case "600m":
                //        return 600;
                //    default:
                //        return -1;
                //}
                return raceLength;
            }
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

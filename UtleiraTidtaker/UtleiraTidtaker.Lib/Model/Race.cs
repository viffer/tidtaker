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

        public Race(string name, int year, bool male)
        {
            var age = DateTime.Now.Year - year;
            var raceage = "";
            var raceagestring = "";
            if (age < 9)
            {
                raceage = "1";
                raceagestring = "8";
            }
            else if (age < 10)
            {
                raceage = "2";
                raceagestring = "9";
            }
            else if (age < 11)
            {
                raceage = "3";
                raceagestring = "10";
            }
            else if (age < 12)
            {
                raceage = "4";
                raceagestring = "11";
            }
            else if (age < 13)
            {
                raceage = "5";
                raceagestring = "12";
            }
            else if (age < 14)
            {
                raceage = "6";
                raceagestring = "13";
            }
            else if (age < 15)
            {
                raceage = "7";
                raceagestring = "14";
            }
            else if (age < 16)
            {
                raceage = "8";
                raceagestring = "15";
            }
            else
            {
                raceage = "9";
                raceagestring = "16";
            }

            key = male ? "M" : "K";
            var gender = male ? "Gutter" : "Jenter";
            this.name = name;
            switch (this.name.ToLowerInvariant())
            {
                case "g 8 år":
                    key = string.Format("{0}{1}500M", raceage, key);
                    _raceTime = new DateTime(2017, 3, 4, 11, 0, 0);
                    this.name = string.Format("{0} {1} {2} år", gender, this.name, raceagestring);
                    break;
                case "j 8 år":
                    key = string.Format("{0}{1}500M", raceage, key);
                    _raceTime = new DateTime(2017, 3, 4, 11, 5, 0);
                    this.name = string.Format("{0} {1} {2} år", gender, this.name, raceagestring);
                    break;
                case "g 9 år":
                    key = string.Format("{0}{1}1KM", raceage, key);
                    _raceTime = new DateTime(2017, 3, 4, 11, 5, 0);
                    break;
                case "j 9 år":
                    key = string.Format("{0}{1}1KM", raceage, key);
                    _raceTime = new DateTime(2017, 3, 4, 11, 5, 0);
                    break;
                case "2 km barneløp 7-12":
                    key = "2KM";
                    _raceTime = new DateTime(2016, 6, 18, 11, 10, 0);
                    this.name += " år";
                    break;
                case "500m barneløp 4-6":
                    key = "500M";
                    _raceTime = new DateTime(2016, 6, 18, 11, 0, 0);
                    this.name += " år";
                    break;
                default:
                    key = this.name;
                    _raceTime = new DateTime(2017, 3, 4, 11, 0, 0);
                    this.name = $"{gender} {name} {raceagestring}";
                    break;
            }
        }

        public int Id { get; set; }

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
                switch (key.ToLowerInvariant())
                {
                    case "1d10km":
                    case "2d10km":
                    case "3d10km":
                    case "4d10km":
                    case "5d10km":
                    case "6d10km":
                    case "1m10km":
                    case "2m10km":
                    case "3m10km":
                    case "4m10km":
                    case "5m10km":
                    case "6m10km":
                        return 10000;
                    case "1d5km":
                    case "2d5km":
                    case "3d5km":
                    case "4d5km":
                    case "5d5km":
                    case "6d5km":
                    case "1m5km":
                    case "2m5km":
                    case "3m5km":
                    case "4m5km":
                    case "5m5km":
                    case "6m5km":
                        return 5000;
                    case "t5km":
                        return 4999;
                    case "2km":
                        return 2000;
                    case "500m":
                        return 500;
                    default:
                        return -1;
                }
            }
        }

        public string fStartTime
        {
            get { return RaceTimeParser.GetTimestring(_raceTime); }
        }

        public DateTime dStartTime
        {
            get { return _raceTime; }
        }

        public string intervalTime
        {
            get { return "1"; }
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

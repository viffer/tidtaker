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

            key = male ? "G" : "J";
            var gender = male ? "Gutter" : "Jenter";
            this.name = name;
            var raceLength = "";
            switch (this.name.ToLowerInvariant())
            {
                case "g 8 år":
                    _raceTime = new DateTime(2017, 3, 4, 11, 0, 0);
                    raceLength = "500 m";
                    break;
                case "j 8 år":
                    _raceTime = new DateTime(2017, 3, 4, 11, 5, 0);
                    raceLength = "500 m";
                    break;
                case "g 9 år":
                    //key = $"{raceage}{key}1KM";
                    _raceTime = new DateTime(2017, 3, 4, 11, 5, 0);
                    raceLength = "1 km";
                    break;
                case "j 9 år":
                    //key = $"{raceage}{key}1KM";
                    _raceTime = new DateTime(2017, 3, 4, 11, 5, 0);
                    raceLength = "1 km";
                    break;
                case "g 10 år":
                    _raceTime = new DateTime(2017, 3, 4, 11, 30, 0);
                    raceLength = "1 km";
                    break;
                case "j 10 år":
                    _raceTime = new DateTime(2017, 3, 4, 11, 30, 0);
                    raceLength = "1 km";
                    break;
                case "g 11 år":
                    _raceTime = new DateTime(2017, 3, 4, 11, 30, 0);
                    raceLength = "2 km";
                    break;
                case "j 11 år":
                    _raceTime = new DateTime(2017, 3, 4, 11, 30, 0);
                    raceLength = "2 km";
                    break;
                case "g 12 år":
                    _raceTime = new DateTime(2017, 3, 4, 11, 30, 0);
                    raceLength = "2 km";
                    break;
                case "j 12 år":
                    _raceTime = new DateTime(2017, 3, 4, 11, 30, 0);
                    raceLength = "2 km";
                    break;
                case "g 13 år":
                    _raceTime = new DateTime(2017, 3, 4, 11, 30, 0);
                    raceLength = "3 km";
                    break;
                case "j 13 år":
                    _raceTime = new DateTime(2017, 3, 4, 11, 30, 0);
                    raceLength = "3 km";
                    break;
                case "g 14 år":
                    _raceTime = new DateTime(2017, 3, 4, 11, 30, 0);
                    raceLength = "3 km";
                    break;
                case "j 14 år":
                    _raceTime = new DateTime(2017, 3, 4, 11, 30, 0);
                    raceLength = "3 km";
                    break;
                case "g 15 år":
                    _raceTime = new DateTime(2017, 3, 4, 11, 30, 0);
                    raceLength = "5 km";
                    break;
                case "j 15 år":
                    _raceTime = new DateTime(2017, 3, 4, 11, 30, 0);
                    raceLength = "5 km";
                    break;
                case "g 16 år":
                    _raceTime = new DateTime(2017, 3, 4, 11, 30, 0);
                    raceLength = "5 km";
                    break;
                case "j 16 år":
                    _raceTime = new DateTime(2017, 3, 4, 11, 30, 0);
                    raceLength = "5 km";
                    break;
                default:
                    //key = this.name;
                    _raceTime = new DateTime(2017, 3, 4, 11, 0, 0);
                    //this.name = $"{gender} {name} {raceagestring}";
                    break;
            }
            //key = $"{raceage}{key}{raceagestring}";
            key = $"{key}{raceagestring}";
            //this.name = $"{gender} {raceagestring} år";
            this.name = $"{gender} {raceagestring} år - {raceLength}";
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
                    case "g8":
                    case "j8":
                        return 500;
                    case "g9":
                    case "j9":
                    case "g10":
                    case "j10":
                        return 1000;
                    case "g11":
                    case "j11":
                    case "g12":
                    case "j12":
                        return 2000;
                    case "g13":
                    case "j13":
                    case "g14":
                    case "j14":
                        return 3000;
                    case "g15":
                    case "j15":
                    case "g16":
                    case "j16":
                        return 5000;
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

        public void SetStartTime(DateTime value)
        {
            _raceTime = value;
        }
    }
}

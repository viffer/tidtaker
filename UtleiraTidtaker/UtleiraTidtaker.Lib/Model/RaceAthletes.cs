namespace UtleiraTidtaker.Lib.Model
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using UtleiraTidtaker.Lib.Utilities;

    public class RaceAthletes
    {
        /*
         * {
         *   "lastUpdate":"Sat Sep 12 2015 19:03:31 GMT 0200 (W. Europe Summer Time)",
         *   "newUpdate":"Sat Sep 12 2015 19:03:31 GMT 0200 (W. Europe Summer Time)",
         *   "athletes":[
         *   {
         *     "Key":"1",
         *     "startNo":"1",
         *     "pName":"Anniken",
         *     "sName":"Løkkeberg",
         *     "club":"",
         *     "run":"K10",
         *     "startTime":"Sat Sep 12 2015 12:00:00 GMTpluss0100 (CET)",
         *     "status":"DNS"
         *   },{
         *     "Key":"2",
         *     "startNo":"2",
         *     "pName":"Mads",
         *     "sName":"Trøan",
         *     "club":"",
         *     "run":"M10",
         *     "startTime":"Sat Sep 12 2015 12:00:00 GMTpluss0100 (CET)",
         *     "status":"DNS"
         *   }]
         * */

        private readonly DateTime _filetime;
        private List<Athlete> _athletes;
        private List<RaceAthlete> _raceathletes;

        public RaceAthletes(List<Athlete> athletes, DateTime filetime)
        {
            _filetime = filetime;
            _athletes = athletes;
            SetStartNumbers();
        }

        public string lastUpdate
        {
            get { return RaceTimeParser.GetRaceupdateTimestring(_filetime); }
        }

        public string newUpdate
        {
            get { return RaceTimeParser.GetRaceupdateTimestring(DateTime.Now); }
        }

        private void SetStartNumbers()
        {
            var sortedList = new SortedList<string, RaceAthlete>();
            foreach (var athlete in _athletes)
            {
                var genderKey = athlete.Gender.Equals("M") ? 1 : 2;
                var birthdateKey = athlete.Birthdate.Year >= 2009 ? 2009 : athlete.Birthdate.Year;
                var key = $"{2017 - birthdateKey:00}-{genderKey}-{athlete.Race.GetLength():00000}-{athlete.Id:00000000}-{athlete.Key:0000}";
                sortedList.Add(key, new RaceAthlete(athlete));
            }
            var i = 0;
            var previousLength = 0;
            var previousNumber = 0;
            var previousGender = "";
            Race previousRace = null;
            _raceathletes = new List<RaceAthlete>();
            foreach (var element in sortedList)
            {
                var athlete = element.Value;
                var newLength = athlete.GetLength();
                if (previousLength != newLength)
                {
                    previousLength = newLength;
                    switch (newLength)
                    {
                        case 5000:
                            //200 til 275 på 10km
                            //i = 300;
                            //break;
                        case 3000:
                            //76 til 200 på 5km
                            //i = 200;
                            //break;
                        case 2000:
                            //76 til 200 på 5km
                            //i = 120;
                            //break;
                        case 1000:
                            // 1-75 på 2km
                            //i = 35;
                            i += 8;
                            break;
                        case 500:
                            i = 0;
                            break;
                        default:
                            //176 til 200 på Trim
                            i = 2000;
                            break;
                    }
                } else if (!previousGender.Equals(athlete.GetGender()))
                {
                    i += 4;
                }
                while (previousNumber - i < 0)
                {
                    var dummy = new RaceAthlete(new Athlete());
                    dummy.SetRace(previousRace);
                    dummy.SetStartNo(++previousNumber);
                    _raceathletes.Add(dummy);
                }
                athlete.SetStartNo(++i);
                previousNumber = i;
                previousGender = athlete.GetGender();
                previousRace = athlete.GetRace();
                _raceathletes.Add(athlete);
            }

            for (var j = i; j < i + 20; j++)
            {
                var dummy = new RaceAthlete(new Athlete());
                dummy.SetRace(previousRace);
                dummy.SetStartNo(j + 1);
                _raceathletes.Add(dummy);
            }
        }

        public IEnumerable<RaceAthlete> athletes
        {
            get
            {
                if (_raceathletes == null) return null;
                //Sortering
                var test = new SortedDictionary<int, RaceAthlete>();
                foreach (var athlete in _raceathletes)
                {
                    test.Add(Convert.ToInt32(athlete.startNo), athlete);
                }
                return test.Values;
            }
        }

        public IEnumerable<Race> GetRaces()
        {
            Race previousRace = null;
            foreach (var athlete in _raceathletes)
            {
                if (previousRace == null)
                {
                    previousRace = athlete.GetRace();
                    previousRace.SetStartTime(athlete.GetStartTime());
                    yield return previousRace;
                }
                else if (!previousRace.key.Equals(athlete.GetRace().key))
                {
                    previousRace = athlete.GetRace();
                    previousRace.SetStartTime(athlete.GetStartTime());
                    yield return previousRace;
                }
            }
        }
    }
}

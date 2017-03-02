namespace UtleiraTidtaker.Lib.Model
{
    using System;
    using System.Collections.Generic;

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
                var key = string.Format("{0:00000}-{1:00000000}-{2:0000}", athlete.Race.GetLength(), athlete.Id, athlete.Key);
                sortedList.Add(key, new RaceAthlete(athlete));
            }
            var i = 0;
            var previousLength = 0;
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
                        case 10000:
                            //200 til 275 på 10km
                            i = 1000;
                            break;
                        case 5000:
                            //76 til 200 på 5km
                            i = 300;
                            break;
                        case 3000:
                            //76 til 200 på 5km
                            i = 200;
                            break;
                        case 2000:
                            // 1-75 på 2km
                            i = 100;
                            break;
                        case 500:
                            i = 0;
                            break;
                        default:
                            //176 til 200 på Trim
                            i = 2000;
                            break;
                    }
                }
                athlete.SetStartNo(++i);
                _raceathletes.Add(athlete);
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
    }
}

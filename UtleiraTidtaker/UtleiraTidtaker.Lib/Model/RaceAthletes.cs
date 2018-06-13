using System;
using System.Collections.Generic;
using System.Linq;
using UtleiraTidtaker.Lib.Utilities;

namespace UtleiraTidtaker.Lib.Model
{
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
        private readonly DateTime _racetime;
        private readonly Config _config;
        private List<Athlete> _athletes;
        private List<RaceAthlete> _raceathletes;

        public RaceAthletes(List<Athlete> athletes, DateTime racetime, DateTime filetime, Config config)
        {
            _filetime = filetime;
            _racetime = racetime;
            _athletes = athletes;
            _config = config;
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
            RaceAthlete previousAthlete = null;
            RaceAthlete lastAthlete = null;
            _raceathletes = new List<RaceAthlete>();
            foreach (var element in sortedList)
            {
                var athlete = element.Value;
                var newLength = athlete.GetLength();
                if (previousLength != newLength)
                {
                    previousLength = newLength;
                    //var next = -1;
                    var next = _config.StartNumbers[newLength] - 1;
                    //switch (newLength)
                    //{
                    //    case 10000:
                    //        //200 til 299 på 10km
                    //        next = 199;
                    //        break;
                    //    case 5000:
                    //        //1 til 99 på 5km
                    //        next = 0;
                    //        break;
                    //    case 2000:
                    //        // 400-499 på 2km
                    //        next = 399;
                    //        break;
                    //    case 600:
                    //        next = 449;
                    //        break;
                    //    default:
                    //        //400++ på Trim
                    //        next = 499;
                    //        break;
                    //}

                    if (previousAthlete != null)
                    {
                        // fill in the gaps
                        for (var j = i + 1; j <= next; j++)
                        {
                            previousAthlete = new RaceAthlete(new Athlete(previousAthlete.GetAthlete()));
                            previousAthlete.SetStartNo(j);
                            if (_raceathletes.Any(x => x.startNo == j.ToString()))
                            {
                                continue;
                            }
                            _raceathletes.Add(previousAthlete);
                        }
                    }

                    i = next;
                }
                athlete.SetStartNo(++i);
                if (lastAthlete == null)
                {
                    lastAthlete = athlete;
                } else if (Convert.ToInt32(athlete.startNo) > Convert.ToInt32(lastAthlete.startNo))
                {
                    lastAthlete = athlete;
                }
                _raceathletes.Add(athlete);
                previousAthlete = athlete;
            }

            if (lastAthlete == null) return;

            // add 30 more
            var startno = Convert.ToInt32(lastAthlete.startNo) + 1;
            for (var k = startno; k < startno + 50; k++)
            {
                lastAthlete = new RaceAthlete(new Athlete(lastAthlete.GetAthlete()));
                lastAthlete.SetStartNo(k);
                if (_raceathletes.Any(x => x.startNo == k.ToString()))
                {
                    continue;
                }
                _raceathletes.Add(lastAthlete);
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

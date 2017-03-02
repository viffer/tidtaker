namespace UtleiraTidtaker.Lib.Model
{
    using System;
    using UtleiraTidtaker.Lib.Utilities;

    public class RaceAthlete
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

        private string _startTime = null;

        private DateTime _raceDTG = new DateTime(2017, 3, 4, 11, 0, 0);

        private readonly Athlete _athlete;

        public RaceAthlete(Athlete athlete)
        {
            _athlete = athlete;
        }

        public void SetStartNo(int value)
        {
            _athlete.Id = value;
        }

        public string key
        {
            get { return _athlete.Id.ToString(); }
        }

        public string startNo
        {
            get { return _athlete.Id.ToString(); }
        }

        public string pName
        {
            get { return _athlete.Name ?? ""; }
        }

        public string sName
        {
            get { return _athlete.Surname ?? ""; }
        }

        public string club
        {
            get { return _athlete.Club ?? ""; }
        }

        public string run
        {
            get { return _athlete.Race != null ? _athlete.Race.key : ""; }
        }

        public string startTime
        {
            get { return _athlete.Race != null ? RaceTimeParser.GetTimestring(GetStartTime()) : ""; }
            //get { return _athlete.Race != null ? RaceTimeParser.GetTimestring(_athlete.Race.dStartTime.AddSeconds(15 * (_athlete.Id - _athlete.Race.Id))) : ""; }
            //get { return RaceTimeParser.GetTimestring(_athlete.Race.dStartTime); }// .AddSeconds(15 * (_athlete.Id - _athlete.Race.Id))); }
        }

        public string status
        {
            get { return ""; }
        }

        public int GetLength()
        {
            return _athlete.Race.GetLength();
        }

        public string GetGender()
        {
            return _athlete.Gender;
        }

        public Race GetRace()
        {
            return _athlete.Race;
        }

        public void SetRace(Race race)
        {
            _athlete.SetRace(race);
        }

        public DateTime GetStartTime()
        {
            return _raceDTG.AddSeconds(15 * (_athlete.Id - _athlete.Race.Id - 1));
        }
    }
}
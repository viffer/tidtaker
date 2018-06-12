namespace UtleiraTidtaker.Lib.Model
{
    using System;
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

        private Athlete _athlete;

        internal RaceAthlete(RaceAthlete raceAthlete)
        {
            _athlete = new Athlete(raceAthlete._athlete);
        }

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
            get { return _athlete.Name; }
        }

        public string sName
        {
            get { return _athlete.Surname; }
        }

        public string club
        {
            get { return _athlete.Club; }
        }

        public string run
        {
            get { return _athlete.Race.key; }
        }

        public string startTime
        {
            get { return _athlete.Race.fStartTime; }
        }

        public string status
        {
            get { return ""; }
        }

        public int GetLength()
        {
            return _athlete.Race.GetLength();
        }

        internal Athlete GetAthlete()
        {
            return _athlete;
        }
    }
}

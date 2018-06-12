﻿using System;

namespace UtleiraTidtaker.Lib.Model
{
    public class Athlete
    {
        private DateTime _raceday;
        public Athlete(DateTime raceday)
        {
            _raceday = raceday;
        }

        internal Athlete(Athlete athlete)
        {
            _raceday = athlete._raceday;
            Id = athlete.Id;
            Key = athlete.Key;
            Surname = "";//athlete.Surname;
            Name = "";//athlete.Name;
            Club = "";//athlete.Club;
            Gender = athlete.Gender;
            Birthdate = athlete.Birthdate;
            Race = athlete.Race;
        }

        public int Id { get; set; }
        public int Key { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Club { get; set; }
        public string Gender { get; set; }
        public DateTime Birthdate { get; set; }

        public string RaceName
        {
            get { return Race.key; }
            set { Race = new Race(value, Birthdate.Year, Gender.Equals("mann", StringComparison.OrdinalIgnoreCase), _raceday); }
        }

        public Race Race { get; private set; }
    }
}

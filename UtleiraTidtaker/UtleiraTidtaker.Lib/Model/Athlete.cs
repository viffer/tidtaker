using System;

namespace UtleiraTidtaker.Lib.Model
{
    public class Athlete
    {
        private DateTime _birthdate;

        public int Id { get; set; }
        public int Key { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Club { get; set; }
        public string Gender { get; set; }

        public DateTime Birthdate
        {
            get { return _birthdate; }
            set
            {
                _birthdate = value;
                Race = new Race("", _birthdate.Year, Gender.Equals("m", StringComparison.OrdinalIgnoreCase));
            }
        }

        public string RaceName
        {
            get { return Race.key; }
            set { Race = new Race(value, _birthdate.Year, Gender.Equals("m", StringComparison.OrdinalIgnoreCase)); }
        }

        public Race Race { get; private set; }

        public void SetRace(Race race)
        {
            this.Race = race;
        }
    }
}

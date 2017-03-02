using System;

namespace UtleiraTidtaker.Lib.Model
{
    public class Athlete
    {
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
            set { Race = new Race(value, Birthdate.Year, Gender.Equals("m", StringComparison.OrdinalIgnoreCase)); }
        }

        public Race Race { get; private set; }

        public void SetRace(Race race)
        {
            this.Race = race;
        }
    }
}

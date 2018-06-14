using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UtleiraTidtaker.Lib.Model;

namespace UtleiraTidtaker.Lib.Repository
{
    public class AthleteRepository
    {
        //private readonly string[] _userColumns = System.Configuration.ConfigurationSettings.AppSettings["UserColumns"].Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        private readonly IList<Athlete> _athletes = new List<Athlete>();
        private readonly DateTime _raceday;
        private readonly Config _config;

        public AthleteRepository(DataTable data, DateTime raceday, Config config)
        {
            _raceday = raceday;
            _config = config;
            var key = 0;
            var sortedathletes = new SortedList<string, Athlete>();
            foreach (DataRow row in data.Rows)
            {
                var athlete = new Athlete(_raceday, _config)
                {
                    Key = key++,
                    Id = Convert.ToInt32(row["OrderID"]),
                    Surname = row["Etternavn"].ToString(),
                    Name = row["Fornavn"].ToString(),
                    Club = row["Klubb"].ToString(),
                    Gender = row["Kjønn"].ToString(),
                    Birthdate = DateTime.Parse(row["Fødselsdato"].ToString()),
                    RaceName = row["Distanse/øvelse og klasse"].ToString()
                };
                //_athletes.Add(athlete);
                sortedathletes.Add($"{athlete.Id:0000000}{athlete.Key:0000}", athlete);
            }
            _athletes = sortedathletes.Values;
        }

        public List<Athlete> GetAthletes()
        {
            return _athletes.ToList();
        }

        public IEnumerable<Race> GetRaces()
        {
            var raceDictionary = new SortedDictionary<string, Race>();
            foreach (var athlete in _athletes)
            {
                var racename = athlete.RaceName;
                if (raceDictionary.ContainsKey(racename)) continue;
                raceDictionary.Add(racename, athlete.Race);
            }


            // Try to fill the races
            var racenames = new[]
            {
                "10 km",
                "10 km tineansatt",
                "10 km uil håndball g15",
                "5 km",
                "5 km tineansatt",
                "5 km uil håndball g15",
                "5 km trim",
                "2 km barneløp 7-12",
                "500m barneløp 4-6",
                "600m barneløp 3-6"
            };
            var genders = new[] {"Mann", "Kvinne"};
            for (var i = 1; i < 99; i++)
            {
                foreach (var racename in racenames)
                {
                    foreach (var gender in genders)
                    {
                        var athlete = new Athlete(_raceday, _config)
                        {
                            Birthdate = DateTime.Now.AddYears(-i),
                            Gender = gender,
                            RaceName = racename
                        };
                        var rname = athlete.RaceName;
                        if (raceDictionary.ContainsKey(rname)) continue;
                        raceDictionary.Add(rname, athlete.Race);
                    }
                }
            }

            foreach (var element in raceDictionary)
            {
                yield return element.Value;
            }
        }
    }
}

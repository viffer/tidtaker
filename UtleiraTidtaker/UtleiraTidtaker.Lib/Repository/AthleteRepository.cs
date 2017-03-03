namespace UtleiraTidtaker.Lib.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using UtleiraTidtaker.Lib.Model;

    public class AthleteRepository
    {
        //private readonly string[] _userColumns = System.Configuration.ConfigurationSettings.AppSettings["UserColumns"].Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        private readonly IList<Athlete> _athletes = new List<Athlete>();

        public AthleteRepository(DataTable data)
        {
            var key = 0;
            var sortedathletes = new SortedList<string, Athlete>();
            //var i = -1;

            var dict = FindRowNumber(data, new[] { "CompetitorId", "LastName", "Firstname", "ClubName", "Sex", "BirthDay", "BirthMonth", "BirthYear", "EntryClassShortName" });

            foreach (DataRow row in data.Rows)
            {
                //i++;
                //if (i == 0) continue;
                //var athlete = new Athlete
                //{
                //    Key = key++,
                //    Id = Convert.ToInt32(row["OrderID"]),
                //    Surname = row["Etternavn"].ToString(),
                //    Name = row["Fornavn"].ToString(),
                //    Club = row["Klubb"].ToString(),
                //    Gender = row["Kjønn"].ToString(),
                //    Birthdate = DateTime.Parse(row["Fødselsdato"].ToString()),
                //    RaceName = row["Distanse/øvelse og klasse"].ToString(),
                //};
                var athlete = new Athlete
                {
                    Key = key++,
                    Id = Convert.ToInt32(row[GetDictionaryValue(dict, "CompetitorId")]),
                    Surname = row[GetDictionaryValue(dict, "LastName")].ToString(),
                    Name = row[GetDictionaryValue(dict, "Firstname")].ToString(),
                    Club = row[GetDictionaryValue(dict, "ClubName")].ToString(),
                    Gender = row[GetDictionaryValue(dict, "Sex")].ToString(),
                    Birthdate = DateTime.Parse($"{row[GetDictionaryValue(dict, "BirthYear")]}-{row[GetDictionaryValue(dict, "BirthMonth")]}-{row[GetDictionaryValue(dict, "BirthDay")]}"),
                    RaceName = row[GetDictionaryValue(dict, "EntryClassShortName")].ToString(),
                };
                // Fixing wrong ages:

                if (athlete.Id == 208254290)
                {
                    var old = athlete.Birthdate;
                    athlete.Birthdate = new DateTime(2007, old.Month, old.Day);
                }

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
            foreach (var element in raceDictionary)
            {
                yield return element.Value;
            }
        }

        private int GetDictionaryValue(Dictionary<string, int> dict, string key)
        {
            int value;
            if (dict.TryGetValue(key, out value)) return value;
            throw new Exception("Not found!");
        }

        private Dictionary<string, int> FindRowNumber(DataTable data, string[] names)
        {
            var dict = new Dictionary<string, int>();

            foreach (var name in names)
            {
                foreach (DataColumn column in data.Columns)
                {
                    if (!column.ColumnName.Equals(name, StringComparison.OrdinalIgnoreCase)) continue;
                    dict.Add(name, column.Ordinal);
                    break;
                }
                if (dict.ContainsKey(name)) continue;
                dict.Add(name, -1);
            }

            return dict;
        }
    }
}

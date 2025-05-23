
using System.IO;
using WorkLife.Dal;
using WorkLife.Dal.Contract;
using WorkLife.Model.Contract;

namespace WorkLife.Model
{
    public class DataProvider : IDataProvider
    {
        private IDbAccess _dbAccess = new DbAccess();

        private PersonBuilder _personBuilder;

        private IList<TargetTimeWeek> _targetWorkingTimes = new List<TargetTimeWeek>();

        public DataProvider() 
        {
            _personBuilder = new PersonBuilder(this);

            // ToDo: proper error handling
            if (_dbAccess.Connect())
            {
                Persons = _dbAccess.LoadPersons(_personBuilder);
                _targetWorkingTimes = _dbAccess.LoadTargetTimeWeeks();
            }
        }

        public IList<IPerson> Persons { get; } = new List<IPerson>();

        public async Task<string> ExportByDate(DateOnly date)
        {
            var userDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var fileName = Path.Combine(userDir, "Documents\\WorkLife_Export.csv");

            await Task.Run(() =>
            {
                var lines = new List<string>(["Persnr;Datum;TagesSollzeit;TagesArbeitszeit;TagesSaldo;GesamtSaldo"]);
                
                int daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);

                foreach (var person in Persons)
                {
                    IndustryTime total = 0.0;
                    for (int day = 1; day <= daysInMonth; day++)
                    {
                        DateOnly current = new DateOnly(date.Year, date.Month, day);
                        (var dailyTotal, var dailyTarget) = person.GetDailyTotalAndTarget(current);
                        total += dailyTotal;
                        lines.Add(
                        $"{person.EmployeeId};{current.ToShortDateString()};{dailyTarget.Value};{dailyTotal.Value};{dailyTotal - dailyTarget};{total.Value}");
                    }
                }
                File.WriteAllLines(fileName, lines);
            });

            return fileName;
        }

        public TargetTimeWeek GeTargetWorkingTimesByDate(TargetTimeModel targetTimeModel, DateOnly date)
        {
            var targeWorkingTimes = _targetWorkingTimes.Where(p => p.TargetTimeModel == targetTimeModel && p.ValidFromDate < date).ToList();

            // The list can contain multiple entries with different valid dates.
            // We just need to pick the latest valid date.
            // Ordering makes independent of the order in the DB
            return targeWorkingTimes.OrderBy(p => p.ValidFromDate).LastOrDefault() ?? new TargetTimeWeek();
        }
    }
}

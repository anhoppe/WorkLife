using System.ComponentModel;
using WorkLife.Model.Contract;

namespace WorkLife.Model
{
    internal class Person : IPerson
    {
        private readonly double _minutesPerHour = 60.0;

        private IDataProvider _dataProvider;

        private IDictionary<DateOnly, IndustryTime> _workingTimeByDate = new Dictionary<DateOnly, IndustryTime>();

        public Person(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public string EmployeeId { get; internal set; } = string.Empty;

        public string Name { get; internal set; } = string.Empty;

        internal IList<(DateOnly, TargetTimeModel)> AppliedTargetTime { get; set; } = new List<(DateOnly, TargetTimeModel)>();

        internal int Id { get; set; }
        
        public (IndustryTime, IndustryTime) GetDailyTotalAndTarget(DateOnly date)
        {
            var targetTimeWorkingWeek = GetTargetWorkTimeByDate(date);

            IndustryTime dailyTotal = 0.0;

            if (_workingTimeByDate.ContainsKey(date))
            {
                dailyTotal = _workingTimeByDate[date];
            }
            
            IndustryTime dailyTargetTime = targetTimeWorkingWeek.TargetWorkingTimeMin[(int)date.DayOfWeek] / _minutesPerHour;

            return (dailyTotal, dailyTargetTime);
        }

        public (IndustryTime, IndustryTime) GetMonthlyTotalAndTarget(DateOnly date)
        {
            IndustryTime monthlyTotal = 0.0;
            IndustryTime monthlyTarget = 0.0;

            int daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);

            for (int day = 1; day <= daysInMonth; day++)
            {
                DateOnly current = new DateOnly(date.Year, date.Month, day);
                (var dailyTotal, var dailyTarget) = GetDailyTotalAndTarget(current);
                monthlyTotal += dailyTotal;
                monthlyTarget += dailyTarget;
            }

            return (monthlyTotal, monthlyTarget);
        }

        public IndustryTime GetWorkingTimeByDate(DateOnly date)
        {
            if (_workingTimeByDate.TryGetValue(date, out IndustryTime workingTime))
            {
                return workingTime;
            }

            return 0.0;
        }

        public void TrackTime(IndustryTime timeToTrack, DateOnly date)
        {
            if (!_workingTimeByDate.ContainsKey(date))
            {
                _workingTimeByDate[date] = 0.0;
            }

            _workingTimeByDate[date] += timeToTrack;
        }

        private TargetTimeWeek GetTargetWorkTimeByDate(DateOnly date)
        {
            if (_dataProvider == null)
            {
                throw new InvalidOperationException("DataProvider is not set.");
            }

            var timeModelAtDate = AppliedTargetTime.OrderBy(p => p.Item1).LastOrDefault(p => p.Item1 <= date).Item2;

            return _dataProvider.GeTargetWorkingTimesByDate(timeModelAtDate, date);
        }
    }
}

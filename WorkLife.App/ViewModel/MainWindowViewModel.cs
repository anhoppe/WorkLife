using System.Collections.ObjectModel;
using System.Windows;
using WorkLife.Model.Contract;

namespace WorkLife.App.ViewModel
{
    public class MainWindowViewModel : BindableBase
    {
        private string _dailyValuesText = "Select person";
        
        private string _monthlyValuesText = "Select person";

        private DateTime _selectedDate = DateTime.Now;

        private IPerson? _selectedPerson = null;

        private string _workingTime = string.Empty;

        public MainWindowViewModel()
        {
            Load = new DelegateCommand(() =>
            {
                UpdateTotalAndBalance();
            },
            () => SelectedPerson != null);

            Save = new DelegateCommand(() =>
            {
                if (SelectedPerson != null)
                {
                    SelectedPerson.TrackTime(WorkingTime, DateOnly.FromDateTime(SelectedDate));
                    WorkingTime = string.Empty;
                    UpdateTotalAndBalance();
                }
            },
            () =>
            {
                IndustryTime workingTime = WorkingTime;
                return SelectedPerson != null && workingTime > 0;
            });

            ExportCommand = new DelegateCommand(async () => 
            {
                if (DataProvider == null)
                {
                    throw new NullReferenceException("DataProvider is not set");
                }
                
                var fileName = await DataProvider.ExportByDate(DateOnly.FromDateTime(SelectedDate));
                MessageBox.Show($"Exported to {fileName}", "Info");
            });
        }

        public string DailyValuesText
        {
            get => _dailyValuesText;
            set => SetProperty(ref _dailyValuesText, value);
        }

        public DelegateCommand ExportCommand { get; }
        
        public DelegateCommand Load { get; }

        public string MonthlyValuesText
        {
            get => _monthlyValuesText;
            set => SetProperty(ref _monthlyValuesText, value);
        }

        public ObservableCollection<IPerson> Persons { get; set; } = new ObservableCollection<IPerson>();

        public DelegateCommand Save { get; }

        public DateTime SelectedDate
        {
            get => _selectedDate;

            set
            {
                if (SetProperty(ref _selectedDate, value))
                {
                    DailyValuesText = $"<<Press Load to see total/balance>>";
                    MonthlyValuesText = $"<<Press Load to see total/balance>>";
                }
            }
        }

        public IPerson? SelectedPerson
        {
            get => _selectedPerson;

            set
            {
                if (SetProperty(ref _selectedPerson, value))
                {
                    Load.RaiseCanExecuteChanged();
                    Save.RaiseCanExecuteChanged();
                    DailyValuesText = $"<<Press Load to see total/balance>>";
                    MonthlyValuesText = $"<<Press Load to see total/balance>>";
                }
            }
        }

        public string WorkingTime
        {
            get => _workingTime;
            set
            {
                if (SetProperty(ref _workingTime, value))
                {
                    Save.RaiseCanExecuteChanged();
                }
            }
        }

        internal IDataProvider? DataProvider { private get; set; }

        internal void Init()
        {
            if (DataProvider == null)
            {
                throw new NullReferenceException("DataProvider is not injected");
            }

            Persons = [.. DataProvider.Persons];
        }

        private void UpdateTotalAndBalance()
        {
            if (SelectedPerson == null)
            {
                throw new NullReferenceException("SelectedPerson not set");
            }

            (var dailyTotal, var dailyTarget) = SelectedPerson.GetDailyTotalAndTarget(DateOnly.FromDateTime(SelectedDate));
            (var monthlyTotal, var monthlyTarget) = SelectedPerson.GetMonthlyTotalAndTarget(DateOnly.FromDateTime(SelectedDate));
            IndustryTime dailyBalance = dailyTotal - dailyTarget;
            IndustryTime monthlyBalance = monthlyTotal - monthlyTarget;

            DailyValuesText = $"Daily Total: {dailyTotal}h, Daily Balance: {dailyBalance}h";
            MonthlyValuesText = $"Monthly Total: {monthlyTotal}h, Monthly Balance: {monthlyBalance}h";
        }
    }
}

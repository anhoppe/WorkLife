using System.Windows;
using WorkLife.App.ViewModel;
using WorkLife.Model;

namespace WorkLife.App.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindowView : Window
    {
        public MainWindowView()
        {
            InitializeComponent();

            // This is a quick hack for DI. In larger applications I would consider using DI, e.g. AutoFac
            // Here we are doing poor mans DI in the view class, not really best practice.
            // For a well-solved DI the projects should only reference other contract projects
            var mainViewModel = new MainWindowViewModel()
            {
                DataProvider = new DataProvider(),
            };
            mainViewModel.Init();
            DataContext = mainViewModel;
        }
    }
}

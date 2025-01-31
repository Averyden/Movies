using System.Windows;

namespace TheMovies
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainViewModel mvm = new MainViewModel();
       
        public MainWindow()
        {
            InitializeComponent();
            DataContext = mvm;
        }

    }
}
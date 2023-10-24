using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using QEAMApp.MVVM.ViewModel;

namespace QEAMApp.MVVM.View
{
    /// <summary>
    /// Interaction logic for CardView.xaml
    /// </summary>
    public partial class CardView : UserControl
    {
        public event EventHandler NavigateBackRequested;

        public CardView()
        {
            InitializeComponent();
        }

        private void ExitCardButton_Click(object sender, RoutedEventArgs e)
        {
            MainViewModel mainViewModel = (MainViewModel)DataContext;
            mainViewModel.CardViewCommand.Execute(null);
            //MessageBox.Show(mainViewModel.CurrentView.ToString());
            // Doesn't go back
        }
    }
}

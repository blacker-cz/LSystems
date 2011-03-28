using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LSystems.Base;

namespace LSystems
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ApplicationWindowBase
    {
        public MainWindow()
        {
            InitializeComponent();

            // attach ViewModel
            this.DataContext = new MainWindowViewModel();
        }

        /// <summary>
        /// Get position of mouse click in preview area
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void previewImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var vm = this.DataContext as MainWindowViewModel;
            vm.ChangePosition(e.GetPosition(this.previewImage));
        }

        /// <summary>
        /// About box, too tired for MVVM binding
        /// @todo: fix me
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            WPFAboutBox about = new WPFAboutBox(this);
            about.ShowDialog();
        }
    }
}

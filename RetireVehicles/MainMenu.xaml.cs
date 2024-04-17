
/* Title:           Main Menu
 * Date:            10-23-17
 * Author:          Terry Holmes
 * 
 * Description:     This is the main menu for the program */

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
using System.Windows.Shapes;

namespace RetireVehicles
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        //setting classes
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();        

        public MainMenu()
        {
            InitializeComponent();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            TheMessagesClass.CloseTheProgram();
        }

        private void btnRetireVehicles_Click(object sender, RoutedEventArgs e)
        {
            ProcessVehicleRetirement ProcessVehicleRetirement = new ProcessVehicleRetirement();
            ProcessVehicleRetirement.Show();
            Close();
        }

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            About About = new About();
            About.ShowDialog();
        }

        private void btnUnRetireVehicles_Copy_Click(object sender, RoutedEventArgs e)
        {
            UnretireVehicle UnretireVehicle = new UnretireVehicle();
            UnretireVehicle.Show();
            Close();
        }
    }
}

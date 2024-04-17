/* Title:           Unretire Vehicles
 * Date:            10-24-17
 * Author:          Terry Holmes
 * 
 * Description:     This is the form used to unretire vehicles. */

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
using NewEventLogDLL;
using NewVehicleDLL;
using DataValidationDLL;

namespace RetireVehicles
{
    /// <summary>
    /// Interaction logic for UnretireVehicle.xaml
    /// </summary>
    public partial class UnretireVehicle : Window
    {
        //setting up the classes
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();
        EventLogClass TheEventLogClass = new EventLogClass();
        VehicleClass TheVehicleClass = new VehicleClass();
        DataValidationClass TheDataValidationClass = new DataValidationClass();

        FindVehicleForUnretirementDataSet TheFindVehicleForUnretirementDataSet = new FindVehicleForUnretirementDataSet();

        public UnretireVehicle()
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

        private void btnMainMenu_Click(object sender, RoutedEventArgs e)
        {
            MainMenu MainMenu = new MainMenu();
            MainMenu.Show();
            Close();
        }

        private void btnFind_Click(object sender, RoutedEventArgs e)
        {
            //setting local variables
            int intBJCNumber;
            string strValueForValidation;
            bool blnFatalError = false;
            int intRecordsReturn;

            try
            {
                strValueForValidation = txtBJCNumber.Text;
                blnFatalError = TheDataValidationClass.VerifyIntegerData(strValueForValidation);
                if (blnFatalError == true)
                {
                    TheMessagesClass.ErrorMessage("The BJC Number is not an Integer");
                    return;
                }

                intBJCNumber = Convert.ToInt32(strValueForValidation);

                TheFindVehicleForUnretirementDataSet = TheVehicleClass.FindVehicleForUnretirement(intBJCNumber);

                intRecordsReturn = TheFindVehicleForUnretirementDataSet.FindVehicleForUnretirement.Rows.Count;

                if(intRecordsReturn == 0)
                {
                    TheMessagesClass.InformationMessage("No Records Found");
                    return;
                }

                dgrResults.ItemsSource = TheFindVehicleForUnretirementDataSet.FindVehicleForUnretirement;
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Retire Vehicles // Unretire Vehicles // Find Button " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }

        private void dgrResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int intSelectedIndex;

            //setting local variables
            int intBJCNumber;
            DataGrid VehicleGrid;
            DataGridRow VehicleRow;
            DataGridCell VehicleID;
            string strVehicleID;

            try
            {
                intSelectedIndex = dgrResults.SelectedIndex;

                if (intSelectedIndex > -1)
                {
                    VehicleGrid = dgrResults;
                    VehicleRow = (DataGridRow)VehicleGrid.ItemContainerGenerator.ContainerFromIndex(VehicleGrid.SelectedIndex);
                    VehicleID = (DataGridCell)VehicleGrid.Columns[0].GetCellContent(VehicleRow).Parent;

                    //converting to String
                    strVehicleID = ((TextBlock)VehicleID.Content).Text;
                    MainWindow.gintVehicleID = Convert.ToInt32(strVehicleID);

                    UnretireSelectedVehicle UnretireSelectedVehicle = new UnretireSelectedVehicle();
                    UnretireSelectedVehicle.ShowDialog();
                }
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Retire Vehicle // Unretire Vehicles // Results Selection Changed " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }
    }
}

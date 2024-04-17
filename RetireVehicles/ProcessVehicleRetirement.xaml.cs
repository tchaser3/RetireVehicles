/* Title:           Process Vehicle Retirement
 * Date:            7-24-17
 * Author:          Terry Holmes */

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
using VehicleStatusDLL;
using DataValidationDLL;

namespace RetireVehicles
{
    /// <summary>
    /// Interaction logic for ProcessVehicleRetirement.xaml
    /// </summary>
    public partial class ProcessVehicleRetirement : Window
    {
        //setting up the classes
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();
        EventLogClass TheEventLogClass = new EventLogClass();
        VehicleClass TheVehicleClass = new VehicleClass();
        VehicleStatusClass TheVehicleStatusClass = new VehicleStatusClass();
        DataValidationClass TheDataValidationClass = new DataValidationClass();

        //setting up the data set
        FindActiveVehicleByBJCNumberDataSet TheFindActiveVehicleByBJCNumberDataSet = new FindActiveVehicleByBJCNumberDataSet();

        public ProcessVehicleRetirement()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            TheMessagesClass.CloseTheProgram();
        }

        private void btnRetire_Click(object sender, RoutedEventArgs e)
        {
            string strValueForValidation;
            int intBJCNumber = 0;
            bool blnFatalError;
            int intVehicleID;
            int intRecordsReturned;

            try
            {
                strValueForValidation = txtBJCNumber.Text;
                blnFatalError = TheDataValidationClass.VerifyIntegerData(strValueForValidation);
                if(blnFatalError == true)
                {
                    TheMessagesClass.ErrorMessage("Not a Valid BJC Number");
                    return;
                }
                else
                {
                    intBJCNumber = Convert.ToInt32(strValueForValidation);
                }

                TheFindActiveVehicleByBJCNumberDataSet = TheVehicleClass.FindActiveVehicleByBJCNumber(intBJCNumber);

                intRecordsReturned = TheFindActiveVehicleByBJCNumberDataSet.FindActiveVehicleByBJCNumber.Rows.Count;

                if(intRecordsReturned == 0)
                {
                    TheMessagesClass.ErrorMessage("Not a Valid BJC Number");
                    return;
                }

                intVehicleID = TheFindActiveVehicleByBJCNumberDataSet.FindActiveVehicleByBJCNumber[0].VehicleID;

                blnFatalError = TheVehicleClass.UpdateVehicleActive(intVehicleID, false);
                if (blnFatalError == false)
                    blnFatalError = TheVehicleStatusClass.RemoveVehicleFromStatus(intVehicleID);

                if(blnFatalError == true)
                {
                    TheMessagesClass.ErrorMessage("There Was A Problem, Contact ID");
                    return;
                }

                TheMessagesClass.InformationMessage("Vehicle Retired");
                txtBJCNumber.Text = "";
                
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Retire Vehicles // Process Vehicle Retirement // Retire Button " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnMainMenu_Click(object sender, RoutedEventArgs e)
        {
            MainMenu MainMenu = new MainMenu();
            MainMenu.Show();
            Close();
        }
    }
}

/* Title:           Retire Vehicles Main Window
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using DataValidationDLL;
using NewEmployeeDLL;
using NewVehicleDLL;
using VehicleStatusDLL;
using NewEventLogDLL;

namespace RetireVehicles
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //setting up the classes
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();
        DataValidationClass TheDataValidationClass = new DataValidationClass();
        EmployeeClass TheEmployeeClass = new EmployeeClass();
        EventLogClass TheEventLogClass = new EventLogClass();

        public static VerifyLogonDataSet TheVerifyLogDataSet = new VerifyLogonDataSet();

        int gintNumberOfMisses;
        public static int gintVehicleID;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnSignIn_Click(object sender, RoutedEventArgs e)
        {
            //setting local variables
            string strValueForValidation;
            int intEmployeeID = 0;
            string strLastName;
            bool blnFatalError = false;
            bool blnThereIsAProblem = false;
            string strErrorMessage = "";
            int intRecordsReturned;

            try
            {
                //data validation
                strValueForValidation = pbxEmployeeID.Password.ToUpper();
                strLastName = txtLastName.Text;
                blnThereIsAProblem = TheDataValidationClass.VerifyIntegerData(strValueForValidation);
                if (blnThereIsAProblem == true)
                {
                    blnFatalError = true;
                    strErrorMessage += "The Employee ID is not an Integer\n";
                }
                else
                {
                    intEmployeeID = Convert.ToInt32(strValueForValidation);
                }
                if (strLastName == "")
                {
                    blnFatalError = true;
                    strErrorMessage += "The Last Name Was Not Entered\n";
                }
                if (blnFatalError == true)
                {
                    TheMessagesClass.ErrorMessage(strErrorMessage);
                    return;
                }

                //getting the employee
                TheVerifyLogDataSet = TheEmployeeClass.VerifyLogon(intEmployeeID, strLastName);

                intRecordsReturned = TheVerifyLogDataSet.VerifyLogon.Rows.Count;

                if (intRecordsReturned == 0)
                {
                    LogonFailed();
                }
                else
                {
                    if (TheVerifyLogDataSet.VerifyLogon[0].EmployeeGroup != "ADMIN")
                    {
                        LogonFailed();
                    }
                    else
                    {
                        MainMenu MainMenu = new MainMenu();
                        MainMenu.Show();
                        Hide();
                    }

                }
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Vehicle Data Entry // Main Window // Sign In Button " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            TheMessagesClass.CloseTheProgram();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            gintNumberOfMisses = 0;

            pbxEmployeeID.Focus();
        }
        private void LogonFailed()
        {
            gintNumberOfMisses++;

            if (gintNumberOfMisses == 3)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "There Have Been Three Attempts to Login Into Vehicle Data Entry");

                TheMessagesClass.ErrorMessage("There Have Been Three Attempts To Sign In\nThe Application Will Now Close");

                Application.Current.Shutdown();
            }
            else
            {
                TheMessagesClass.InformationMessage("You Have Failed the Sign In Process");
            }
        }
    }
}

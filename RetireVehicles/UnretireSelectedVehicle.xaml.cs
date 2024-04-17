/* Title:           Unretire Selected Vehicles
 * Date:            10-24-17
 * Author:          Terry Holmes
 * 
 * Description:     This is will be used to update a vehicle */

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

namespace RetireVehicles
{
    /// <summary>
    /// Interaction logic for UnretireSelectedVehicle.xaml
    /// </summary>
    public partial class UnretireSelectedVehicle : Window
    {
        EventLogClass TheEventLogClass = new EventLogClass();
        VehicleClass TheVehicleClass = new VehicleClass();
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();

        FindVehicleByVehicleIDDataSet TheFindVehicleByVehicleIDDataSet = new FindVehicleByVehicleIDDataSet();

        public UnretireSelectedVehicle()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                TheFindVehicleByVehicleIDDataSet = TheVehicleClass.FindVehicleByVehicleID(MainWindow.gintVehicleID);

                txtAssignedOffice.Text = TheFindVehicleByVehicleIDDataSet.FindVehicleByVehicleID[0].AssignedOffice;
                txtBJCNumber.Text = Convert.ToString(TheFindVehicleByVehicleIDDataSet.FindVehicleByVehicleID[0].BJCNumber);
                txtLicensePlate.Text = TheFindVehicleByVehicleIDDataSet.FindVehicleByVehicleID[0].LicensePlate;
                txtNotes.Text = TheFindVehicleByVehicleIDDataSet.FindVehicleByVehicleID[0].Notes;
                txtOilChangeDate.Text = Convert.ToString(TheFindVehicleByVehicleIDDataSet.FindVehicleByVehicleID[0].OilChangeDate);
                txtOilChangeOdometer.Text = Convert.ToString(TheFindVehicleByVehicleIDDataSet.FindVehicleByVehicleID[0].OilChangeOdometer);
                txtVehicleID.Text = Convert.ToString(TheFindVehicleByVehicleIDDataSet.FindVehicleByVehicleID[0].VehicleID);
                txtVehicleMake.Text = TheFindVehicleByVehicleIDDataSet.FindVehicleByVehicleID[0].VehicleMake;
                txtVehicleModel.Text = TheFindVehicleByVehicleIDDataSet.FindVehicleByVehicleID[0].VehicleModel;
                txtVehicleYear.Text = Convert.ToString(TheFindVehicleByVehicleIDDataSet.FindVehicleByVehicleID[0].VehicleYear);
                txtVINNumber.Text = TheFindVehicleByVehicleIDDataSet.FindVehicleByVehicleID[0].VINNumber;
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Retire Vehicle // Unretire Selected Vehicle // Window Loaded " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }

        private void btnUnretire_Click(object sender, RoutedEventArgs e)
        {
            bool blnFatalError = false;

            try
            {
                blnFatalError = TheVehicleClass.UpdateVehicleActive(MainWindow.gintVehicleID, true);

                if (blnFatalError == true)
                    throw new Exception();

                TheMessagesClass.InformationMessage("The Vehicle Is Reinstated");
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Retire Vehicles // Unretire Selected Vehicles // Unretire Button " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }
    }
}

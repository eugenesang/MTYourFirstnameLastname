using System;
using System.CodeDom;
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

namespace MTYourFirstnameLastname
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();

            
        }

        public enum EmployeeType {
            Hourly,
            Commission,
            Weekly
        }
        private EmployeeType employeeType = EmployeeType.Hourly;
        private List<Employee> employees = new List<Employee>();
        private int EMPLOYEE_COUNT = 0;

        private void LoadSampleEmployees()
        {
            employees.Add(new WeeklyEmployee { EmployeeId = 20, Name = "Eugene", Type = EmployeeType.Weekly, WeeklySalary = 500 });
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;

            if (radioButton != null)
            {
                if (radioButton.IsChecked == true)
                {

                    // Handle the change in radio button selection here
                    if (radioButton.Content.ToString() == "Commission Based")
                    {
                        changeOptions("Gross Sales: ", "Commission Rate: ", true);
                        employeeType = EmployeeType.Commission;
                    }
                    else if (radioButton.Content.ToString() == "Weekly Salary")
                    {
                        changeOptions("Weekly Salary: ", "Weekly Salary: ", false);
                        employeeType = EmployeeType.Weekly;
                    }
                    else if (radioButton.Content.ToString() == "Hourly Paid")
                    {
                        changeOptions("Hours Worked: ", "Hourly Wage: ", true);
                        employeeType = EmployeeType.Hourly;
                    }
                    //ClearInputBoxes();
                }
            }
        }

       private void changeOptions(String worked, String wage, Boolean stackPanel)
        {
            try
            {
                if (stackPanel)
                {
                    hourlyWageStackPanel.Visibility = Visibility.Visible;
                }
                else
                {
                    hourlyWageStackPanel.Visibility = Visibility.Collapsed;
                }
                
                hoursWorkedLabel.Text = worked;
                hourlyWageLabel.Text = wage;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }


        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                // Collect and display values
                string name = nameInput.Text;
                string hoursWorkedText = hoursWorkedInput.Text;
                string hourlyWageText = hourlyWageInput.Text;

                // clear input fields

                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(hoursWorkedText) || string.IsNullOrWhiteSpace(hourlyWageText))
                {
                    // Display error message or take appropriate action
                    MessageBox.Show("Please fill in all fields.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!double.TryParse(hoursWorkedText, out double hoursWorked) || !double.TryParse(hourlyWageText, out double hourlyRate))
                {
                    // Display error message or take appropriate action
                    MessageBox.Show("Please enter valid numeric values for hours worked and hourly wage.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (employeeType == EmployeeType.Hourly)
                {
                    employees.Add(new HourlyEmployee { EmployeeId = EMPLOYEE_COUNT++, Name = name, Type = EmployeeType.Hourly, HoursWorked = hoursWorked, HourlyRate = hourlyRate });
                }
                else if (employeeType == EmployeeType.Weekly)
                {
                    employees.Add(new WeeklyEmployee { EmployeeId = EMPLOYEE_COUNT++, Name = name, Type = EmployeeType.Weekly, WeeklySalary = hoursWorked });
                }
                else if (employeeType == EmployeeType.Commission)
                {
                    employees.Add(new CommissionEmployee { EmployeeId = EMPLOYEE_COUNT++, Name = name, Type = EmployeeType.Commission, CommissionRate = hourlyRate, GrossSales = hoursWorked });
                }


                // Continue with the rest of your logic
                PrintEmployeesListBox();
            } catch(Exception ex) {
                Console.WriteLine(ex);
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PrintEmployeesListBox()
        {
            
            // Clear existing items from the ListBox
            employeesListBox.Items.Clear();

            netOutput.Text = "";
            grossOutput.Text = "";
            taxOutput.Text = "";

            // Loop through the global employees array and add each employee name to the ListBox
            foreach (var employee in employees)
            {
                ListBoxItem listBoxItem = new ListBoxItem
                {
                    Content = employee.Name
                };
                employee.CalculateEarnings();

                listBoxItem.PreviewMouseLeftButtonUp += (sender, e) => Viewemployee(employee); 

                netOutput.Text = "$"+ employee.NetEarnings.ToString();
                grossOutput.Text = "$" + employee.Gross.ToString();
                taxOutput.Text = "$" + employee.Tax.ToString();

                Console.WriteLine(employee);
                // Add the ListBoxItem to the employeesListBox
                employeesListBox.Items.Add(listBoxItem);
            }
        }

        private void ClearInputBoxes()
        {
            nameInput.Clear();
            hourlyWageInput.Clear();
            hoursWorkedInput.Clear();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoadSampleEmployees();
            PrintEmployeesListBox();
        }

        private void CloseApp(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to close the application?", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Question);

            if (result == MessageBoxResult.OK)
            {
                // Close the application
                Application.Current.Shutdown();
            }
            // If result is Cancel, do nothing
        }

        private void ClearContent(object sender, RoutedEventArgs e)
        {
            employees.Clear();
            PrintEmployeesListBox();
        }

        private void Viewemployee(Employee employee)
        {
            if(employee.Type == EmployeeType.Weekly)
            {
                weekklyRadioButton.IsChecked = true;
            }
            else if(employee.Type == EmployeeType.Commission)
            {
                commissionRadioButton.IsChecked = true;
            }
            else
            {
                hourlyRadioButton.IsChecked = true;
            }

            nameInput.Text = employee.Name;
            employee.CalculateEarnings();

            netOutput.Text = "$" + employee.NetEarnings.ToString();
            grossOutput.Text = "$" + employee.Gross.ToString();
            taxOutput.Text = "$" + employee.Tax.ToString();
        }
    }
}

// Project by Eugene Sang
// github: https://github.com/eugenesang
// linkedin: https://www.linkedin.com/in/eugenesang
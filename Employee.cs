using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation.Peers;
using static MTGarimaBhatia.MainWindow;

namespace MTGarimaBhatia
{
    public abstract class Employee
    {
        // Properties common to all employees
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public EmployeeType Type { get; set; }
        public double Gross { get; set; }
        public double Tax { get; set; }
        public double NetEarnings { get; set; }

        // Abstract method for calculating earnings
        public abstract double CalculateEarnings() ;
        // Abstract method for getting employee type
        public abstract string GetEmployeeType();
    }

    public class HourlyEmployee : Employee
    {
        public double HoursWorked { get; set; }
        public double HourlyRate { get; set; }

        public override double CalculateEarnings()
        {
            
            double v = HoursWorked * HourlyRate;
            if(HoursWorked > 40)
            {
                v += (HoursWorked - 40) * HourlyRate * 0.5;
            }

            this.Gross = v;
            this.Tax = v * 0.2;
            this.NetEarnings = this.Gross - this.Tax;
           
            return this.NetEarnings;
        }

        public override string GetEmployeeType()
        {
            return "Hourly Employee";
        }
    }

    public class CommissionEmployee : Employee
    {
        public double GrossSales { get; set; }
        public double CommissionRate { get; set; }
        public override double CalculateEarnings()
        {
            double v = GrossSales * (CommissionRate / 100.0);
            this.Gross = v;
            this.Tax = v * 0.2;
            this.NetEarnings = this.Gross - this.Tax;


            return this.NetEarnings;
        }

        public override string GetEmployeeType()
        {
            return "Commission Employee";
        }
    }

    public class WeeklyEmployee : Employee
    {
        public double WeeklySalary { get; set; }
        public override double CalculateEarnings()
        {
            double v= WeeklySalary;
            this.Gross = v;
            this.Tax = v * 0.2;
            this.NetEarnings = this.Gross - this.Tax;


            return this.NetEarnings;
        }

        public override string GetEmployeeType()
        {
            return "Weekly Employee";
        }
    }

}

using System.Diagnostics;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using System.Threading.Channels;
using System.Xml.Linq;

namespace Wages_App
{
    public class Program
    {
        // Global variables
        public const decimal PAYRATE = 22.0m;
        public static readonly IReadOnlyCollection<decimal> TAXRATES = new List<decimal> { 0.075m, 0.08m }.AsReadOnly();
        public static readonly int BONUS = 5, BONUSCONDITION = 30;
        public static string payslips = "";
        static readonly int[] IDMINMAX = { 0, 5000 };
        static readonly int[] HOURSMINMAX = { 0, 24 };


        //global constants
        public static string topEmployee = "";
        public static int employeeCounter = 0, topHoursWorked = -1;
        public static decimal totalWages = 0.0m;   

        static void Main(string[] args)
        {
            // Display App Title Screen
            Console.WriteLine(" /$$      /$$                                                /$$$$$$                     \r\n| $$  /$ | $$                                               /$$__  $$                    \r\n| $$ /$$$| $$  /$$$$$$   /$$$$$$   /$$$$$$   /$$$$$$$      | $$  \\ $$  /$$$$$$   /$$$$$$ \r\n| $$/$$ $$ $$ |____  $$ /$$__  $$ /$$__  $$ /$$_____/      | $$$$$$$$ /$$__  $$ /$$__  $$\r\n| $$$$_  $$$$  /$$$$$$$| $$  \\ $$| $$$$$$$$|  $$$$$$       | $$__  $$| $$  \\ $$| $$  \\ $$\r\n| $$$/ \\  $$$ /$$__  $$| $$  | $$| $$_____/ \\____  $$      | $$  | $$| $$  | $$| $$  | $$\r\n| $$/   \\  $$|  $$$$$$$|  $$$$$$$|  $$$$$$$ /$$$$$$$/      | $$  | $$| $$$$$$$/| $$$$$$$/\r\n|__/     \\__/ \\_______/ \\____  $$ \\_______/|_______/       |__/  |__/| $$____/ | $$____/ \r\n                        /$$  \\ $$                                    | $$      | $$      \r\n                       |  $$$$$$/                                    | $$      | $$      \r\n                        \\______/                                     |__/      |__/      ");

            Console.WriteLine("\n\nThis program helps the landscaping team manager quickly and accurately calculate each part‑time worker’s weekly pay. \nThe app takes the worker’s name and hours worked, applies the company’s pay rules, and automatically works out \nbonus hours, gross pay, tax, and final net pay. A clear payslip is then displayed, with an optional summary of\nall workers processed.");

            Console.WriteLine("\n\nPress enter to Continue....");
            Console.ReadLine();

            Console.Clear(); 

            // Repeat OneEmployee() until all employee pay slips have been generated
            char continueInput = 'y';
            while (continueInput == 'y' || continueInput.Equals('y'))
            {

                Console.WriteLine("\n\nPress enter to Continue....");
                Console.ReadLine();

                Console.Clear();

                Console.WriteLine(OneEmployee());

                Console.WriteLine("\n\nDo you want to process another employee? (y/n)");
                continueInput = Console.ReadLine()[0];


                Console.Clear();

            }

            Console.WriteLine(payslips);

            Console.WriteLine("\n\n----------Payroll Summary----------");
            Console.WriteLine($"Total Employee Processed: {employeeCounter}");
            Console.WriteLine($"Total Wages Paid: {totalWages:C}");
            Console.WriteLine($"Top Employee: {topEmployee} with {topHoursWorked} Hours Worked");
            Console.WriteLine("--------------------");

        }

        // Collect employee data and generate payslip
        public static string OneEmployee()
        {
            //Declare local variables
            int id;
            string name, lastName;
            List<int> hoursWorked = new List<int>();
            List<string> DAYSOFWEEK = new List<string> { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            DAYSOFWEEK.AsReadOnly();
            List<string> QUESTIONS = new List<string> { "Enter Employee ID:", "Enter the employee's firstname:", "Enter the employee's last name:" };


            Console.WriteLine("$$$$$$$$ Add Employee $$$$$$$$");

            //Capture employee data
            //Recieve employee id
            Console.WriteLine();
            id = checkInt(QUESTIONS[0], IDMINMAX[0], IDMINMAX[1]);

            //Recieve employee name
            name = checkName(QUESTIONS[1]);


            //Recieve employee last name
            lastName = checkName(QUESTIONS[2]);

            //Recieve hours worked (for each day of the week)
            foreach (string day in DAYSOFWEEK)
            {
                hoursWorked.Add(checkInt($"Enter hours worked on {day}:", HOURSMINMAX[0], HOURSMINMAX[1]));
            }

            //generate payslip and add to global variabes
            payslips += GeneratePayslip(id, name, lastName, hoursWorked);

            //increase employee counter -> employeeCounter = employeeCOunter + 1
            employeeCounter++;

            //add gross wages earned to total wages
            totalWages += CalculateWeeklyPay(hoursWorked) + CalculateBonus(hoursWorked);

            //check if hoursWorked is greater then current top hours worked -> if so update top employee and top hors worked
            if(hoursWorked.Sum() > topHoursWorked)
            {
                topEmployee = $"{name} {lastName}";
                topHoursWorked = hoursWorked.Sum();
            }

            return "Employee proccessed";
        }

        //Calculate weekly pay
        static decimal CalculateWeeklyPay(List<int> hoursWorked)
        {
            decimal weeklyPay = 0.0m;

            // Calculate total hours worked
            int totalHours = hoursWorked.Sum();

            weeklyPay = totalHours * PAYRATE;

            return weeklyPay;
        }

        //Calculate bonus
        static decimal CalculateBonus(List<int> hoursWorked)
        {
            if (hoursWorked.Sum() >= BONUSCONDITION)
            {
                return BONUS * PAYRATE;
            }

            return 0;
        }
        //Calculate tax
        static decimal CalculateTax(List<int> hoursWorked)
        {
            decimal tax = 0;

            //calculate total wages (including bonus)
            if (CalculateWeeklyPay(hoursWorked) + CalculateBonus(hoursWorked) < 450)
            {
                tax = (CalculateWeeklyPay(hoursWorked) + CalculateBonus(hoursWorked)) * TAXRATES.ElementAt(0);
            }
            else
            {
                tax = (CalculateWeeklyPay(hoursWorked) + CalculateBonus(hoursWorked)) * TAXRATES.ElementAt(1);
            }

            return tax;
        }

        //Generate payslip
        private static string GeneratePayslip(int id, string name, string surname, List<int> hoursWorked)
        {
            string payslip = "";

            payslip += "----------Payslip----------\n";

            payslip += $"Employee ID: {id}\n";
            payslip += $"Employee Name: {name} {surname}\n";
            payslip += $"Hours Worked: {hoursWorked.Sum()}\n";
            payslip += $"Gross Income: {(CalculateWeeklyPay(hoursWorked) + CalculateBonus(hoursWorked)):C}\n";
            payslip += $"Tax: {CalculateTax(hoursWorked):C}\n";
            payslip += $"Net Income: {(CalculateWeeklyPay(hoursWorked) + CalculateBonus(hoursWorked) - CalculateTax(hoursWorked)):C}\n";

            payslip += "---------------------------\n\n";

            return payslip;
        }

        //check if a name is lowercase and convert to tital case
        static string checkName(string question)
        {
            while (true)
            {
                //ask for name imput
                Console.WriteLine(question);

                string nameImput = Console.ReadLine();

                //check if name imput is alpahbetical characters and '-' only
                if (Regex.IsMatch(nameImput, @"^[A-Za-z-]+$"))
                {
                    nameImput = nameImput[0].ToString().ToUpper() + nameImput.Substring(1);

                    return nameImput;
                }
                else
                {
                    Console.WriteLine($"Error:names can only contain alphabetic characters and '-'");
                }
               

            }

        }

        //check if user imput is a number between a min and max value
        static int checkInt(string question, int min, int max)
        {
            while (true)
            {

                try
                {
                    Console.WriteLine(question);
                    int userImput = Convert.ToInt32(Console.ReadLine());

                    //check if user imput between min na dmax value
                    if (userImput >= min && userImput <= max)
                    {
                        return userImput;
                    }
                    else
                    {
                        Console.WriteLine($"Please enter a number between {min} and {max}");
                    }

                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Error: you must enter an number between {min} and {max}");
                }

            }
        }

        static char CheckContinueImput(string question)
        {
            while(true)
            {
                string userImput;
                Console.WriteLine(question);
                userImput = Console.ReadLine();

                //if user imput is not emptyan is either 'y' or 'n'
                if (string.IsNullOrEmpty(userImput) && Regex.IsMatch(userImput, "^[yYnN]$"))
                {
                    return userImput.ToLower()[0];
                }
                else
                {
                    Console.WriteLine("error: Only 'y' or 'n' is accepted");
                }
            }
        }

    }


}
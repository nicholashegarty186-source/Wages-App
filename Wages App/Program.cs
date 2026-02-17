using System.Diagnostics;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Wages_App
{
    public class Program
    {
        //global variables

        //constant
        public const decimal PAYRATE = 22.0m;
        public static readonly IReadOnlyCollection<decimal> TAXRATES = new List<decimal> { 0.075m, 0.09m }.AsReadOnly();
        public static readonly int BONUS = 5, BONUSCONDITION = 30;


        static void Main(string[] args)
        {
            //display app tital sreen
            Console.WriteLine("  /$$$$$$                                                    /$$$$$$$            /$$      \r\n /$$__  $$                                                  | $$__  $$          | $$      \r\n| $$  \\__/  /$$$$$$   /$$$$$$  /$$$$$$$   /$$$$$$   /$$$$$$ | $$  \\ $$  /$$$$$$ | $$$$$$$ \r\n|  $$$$$$  /$$__  $$ /$$__  $$| $$__  $$ /$$__  $$ /$$__  $$| $$$$$$$  /$$__  $$| $$__  $$\r\n \\____  $$| $$  \\ $$| $$  \\ $$| $$  \\ $$| $$  \\ $$| $$$$$$$$| $$__  $$| $$  \\ $$| $$  \\ $$\r\n /$$  \\ $$| $$  | $$| $$  | $$| $$  | $$| $$  | $$| $$_____/| $$  \\ $$| $$  | $$| $$  | $$\r\n|  $$$$$$/| $$$$$$$/|  $$$$$$/| $$  | $$|  $$$$$$$|  $$$$$$$| $$$$$$$/|  $$$$$$/| $$$$$$$/\r\n \\______/ | $$____/  \\______/ |__/  |__/ \\____  $$ \\_______/|_______/  \\______/ |_______/ \r\n          | $$                           /$$  \\ $$                                        \r\n          | $$                          |  $$$$$$/                                        \r\n          |__/                           \\______/                                         ");
            Console.WriteLine("The app takes a worker’s name and weekly hours, applies company pay and tax rules, calculates their bonus, gross pay, tax, and net pay, and then outputs a clear payslip for each worker. Optionally, it can also generate a summary of all workers processed.");

            //repeat (oneEmployee) method till all employee payslips generated


        }


    }


}

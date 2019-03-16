using System;
using System.Windows.Forms;

namespace CSVParser
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            string csvPath = "";
            Console.WriteLine
                ("Welcome to Account Analyzer\n" +
                "Please Select a CSV file");
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.ShowDialog();
                csvPath = ofd.FileName;
            }
            CSVparser AccountRecords = new CSVparser(csvPath);
            AccountRecords.getTransactionCount(500);
            AccountRecords.getUniqueBuySell();
            AccountRecords.getAvgTransByPlace(3);
            string account = Console.ReadLine();
            AccountRecords.getAccountTransaction(account);
            Console.Read();
        }
    }
}

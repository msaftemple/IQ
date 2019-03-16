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
            CSVparser obj = new CSVparser();
            obj.getTransactionCount(csvPath, 500);
            obj.getUniqueBuySell(csvPath);
            Console.Read();
        }
    }
}

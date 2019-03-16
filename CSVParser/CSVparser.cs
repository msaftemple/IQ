using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using CsvHelper;

namespace CSVParser
{
    class CSVparser
    {
        private Record[] records;
        private Dictionary<string, Account> accountInfo = new Dictionary<string, Account>();
        // Taking transaction information from CSV into an array 
        // and sorting Accounts into dictionary w/ KeyValue 'Account Name : Account info object'
        public CSVparser(string path)
        {
            using (StreamReader reader = new StreamReader(path))
            using (CsvReader csv = new CsvReader(reader))
            {
                records = csv.GetRecords<Record>().ToArray();
                for (int i = 0; i < records.Length; i++)
                {
                    Account ti = new Account();
                    if (!accountInfo.ContainsKey(records[i].AccountOwner))
                    {
                        ti.TransCount = 1;
                        ti.TransTotal = records[i].DollarValue;
                        ti.TransIDs.Add(i);
                        accountInfo.Add(records[i].AccountOwner, ti);
                    }
                    else
                    {
                        accountInfo[records[i].AccountOwner].TransIDs.Add(i);
                        accountInfo[records[i].AccountOwner].TransCount += 1;
                        accountInfo[records[i].AccountOwner].TransTotal += records[i].DollarValue;
                    }
                }
                foreach (KeyValuePair<string, Account> kvp in accountInfo)
                {
                    kvp.Value.setAverage();
                }
            }
        }
        // Getting all unique buy sell combinations
        public void getUniqueBuySell()
        {
            List<string> BuySell = new List<string>();
            Console.WriteLine("Unique Buy Sell Combinations");
            foreach(Record rc in records)
            {
                string temp = "";
                temp = rc.Assetbeingsold + " " + rc.Assetbeingpurchased;
                if (!BuySell.Contains(temp))
                {
                    BuySell.Add(temp);
                    Console.WriteLine(temp);
                }
            }
        }
        // Getting Accounts with over X amount of transactions
        public void getTransactionCount(int count)
        {
            Console.WriteLine("Accounts with over " + count.ToString() + " orders");
            foreach(KeyValuePair<string,Account> kvp in accountInfo)
            {
                if(kvp.Value.TransCount > count)
                {
                    Console.WriteLine(kvp.Key + " " + kvp.Value.TransCount);
                }
            }
        }
        // Getting the average transaction total by place
        public void getAvgTransByPlace(int place)
        {
            IOrderedEnumerable<KeyValuePair<string, Account>> order = accountInfo.OrderBy(TransactionInfo => TransactionInfo.Value.TransAverage);
            Console.WriteLine("#" + place.ToString() + " Sales Average is " + order.ElementAt(accountInfo.Count - place).Key + " with $"  + order.ElementAt(accountInfo.Count - place).Value.TransAverage);
        }
        // Getting all transactions by specified Account
        public void getAccountTransaction(string account)
        {
            for (int i = 0; i < accountInfo[account].TransIDs.Count; i++)
            {
                int j = accountInfo[account].TransIDs[i];
                Console.WriteLine(records[j].Assetbeingpurchased + " " + records[j].Assetbeingsold);
            }
        }
    }
    // CSV data
    public class Record
    {
        public string Assetbeingsold { get; set; }
        public string Assetbeingpurchased { get; set; }
        public int DollarValue { get; set; }
        public string AccountOwner { get; set; }
    }
    // Basic data for each account
    public class Account
    {
        public List<int> TransIDs = new List<int>();
        public int TransCount { get; set; }
        public int TransTotal { get; set; }
        public int TransAverage;

        public void setAverage()
        {
            TransAverage = TransTotal / TransCount;
        }
    }
}

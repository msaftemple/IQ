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
        

        public IEnumerable<Record> parseRecords(string path)
        {
            IEnumerable<Record> records;
            StreamReader reader = new StreamReader(path);
            CsvReader csv = new CsvReader(reader);
            records = csv.GetRecords<Record>();
            return records;
            
        }
        public void getUniqueBuySell(string path)
        {
            IEnumerable<Record> records = parseRecords(path);
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
        public void getTransactionCount(string path, int count)
        {
            IEnumerable<Record> records = parseRecords(path);
            Dictionary<string, TransactionInfo> AcctInfo = new Dictionary<string, TransactionInfo>();
            foreach (Record rc in records)
            {
                TransactionInfo ti = new TransactionInfo();
                if (!AcctInfo.ContainsKey(rc.AccountOwner))
                {
                    ti.TransCount = 1;
                    ti.TransTotal = rc.DollarValue;
                    AcctInfo.Add(rc.AccountOwner,ti);
                }
                else
                {
                    AcctInfo[rc.AccountOwner].TransCount += 1;
                    AcctInfo[rc.AccountOwner].TransTotal += rc.DollarValue;
                }
                
            }
            Console.WriteLine("Accounts with over " + count.ToString() + " orders");
            foreach(KeyValuePair<string,TransactionInfo> kvp in AcctInfo)
            {
                kvp.Value.setAverage();
                if(kvp.Value.TransCount > count)
                {
                    Console.WriteLine(kvp.Key + " " + kvp.Value.TransCount);
                }
            }

            getAverageTransAmount(AcctInfo);
        }

        public void getAverageTransAmount(Dictionary<string,TransactionInfo> act)
        {
            IOrderedEnumerable<KeyValuePair<string, TransactionInfo>> order = act.OrderBy(TransactionInfo => TransactionInfo.Value.TransAverage);
            Console.WriteLine("Account with the 3rd highest average sales amount");
            Console.WriteLine(order.ElementAt(act.Count-3).Value.TransAverage);
        }
    }

    public class Record
    {
        public string Assetbeingsold { get; set; }
        public string Assetbeingpurchased { get; set; }
        public int DollarValue { get; set; }
        public string AccountOwner { get; set; }
    }

    public class TransactionInfo
    {
        public int TransCount { get; set; }
        public int TransTotal { get; set; }
        public int TransAverage;

        public void setAverage()
        {
            TransAverage = TransTotal / TransCount;
        }
    }
}

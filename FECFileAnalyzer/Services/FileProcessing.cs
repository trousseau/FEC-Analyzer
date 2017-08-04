using FECFileAnalyzer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FECFileAnalyzer.Services
{
    class FileProcessing
    {
        public static void GetDiscrepancyList(List<FECTransaction> fecList, List<FECTransaction> csvList)
        {
            List<FECTransaction> fecDiscrepList = new List<FECTransaction>();
            List<FECTransaction> csvDiscrepList = new List<FECTransaction>();

            bool isInFecList = false;
            bool isInCsvList = false;

            foreach (var fecTrans in fecList)
            {
                isInFecList = false;

                foreach (var csvTrans in csvList)
                {
                    if (fecTrans.IsEqualTo(csvTrans))
                    {
                        isInFecList = true;
                        break;
                    }
                }

                if (isInFecList == false)
                {
                    fecDiscrepList.Add(fecTrans);
                }
            }

            foreach (var csvTrans in csvList)
            {
                isInCsvList = false;

                foreach (var fecTrans in fecList)
                {
                    if (csvTrans.IsEqualTo(fecTrans))
                    {
                        isInCsvList = true;
                        break;
                    }
                }
                if (isInCsvList == false)
                {
                    csvDiscrepList.Add(csvTrans);
                }

            }
            PrintDiscrepList(fecDiscrepList, csvDiscrepList);
        }

        public static void PrintDiscrepList(List<FECTransaction> fecDiscrepList, List<FECTransaction> csvDiscrepList)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("----Receipts----");
            Console.ResetColor();
            Console.WriteLine("From FEC File:");
            foreach (var trans in fecDiscrepList)
            {
                if (trans.TransType == "receipt")
                {
                    Console.WriteLine(trans.ToString());
                }
            }

            Console.WriteLine("From CSV File:");
            foreach (var trans in csvDiscrepList)
            {
                if (trans.TransType == "receipt")
                {
                    Console.WriteLine(trans.ToString());
                }
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("----Disbursements----");
            Console.ResetColor();
            Console.WriteLine("From FEC File:");
            foreach (var trans in fecDiscrepList)
            {
                if (trans.TransType == "disbursement")
                {
                    Console.WriteLine(trans.ToString());
                }
            }

            Console.WriteLine("From CSV File:");
            foreach (var trans in csvDiscrepList)
            {
                if (trans.TransType == "disbursement")
                {
                    Console.WriteLine(trans.ToString());
                }
            }
        }

        public static void FindDuplicates(List<FECTransaction> fecList, List<FECTransaction> csvList)
        {
            int dups = 0;
            List<FECTransaction> csvDupList = new List<FECTransaction>();
            List<FECTransaction> fecDupList = new List<FECTransaction>();

            foreach (var trans in csvList)
            {
                dups = 0;
                foreach (var trans2 in csvList)
                {
                    if (trans == trans2)
                    {
                        dups++;
                    }
                }
                if (dups > 1)
                {
                    csvDupList.Add(trans);
                }
            }

            foreach (var trans in fecList)
            {
                dups = 0;
                foreach (var trans2 in fecList)
                {
                    if (trans == trans2)
                    {
                        dups++;
                    }
                }
                if (dups > 1)
                {
                    fecDupList.Add(trans);
                }
            }

            PrintDupList(fecDupList, csvDupList);
        }

        public static void PrintDupList(List<FECTransaction> fecDupList,List<FECTransaction> csvDupList)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Duplicates:");
            Console.ResetColor();

            Console.WriteLine("CSV Dups:");
            foreach (var trans in csvDupList)
            {
                Console.WriteLine(trans);
            }

            Console.WriteLine("FEC Dups:");
            foreach (var trans in fecDupList)
            {
                Console.WriteLine(trans);
            }

        }

    }
}

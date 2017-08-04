using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FECFileAnalyzer.Models;
using System.Globalization;
using Microsoft.VisualBasic.FileIO;
using System.IO;
using System.Windows.Forms;

namespace FECFileAnalyzer.Services
{
    class CSVFileService
    {
        public static string OpenFilePath(string[] args, ref string path)
        {
            string tempath = null;
            byte exitMethod = 1;
            OpenFileDialog openDialog1 = new OpenFileDialog();
            openDialog1.Filter = "csv files (*.csv)|*.csv";

            if (args.Length == 0)
            {
                do
                {
                    openDialog1.FileName = String.Empty;
                    if (openDialog1.ShowDialog() == DialogResult.Cancel)
                    {
                        Environment.Exit(0);
                        exitMethod = 0;
                    }
                    tempath = openDialog1.FileName.ToLower();
                    if (Path.GetExtension(tempath) == ".csv")
                    {
                        path = openDialog1.FileName;
                        exitMethod = 0;
                    }
                    else
                    {
                        MessageBox.Show("Invalid File Extension");
                    }
                }
                while (exitMethod != 0);
            }
            else
            {

                tempath = args[0];

                if (Path.GetExtension(tempath) != ".csv")
                {
                    MessageBox.Show("Invalid File Extension");

                    while (Path.GetExtension(path) != ".csv")
                    {
                        if (openDialog1.ShowDialog() == DialogResult.Cancel)
                        {
                            Environment.Exit(0);
                        }
                        tempath = openDialog1.FileName;
                        if (Path.GetExtension(tempath) == ".csv")
                        {
                            path = openDialog1.FileName;
                            exitMethod = 0;
                        }
                        else
                        {
                            MessageBox.Show("Invalid File Extension");
                        }
                    }
                }
                else
                {
                    path = args[0];
                }
            }
            return tempath;
        }
        public static List<FECTransaction> ParseCSVFileToList(string path)
        {
            List<FECTransaction> transObjList = new List<FECTransaction>();
            string extension = Path.GetExtension(path).ToLower();
            FECTransaction newTrans = new FECTransaction();

            using (TextFieldParser parser1 = new TextFieldParser(path))
            {
                try
                {
                    parser1.SetDelimiters(new string[] { "," });

                    if (extension != ".csv" && extension != ".fec")
                    {
                        throw new ArgumentException("File must be .fec or .csv");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                transObjList = PopulateTransList(parser1);
            }
            return transObjList;
        }

        public static FECTransaction CSVFieldsToTransObj(string[] fields, CSVHeaderIndexes headerIndexes)
        {
            decimal amount = 0;
            string orgName = string.Empty;
            string firstName = string.Empty;
            string lastName = string.Empty;
            string committeeName = string.Empty;
            string legFirst = string.Empty;
            string legLast = string.Empty;
            string recordType = string.Empty;
            string city = string.Empty;
            string state = string.Empty;
            string electionPeriod = string.Empty;
            string schedString = string.Empty;
            string transType = string.Empty;
            string tempDateString = string.Empty;
            DateTime transDate = new DateTime();

            if (headerIndexes.FirstName != -1)
            {
                firstName = fields[headerIndexes.FirstName];
                if (!string.IsNullOrWhiteSpace(firstName))
                {
                    recordType = "IND";
                }
            }

            if (headerIndexes.LastName != -1)
            {
                lastName = fields[headerIndexes.LastName];
            }

            if (headerIndexes.OrgName != -1)
            {
                orgName = fields[headerIndexes.OrgName];
                if (!string.IsNullOrWhiteSpace(orgName))
                {
                    recordType = "ORG";
                }
            }

            if (headerIndexes.CommitteeName != -1)
            {
                committeeName = fields[headerIndexes.CommitteeName];
                if (!string.IsNullOrEmpty(committeeName))
                {
                    recordType = "COM";
                }
            }

            if (headerIndexes.LegislatorFirstName != -1)
            {
                legFirst = fields[headerIndexes.LegislatorFirstName];
                if (!string.IsNullOrEmpty(legFirst))
                {
                    recordType = "LEG";
                }
            }

            if (headerIndexes.LegislatorLastName != -1)
            {
                legLast = fields[headerIndexes.LegislatorLastName];
                if (!string.IsNullOrEmpty(legLast))
                {
                    recordType = "LEG";
                }
            }

            if (headerIndexes.City != -1)
            {
                city = fields[headerIndexes.City];
            }

            if (headerIndexes.State != -1)
            {
                state = fields[headerIndexes.State];
            }

            if (headerIndexes.Date != -1)
            {
                tempDateString = fields[headerIndexes.Date];
                 DateTime.TryParse(tempDateString,out transDate);
            }

            if (headerIndexes.ElectionPeriod != -1)
            {
                electionPeriod = fields[headerIndexes.ElectionPeriod];
            }

            if (headerIndexes.ReceiptAmount != -1)
            {
                decimal.TryParse(fields[headerIndexes.ReceiptAmount], out amount);
                if (!string.IsNullOrWhiteSpace(fields[headerIndexes.ReceiptAmount]))
                {
                    transType = "receipt";
                }
            }
            else if (headerIndexes.DisbursementAmount != -1)
            {
                decimal.TryParse(fields[headerIndexes.ReceiptAmount], out amount);
                if (!string.IsNullOrWhiteSpace(fields[headerIndexes.DisbursementAmount]))
                {
                    transType = "disbursement";
                }
            }

            FECTransaction newTrans = new FECTransaction(orgName, firstName, lastName, recordType, city, state, electionPeriod, transDate, amount, transType);
            return newTrans;
        }

        public static List<FECTransaction> PopulateTransList(TextFieldParser parser1)
        {
            FECTransaction newTrans = new FECTransaction();
            List<FECTransaction> transObjList = new List<FECTransaction>();
            CSVHeaderIndexes headerIndex = new CSVHeaderIndexes();
            int count = 0;

                while (!parser1.EndOfData)
                {
                    string[] fields = null;

                    try
                    {
                        fields = parser1.ReadFields();
                    }
                    catch (MalformedLineException)
                    {
                        break;
                    }

                    if (count == 0)
                    {
                        headerIndex = ReadCSVHeader(fields);
                    }

                if (count > 0)
                {
                    newTrans = CSVFieldsToTransObj(fields, headerIndex);
                    transObjList.Add(newTrans);
                }
                    count++;
                }
                return transObjList;
        }

        public static CSVHeaderIndexes ReadCSVHeader(string[] fields)
        {
            int fieldsLength = fields.Length;
            string lowerFields = string.Empty;
            CSVHeaderIndexes headerIndexes = new CSVHeaderIndexes();

            for (int i = 0; i < fieldsLength; i++)
            {
                lowerFields = fields[i].ToLower();

                if ( lowerFields == "first name" || lowerFields ==  "first")
                {
                    headerIndexes.FirstName = i;
                }
                else if (lowerFields == "last name" || lowerFields == "last")
                {
                    headerIndexes.LastName = i;
                }
                else if (lowerFields.Contains("legislator") && lowerFields.Contains("first"))
                {
                    headerIndexes.LegislatorFirstName = i;
                }
                else if (lowerFields.Contains("legislator") && lowerFields.Contains("last"))
                {
                    headerIndexes.LegislatorLastName = i;
                }
                else if (lowerFields == "organization name")
                {
                    headerIndexes.OrgName = i;
                }
                else if (lowerFields == "committee name")
                {
                    headerIndexes.CommitteeName = i;
                }
                else if (lowerFields.Contains("receipt amount"))
                {
                    headerIndexes.ReceiptAmount = i;
                }
                else if (lowerFields.Contains("disbursement amount"))
                {
                    headerIndexes.DisbursementAmount = i;
                }
                else if (lowerFields.Contains("receipt date") || lowerFields.Contains("disbursement date"))
                {
                    headerIndexes.Date = i;
                }
                else if (lowerFields.Contains("state"))
                {
                    headerIndexes.State = i;
                }
                else if (lowerFields.Contains("city"))
                {
                    headerIndexes.City = i;
                }
                else if (lowerFields.Contains("period text"))
                {
                    headerIndexes.ElectionPeriod = i;
                }
            }

            return headerIndexes;
        }
    }
}

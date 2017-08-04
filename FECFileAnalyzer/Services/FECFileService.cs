using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FECFileAnalyzer.Models;
using System.IO;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;
using System.Globalization;

namespace FECFileAnalyzer.Services
{
    public class FECFileService
    {
        public static List<FECTransaction> GetListFecTransactions(List<FECTransaction> fecTransactionList)
        {
            //TODO: parse list to fectransaction objects
            return fecTransactionList;
        }

        public static string OpenFilePath(string[] args, ref string path)
        {
            string tempath = null;
            byte exitMethod = 1;
            OpenFileDialog openDialog1 = new OpenFileDialog();
            openDialog1.Filter = "fec files (*.fec)|*.fec";

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
                    tempath = openDialog1.FileName;
                    if (Path.GetExtension(tempath) == ".fec")
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

                if (Path.GetExtension(tempath) != ".fec")
                {
                    MessageBox.Show("Invalid File Extension");

                    while (Path.GetExtension(path) != ".fec")
                    {
                        if (openDialog1.ShowDialog() == DialogResult.Cancel)
                        {
                            Environment.Exit(0);
                        }
                        tempath = openDialog1.FileName;
                        if (Path.GetExtension(tempath) == ".fec")
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

        public static List<FECTransaction> ParseFECFileToList(string path)
        {
            List<FECTransaction> transObjList = new List<FECTransaction>();
            string extension = Path.GetExtension(path);
            string reportType = string.Empty;
            FECTransaction newTrans = new FECTransaction();

            using (TextFieldParser parser1 = new TextFieldParser(path))
            {
                try
                {
                    if (extension == ".fec")
                    {
                        //parser1.HasFieldsEnclosedInQuotes = true;
                        parser1.SetDelimiters(new string[] { "\x1c" });
                    }
                    else if (extension == ".csv")
                    {
                        parser1.SetDelimiters(new string[] { "," });
                    }
                    else
                    {
                        throw new ArgumentException("File must be .fec or .csv");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                reportType = GetReportType(parser1);
                transObjList = PopulateTransList(parser1, reportType);
            }
            return transObjList;
        }

        public static FECTransaction Form3FieldsToTransObj(string[] fields)
        {
            decimal amount = 0;
            string orgName = string.Empty;
            string committeeName = string.Empty;
            string firstName = string.Empty;
            string lastName = string.Empty;
            string legFirst = string.Empty;
            string legLast = string.Empty;
            string recordType = string.Empty;
            string city = string.Empty;
            string state = string.Empty;
            string electionPeriod = string.Empty;
            string schedString = string.Empty;
            string transType = string.Empty;
            Schedule sched = new Schedule();
            DateTime transDate = new DateTime();
            Enum.TryParse<Schedule>(fields[0], out sched);

            schedString = fields[0];

            //sched A

            if (schedString[1] == 'A')
            {
                if (schedString.Substring(0,4) == "SA11")
                {
                    recordType = fields[5];
                    if (schedString[4] == 'A')
                    {
                        firstName = fields[8];
                        lastName = fields[7];
                        orgName = fields[6];
                    }
                    else if (schedString[4] == 'B' || schedString[4] == 'C')
                    {
                        committeeName = fields[6];
                    }

                    if (recordType.ToLower() == "leg")
                    {
                        legFirst = fields[8];
                        legLast = fields[7];
                    }
                }
                else
                {
                    firstName = fields[8];
                    lastName = fields[7];
                    orgName = fields[6];
                }

                city = fields[14];
                state = fields[15];
                electionPeriod = fields[18];
                transDate = DateTime.ParseExact(fields[19].ToString(), "yyyyMMdd", CultureInfo.InvariantCulture);
                decimal.TryParse(fields[20], out amount);
                transType = "receipt";

                FECTransaction newTrans = new FECTransaction(sched, orgName, firstName, lastName, recordType, city, state, electionPeriod, transDate, amount,transType);
                return newTrans;
            }
            //sched B
            else if (schedString[1] == 'B')
            {
                recordType = fields[5];
                orgName = fields[6];
                firstName = fields[8];
                lastName = fields[7];
                city = fields[14];
                state = fields[15];
                electionPeriod = fields[18];
                transDate = DateTime.ParseExact(fields[19], "yyyyMMdd", CultureInfo.InvariantCulture);
                decimal.TryParse(fields[20], out amount);
                transType = "disbursement";

                FECTransaction newTrans = new FECTransaction(sched, orgName, firstName, lastName, recordType, city, state, electionPeriod, transDate, amount, transType);
                return newTrans;
            }
            //sched C
            else if (schedString[1] == 'C')
            {
                recordType = fields[5];
                orgName = fields[6];
                firstName = fields[8];
                lastName = fields[7];
                city = fields[14];
                state = fields[15];
                electionPeriod = fields[18];
                transDate = DateTime.ParseExact(fields[19], "yyyymmdd", CultureInfo.InvariantCulture);
                decimal.TryParse(fields[20], out amount);
                transType = "loan";

                FECTransaction newTrans = new FECTransaction(sched, orgName, firstName, lastName, recordType, city, state, electionPeriod, transDate, amount,transType);
                return newTrans;
            }
            //sched D
            else if (schedString[1] == 'D')
            {
                recordType = fields[3];
                orgName = fields[4];
                firstName = fields[6];
                lastName = fields[5];
                decimal.TryParse(fields[16], out amount);
                transType = "debt";

                FECTransaction newTrans = new FECTransaction(sched, orgName, firstName, lastName, recordType, amount, transType);
                return newTrans;
            }
            else
            {
                FECTransaction newTrans = new FECTransaction();
                return newTrans;
            }
        }

        public static List<FECTransaction> PopulateTransList(TextFieldParser parser1,string reportType)
        {
            FECTransaction newTrans = new FECTransaction();
            List<FECTransaction> transObjList = new List<FECTransaction>();

            if (reportType == "F3N" || reportType == "F3A")
            {
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

                    if (Schedule.IsDefined(typeof(Schedule), fields[0]))
                    {
                        newTrans = Form3FieldsToTransObj(fields);
                        transObjList.Add(newTrans);
                    }
                    else
                    {
                        continue;
                    }
                }
                return transObjList;
            }
            else if (reportType == "F3XN" || reportType == "F3XA")
            {

            }

            //if (fields != null)
            //{
            //    var newFields = fields
            //    //.Select(f => f.Contains(",") ? string.Format("\"{0}\"", f) : f); //only quotes fields with comma
            //    .Select(f => f.Replace("\"", "")).Select(f => string.Format("\"{0}\"", f));
            //    //.Select(f => string.Format("\"{0}\"", f)); //quotes all fields
            //    //lines.Add(string.Join(",", newFields));
            //}
            return transObjList;
        }

        public static string GetReportType(TextFieldParser parser1)
        {
            int lineCount = 0;
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

                lineCount += 1;

                if (lineCount == 2)
                {
                    return fields[0];
                }

            }
            return null;
        }
    }
}

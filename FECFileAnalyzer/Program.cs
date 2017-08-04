using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;
using FECFileAnalyzer.Services;
using FECFileAnalyzer.Models;

namespace FECFileAnalyzer
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            List<FECTransaction> transObjListFEC = new List<FECTransaction>();
            List<FECTransaction> transObjListCSV = new List<FECTransaction>();
            string path = string.Empty;
            string tempathFEC = string.Empty;
            string tempathCSV = string.Empty;

            SaveFileDialog saveDialog1 = new SaveFileDialog();
            saveDialog1.Filter = "comma separated values (*.csv)|*.csv";

            Console.WriteLine("Select .fec file...");
            FECFileService.OpenFilePath(args, ref tempathFEC);
            transObjListFEC = FECFileService.ParseFECFileToList(tempathFEC);

            Console.WriteLine("Select .csv file to compare to...");
            CSVFileService.OpenFilePath(args, ref tempathCSV);
            transObjListCSV = CSVFileService.ParseCSVFileToList(tempathCSV);

            Console.WriteLine();

            Console.WriteLine("Discrepancies: ");

            FileProcessing.GetDiscrepancyList(transObjListFEC, transObjListCSV);
            Console.WriteLine();

            FileProcessing.FindDuplicates(transObjListFEC, transObjListCSV);

            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }
    }
}
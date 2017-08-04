using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FECFileAnalyzer.Models
{
    public class CSVHeaderIndexes
    {
        public int FirstName { get; set; } = -1;
        public int LastName { get; set; } = -1;
        public int LegislatorFirstName { get; set; } = -1;
        public int LegislatorLastName { get; set; } = -1;
        public int OrgName { get; set; } = -1;
        public int CommitteeName { get; set; } = -1;
        public int Date { get; set; } = -1;
        public int ReceiptAmount { get; set; } = -1;
        public int DisbursementAmount { get; set; } = -1;
        public int City { get; set; } = -1;
        public int State { get; set; } = -1;
        public int ElectionPeriod { get; set; } = -1;

        public CSVHeaderIndexes()
        {

        }

    }
}

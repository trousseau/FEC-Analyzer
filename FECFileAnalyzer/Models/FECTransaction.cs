using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FECFileAnalyzer.Models
{
    //possibly implement different enums for each schedule/transtype
    public enum Schedule
    {
        None,
        SA11AI,
        SA11AII,
        SA11AIII,
        SA11B,
        SA11C,
        SA11D,
        SA12,
        SA13,
        SA13A,
        SA13B,
        SA13C,
        SA14,
        SA15,
        SB17,
        SA17A,
        SA18,
        SA19,
        SA19A,
        SA19B,
        SA19C,
        SA20A,
        SA20B,
        SA20C,
        SA21,
        SB21,
        SB23,
        SB28A,
        SB28C,
        SD9,
        SD10,
        SD12
    }
    public class FECTransaction
    {
        //add committee + legislator
        public Schedule Schedule { get; set; }
        public string OrgName { get; set; }
        public string CommitteeName { get; set; }
        public string LegislatorFirstName { get; set; }
        public string LegislatorLastName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RecordType { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ElectionPeriod { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string TransType { get; set; }

        public FECTransaction()
        {

        }

        //schedule a,b,c constructor
        public FECTransaction(Schedule schedule, string orgName, string firstName, string lastName, string recordType, string city, string state, string electionPeriod, DateTime date, decimal amount, string transType)
        {
            Schedule = schedule;
            OrgName = orgName;
            FirstName = firstName;
            LastName = lastName;
            RecordType = recordType;
            City = city;
            State = state;
            ElectionPeriod = electionPeriod;
            Date = date;
            Amount = amount;
            TransType = transType;
        }

        public FECTransaction(Schedule schedule, string orgName, string firstName, string lastName, string recordType, string city, string state, string electionPeriod, DateTime date, decimal amount)
        {
            Schedule = schedule;
            OrgName = orgName;
            FirstName = firstName;
            LastName = lastName;
            RecordType = recordType;
            City = city;
            State = state;
            ElectionPeriod = electionPeriod;
            Date = date;
            Amount = amount;
            TransType = string.Empty;
        }

        public FECTransaction(string orgName, string firstName, string lastName, string recordType, string city, string state, string electionPeriod, DateTime date, decimal amount, string transType)
        {
            OrgName = orgName;
            FirstName = firstName;
            LastName = lastName;
            RecordType = recordType;
            City = city;
            State = state;
            ElectionPeriod = electionPeriod;
            Date = date;
            Amount = amount;
            TransType = transType;
        }

        public FECTransaction(Schedule schedule, string orgName, string recordType, string city, string state, string electionPeriod, DateTime date, decimal amount)
        {
            Schedule = schedule;
            OrgName = orgName;
            FirstName = string.Empty;
            LastName = string.Empty;
            RecordType = recordType;
            City = city;
            State = state;
            ElectionPeriod = electionPeriod;
            Date = date;
            Amount = amount;
            TransType = string.Empty;
        }

        public FECTransaction(Schedule schedule, string firstName, string lastName, string recordType, string city, string state, string electionPeriod, DateTime date, decimal amount)
        {
            Schedule = schedule;
            OrgName = string.Empty;
            FirstName = firstName;
            LastName = lastName;
            RecordType = recordType;
            City = city;
            State = state;
            ElectionPeriod = electionPeriod;
            Date = date;
            Amount = amount;
        }

        public FECTransaction(Schedule schedule, string firstName, string lastName, string recordType, string city, string state, decimal amount)
        {
            Schedule = schedule;
            OrgName = string.Empty;
            FirstName = firstName;
            LastName = lastName;
            RecordType = recordType;
            City = city;
            State = state;
            ElectionPeriod = string.Empty;
            Date = DateTime.MinValue;
            Amount = amount;
        }

        public FECTransaction(Schedule schedule, string orgName, string firstName, string lastName, string recordType, decimal amount,string transType)
        {
            Schedule = schedule;
            OrgName = orgName;
            FirstName = firstName;
            LastName = lastName;
            RecordType = recordType;
            City = string.Empty;
            State = string.Empty;
            ElectionPeriod = string.Empty;
            Date = DateTime.MinValue;
            Amount = amount;
            TransType = transType;
        }

        public FECTransaction(Schedule schedule, string orgName, string firstName, string lastName, string recordType, decimal amount)
        {
            Schedule = schedule;
            OrgName = orgName;
            FirstName = firstName;
            LastName = lastName;
            RecordType = recordType;
            City = string.Empty;
            State = string.Empty;
            ElectionPeriod = string.Empty;
            Date = DateTime.MinValue;
            Amount = amount;
        }

        public bool IsEqualTo(FECTransaction trans2)
        {
            if ((OrgName == trans2.OrgName && !string.IsNullOrEmpty(OrgName) || FirstName == trans2.FirstName && LastName == trans2.LastName && !string.IsNullOrEmpty(LastName) || CommitteeName == trans2.CommitteeName && !string.IsNullOrEmpty(CommitteeName) || LegislatorFirstName == trans2.LegislatorFirstName && LegislatorLastName == trans2.LegislatorLastName && !string.IsNullOrEmpty(LegislatorLastName)) && Date == trans2.Date && Amount == trans2.Amount)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override string ToString()
        {
            return string.Format($"{Schedule} {OrgName}{FirstName} {LastName} {Date:MM-dd-yyyy} {Amount:c}");
        }
    }
}

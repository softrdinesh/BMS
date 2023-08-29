using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BMS
{
    class Bank
    {
        string id;
       

        List<AccountMaster> lst = new List<AccountMaster>();
        List<BalanceMaster> BalLst = new List<BalanceMaster>();

        IDGenerator Id = new IDGenerator();
        DOB dob = new DOB();
        

        public bool isFirstEntry = false;

        List<InterestRule> lstRule = new List<InterestRule>();
        public Bank()
        {
            lstRule.Add(new InterestRule()
            {
                FromDate=Convert.ToDateTime("2023-08-01"),
                ToDate= Convert.ToDateTime("2023-08-14"),
                NoOfDays=14,
                EODbalance=25000,
                RuleID= "RULE02",
                Rate= 1.9,
                AnnualInterest= ((25000*1.9)/100)*14


            });
            lstRule.Add(new InterestRule()
            {
                FromDate = Convert.ToDateTime("2023-08-15"),
                ToDate = Convert.ToDateTime("2023-08-25"),
                NoOfDays = 11,
                EODbalance = 25000,
                RuleID = "RULE03",
                Rate = 2.20,
                AnnualInterest = ((25000 * 2.20) / 100) * 11


            });
            lstRule.Add(new InterestRule()
            {
                FromDate = Convert.ToDateTime("2023-08-26"),
                ToDate = Convert.ToDateTime("2023-08-31"),
                NoOfDays = 6,
                EODbalance = 13000,
                RuleID = "RULE03",
                Rate = 2.20,
                AnnualInterest = ((13000 * 2.20) / 100) * 6


            });

        }
       
       


        public void ShowTransactions(string Input)
        {
            string[] sptr = Input.Split("|");
            string Acno = sptr[0].ToUpper();
            int month = Int32.Parse(sptr[1]);
            var recVal = lst.Where(s => s.Acno == Acno && s.Month== month).ToList();
            if(recVal.Count>0 && recVal!=null)
            {
                Console.WriteLine("Account Number:" + Acno);
                Console.WriteLine("Date     |   Txn ID      |   Type   |   Amount  |    Balance");
                var ShowTranVal = lst.Where(s => s.Acno == Acno && s.Month == month).OrderBy(s=>s.Actype);
                var finalBal = BalLst.FirstOrDefault(s => s.Acno == Acno).Balance;
                decimal depo = 0;
                foreach (var k in ShowTranVal)
                {
                   
                    if (k.Actype == "D")
                    {
                        depo = depo + k.Amount;
                        Console.WriteLine(String.Format("{0}|   {1}    |    {2}     |   {3}     |   {4} ", k.Date, k.TranID, k.Actype,k.Amount.ToString("#0.00"),depo.ToString("#0.00")));
                    }
                    else if (k.Actype == "W")
                    {
                        depo = depo - k.Amount;
                        Console.WriteLine(String.Format("{0}|   {1}    |    {2}     |   {3}     |   {4} ", k.Date, k.TranID, k.Actype, k.Amount.ToString("#0.00"), depo.ToString("#0.00")));
                    }
                }
            }
            else
            {
                Console.WriteLine("Transactions not found!");
                Console.ReadKey();
            }
        }

        public void GetRule(string input)
        {
            string[] sptr = input.Split("|");
            int year = Convert.ToInt32(sptr[0].Substring(0, 4));
            int month = Convert.ToInt32(sptr[0].Substring(4, 2));
            int day = Convert.ToInt32(sptr[0].Substring(6, 2));
            string rule = sptr[1].ToUpper() ;
            double rate=Convert.ToDouble(sptr[2]);
            if (rate < 0)
            {
                Console.Write("Rate of interest should be greater than 0 ");
                Console.WriteLine("Enter Rate of Interest:");
                rate = Convert.ToDouble(Console.ReadLine());
            }
            DateTime frmDate = Convert.ToDateTime(year + "-" + month + "-" + day);
            var s = lstRule.Where(x => x.FromDate <= frmDate && x.ToDate >= frmDate);

            if (s != null)
            {
                Console.WriteLine("Interest rules:");
                Console.WriteLine("Date     |   Rule    |   Rate(%)");
                foreach (var rec in s)
                {
                    Console.WriteLine(String.Format("{0}|   {1}    |    {2} ", sptr[0].Substring(0, 8), rec.RuleID, rec.Rate));

                }
            }
                    
                
        }
        public void CreateTransaction(string Input)
        {
            try
            {
                string[] sptr = Input.Split("|");
                int year = Convert.ToInt32(sptr[0].Substring(0, 4));
                int month = Convert.ToInt32(sptr[0].Substring(4, 2));
                int day = Convert.ToInt32(sptr[0].Substring(6, 2));
                string Acno = sptr[1].ToString().ToUpper();
                string type = sptr[2].ToString().ToUpper();
                decimal amount = Convert.ToDecimal(sptr[3]);
                dob.set(day, month, year);
                string Date = "";
                if (dob.dateFormat() == false)
                {
                    Console.Write("Enter Date(YYYYMMDD):");
                    Date = Console.ReadLine();
                }
                else
                {
                    Date = sptr[0].Substring(0, 8);
                }


                lst.Add(new AccountMaster()
                {
                    Acno = Acno,
                    Actype = type,
                    Amount = amount,
                    Date = Date,
                    TranID = Id.generate(),
                    Month = month,
                    year = year

                });

                var chkBalAcno = BalLst.Where(s => s.Acno == Acno).ToList();
                if (chkBalAcno.Count == 0)
                {
                    BalLst.Add(new BalanceMaster()
                    {
                        Acno = Acno,
                        Balance = amount,
                        type = type,
                        amount = amount
                    });
                }

                else
                {
                    UpdateBalance(type, Acno, amount);
                }

                var disValue = lst.Select(s => s.Acno).Distinct().ToList();
                foreach (var i in disValue)
                {
                    Console.WriteLine("Account Number:" + i);
                    Console.WriteLine("Date     |   Txn ID      |   Type   |   Amount  |");
                    var recVal = lst.Where(s => s.Acno == i).ToList();
                    foreach (var k in recVal)
                        Console.WriteLine(String.Format("{0}|   {1}    |    {2}     |   {3}", k.Date, k.TranID, k.Actype, k.Amount.ToString("#0.00")));
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception:" + ex.Message.ToString());
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        public void UpdateBalance(string type,string Accno,decimal amount)
        {
            if (type == "D")
            {
                foreach (var B in BalLst.Where(r => r.Acno == Accno))
                {

                    B.Balance = B.Balance + amount;
                }
            }
            else
            {
                foreach (var B in BalLst.Where(r => r.Acno == Accno))
                {

                    B.Balance = B.Balance - amount;
                }
            }
        }
        
    }

    class AccountMaster
    {
        public string Acno { get; set; }
        public string Date { get; set; }
        public decimal Amount { get; set; }
        public string TranID { get; set; }
        public string Actype { get; set; }
        public int Month { get; set; }
        public int year { get; set; }
    }

    class BalanceMaster
    {
        public string Acno { get; set; }
        public decimal Balance { get; set; }
        public string type { get; set; }
        public decimal amount { get; set; }
    }

    class InterestRule
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int NoOfDays { get; set; }
        public decimal EODbalance { get; set; }
        public string RuleID { get; set; }
        public double Rate { get; set; }
        public double AnnualInterest { get; set; }
    }
}


using System;

namespace BMS
{
    class Program
    {
        static void Main(string[] args)
        {
            String input;
            DOB dob = new DOB();
            IDGenerator id = new IDGenerator();
            
            Bank bn = new Bank();
            Console.WriteLine("****  Welcome to AwesomeGIC Bank!  ***");
            while (true)
            {
                Console.WriteLine("\nWhat would you like to do? :");
               
                Console.WriteLine("[I]nput transactions ");
                Console.WriteLine("[D]efine interest rules ");
                Console.WriteLine("[P]rint statement ");
                Console.WriteLine("[Q]uit ");
                
                object ob1 = Console.ReadLine();
                input = Convert.ToString(ob1);

                
                 if (input.ToString().ToUpper() == "I")
                {
                    Console.WriteLine("Please enter transaction details in <Date>|<Account>|<Type>|<Amount>:");
                    object ob = Console.ReadLine();
                    string Param= Convert.ToString(ob);
                    bn.CreateTransaction(Param);
                }
                else if(input.ToString().ToUpper() == "D")
                {
                    Console.WriteLine("Please enter interest rules details in <Date>|<RuleId>|<Rate in %> format:");
                    object ob = Console.ReadLine();
                    string Param = Convert.ToString(ob);
                    bn.GetRule(Param);
                }
                else if (input.ToString().ToUpper() == "P")
                {
                    Console.WriteLine("Please enter account and month to generate the statement <Account>|<Month>:");
                    object ob = Console.ReadLine();
                    string Param = Convert.ToString(ob);
                    bn.ShowTransactions(Param);
                }
                else if (input.ToUpper() == "Q")
                {
                    Console.WriteLine("Thank you for banking with AwesomeGIC Bank.");
                    Console.WriteLine("Have a nice day!");
                    Environment.Exit(0);
                }
                Console.ReadKey();


            }

        }

    }
}
    

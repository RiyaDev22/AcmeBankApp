namespace AcmeBank.AccountCreation
{
    public class AccountCreation
    {
        public static void DisplayMenu()
        {
            bool condition = true;

            while (condition)
            {
                /*Console.Clear();*/
                Console.Write("""
                ============================
                    **ACCOUNT CREATION** 
                ============================
                1. Personal Account.
                2. Individual Savings Account (ISA).
                3. Business Account.
                ============================
                Please, Select Account Type: 
                """);
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine("\nCreating Personal Account...");
                        // Call personal account creation method
                        CreatePersonalAccount();
                        break;
                    case "2":
                        Console.WriteLine("\nCreating Individual Savings Account (ISA)...");
                        // Call ISA account creation method
                        CreateISA();
                        break;
                    case "3":
                        Console.WriteLine("\nCreating Business Account...");
                        // Call business account creation method
                        CreateBusinessAccount();
                        break;
                    default:
                        Console.WriteLine("\nInvalid choice. Please try again.");
                        break;
                }
            }
        }

        private static void CreatePersonalAccount()
        {
            // Request photo and address ID
            Console.Write("Please, provide photo ID (Just add any charachter): ");
            string photoID = Console.ReadLine();
            Console.Write("Please, provide address ID (Just add any charachter): ");
            string addressID = Console.ReadLine();

            // Verify photo and address ID (DUMMY VERIFICATION)
            if (string.IsNullOrWhiteSpace(photoID) || string.IsNullOrWhiteSpace(addressID))
            {
                Console.WriteLine("Invalid photo ID or address ID. Account creation failed.");
                return;
            }

            // Request date of birth
            Console.Write("Please, provide your date of birth in this format -> (DD-MM-YYYY): ");
            string dobInput = Console.ReadLine();
            DateTime dob;
            if (!DateTime.TryParse(dobInput, out dob))
            {
                Console.WriteLine("Invalid date of birth format. Account creation failed.");
                return;
            }

            // Verify age (minimum age: 18)
            if (DateTime.Today.Subtract(dob).TotalDays / 365 < 18)
            {
                Console.WriteLine("You must be at least 18 years old to open a personal account. Account creation failed.");
                return;
            }

            // Request initial deposit
            decimal initialDeposit = RequestInitialDeposit();
            if (initialDeposit < 1)
            {
                Console.WriteLine("Initial deposit must be at least £1. Account creation failed.");
                return;
            }

            // If all checks pass, account is created
            Console.WriteLine("Personal account created successfully!");
        }

        private static void CreateISA() 
        {
            Console.WriteLine("""
                =======================================
                (ISA) created successfully!
                =======================================
                """);
        }

        private static void CreateBusinessAccount()
        {
            Console.WriteLine("""
                =======================================
                Business acccount created successfully!
                =======================================
                """);
        }

        private static decimal RequestInitialDeposit()
        {
            // Request initial deposit amount
            Console.Write("Please, provide the initial deposit amount: ");
            string depositInput = Console.ReadLine();
            decimal initialDeposit;
            if (!decimal.TryParse(depositInput, out initialDeposit))
            {
                Console.WriteLine("Invalid initial deposit amount. Defaulting to £0.");
                return 0;
            }
            return initialDeposit;
        }
    }
}

namespace AcmeBank
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CreateCustomer();
        }

        public static void CreateCustomer()
        {
            Console.WriteLine("""
                Hello and welcome to a new and Exciting Journey with us.
                I will start by asking for your name.
                """);
            string firstName = InputName("first");
            string lastName = InputName("last");
            string otherName = InputName("middle");
        }

        //Method to facilitate inputting of name
        private static string InputName(string nameType)
        {
            string name;
            do
            {
                Console.WriteLine($"What is your {nameType} name?");
                
                //Case that the name type is a middle name
                if (nameType == "middle")
                    Console.WriteLine("If you don't have a middle name, you can press the RETURN key.");
                
                name = Console.ReadLine();
                
                //Checking Condition for the loop. If true, it Tells the Teller what went wrong with the input
                if (name == "" && nameType != "middle")
                {
                    Console.Clear();
                    Console.WriteLine("Please do not input an empty value.");
                }
            } while (name == "" && nameType != "middle");
            
            Console.Clear();
            return name;

        }
    }
}

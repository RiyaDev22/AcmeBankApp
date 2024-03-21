namespace AcmeBank
{
    public class InputUtilities
    {

        //we need a method that replaces the usual readlines with one that ensures that input happens within a minute
        public static string? GetInputWithinTimeLimit()
        {
            //initialise input
            string? input = null;
            //we create a task to read the input
            var task = Task.Run(() => input = Console.ReadLine());
            //if this task takes less than 2 minutes, we just return the input
            if (task.Wait(TimeSpan.FromMinutes(2)))
            {
                return input;
            }
            else
            {
                //otherwise, we will show an error message
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("""
                    Input timed out.
                    You have been logged out for security.
                    Please press Return and start over.
                    """);
                Console.ResetColor();

                //since security is now a concern, we will exit the program
                Environment.Exit(1);
                //we still need a return statement for the method, even though it will never be reached
                return null;
            }
        }
    }
}

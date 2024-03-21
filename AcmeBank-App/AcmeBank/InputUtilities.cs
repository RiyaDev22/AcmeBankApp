namespace AcmeBank
{
    public class InputUtilities
    {

        //we need a method that replaces the usual readlines with one that ensures that input happens within a minute
        public static string? GetInputWithinTimeLimit()
        {
            string? input = null;
            var task = Task.Run(() => input = Console.ReadLine());
            if (task.Wait(TimeSpan.FromMinutes(0.25)))
            {
                return input;
            }
            else
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("""
                    Input timed out.
                    You have been logged out for security.
                    Please press Return and start over.
                    """);
                Console.ResetColor();
                Environment.Exit(1);
                return null;
            }
        }
    }
}

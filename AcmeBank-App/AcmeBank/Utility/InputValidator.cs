using System.Runtime.CompilerServices;

namespace AcmeBank.Utility;

// Utility class for validating input
public static class InputValidator
{
    // Method to retrieve a decimal input from the user
    // Returns the validated decimal input
    public static decimal TryGetDecimalInput(string messagePrompt, string outsideErrorPrompt = "")
    {
        string? input; // Variable to store user input
        decimal result = 1m; // Variable to store the parsed decimal value, initialized to a default value

        // Repeat until a valid decimal input is provided
        do
        {
            Console.Clear();
            Console.WriteLine(messagePrompt);
            // Provides a prompt to enter a value,
            // this also handles interal invalid inputs and can prompt external invalid inputs too
            // such as in personal account constructor
            Console.Write(result != 0? $"{outsideErrorPrompt}Enter a value: " : "-! Invalid Value !-\nEnter a value: "); 
            input = Console.ReadLine();

        } while(!decimal.TryParse(input, out result)); // Repeat until a valid decimal input is provided

        return result;
    }
}

using System.Globalization;
using System.Xml;

namespace AcmeBank
{
    class Teller
    {
        #region Global Variables
        //Variable used to store the teller's credentials
        private string? _sUsername;
        private string? _sPassword;
        //Variable used to store all teller accounts
        private Dictionary<string, string> _dTellerAccounts;
        
        #endregion

        #region Constructor
        //Default constructor
        public Teller ()
        {
            //Initialise dictionary
            _dTellerAccounts = new Dictionary<string, string> ();
            //Invoke function to populate dictionary
            populateDictionary();
            //Invoke function for the user to log in
            login();
        }
        #endregion

        #region Getters and Setters        
        /*Getter and setter for the username*/
        public string sUsername
        {
            get { return _sUsername; }
            set { _sUsername = value; }
        }

        /*Getter and setter for the password*/
        public string sPassword
        {
            get { return _sPassword; }
            set { _sPassword = value; }
        }
        #endregion

        #region Methods
        /*This method is invoked when the teller logs in*/
        public void login()
        {
            //Local variable which determines if the user input for username is valid
            bool bUsernameValid = false;
            //Local variable which determines if the user input for password is valid
            bool bPasswordValid = false;

            //Print initial login screen
            Console.Write("""
                                --- Teller Login ---
                                Username: 
                                """);

            do
            {
                //Prompt teller to enter their username
                string? sUsernameInput = Console.ReadLine();

                //If username exists
                if (_dTellerAccounts.ContainsKey(sUsernameInput))
                {
                    //Set boolean to true
                    bUsernameValid = true;
                    //Store the username
                    sUsername = sUsernameInput;
                    //Store the password associated with the username
                    sPassword = _dTellerAccounts[sUsernameInput];
                    //Print the message
                    Console.Write("Password: ");
                    do
                    {
                        //Prompt teller to enter their password
                        string? sPasswordInput = Console.ReadLine();
                        //If password is valid
                        if (sPasswordInput.CompareTo(sPassword) == 0)
                        {
                            //Set boolean to true
                            bPasswordValid = true;
                            //Print message
                            Console.WriteLine($"Welcome {sUsername}!");
                        }
                        //If password is not valid
                        else
                        {
                            //Print message
                            Console.Write("""
                                                Invalid password. Please try again.
                                                Password: 
                                                """);
                        }
                    } while (!bPasswordValid); //Loop will keep executing until password is valid
                }
                //If username does not exist
                else
                {
                    //Print message
                    Console.Write("""
                                        Username does not exist. Please try again.
                                        Username: 
                                        """);
                }
            } while (!bUsernameValid); //Loop will keep executing until username is valid
        }

        /*This method is invoked when the teller logs out*/
        public void logout()
        {
            //Reset the global variables
            sUsername = "";
            sPassword = "";
        }

        /*This method is invoked when the teller object is instantiated*/
        private void populateDictionary()
        {
            //Initialise local variable that contains the name of the csv file
            string sFileName = "Teller Accounts.csv"; //Note the csv file is in bin\debug\net8.0
            
            try
            {
                //Open the file using a stream reader
                using (StreamReader oReader = File.OpenText(sFileName))
                {
                    //Initialise a string that will store the content from the file
                    string sFileInput = "";
                    //Initialise a count that will count the number of lines in the csv file
                    int iCount = 0;
                    //Read the whole file
                    while (!oReader.EndOfStream)
                    {
                        //Store a line into a string
                        sFileInput = oReader.ReadLine();                        
                        //Make sure the headers won't get appended to the dictionary
                        if (iCount != 0)
                        {
                            //Split the string and store into string array
                            string[] saFileInput = sFileInput.Split(",");
                            //Add the content to the dictionary
                            _dTellerAccounts.Add(saFileInput[0], saFileInput[1]);
                        }
                        //Increment counter
                        iCount++;
                    }
                }
            }
            //Catch exception if file has not been found
            catch (Exception oException)
            {
                //Print message
                Console.WriteLine($"File '{sFileName}' not found");
            }
        }
        #endregion
    }
}
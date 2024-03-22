# AcmeBankApp

## Overview
This is a group project for the development of a C# application for Acme Bank. The application is designed to cater to tellers who need to access customer accounts. It involves the handling of three types of accounts: Personal, ISA, and Business.

## Project Scope: The Banking App
### Scenario
Acme Bank has enlisted our team to create a C# application facilitating tellers in accessing customer accounts. Here are the key points regarding each type of account:

### Personal Account:
- Customers opening a personal account must provide both a photo ID (passport, driving license) and an address-based ID (utility bill, council letter).
- The minimum opening balance for a personal account is £1 (GBP).
- No charges apply for owning the account, but overdrafts may incur charges.
- The account supports services like direct debits and standing orders for bill payments.

### ISA Account:
- Rules for the ISA account can be found at [Individual Savings Accounts (ISAs): Overview - GOV.UK](https://www.gov.uk/individual-savings-accounts).
- Customers with ISA accounts benefit from an annual APR of 2.75% on the average annual balance.
- Each customer is limited to one ISA account.

### Business Account:
- Business account eligibility requires proof and details of an existing business.
- Certain business types (enterprise, plc, charity, public sector) will be handled by a separate department.
- A business account comes with a business cheque book (upon request) and incurs an annual charge of £120 (GBP).
- Account holders receive debit/credit cards and may access an overdraft facility, international trading, and loans.

## General Rules:
- A single customer can have each of the three account types, adhering to specific rules within each account.
- Customers do not have direct access to the software; authentication methods must be implemented.
- The software should gracefully handle unexpected data entries and remain functional.
- A graphical user interface (GUI) is not mandatory; ensure the core functionality works before GUI implementation.
- A simple menu system is required with validated information submission.
- A help system linked to user actions should support the user.
- Document any assumptions made in a separate document.
- Upload the completed work to a group agreed Git repository.
- Questions during the requirements gathering phase are encouraged.

## Team Members
- Tom
- Riya
- Ted
- Moeez
- Kawsar

## Questions to ASK:
- Do we need to create customer accounts?
- Do we a need a personal account to open an ISA or Busisness?
- Do we need to consider transactions?
- Do we need to consider loans and business cheques? If so how would we manage cheques?
- There are 4 types of ISA accounts in the GOV website. Are we expected to implement all of them?
- There are inflexible and flexible ISA accounts. Are we expecte to account for them?

## Assumptions:
- Assume that the user knows their card information
- Trevor has VIsual Paradigm installed on his PC

## UML (Click for clear and full view):
<div style="text-align: center;">
    <a href="https://ibb.co/YkvccZq">
        <img src="https://i.ibb.co/0MPCCtd/group2-class-diagram.png" alt="group2-class-diagram" border="1" width="600" height="300">
</div>

## FlowChart (Click for clear and full view):
<div style="text-align: center;">
    <a href="https://ibb.co/t4VTb00">
        <img src="https://i.ibb.co/kxNshLL/Flowchart.png" alt="group2-class-diagram" border="1" width="600" height="300">
</div>

## Application Requirements (Click to open the document):
<div style="text-align: center;">
    <a href="https://docs.google.com/document/d/1aQmesj4Bg827qxOpJ2BBC-365B36BvHxO5N-FZGWXD8/edit?usp=sharing">
      <img src="https://i.ibb.co/dD5g82z/checklist-1919292-1280.jpg" alt="checklist" border="1" width="600" height="350">
    </a>
</div>


## How the Application works - Logging in
- To log into the system, you have to give your teller details. Both the username and password are '**admin**'
- You are then presented with a main menu which allows you to do the following
    1. View a Customer account
    2. Create a Customer account
    3. Remove a Customer account
    4. Log out
    5. Log out & Quit

### View a Customer account
- Input in the your customer details
    - The details are as follows
        - First Name: '**Kawsar**'
        - Last Name: '**Hussain**'
        - Other Name: **PRESS RETURN**
        - Date of Birth (YYYY-MM-DD): '**2001-04-17**'
        - Postcode: '**E15 5DP**'
        - What is the manufacturer of the first car you owned or drove?: '**Toyota**'
- If all these have been done correctly, It should display the accounts you have. Select which account you want selected and then the 
### Create a Customer account
- Input in the your customer details
    - The details are as follows
    - First Name:
    - Last Name:
    - Other Name(can leave empty):
    - Day of Birth (DD):
    - Month of Birth (MM):
    - Year of Birth (YYYY):
    - Gives an option between 6 security questions. Input a number between 1-6 to select which one
    - What is the answer to your security question?:
- Once all that has been inputted, The customer has been created and the results can be viewed at '**Customers.csv**' in the debug folder

### Remove a Customer account
- Input in the your customer details
    - The details are as follows
        - First Name: '**Kawsar**'
        - Last Name: '**Hussain**'
        - Other Name: **PRESS RETURN**
        - Date of Birth (YYYY-MM-DD): '**2001-04-17**'
        - Postcode: '**E15 5DP**'
        - What is the manufacturer of the first car you owned or drove?: '**Toyota**'
- If done correctly, the customer has been removed and the results can be viewed at '**Customers.csv**' in the debug folder. You can also try with the customer details that you have created.

### Log out
- By selecting this option, the teller logs out and it shows the screen for inputting teller log in.

### Log out and quit
- By selecting this option, the teller logs out and the application closes.

## How the application works - Account options loop
- The account details will be displayed and all the options will be shown. The options are as follows for the different account types:
    - Personal Account
        1. Deposit
        2. Withdraw
        3. Payment
        4. Transfer
        5. Manage Standing Orders/Direct Debits
        6. Request Debit Card
        7. Manage Overdraft
    - ISA Account
        1. Deposit
        2. Withdraw
        3. Payment
        4. Transfer
        5. Statement
        6. Exit
    - Business
        1. Deposit
        2. Withdraw
        3. Payment
        4. Transfer
        5. Request Credit/Debit Card
        6. Request Cheque Book
        7. Manage Loans
        8. Manage Overdraft
        9. Exit


### Deposit
The teller inputs a value which gets deposited into the account. It checks to see if the amount value inputted is valid. If so, the money gets added to the account.

### Withdraw
The teller inputs a value which gets withdrawn from the account. The value is checked to see if the transaction can be achieved. For different account types, it is different. For the personal and business accounts, it checks to see if the amount withdrawn will exceed the overdraft limit. For the ISA account, it updates the deposit limit attached to the account.

### Payment
It first checks to see if the customer can pay another account. Then it tells the teller inputs an account number to which the customer wants to put it into, excluding the account which is doing the payment and excluding ISA accounts. Once a calid account number is inputted, the teller is then prompted to put an amount to pay to the inputted account. Once the value inputted has been validated, the money is withdrawn from the customer account and deposited to the payee account.

### Transfer
TODO

### Manage Standing Order / Direct debits
This is a method exclusive to a personal account type. Still in **TODO**

### Statement
Method which displays the statement of the account to the teller

### Request Credit/Debit Card
A method to say that a card would be delivered to the account address. A personal account can only request for a debit card but a business account can request for both credit and debit

### Manage Overdraft
For the business account, the teller is told to direct the customer to the business overdraft division.

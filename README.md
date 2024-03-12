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

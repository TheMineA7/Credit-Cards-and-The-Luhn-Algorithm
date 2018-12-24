//This program generates a credit card number that is valid with the Luhn Algorithm.
//It also checks if the entered credit card number is valid.
//DO NOT USE THIS FOR ILLEGAL AND UNETHICAL ACTIVITIES!
//I, the programmer of this code, am not responsible for misusage of this program.
//The purpose of this program was to understand the Luhn Algorithm and expand my knowledge in programming.
//It was not meant to be used for illegal and unethical activities.
//By using/running/compling this program you, the user, agree that you will not conduct illegal and unethical activities with this program.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Credit_Card_App
{
    public partial class CreditCardApp : Form
    {
        //Constructor Method
        public CreditCardApp()
        {
            InitializeComponent();
            MaximizeBox = false;
            listOfCreditCardTypes.SelectedIndex = 0;
        }

        //Event Handler when "Check" button is clicked
        private void CheckButton_Click(object sender, EventArgs e)
        {
            string creditCardNumber = creditCardNumberInputBox.Text;
            string creditCardTypeResult = CreditCardType(creditCardNumber);
            int luhnAlgorithmResult = LuhnAlgorithm(creditCardNumber, creditCardTypeResult);
            if (luhnAlgorithmResult == 0)
            {
                creditCardTypeOutputLabel.Text = "Card Type: " + creditCardTypeResult;
                creditCardValidiityOutputLabel.Text = "Credit Card Number: Valid!";
            }
            else
            {
                creditCardTypeOutputLabel.Text = "Card Type: Invalid";
                creditCardValidiityOutputLabel.Text = "";
            }
        }

        //Event Handler when Create button is clicked
        private void Create_Click(object sender, EventArgs e)
        {
            bool cardIsValid = false;
            string creditCardNumber = "";

            while (cardIsValid == false)
            {
                creditCardNumber = GenerateCCNum();
                if (creditCardNumber != "Didn't Work")
                    cardIsValid = true;
            }

            RandomCCNumLabel.Text = creditCardNumber;
            creditCardNumberInputBox.Text = creditCardNumber;
        }

        //Determine Credit Card Type
        private string CreditCardType(string CreditCardNumber)
        {
            int creditCardNumberLength = CreditCardNumber.Length;
            //Error Checking if credit card length is 
            if (14 > creditCardNumberLength || creditCardNumberLength > 16)
            {
                return "Invalid";
            }
            int firstTwoDigits = Convert.ToInt32(CreditCardNumber.Substring(0, 2));

            if (CreditCardNumber.Substring(0, 1) == "4" && creditCardNumberLength == 16)
                return "Visa";
            else if ((firstTwoDigits <= 55 && firstTwoDigits >= 51) && creditCardNumberLength == 16)
                return "MasterCard or Diners Club in USA and Canada";
            else if ((firstTwoDigits == 34 || firstTwoDigits == 37) && creditCardNumberLength == 15)
                return "American Express";
            else if (CreditCardNumber.Substring(0, 4) == "6011" && creditCardNumberLength == 16)
                return "Discover";
            else if (firstTwoDigits == 36 && creditCardNumberLength == 14)
                return "Diners Club outside of USA and Canada";
            else
                return "Invalid";
        }

        //Check sum using Luhn Alogrithm
        private int LuhnAlgorithm(string creditCardNumber, string creditCardType)
        {
            if (creditCardType == "Invalid")
                return 1;

            int creditCardLength = creditCardNumber.Length;
            int checkSum = 0;

            for (int i = creditCardLength - 1; i >= 0; i -= 2)
            {
                checkSum += int.Parse(creditCardNumber.Substring(i, 1));
            }
            for (int i = creditCardLength - 2; i >= 0; i -= 2)
            {
                int digit = int.Parse(creditCardNumber.Substring(i, 1));
                digit = digit * 2;
                if (digit > 9)
                    checkSum += digit / 10 + digit % 10;
                else
                    checkSum += digit;
            }
            if (checkSum % 10 == 0)
                return checkSum % 10;
            else
                return 99;
        }

        //Generate Credit Card Number
        //Change Algorithm that creates the cc num upto the last one then create the last one mathematically
        //Revert back to original method
        private string GenerateCCNum()
        {
            Random rnd = new Random();
            string randomCreditCardNumber = "";
            int creditCardLength = 0;

            //Generating prefixes and initializing card lengths
            if (listOfCreditCardTypes.SelectedIndex == 0) //If Visa is selected
            {
                creditCardLength = 15;
                randomCreditCardNumber = "4";
            }
            else if (listOfCreditCardTypes.SelectedIndex == 1) //If MasterCard is selected
            {
                creditCardLength = 14;
                string randomPrefix = rnd.Next(51, 56).ToString();
                randomCreditCardNumber = randomPrefix;
            }
            else if (listOfCreditCardTypes.SelectedIndex == 2) //If American Express is selected
            {
                creditCardLength = 13;
                int randomPrefix = rnd.Next(1, 3);
                if (randomPrefix == 1)
                    randomCreditCardNumber = "34";
                else if (randomPrefix == 2)
                    randomCreditCardNumber = "37";
            }
            else if (listOfCreditCardTypes.SelectedIndex == 3) //If Discover is selected
            {
                creditCardLength = 12;
                randomCreditCardNumber = "6011";
            }
            else if (listOfCreditCardTypes.SelectedIndex == 4) //If Diners Club is selected
            {
                creditCardLength = 12;
                randomCreditCardNumber = "36";
            }
            else //If no card type is selected
            {
                creditCardLength = 0;
                randomCreditCardNumber = "Please select a valid card type";
            }

            //Adding random digits to the card number
            for (int i = 0; i < creditCardLength - 1; i++)
            {
                randomCreditCardNumber += rnd.Next(0, 10).ToString();
            }

            randomCreditCardNumber += "0";

            int checkSum = LuhnAlgorithm(randomCreditCardNumber, "0");
            int lastDigit = 10 - checkSum;
            if (lastDigit == 10)
            {
                lastDigit = 0;
            }
            randomCreditCardNumber.Remove(15, 1);
            randomCreditCardNumber += Convert.ToString(lastDigit);

            return randomCreditCardNumber;
            //Checking if the random cc number is a valid card
            /*
            bool validCreditCardNumber = true;
            if (randomCreditCardNumber != "Please select a valid card type")
                validCreditCardNumber = LuhnAlgorithm(randomCreditCardNumber, "0");

            if (validCreditCardNumber == true)
                return randomCreditCardNumber;
            else
                return "Didn't Work";
            */
        }

        //Key Filtering for credit card number input box
        private void CreditCardNumberBox_KeyDown(object sender, KeyEventArgs e)
        {
            var eKeyValue = e.KeyValue;
            var eKeyData = e.KeyData;
            e.SuppressKeyPress = true;
            //Only allows numbers, and shortcuts for copy, cut, and select all, right and left arrow keys, delete and backspace
            if ((eKeyValue >= 48 && eKeyValue <= 57) || (eKeyValue >= 96 && eKeyValue <= 105) || eKeyValue == 8 || eKeyValue == 46 || eKeyValue == 37 || eKeyValue == 39 || eKeyData == (Keys.Control | Keys.A) || eKeyData == (Keys.Control | Keys.C) || eKeyData == (Keys.Control | Keys.X))
            {
                e.SuppressKeyPress = false;
                return;
            }
            return;
        }

        //Event handler for "Select All" in the custom context menu
        private void SelectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            creditCardNumberInputBox.SelectAll();
        }

        //Event handler for "Copy" in the custom context menu
        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(creditCardNumberInputBox.SelectedText);
        }

        //Event handler for "Cut" in the custom context menu
        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(creditCardNumberInputBox.Text);
            creditCardNumberInputBox.SelectedText = "";
        }
    }
}
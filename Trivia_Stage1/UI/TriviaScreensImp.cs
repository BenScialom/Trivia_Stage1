using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Trivia_Stage1.Models;

namespace Trivia_Stage1.UI
{
    public class TriviaScreensImp:ITriviaScreens
    {
        private TriviaDbContext context = new TriviaDbContext();
        public Player loggedPlayer {get; private set;}
    //Place here any state you would like to keep during the app life time
    //For example, player login details...


    private DbContext db = new TriviaDbContext();

        //Implememnt interface here
        public bool ShowLogin()
        {
            Console.WriteLine("Not implemented yet! Press any key to continue...");
            Console.ReadKey(true);
            return true;
        }
        public bool ShowSignUp()
        {
            //Logout user if anyone is logged in!
            //A reference to the logged in user should be stored as a member variable
            //in this class! Example:


            //Loop through inputs until a user/player is created or 
            //user choose to go back to menu
            char c = ' ';
            while (c != 'B' && c != 'b' && loggedPlayer == null)
            {
                //Clear screen
                ClearScreenAndSetTitle("Signup");

                Console.Write("Please Type your email: ");
                string email = Console.ReadLine();

                while (!IsEmailValid(email))
                {
                    if (email == "b" || email == "B") { return false; }
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Bad Email Format! Please try again:");
                    Console.ResetColor();
                    email = Console.ReadLine();

                }
                loggedPlayer.Mail = email;

                Console.Write("Please Type your password: ");
                string password = Console.ReadLine();
                while (!IsPasswordValid(password))
                {
                    if (password == "b" || password == "B") { return false; }
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("password must be at least 4 characters! Please try again: ");
                    Console.ResetColor();
                    password = Console.ReadLine();
                }
                loggedPlayer.Password = password;

                Console.Write("Please Type your Name: ");
                string name = Console.ReadLine();
                while (!IsNameValid(name))
                {
                    if (name == "b" || name == "B") { return false; }
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("name must be at least 3 characters! Please try again: ");
                    Console.ResetColor();
                    name = Console.ReadLine();
                }
                loggedPlayer.Name = name;

                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("Connecting to Server...");
                Console.ResetColor();
                //Create instance of Business Logic and call the signup method
                // *For example:
                //try
                //{
                //    TriviaDBContext db = new TriviaDBContext();
                //    this.currentyPLayer = db.SignUp(email, password, name);
                //}
                //catch (Exception ex)
                //{
                //    Console.ForegroundColor = ConsoleColor.Red;
                //    Console.WriteLine("Failed to signup! Email may already exist in DB!");
                //    Console.ResetColor();
                //}
                try
                {
                    context.Players.Add(loggedPlayer);
                    context.SaveChanges();

                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Sign Up failed");
                    Console.ResetColor();
                }



                //Provide a proper message for example:
                Console.WriteLine("Press (B)ack to go back or any other key to signup again...");
                //Get another input from user
                c = Console.ReadKey(true).KeyChar;
            }
            //return true if signup suceeded!
            return true;

        }
        //Ben
        public void ShowAddQuestion()
        {//פעולה הבודקת האם יש למשתמש מספיק נקודות כדי להוסיף שאלה ומבקשת ממנו להוסיף שאלה
            if (loggedPlayer.Points == 100)
            {
                Console.WriteLine("Add the questions text press b to go back");
                string qText= Console.ReadLine();
                Question q = new Question();
                if (qText.ToUpper() == "B")
                    return;
                q.Text = qText;
                Console.WriteLine("Choose a subject 1 - Sports, 2 - Poiltics, 3 - History, 4 - Science ,5 -Ramon:");
                char y = '0';
                while (y == 0)
                {
                    y= Console.ReadKey().KeyChar;
                    if (y == '1')
                        q.StatusId = 1;
                    else if (y == '2')
                        q.StatusId = 2;
                    else if (y == '3')
                        q.StatusId = 3;
                    else if (y == '4')
                        q.StatusId = 4;
                    else if (y == '5')
                        q.StatusId = 5;
                    else y = '0';

                }
                Console.WriteLine();
                string x;
                Console.WriteLine("Add the right answer:");
                x=Console.ReadLine();
                q.RightA = x;
                Console.WriteLine("Add worng answer 1");
                q.WrongA1 = x;
                Console.WriteLine("Add wrong answer 2");
                q.WrongA2 = x;
                Console.WriteLine("Add wrong answer 3");
                q.WrongA3 = x;
                q.StatusId = 2;
                q.PlayerId = loggedPlayer.PlayerId;
                context.Questions.Add(q);
                context.SaveChanges();
                loggedPlayer.Points = 0;
                loggedPlayer.Questionsadded++;
            }
            else
            {
                Console.Clear();

            }
        
        }

        public void ShowPendingQuestions()//Ben
        {
            // Shows a PendingQuestion
            Console.WriteLine("Pending question");
            char c;
            c = '5';
            foreach (Question q in context.Questions)
            {
                if (q.StatusId == 1)
                {
                    Console.WriteLine(q.RightA);
                    Console.WriteLine(q.WrongA1);
                    Console.WriteLine(q.WrongA2);
                    Console.WriteLine(q.WrongA3);
                    Console.WriteLine("Press 1 to approve ,Press 2 to reject, Press 3 to skip");

                    while (c == '5')
                    {
                        c = Console.ReadKey().KeyChar;
                        if (c == 1)
                        {
                            q.StatusId = 2;
                        }
                        if (c == 2)
                        {
                            q.StatusId = 3;
                        }
                        else
                            c = '5';
                    }
                }
            }
        }
        //איתמר
        public void ShowGame()
        {
            Console.WriteLine("Not implemented yet! Press any key to continue...");
            Console.ReadKey(true);
        }
        public void ShowProfile()
        {

            char c = ' ';
            while (c != 'B' && c != 'b')
            {
                Console.WriteLine($"Email:{loggedPlayer.Mail}");
                Console.WriteLine($"Name:{loggedPlayer.Name}");
                Console.WriteLine($"Password:{loggedPlayer.Password}");
                Console.WriteLine($"Rank:{loggedPlayer.RankId}");
                Console.WriteLine($"Score:{loggedPlayer.Points}");
                Console.WriteLine("   ");

                char ch;
                Console.WriteLine("if you want to update anything enter U");
                Console.WriteLine("if you dont want to change anything enter N");
                ch = char.Parse(Console.ReadLine());
                bool playerUpdate = false;
                if (ch == 'U' || ch == 'u')
                {
                    int num;
                    Console.WriteLine("Enter 1 if you want to change Email");
                    Console.WriteLine("Enter 2 if you want to change name");
                    Console.WriteLine("Enter 3 if you wanr to change password");
                    num = int.Parse(Console.ReadLine());
                    while (num != 1 && num != 2 && num != 3)
                    {

                        Console.WriteLine("Enter 1 if you want to change Email");
                        Console.WriteLine("Enter 2 if you want to change name");
                        Console.WriteLine("Enter 3 if you wanr to change password");
                        num = int.Parse(Console.ReadLine());
                    }
                    if (num == 1)
                    {
                        string updatedEmail;
                        Console.WriteLine("enter new Email");
                        updatedEmail = Console.ReadLine();
                        if (!IsEmailValid(updatedEmail))
                        {
                            Console.Write("Email not valid,try again");
                            updatedEmail = Console.ReadLine();
                        }
                        loggedPlayer.Mail = updatedEmail;
                        playerUpdate = true;
                    }
                    else if (num == 2)
                    {
                        string updatedName;
                        Console.WriteLine("enter new name");
                        updatedName = Console.ReadLine();
                        while (!IsNameValid(updatedName))
                        {
                            Console.Write("name must have at least 3 letter");
                            updatedName = Console.ReadLine();
                        }
                        loggedPlayer.Name = updatedName;
                        playerUpdate = true;
                    }
                    else if (num == 3)
                    {
                        string updatedPass;
                        Console.WriteLine("enter new Password");
                        updatedPass = Console.ReadLine();
                        while (!IsPasswordValid(updatedPass))
                        {
                            Console.Write("Password must have at least 4 letters/numbers,try again");
                            updatedPass = Console.ReadLine();
                        }
                        loggedPlayer.Password = updatedPass;
                        playerUpdate = true;
                    }
                    if (playerUpdate == true)
                    {
                        try
                        {
                            //TriviaDbContext db = new TriviaDbContext();
                            //db.UpdatedPlayer(loggedPlayer);

                           // context.UpdatePlayer(loggedPlayer);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("update player failed");
                        }

                    }
                    Console.WriteLine("Press B to go back to the menu");
                    Console.WriteLine("enter any other key to continue updating");
                    c = Console.ReadKey(true).KeyChar;
                }

            }

        }


        //Private helper methods down here...
        private void ClearScreenAndSetTitle(string title)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{title,65}");
            Console.WriteLine();
            Console.ResetColor();   
        }

        private bool IsEmailValid(string emailAddress)
        {
            //regex is string based pattern to validate a text that follows a certain rules
            // see https://learn.microsoft.com/en-us/dotnet/standard/base-types/regular-expression-language-quick-reference

            var pattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

            var regex = new Regex(pattern);
            return regex.IsMatch(emailAddress);

        //another option is using .net System.Net.Mail library which has an EmailAddress class that stores email
        //we can use it to validate the structure of the email:
       // https://learn.microsoft.com/en-us/dotnet/api/system.net.mail.mailaddress?view=net-7.0
            /*
             * try
             * {
             *     //try to create MailAddress objcect from the email address string
             *      var email=new MailAddress(emailAddress);
             *      //if success
             *      return true;
             * }
             *      //if it throws a formatExcpetion then the string is not email format.
             * catch (Exception ex)
             * {
             * return false;
             * }
             */

        }



        private bool IsPasswordValid(string password)
        {
            return !string.IsNullOrEmpty(password) && password.Length >= 3;
        }

        private bool IsNameValid(string name)
        {
            return !string.IsNullOrEmpty(name) && name.Length >= 3;
        }
    }
}

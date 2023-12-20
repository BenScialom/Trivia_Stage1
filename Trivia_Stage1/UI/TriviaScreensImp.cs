using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
        public bool ShowLogin()//Itamar
        {
            loggedPlayer = null;
            bool loggedIn = false;
            while (!loggedIn)
            {
                ClearScreenAndSetTitle("Login:\n");
                string mail;
                string password;
                Console.WriteLine("Email:");
                mail = Console.ReadLine();
                while (mail == null)
                {
                    Console.WriteLine("Please email enter again:");
                    mail = Console.ReadLine();
                }
                try
                {
                    if (context.DoesMailExistsInDb(mail))
                    {
                        Console.WriteLine("Password:");
                        password = Console.ReadLine();
                        while (password == null)
                        {
                            Console.WriteLine("Please password enter again:");
                            password = Console.ReadLine();
                        }
                        loggedPlayer = context.GetPlayerByMail(mail);
                        if (loggedPlayer != null)
                        {
                            if (loggedPlayer.Password == password)
                            {
                                Console.WriteLine("Logged in successfully!");
                                loggedIn = true;
                            }
                            else
                                Console.WriteLine("Wrong email or password. Please try again");
                        }
                    }
                    else
                        Console.WriteLine("Email doesn't exists, please try again");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Sorry, there is a problem. Press any key to return to menu-");
                    Console.ReadKey(true);
                    return false;
                }

            }
            Console.WriteLine("Press any key");
            Console.ReadKey(true);
            return loggedIn;
        }
        public bool ShowSignUp()//Ran
        {
            bool signUpStatus = false;

            //Logout user if anyone is logged in!
            //A reference to the logged in user should be stored as a member variable
            //in this class! Example:
            loggedPlayer = null;

            //Loop through inputs until a user/player is created or 
            //user choose to go back to menu
            char c = ' ';
            while (c.ToString().ToUpper() != "B")
            {
                //Clear screen
                ClearScreenAndSetTitle("Signup");
                loggedPlayer = new Player();

                if (!UpdateEmail("Please Type your email: ", "Bad Email Format! Please try again:"))
                    return false;

                Console.Write("Please Type your password: ");
                string password = Console.ReadLine();
                while (!IsPasswordValid(password))
                {
                    if (password.ToUpper() == "B") { return false; }
                    password = ErrorMessage("Password must be at least 3 characters! Please try again: ");
                }
                loggedPlayer.Password = password;

                Console.Write("Please Type your Name: ");
                string name = Console.ReadLine();
                while (!IsNameValid(name))
                {
                    if (name.ToUpper() == "B") { return false; }
                    name = ErrorMessage("name must be at least 3 characters! Please try again: ");
                }
                loggedPlayer.Name = name;
                loggedPlayer.Rank = new Rank();
                loggedPlayer.Rank.RankName = "Rookie";

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
                    Console.WriteLine("Sign Up succeeded");
                    signUpStatus = true;
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Sign Up failed");
                    Console.ResetColor();
                    signUpStatus = false;
                }

                //Provide a proper message for example:
                Console.WriteLine("Press (B)ack to go back or any other key to signup again...");
                //Get another input from user
                c = Console.ReadKey(true).KeyChar;
            }


            //return true if signup suceeded!
            return signUpStatus;

        }
        //Ben
        public void ShowAddQuestion()//Ben
        {//פעולה הבודקת האם יש למשתמש מספיק נקודות כדי להוסיף שאלה ומבקשת ממנו להוסיף שאלה
            if (loggedPlayer.Points == 100)
            {
                Console.WriteLine("Add the questions text press b to go back");
                string qText= Console.ReadLine();
                Question q = new Question();
                if (qText.ToUpper() == "B")
                    return;
                q.Question1 = qText;
                Console.WriteLine("Choose a subject 1 - Sports, 2 - Poiltics, 3 - History, 4 - Science ,5 -Ramon:");
                char y = '0';
                while (y == 0)
                {
                    y= Console.ReadKey().KeyChar;
                    if (y == '1')
                        q.SubjectId = 1;
                    else if (y == '2')
                        q.SubjectId = 2;
                    else if (y == '3')
                        q.SubjectId = 3;
                    else if (y == '4')
                        q.SubjectId = 4;
                    else if (y == '5')
                        q.SubjectId = 5;
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
               
            }
            else
            {
                Console.Clear();

            }
        
        }

        public void ShowPendingQuestions()//Ben
        {
            if(loggedPlayer.RankId==1|| loggedPlayer.RankId == 2)
            {
                foreach(Question q in context.Questions)
                {
                    char x = '5';
                    if (q.StatusId == 2)
                    {
                        ClearScreenAndSetTitle("Pending Questions    ");
                        Console.WriteLine($"Question: {q.Question1}");
                        Console.WriteLine($"Correct answer {q.RightA}");
                        Console.WriteLine($"Wrong answer #1 {q.WrongA1}");
                        Console.WriteLine($"Wrong answer #2 {q.WrongA2}");
                        Console.WriteLine($"Wrong answer #3 {q.WrongA3}");
                        Console.WriteLine("Press 1 to approve press 2 to reject press 3 to skip and press 4 to exit");
                        while (x == '5')
                        {
                            x = Console.ReadKey().KeyChar;
                            if (x == '1')
                            {
                                q.StatusId = 1;

                            }
                            else if (x == '2')
                                q.StatusId = 3;
                            else if (x == '3')
                                q.StatusId = 2;
                            else if (x == '4')
                            {
                                context.SaveChanges();
                            }

                        }
                    }
                        context.SaveChanges() ;
                   
                }
            }
            else
            {
                Console.WriteLine("You dont have acces to this page", 80);
                Console.WriteLine();
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
            }
        }
        //איתמר
        public void ShowGame()
        {
            Question question;
            List<string> answers = new List<string>(4);
            while (true)
            {

                ClearScreenAndSetTitle("Game on");
                try
                {


                    question = context.GetRandomQuestion();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Sorry, there is a problem. Press any key to return to menu-");
                    Console.ReadKey(true);
                    return;
                }
                answers.Add(question.RightA);
                answers.Add(question.WrongA1);
                answers.Add(question.WrongA2);
                answers.Add(question.WrongA3);
                answers = answers.OrderBy(x => Random.Shared.Next()).ToList();
                Console.WriteLine($"{question.Question1}\n");
                Console.WriteLine($"1. {answers[0]}");
                Console.WriteLine($"2. {answers[1]}");
                Console.WriteLine($"3. {answers[2]}");
                Console.WriteLine($"4. {answers[3]}");
                Console.WriteLine("Write the number of the correct answer\nYour answer:");
                int answer = 0;
                bool validAnswer = false;
                while (!validAnswer)
                {
                    try
                    {
                        answer = int.Parse(Console.ReadLine());
                        if (answer > 0 && answer < 5) { validAnswer = true; }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Invalid answer. Please try again. Press any key to continue");
                        Console.ReadKey(true);
                        answer = 0;
                        continue;
                    }
                }
                if (answers[answer] == question.RightA)
                {
                    Console.WriteLine("Correct!");
                    loggedPlayer.Points += 10;
                }
                else
                {
                    Console.WriteLine($"Wrong answer! The answer is '{question.RightA}'");
                    loggedPlayer.Points -= 5;
                }
                if (loggedPlayer.Points < 0)
                    loggedPlayer.Points = 0;
                if (loggedPlayer.Points > 100)
                    loggedPlayer.Points = 100;
                try
                {
                    Console.WriteLine("If you want to exist press [E]");
                    string exit = Console.ReadLine();
                    if (exit == "E" || exit == "e")
                        return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Invalid input. Press any key to exist");
                    Console.ReadKey(true);
                    return;
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
        private string ErrorMessage(string text)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(text);
            Console.ResetColor();
            return Console.ReadLine();
        }


        private bool UpdateEmail(string text, string error)
        {
            Console.Write(text);
            string email = Console.ReadLine();
            while (!IsEmailValid(email))
            {
                if (email.ToUpper() == "B") { return false; }
                email = ErrorMessage(error);
            }
            this.loggedPlayer.Mail = email;
            return true;
        }
        public void ShowProfile()//Ran
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
                    int num = 0;
                    while (num != 1 && num != 2 && num != 3)
                    {
                        Console.WriteLine("Enter 1 if you want to change email");
                        Console.WriteLine("Enter 2 if you want to change name");
                        Console.WriteLine("Enter 3 if you want to change password");
                        num = int.Parse(Console.ReadLine());
                    }
                    if (num == 1)
                    {
                        if (UpdateEmail("Enter new email: ", "Bad Email Format! Please try again:"))
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
                           
                            context.UpdatePlayer(loggedPlayer);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("update player failed");
                            Console.WriteLine("Press any key");
                            Console.ReadKey(true);
                            return;
                        }

                    }
                    Console.WriteLine("Press B to go back to the menu");
                    Console.WriteLine("enter any other key to continue updating");
                    c = Console.ReadKey(true).KeyChar;
                }

            }





        }
    }
}

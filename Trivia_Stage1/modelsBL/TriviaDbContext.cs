using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trivia_Stage1.Models;

namespace Trivia_Stage1.modelsBL
{
    public partial class TriviaDbContext
    {
        public Player Login(string mail, string password)
        {
            return this.Players.Where(p => p.Mail == mail && p.Password == password).First();
        }

        public Question GetRandomQuestion()
        {
            Random random = new Random();
            int questionNum = random.Next(1, Questions.Count() + 1);
            return this.Questions.Where(question => question.QuestionId == questionNum && question.StatusId == 0).First();
        }
        public Player SignUp(string email, string password, string name)
        {
            Player newPlayer = new Player();

            newPlayer.Name = name;
            newPlayer.Password = password;
            newPlayer.Rank = new Rank();
            newPlayer.Rank.RankName = "Rookie";
            this.Players.Add(newPlayer);
            this.SaveChanges();
            return newPlayer;
        }
    }
}

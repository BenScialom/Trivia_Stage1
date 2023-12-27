using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Trivia_Stage1.UI
{
    class CheckPendingQuestions:Screen
    {
        public CheckPendingQuestions():base("Show Pending Questions")
        {
            

        }

        public override void Show()
        {
            base.Show();
            Program.ui.Screens.ShowPendingQuestions();
            
        }
    }
}

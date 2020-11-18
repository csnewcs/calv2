using System;
using Gtk;

namespace Calv2
{
    partial class Program : Window
    {
        int userGrade = 0;
        string  userName = "";
        string gameDifficulty = "";
        static void Main(string[] args)
        {
            Application.Init();
            new Program();
            Application.Run();
        }

        void startClick(object o, EventArgs e)
        {
            Hide();
            userGrade = this.grade.Active + 1;
            userName = this.name.Text;

           if (easy.Active)
           {
               gameDifficulty = "easy";
               Application.Init();
               Easy es = new Easy();
               Application.Run();
           }
           else if (normal.Active)
           {
              gameDifficulty = "normal";
               Application.Init();
              Normal nm   = new Normal();
               Application.Run();
           }
           else if (hard.Active)
           {
                gameDifficulty = "hard";
               Application.Init();
               Hard hd = new Hard();
               Application.Run();
           }
           else if (extreme.Active)
           {
                 gameDifficulty = "extreme";
                 Application.Init();
                 Extreme ex = new Extreme();
                 Application.Run();
           }
            
        }
    }
}

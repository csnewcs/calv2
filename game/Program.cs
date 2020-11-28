using System;
using System.Threading.Tasks;
using Gtk;
using Newtonsoft.Json.Linq;

namespace Calv2
{
    partial class Program : Window
    {
        static void Main(string[] args)
        {
            Application.Init();
            new Program();
            Application.Run();
        }

        async void startClick(object o, EventArgs e)
        {
            if (name.Text == "")
            {
                name.StyleContext.AddClass("ErrorEntry");
                await  Task.Delay(400);
                name.StyleContext.RemoveClass("ErrorEntry");
                return;
            }
            JObject data = new JObject();
            data.Add("name", name.Text);
            data.Add("grade", this.grade.Active + 1);
            Hide();


           if (easy.Active)
           {
               data.Add("difficulty",  "easy");
               Console.Write("difficulty: easy, ");
           }
           else if (normal.Active)
           {
               data.Add("difficulty", "normal");
               Console.Write("difficulty: normal, ");
           }
           else if (hard.Active)
           {
               data.Add("difficulty", "hard");
               Console.Write("difficulty: hard, ");
           }
           else if (extreme.Active)
           {
               data.Add("difficulty", "extreme");
               Console.Write("difficulty: extreme, ");
           }

            Console.WriteLine("name: " + name.Text);
            
            Application.Init();
            Game gm = new Game(data);
            gm.DeleteEvent += windowClosed;
            Application.Run();
        }
   
        void windowClosed(object o , EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}

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
               Console.WriteLine("difficulty:  easy ");
               Application.Init();
               Easy es = new Easy(out data);
               es.DeleteEvent += windowClosed;
               Application.Run();
           }
           else if (normal.Active)
           {
              data.Add("difficulty",  "normal");
              Console.WriteLine("difficulty:  normal");
               Application.Init();
              Normal nm   = new Normal();
              nm.DeleteEvent += windowClosed;
               Application.Run();
           }
           else if (hard.Active)
           {
                data.Add("difficulty",  "hard");
                Console.WriteLine("difficulty:  hard ");
               Application.Init();
               Hard hd = new Hard();
               hd.DeleteEvent += windowClosed;
               Application.Run();
           }
           else if (extreme.Active)
           {
                 data.Add("difficulty",  "extreme");
                 Console.WriteLine("difficulty:  extreme ");
                 Application.Init();
                 Extreme ex = new Extreme();
                 ex.DeleteEvent += windowClosed;
                 Application.Run();
           }
            Console.Write("name: " + name.Text);
        }
   
        void windowClosed(object o , EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}

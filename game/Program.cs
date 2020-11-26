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

            Application.Init();
            Game gm = new Game(out data);
            gm.DeleteEvent += windowClosed;
            Application.Run();

           if (easy.Active)
           {
               data.Add("difficulty",  "easy");
               Console.WriteLine("difficulty:  easy ");
           }
            Console.Write("name: " + name.Text);
        }
   
        void windowClosed(object o , EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}

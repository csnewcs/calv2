using System;
using System.Threading.Tasks;
using Gtk;
using Newtonsoft.Json.Linq;
using System.Net;

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
   
        ProgressBar scoreProgressBar = new ProgressBar();
        void windowClosed(object o , EventArgs e)
        {
            Game gm = (Game)o;
            JObject jsonData = gm.jsonData;
            WebClient client = new WebClient();
            foreach (var a in main.Children) main.Remove(a); // main아래 있는 것 전부 제거
            string name = jsonData["name"].ToString();
            ulong score = (ulong)jsonData["score"];
            Label title = new Label($"{jsonData["name"].ToString()}님의 점수는 {jsonData["score"].ToString()}점 입니다.");
            RadioButton[] hideName = new RadioButton[] {
                new RadioButton($"이름 가리지 않기: {jsonData["name"].ToString()}"),
                new RadioButton($"2번째 글자만 가리기: {jsonData["name"].ToString().Remove(1, 1).Insert(1, "*")}"),
                new RadioButton("모두 가리기: ***")
            };
            scoreProgressBar.Orientation = Orientation.Vertical;
            scoreProgressBar.Valign = Align.Start;

            hideName[1].JoinGroup(hideName[0]);
            hideName[2].JoinGroup(hideName[0]);

            Button ok = new Button("확인");
            main.Attach(scoreProgressBar, 1, 1, 1, 4);
            main.Attach(title, 2, 1, 3, 1);
            main.Attach(hideName[0], 2, 2, 1, 1);
            main.Attach(hideName[1], 2, 3, 1, 1);
            main.Attach(hideName[2], 2, 4, 1, 1);
            main.Attach(ok, 3, 2, 1, 2);

            ShowAll();
            GC.Collect();
        }
        void animation()
        {

        }
    }
}

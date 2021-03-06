﻿using System;
using System.Threading.Tasks;
using Gtk;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Threading;

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
        JObject jsonData;
        string url;
        RadioButton[] hideName = new RadioButton[3] ;
        Label title;
        void windowClosed(object o , EventArgs e)
        {
            Game gm = (Game)o;
            jsonData = gm.jsonData;
            title = new Label($"{jsonData["name"].ToString()}님의 점수는 {jsonData["score"].ToString()}점 입니다.");
            url = gm.serverUrl;
            Remove(main);
            main = new Grid();
            main.RowSpacing = 15;
            main.ColumnSpacing = 15;
            main.RowHomogeneous = true;
            CssProvider cssp = new CssProvider();
            cssp.LoadFromPath("css/Program.css");
            StyleContext.AddProviderForScreen(Gdk.Screen.Default, cssp, 800);
            
            string name = jsonData["name"].ToString();
            ulong score = (ulong)jsonData["score"];
            try
            {
                hideName = new RadioButton[3] {
                    new RadioButton($"이름 가리지 않기: {jsonData["name"].ToString()}"),
                    new RadioButton($"2번째 글자만 가리기: {jsonData["name"].ToString().Remove(1, 1).Insert(1, "*")}"),
                    new RadioButton("모두 가리기: ***")
                };
                hideName[1].JoinGroup(hideName[0]);
                hideName[2].JoinGroup(hideName[0]);
            }
            catch
            {
                hideName = new RadioButton[2] {
                    new RadioButton($"이름 가리지 않기: {jsonData["name"].ToString()}"),
                    new RadioButton("모두 가리기: ***")
                };
                hideName[1].JoinGroup(hideName[0]);
            }
            

            scoreProgressBar.Name = "first";
            scoreProgressBar.Orientation = Orientation.Vertical;
            scoreProgressBar.Valign = Align.Start;
            scoreProgressBar.Inverted = true;


            Button ok = new Button("확인");
            ok.Clicked += upload;
            main.Attach(title, 2, 1, 3, 1);
            main.Attach(hideName[0], 2, 2, 1, 1);
            main.Attach(hideName[1], 2, 3, 1, 1);
            main.Attach(ok, 3, 2, 1, 2);
            main.Attach(scoreProgressBar, 1, 1, 1, 5);

            if (hideName.Length == 3) main.Attach(hideName[2], 2, 4, 1, 1);
            Add(main);
            new Thread(() => {animation(score,  (ulong)jsonData["firstScore"], jsonData["name"].ToString());}).Start(); 

            ShowAll();
            GC.Collect();
        }
        void animation(ulong score, ulong firstScore, string name)
        {
            Console.WriteLine("시작");
            int devide = 384;
            double sin = 1.57 / devide;
            
            double present = 0;
            int wait = 10;

            for (int i = 0; i < devide && present < firstScore; i++)
            {
                double persentage = Math.Sin(i * sin);
                Console.WriteLine(persentage);
                present = persentage * score;
                Application.Invoke(delegate {
                    scoreProgressBar.Fraction = present / firstScore;
                    title.Text = $"{name}님의 점수는 {(ulong)present}";
                });
                Thread.Sleep(wait);
            }
            Application.Invoke(delegate {
                    scoreProgressBar.Fraction = (double)score / (double)firstScore;
                    title.Text = $"{name}님의 점수는 {score}";
                });
        }
        async void upload(object o, EventArgs e)
        {
            if (hideName[0].Active) jsonData["hideName"] = 0;
            else if (hideName[1].Active) jsonData["hideName"] = 1;
            else if (hideName[2].Active) jsonData["hideName"] = 2;
            try
            {
                Console.WriteLine(jsonData.ToString());
                WebClient client = new WebClient();
                Console.WriteLine(jsonData.ToString());
                client.Headers.Add("Content-Type", "application/json");
                client.UploadString(url + "/api/add",  "PUT", jsonData.ToString());
                MessageDialog dialog = new MessageDialog(null, DialogFlags.DestroyWithParent, MessageType.Info, ButtonsType.Ok, false, "업로드에 성공했습니다. 10초 후 게임이 열립니다.");
                dialog.Run();
                dialog.Dispose();
                await Task.Delay(10000);
                
            }
            catch
            {
                MessageDialog dialog = new MessageDialog(null, DialogFlags.DestroyWithParent, MessageType.Error, ButtonsType.Cancel, false, "업로드를 실패했습니다.");
                dialog.Run();
                dialog.Dispose();
            }
        }
    }
}

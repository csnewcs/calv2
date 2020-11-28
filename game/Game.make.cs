using System;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Gtk;
using Newtonsoft.Json.Linq;

namespace Calv2
{
    partial class Game : Window
    { 
        Label countdownLabel = new Label("5초 후 시작");
        Label quastionLabel = new Label("문제가 나오는 레이블");
        Label levelLabel = new Label("레벨을 표시하는 레이블");
        Label nextScoreLabel = new Label("1");
        Label prevScroeLabel = new Label("0");
        
        Button[] buttons = new Button[1];
         int buttonCount = 0;
        ProgressBar timer = new ProgressBar();
        ProgressBar rank = new ProgressBar();
        Grid main = new Grid();
        
        public Game(JObject data) : base("사칙연산v2")
        {
            CssProvider cssp = new CssProvider();
            cssp.LoadFromPath("css/game.css");
            StyleContext.AddProviderForScreen(Gdk.Screen.Default, cssp, 800);
            SetDefaultSize(1280,720);
            main.ColumnHomogeneous = true;
            main.RowHomogeneous = true;
            this.data = data;

            main.Margin = 20;
            main.RowSpacing = 15;
            main.ColumnSpacing = 15;
            Add(main);
            countdownLabel.StyleContext.AddClass("BigLabel");
            // main.Add(countdownLabel);
            
            
            // new Thread(new ThreadStart(countdown)).Start();
            
            baseSetting(data["difficulty"].ToString());

            make();
            for (int i = 0; i < 5; i++)
            {
                Quation qt  = new Quation(maxTerm, maxNumber, bracketPersentage, buttonCount);
                Console.WriteLine(qt.quationString + " = " + qt.answer);
            }
        }

        private void make()
        {
               buttons = new Button[buttonCount];
               Grid buttonGrid = new Grid();
               Grid rankGrid = new Grid();
                buttonGrid.ColumnHomogeneous = true;
                // buttonGrid.RowHomogeneous = true;
                rank.Orientation = Orientation.Vertical;
               timer.StyleContext.AddClass("progressbar");
               rank.StyleContext.AddClass("progressbar");
               quastionLabel.StyleContext.AddClass("BigLabel");
               
               rank.Halign = Align.Center;

               for (int i = 0; i < buttons.Length; i++)
               {
                   buttons[i] = new Button("정답 버튼");
                   int devide = 60/buttonCount;
                   buttonGrid.Attach(buttons[i], i*devide + 1, 1, devide, 1);
               }
               buttonGrid.MarginBottom = 125;
               buttonGrid.RowHomogeneous = true;
               timer.Valign = Align.Start;

                rankGrid.Attach(nextScoreLabel, 1, 1, 1,1);
                rankGrid.Attach(rank, 1, 2, 1, 2);
                rankGrid.Attach(prevScroeLabel, 1, 4, 1, 1);
                rank.Halign = Align.Center;
                rankGrid.RowHomogeneous = true;
                rankGrid.ColumnHomogeneous = true;
                nextScoreLabel.Valign = Align.End;
                prevScroeLabel.Valign = Align.Start;

               main.Attach(levelLabel, 1, 1, 1, 1);
               levelLabel.MarginBottom = 20;
               main.Attach(rankGrid, 1, 2, 1, 3);
               main.Attach(quastionLabel, 2, 1, 3, 2);
               main.Attach(buttonGrid, 2, 3, 3, 2);
               main.Attach(timer, 5, 1, 1, 1);
               ShowAll();
        }
    }
}
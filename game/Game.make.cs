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
        Label questionLabel = new Label("5초 후 시작");
        Label levelLabel = new Label("");
        Label nextScoreLabel = new Label("");
        Label prevScroeLabel = new Label("");
        
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
            SetDefaultSize(1366, 768);
            main.ColumnHomogeneous = true;
            main.RowHomogeneous = true;
            this.data = data;

            main.Margin = 20;
            main.RowSpacing = 15;
            main.ColumnSpacing = 15;
            Add(main);
            // main.Add(countdownLabel);
            
            
            
            baseSetting(data["difficulty"].ToString());

            make();
        }

        private void make()
        {
               buttons = new Button[buttonCount];
               Grid buttonGrid = new Grid();
               Grid rankGrid = new Grid();
                buttonGrid.ColumnHomogeneous = true;
                // buttonGrid.RowHomogeneous = true;
                rank.Orientation = Orientation.Vertical;
                rank.ShowText = true;
               timer.StyleContext.AddClass("progressbar");
               rank.StyleContext.AddClass("progressbar");
               questionLabel.StyleContext.AddClass("BigLabel");
               timer.ShowText = true;
               
               rank.Halign = Align.Center;
                rank.MarginEnd= 30;

               for (int i = 0; i < buttons.Length; i++)
               {
                   buttons[i] = new Button();
                   buttons[i].StyleContext.AddClass("answerButton");
                   int devide = 60/buttonCount;
                   buttonGrid.Attach(buttons[i], i*devide + 1, 1, devide, 1);                   
               }
               buttonGrid.MarginBottom = 125;
               buttonGrid.RowHomogeneous = true;
               timer.Valign = Align.Start;
                levelLabel.Halign = Align.Start;

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
               main.Attach(questionLabel, 2, 1, 3, 2);
               main.Attach(buttonGrid, 2, 3, 3, 2);
               main.Attach(timer, 5, 1, 1, 1);
               ShowAll();

                new Thread(new ThreadStart(countdown)).Start();
               new Thread(new ThreadStart(game)).Start();
        }
    }
}
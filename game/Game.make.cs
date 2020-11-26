using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Gtk;
using Newtonsoft.Json.Linq;

namespace Calv2
{
    partial class Game : Window
    { 
        Label countdownLabel = new Label("5초 후 시작");
        Label quastionLabel = new Label();
        Button[] buttons = new Button[1];
         int buttonCount = 0;
        ProgressBar timer = new ProgressBar();
        ProgressBar rank = new ProgressBar();
        Grid main = new Grid();
        
        public Game(ref JObject data) : base("사칙연산v2")
        {
            CssProvider cssp = new CssProvider();
            cssp.LoadFromPath("css/game.css");
            StyleContext.AddProviderForScreen(Gdk.Screen.Default, cssp, 800);
            SetDefaultSize(1280,720);
            Add(main);
            countdown();            
            baseSetting(data["difficulty"].ToString());
            make();
            Quation qt  = new Quation(maxTerm, maxNumber, bracketPersentage, buttonCount);
            Console.WriteLine(qt.quationString + "   " + qt.answer);
            foreach(var a in qt.wrongAnswer) Console.WriteLine(a);
        }

        private void make()
        {
               buttons = new Button[buttonCount];
        }
    }
}
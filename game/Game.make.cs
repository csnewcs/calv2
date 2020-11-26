using System;
using System.Collections.Generic;
using Gtk;
using Newtonsoft.Json.Linq;

namespace Calv2
{
    partial class Game : Window
    { 
        Label countdownLabel = new Label("5초 후 시작");
        Dictionary<int, Button> buttons = new Dictionary<int, Button>();
        ProgressBar timer = new ProgressBar();
        ProgressBar rank = new ProgressBar();
        
        public Game(out JObject data) : base("쉬움")
        {
            CssProvider cssp = new CssProvider();
            cssp.LoadFromPath("css/game.css");
            StyleContext.AddProviderForScreen(Gdk.Screen.Default, cssp, 800);
            SetDefaultSize(1280,720);
            countdown();
            Remove(countdownLabel);

            data = new JObject();
        }
    }
}
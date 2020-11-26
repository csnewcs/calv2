using System;
using System.Threading.Tasks;
using Gtk;
using Newtonsoft.Json.Linq;

namespace Calv2
{
    partial class Easy : Window
    { 
        Label timer = new Label("5초 후 시작");
        public Easy(out JObject data) : base("쉬움")
        {
            CssProvider cssp = new CssProvider();
            cssp.LoadFromPath("css/game.css");
            StyleContext.AddProviderForScreen(Gdk.Screen.Default, cssp, 800);
            SetDefaultSize(1280,720);
            countdown();
            data = new JObject();
        }
    }
}
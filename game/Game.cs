using System;
using System.Threading.Tasks;
using Gtk;
using Newtonsoft.Json.Linq;

namespace Calv2
{
    partial class Game : Window
    { 
        async private void countdown()
        {
            countdownLabel.StyleContext.AddClass("BigLabel");
            Add(countdownLabel);
            ShowAll();
            for (int i = 4; i >= 0; i--)
            {
                await Task.Delay(1000);
                countdownLabel.Text = $"{i}초 후 시작";
            }
        }
    }
}
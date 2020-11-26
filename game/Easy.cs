using System;
using System.Threading.Tasks;
using Gtk;
using Newtonsoft.Json.Linq;

namespace Calv2
{
    partial class Easy : Window
    { 
        async private void countdown()
        {
            timer.StyleContext.AddClass("BigLabel");
            Add(timer);
            ShowAll();
            for (int i = 4; i >= 0; i--)
            {
                await Task.Delay(1000);
                timer.Text = $"{i}초 후 시작";
            }
        }
    }
}
using System;
using Gtk;

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
    }
}

using System;
using System.Threading.Tasks;
using Gtk;

namespace Calv2
{
    partial class Program : Window
    {
        public Program() : base("사칙연산 V2")
        {
            DeleteEvent += delegate {
                Environment.Exit(0);
            };
                start();
        }

        RadioButton easy = new RadioButton("쉬움");
        RadioButton normal = new RadioButton("보통");
        RadioButton hard = new RadioButton("어려움");
        RadioButton extreme = new RadioButton("극한");
        ComboBox grade = new ComboBox(new string[] {"1학년", "2학년", "3학년"});
        Entry name = new Entry();
        Grid main = new Grid();
        
        private void start()
        {
            Resizable = false;

            CssProvider cssp = new CssProvider();
            cssp.LoadFromData(System.IO.File.ReadAllText("css/Program.css"));
            StyleContext.AddProviderForScreen(Gdk.Screen.Default, cssp, 800);
            Console.WriteLine("프로그램 실행");

             main.Margin = 10;
             main.RowSpacing = 15;
             main.ColumnSpacing = 15;
             main.RowHomogeneous = true;
             main.ColumnHomogeneous = true;

             Label difficulty = new Label("난이도를 선택하세요");
             difficulty.StyleContext.AddClass("Notice");

            easy.Halign = Align.Center;            
            normal.Halign = Align.Center;            
            hard.Halign = Align.Center;            
            extreme.Halign = Align.Center;

            normal.JoinGroup(easy); hard.JoinGroup(easy); extreme.JoinGroup(easy);

            main.Attach(difficulty, 1, 1, 1, 1);
            main.Attach(easy, 1, 2, 1, 1);
            main.Attach(normal, 1, 3, 1, 1);
            main.Attach(hard, 1, 4, 1, 1);
            main.Attach(extreme, 1, 5, 1, 1);

            Separator sep = new Separator(Orientation.Vertical);
            sep.Halign = Align.Center;
            main.Attach(sep, 1, 1, 2, 5);

            grade.Active = 0;
            Button start = new Button("시작");
            name.PlaceholderText = "이름을 입력하세요";
            main.Attach(new Label("정보를 입력하세요"), 2, 1, 1, 1);
            start.Clicked += startClick;
            main.Attach(grade, 2, 2, 1, 1);
            main.Attach(name, 2, 3, 1, 1);
            main.Attach(start, 2, 4, 1, 2);
            Add(main);
            ShowAll();
        }
    }
}
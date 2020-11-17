using System;
using System.Threading.Tasks;
using Gtk;

namespace Calv2
{
    partial class Program : Window
    {
        public Program() : base("사칙연산 V2")
        {
             Grid main = new Grid();

             Label difficulty = new Label("난이도를 선택하세요");
             difficulty.StyleContext.AddClass("Notice");

            RadioButton easy = new RadioButton("쉬움");
            RadioButton normal = new RadioButton("보통");
            RadioButton hard = new RadioButton("어려움");
            RadioButton extreme = new RadioButton("극한");
            normal.JoinGroup(easy); hard.JoinGroup(easy); extreme.JoinGroup(easy);

            main.Attach(difficulty, 1, 1, 1, 1);
            main.Attach(easy, 1, 2, 1, 1);
            main.Attach(normal, 1, 3, 1, 1);
            main.Attach(hard, 1, 4, 1, 1);
            main.Attach(extreme, 1, 5, 1, 1);

            main.Attach(new Separator(Orientation.Vertical), 2, 1, 1, 5);

            ComboBox grade = new ComboBox(new string[] {"1학년", "2학년", "3학년"});
            Entry name = new Entry();
            Button start = new Button("시작");
            name.PlaceholderText = "이름을 입력하세요";
            main.Attach(grade, 3, 2, 1, 1);
            main.Attach(name, 3, 3, 1, 1);
            main.Attach(start, 3, 4, 1, 2);
            Add(main);
            ShowAll();
        }
    }
}
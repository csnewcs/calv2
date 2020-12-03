using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using System.Linq.Expressions;
using System.Data;
using Gtk;
using Newtonsoft.Json.Linq;

namespace Calv2
{
    partial class Game : Window
    { 
        string enhe;
        bool doing = false; //게임을 하고 있다, 그렇지 않다
        int maxNumber = 0;
        ulong score = 0; //현재 점수
        uint scoreUnit = 0; //문제 하나당 점수
        uint plusScoreUnit = 0; //단계당 추가되는 점수
        int bracketPersentage = 0; //괄호가 들어가는 확률
        int maxTerm = 0; // 최대 계산 수
        decimal maxTime = 0;
        decimal timeleft = 0; //남은 시간
        decimal minusTime = 0; //단계당 감소하는 제한시간
        int nextScore = 0;
        int prevScore = 0;

        int number = 1;
        int level = 1;
        int life = 5;
        bool succeed = false;
        
        JObject data = new JObject();
        public JObject jsonData
        {
            get
            {
                return data;
            }
        }

        private void countdown()
        {   
            for (int i = 4; i >= 0; )
            {
                Thread.Sleep(1000);
                Application.Invoke(delegate {questionLabel.Text = $"{i}초 후 시작";});
                i--;
            }
            doing = true;
        }

        private void baseSetting(string difficulty)
        { 
            // 버튼의 수, 문제당 점수, 단계별 추가 점수, 단계별 줄어드는 시간, 괄호 나올 확률, 최대 혼합연산 수
            switch (difficulty)
            {
                case "easy" :
                    buttonCount = 3;
                    scoreUnit = 1;
                    plusScoreUnit = 1;
                    minusTime = 1.7m;
                    bracketPersentage = 0;
                    maxTerm = 3;
                    maxNumber = 11;
                    maxTime = 10;
                    enhe = "쉬움";
                    break;
                case "normal" :
                    buttonCount = 3;
                    scoreUnit = 3;
                    plusScoreUnit = 3;
                    minusTime = 1.8m;
                    bracketPersentage = 5;
                    maxTerm = 4;
                    maxNumber = 21;
                    maxTime = 10;
                    enhe = "보통";
                    break;
                case "hard":
                    buttonCount = 4;
                    scoreUnit = 10;
                    plusScoreUnit = 10;
                    minusTime = 1.4m;
                    bracketPersentage = 15;
                    maxTerm = 5;
                    maxNumber = 51;
                    maxTime = 8;
                    enhe = "어려움";
                    break;
                case "extreme":
                    buttonCount = 5;
                    scoreUnit = 30;
                    plusScoreUnit = 34;
                    minusTime = 1.5m;
                    bracketPersentage = 30;
                    maxTerm = 6;
                    maxNumber = 101;
                    maxTime = 8;
                    enhe = "극한";
                    break;
            }
        }
        struct question
        {
            public string questionString ;
            public double answer;
            public List<double> wrongAnswer;
            public question(int maxTerm, int maxNumber, int bracketPersentage, int buttonCount)
            {
                //초기화
                wrongAnswer = new List<double>();
                questionString = "";
                answer = 0;

                DataTable dt = new DataTable();
                Random rd = new Random(); 

                for (int i = 0; i < buttonCount - 1; i++) wrongAnswer.Add(0);
                
                bool opened = false;
                bool looped = false;
                int term = rd.Next(2, maxTerm);
                for(int i = 0; i < term; i++)
                {
                    questionString += rd.Next(1, maxNumber);
                    if (opened)
                    {
                        if (looped)
                        {
                            questionString += ")";
                            opened = false;
                        }
                        else looped = true;
                    }
                    if (i == term - 1) continue; //마지막에 괄호가 열리거나 부호가 찍히는거 방지
                    switch (rd.Next(0, 4))
                    {
                        //0: 덧셈, 1: 뺄셈, 2: 곱셈, 3: 나눗셈
                        case 0:
                            questionString += " + ";
                            break;
                        case 1:
                            questionString += " - ";
                            break;
                        case 2:
                            questionString += " * ";
                            break;
                        case 3:
                            questionString += " / ";
                            break;
                    }
                    if (rd.Next(0, 101) <= bracketPersentage && term != 2 && !opened && i != term-2)
                    {
                        questionString += "(";
                        opened = true;
                    }
                }
                object compute = dt.Compute(questionString, null) ;
                if (compute.GetType() == typeof(int)) answer = (double)((int)compute) ; //정답 완성
                else
                {
                    answer = (double)compute;
                    answer = Math.Round(answer, 2);
                } 
                Console.WriteLine(questionString + " = " + answer);

                for (int i = 0; i < buttonCount - 1; i++)
                {
                    try
                    {
                        double wrong = answer;
                        int plusminus = rd.Next(-1 * Math.Abs((int)answer),  Math.Abs((int)answer));
                        if (questionString.Contains('-')) wrong += rd.Next(-1 * maxNumber,  maxNumber);
                        else wrong += rd.Next(0, maxNumber);                   

                        if (questionString.Contains('/'))
                        {
                            wrong += (double)(rd.Next(-1000, 1000)) / 1000;
                            wrong = Math.Round(wrong, 2);
                        }
                        if (answer == wrong || wrongAnswer.Contains(wrong)) i--;
                        else wrongAnswer[i] = wrong;
                    }
                    catch 
                    {
                        i--;
                    }
                }
                questionString = questionString.Replace('*', '×');
                questionString = questionString.Replace('/', '÷');
            }
        }
        
        private void game()
        {
            while (!doing) {Thread.Sleep(10);} // 카운트다운 끝날 때 까지 기다림
            JArray questionData = new JArray();
            Random rd = new Random();
            question question = new question(maxTerm, maxNumber, bracketPersentage, buttonCount);
            double[] answers = new double[buttonCount];
            JObject oneQuestion = new JObject();
            // int i = 0;
            levelLabel.Text = enhe + " - 1" ;
            
            Application.Invoke(delegate {
                    for (int i = 0; i < buttonCount; i++) //버튼에 넣기
                    {
                        buttons[i].Clicked  += (o, e) => {
                            Button copy = (Button)o;
                            buttonClick(question.answer, double.Parse(copy.Label), ref oneQuestion);
                        };
                    }
            });

            while(doing)
            {
                timeleft = maxTime;
                question = new question(maxTerm, maxNumber, bracketPersentage, buttonCount);
                oneQuestion.RemoveAll();
                oneQuestion.Add("question", question.questionString);
                oneQuestion.Add("answer", question.answer);
                answers = new double[buttonCount];
                answers[0] = question.answer;
                int looped = 0;
                foreach (double incorrect in question.wrongAnswer) //보기 만들기
                {
                    answers[looped + 1] = incorrect;
                    looped++;
                }
                for (int i = 0; i < answers.Length; i++) //보기들 섞기
                {
                    int swap = rd.Next(i, answers.Length);
                    var temp = answers[i];
                    answers[i] = answers[swap];
                    answers[swap] = temp;
                }
                Application.Invoke(delegate {
                    for (int i = 0; i < buttonCount; i++) //버튼에 넣기
                    {
                        buttons[i].Label = answers[i].ToString();
                    }
                    questionLabel.Text = question.questionString;
                });
                while(timeleft > 0) 
                {
                    timeleft -= 0.01m;
                    Application.Invoke(delegate {
                        timer.Fraction = (double)(timeleft / maxTime);
                        timer.Text = timeleft.ToString() ;
                    });
                    Thread.Sleep(10);
                }
                number++;
                questionData.Add(oneQuestion);
                if (number == 4 || number == 10 || number == 19 || number == 31 || number == 46)
                {
                    level++;
                    scoreUnit += plusScoreUnit;
                    maxTime -= minusTime;
                    Application.Invoke(delegate {levelLabel.Text = enhe + " - " + level.ToString();});
                }
                rank.Text = score.ToString();
                if (!succeed) minusHeart();
                if (life == 0)
                {
                    data["score"] = score;
                    Application.Invoke(delegate {Close();});
                }
                succeed = false;
            }
        }
        private void buttonClick(double correct,double selected, ref JObject oneQuestion)
        {
            oneQuestion.Add("selected", selected);
            if (correct == selected)
            {
                oneQuestion.Add("correct", true);
                score += scoreUnit;
                succeed = true;
            }
            else
            {
                oneQuestion.Add("correct", false);
            }
            timeleft = 0m;
        }
        int lastWidth = 0, lastHeight = 0;
        private void resize(object o, EventArgs e)
        {
            int width, height;
            GetSize(out width, out height);
            if (lastWidth == width && lastHeight == height) return;
            lastWidth = width; lastHeight = height;
            int imageheight = height/5;
            Console.WriteLine("imageheight = " + imageheight);
            for (int i = 0; i < 5; i++)
            {
                Gdk.Pixbuf pixbuf = images[i].Pixbuf;
                pixbuf = pixbuf.ScaleSimple(imageheight, imageheight, Gdk.InterpType.Bilinear);
                images[i].Pixbuf = pixbuf;
            }
        }
        private void minusHeart()
        {
            life--;
            Console.WriteLine(life);
            int width, height;
            GetSize(out width, out height);
            int imageheight = height/5;
            Application.Invoke(delegate {
                Gdk.Pixbuf pixbuf = new Gdk.Pixbuf("heart-broken.png");
                images[life].Pixbuf = pixbuf.ScaleSimple(imageheight, imageheight, Gdk.InterpType.Bilinear);
            });
        }
    }
}
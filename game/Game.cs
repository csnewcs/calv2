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
        bool doing = false; //게임을 하고 있다, 그렇지 않다
        int maxNumber = 0;
        ulong score = 0; //현재 점수
        int scoreUnit = 0; //문제 하나당 점수
        int plusScoreUnit = 0; //단계당 추가되는 점수
        int bracketPersentage = 0; //괄호가 들어가는 확률
        int maxTerm = 0; // 최대 계산 수
        decimal timeleft = 0; //남은 시간
        decimal minusTime = 0; //단계당 감소하는 제한시간

        int[] levelup = new int[] {3, 9, 18, 30, 45, 63};
        int number = 1;
        
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
                Application.Invoke(delegate {countdownLabel.Text = $"{i}초 후 시작";});
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
                    break;
                case "normal" :
                    buttonCount = 3;
                    scoreUnit = 3;
                    plusScoreUnit = 3;
                    minusTime = 1.8m;
                    bracketPersentage = 5;
                    maxTerm = 4;
                    maxNumber = 21;
                    break;
                case "hard":
                    buttonCount = 4;
                    scoreUnit = 10;
                    plusScoreUnit = 10;
                    minusTime = 1.4m;
                    bracketPersentage = 15;
                    maxTerm = 5;
                    maxNumber = 51;
                    break;
                case "extreme":
                    buttonCount = 5;
                    scoreUnit = 30;
                    plusScoreUnit = 34;
                    minusTime = 1.5m;
                    bracketPersentage = 30;
                    maxTerm = 6;
                    maxNumber = 101;
                    break;
            }
        }
        struct Quation
        {
            public string quationString ;
            public double answer;
            public List<double> wrongAnswer;
            public Quation(int maxTerm, int maxNumber, int bracketPersentage, int buttonCount)
            {
                //초기화
                wrongAnswer = new List<double>();
                quationString = "";
                answer = 0;

                DataTable dt = new DataTable();
                Random rd = new Random(); 

                for (int i = 0; i < buttonCount - 1; i++) wrongAnswer.Add(0);
                
                bool opened = false;
                bool looped = false;
                int term = rd.Next(2, maxTerm);
                for(int i = 0; i < term; i++)
                {
                    quationString += rd.Next(1, maxNumber);
                    if (opened)
                    {
                        if (looped)
                        {
                            quationString += ")";
                            opened = false;
                        }
                        else looped = true;
                    }
                    if (i == term - 1) continue; //마지막에 괄호가 열리거나 부호가 찍히는거 방지
                    switch (rd.Next(0, 4))
                    {
                        //0: 덧셈, 1: 뺄셈, 2: 곱셈, 3: 나눗셈
                        case 0:
                            quationString += " + ";
                            break;
                        case 1:
                            quationString += " - ";
                            break;
                        case 2:
                            quationString += " * ";
                            break;
                        case 3:
                            quationString += " / ";
                            break;
                    }
                    if (rd.Next(0, 101) <= bracketPersentage && term != 2 && !opened && i != term-2)
                    {
                        quationString += "(";
                        opened = true;
                    }
                }
                object compute = dt.Compute(quationString, null) ;
                if (compute.GetType() == typeof(int)) answer = (double)((int)compute) ; //정답 완성
                else
                {
                    answer = (double)compute;
                    answer = Math.Round(answer, 2);
                } 

                for (int i = 0; i < buttonCount - 1; i++)
                {
                    try
                    {
                        double wrong = answer;
                        int plusminus = rd.Next(-1 * Math.Abs((int)answer),  Math.Abs((int)answer));
                        if (quationString.Contains('-')) wrong += rd.Next(-1 * maxNumber,  maxNumber);
                        else wrong += rd.Next(0, maxNumber);                   

                        if (quationString.Contains('/'))
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
            }
        }
    }
}
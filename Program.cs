using ProjetoDiegoSla.Class;
using System;

namespace ProjetoDiegoSla
{
    class Program
    {

        private static DateTime _toDay;
        static void Main(string[] args)
        {
            People people = new People();

            people.name = "Cintia";
            
            DateTime toDay = new DateTime(2022, 03, 23, 08, 20, 00);
            _toDay = toDay;
            TimeSpan taskTime = TimeSpan.FromHours(5);
            
            string fisrtWorkTimeStart = "08:00";
            string fisrtWorkTimeEnd = "11:00";

            string secondWorkTimeStart = "12:00";
            string secondWorkTimeEnd = "14:00";

            string thirdtWorkTimeStart = "16:00";
            string thirdWorkTimeEnd = "18:00";

            string[] fistTurn = { fisrtWorkTimeStart , fisrtWorkTimeEnd };
            string[] secondTurn = { secondWorkTimeStart, secondWorkTimeEnd };
            string[] thirdTurn = { thirdtWorkTimeStart, thirdWorkTimeEnd };

            string[][] workTunrs = { fistTurn, secondTurn, thirdTurn };

            CalculaSla(workTunrs, toDay, taskTime);

            Console.ReadKey();

        }

        public static DateTime CalculaSla(string[][] workTunrs,DateTime toDay,TimeSpan taskTime)
        {
            int whatIsTurn = WhatIsTurn(workTunrs, toDay);
            int whatIsNext = WhatIsNext(workTunrs, toDay);

            TimeSpan contador = new TimeSpan();
            TimeSpan timeLeft = new TimeSpan();
            TimeSpan duracaoDeTrabalho = CalcTurnDay(workTunrs, toDay);

            TimeSpan taskCallTotal = taskTime - timeLeft;


            if (whatIsTurn >= 0)
            {
                timeLeft = TimeLeft(workTunrs[whatIsTurn],toDay);
            }

            //Caso o tempo restante seja maior do tempo do chamado precisa de uma tratativa especial
            if(taskTime >= timeLeft)  {
            }

            int inicio = whatIsNext;
            
            while (contador < taskCallTotal)
            {
                for (int i = 0; i < workTunrs.Length; i++)
                {
                    if (inicio == 0)
                    {
                        i = inicio;
                        contador += CalculaDuracaoDoTurno(workTunrs[i]);

                    }
                    else
                    {
                        contador += CalculaDuracaoDoTurno(workTunrs[i]);
                    }


                    if (contador >= taskCallTotal)
                    {
                       
                            var restante = contador - taskCallTotal;

                            string[] timeStart = workTunrs[i][1].Split(":");

                            int hourTurnStart = int.Parse(timeStart[0]);
                            int minuteTurnStart = int.Parse(timeStart[1]);

                            DateTime timeTurnStart = new DateTime(toDay.Year,
                                           toDay.Month,
                                           toDay.Day,
                                           hourTurnStart,
                                           minuteTurnStart,
                                           00);

                            var teste = timeTurnStart - restante;

                            Console.WriteLine($"Chamado deve ser finalizado {toDay.Day} as {teste}");

                        break;
                    }
                }

                //inicio = -1;
                //toDay.AddDays(1);
            }
            
            return new DateTime();
        }

        public static int WhatIsNext(string[][] workTunrs, DateTime toDay)
        {
            for(int i = 0; i < workTunrs.Length; i++)
            {
                DateTime[] turn = ConvertDateTime(workTunrs[i], toDay);

                if(toDay <= turn[0])
                {
                    return i;
                }
            }
            return 0;
        }

        public static TimeSpan CalcTurnDay(string[][] workTunrs,DateTime toDay)
        {
            TimeSpan span = new TimeSpan();

            for (int i = 0; workTunrs.Length > i; i++)
            {
                span += CalculaDuracaoDoTurno(workTunrs[i]);              
            }

            return span;
        }

        public static TimeSpan CalculaDuracaoDoTurno(string[] turno)
        {
            DateTime[] turn = ConvertDateTime(turno, _toDay);

           return turn[1] - turn[0];
        }

        public static TimeSpan TimeLeft(string[] turn, DateTime toDay)
        {
            DateTime[] turnos = ConvertDateTime(turn, toDay);

            TimeSpan timeLeft = turnos[1] - toDay;

            return timeLeft;
        }

        public static int WhatIsTurn(string[][] workTunrs, DateTime toDay)
        {
            for(int i = 0; workTunrs.Length > i; i++)
            {
               if(isYourTurn(workTunrs[i], toDay))
                {
                    return i;
                }
            }            

            return -1;
        }

        public static bool isYourTurn(string[] turn, DateTime toDay)
        {

          DateTime[] turnos = ConvertDateTime(turn,toDay);

            if (toDay >= turnos[0] & toDay <= turnos[1])
            return true;

            return false;
        }
        
        public static DateTime[] ConvertDateTime(string[] turn, DateTime toDay)
        {
            string[] timeStart = turn[0].Split(":");

            int hourTurnStart = int.Parse(timeStart[0]);
            int minuteTurnStart = int.Parse(timeStart[1]);

            DateTime timeTurnStart = new DateTime(toDay.Year,
                                             toDay.Month,
                                             toDay.Day,
                                             hourTurnStart,
                                             minuteTurnStart,
                                             00);

            string[] timeEnd = turn[1].Split(":");

            int hourTurnEnd = int.Parse(timeEnd[0]);
            int minuteTurnEnd = int.Parse(timeEnd[1]);

            DateTime timeTurnEnd = new DateTime(toDay.Year,
                                             toDay.Month,
                                             toDay.Day,
                                             hourTurnEnd,
                                             minuteTurnEnd,
                                             00);

            return new DateTime[] { timeTurnStart, timeTurnEnd };
            
        }
    }
}

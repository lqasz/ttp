﻿using BiPA.Struktura.Algorytmy.Abstrakcyjny;
using BiPA.Struktura.Algorytmy.Abstrakcyjny.Osobnik;
using BiPA.Struktura.Algorytmy.Losowy.Losowanie;
using BiPA.Struktura.ProblemyOptymalizacyjne.Abstrakcyjny;
using System.Collections.Generic;

namespace BiPA.Struktura.Algorytmy.Wspinaczkowy.Wspinaczka
{
    /// <summary>
    /// Klasa konkretna reprezentująca rozwiązanie dla Algorytmu Wspinaczkowego pod Problem Podróżującego Złodzieja
    /// </summary>
    class WspinaczkaTTP : AWspinaczka
    {
        public WspinaczkaTTP(ALosowanie losowanie, int parametrP) : base(losowanie, parametrP) { }

        public override void ZnajdzOptimum()
        {
            int poprawy = 0,
                marginesBledu = parametrP;

            ushort[][] genotyp = reprezentacjaRozwiazania.ZwrocGenotyp2Wymiarowy(),
                       tmpGenotyp = (ushort[][])genotyp.Clone();

            string stan = "kp";
            AOsobnik osobnik = losowanie.ZwrocOsobnika();
            ProblemOptymalizacyjny problemOptymalizacyjny = osobnik.ZwrocInstancjeProblemu();
            Dictionary<string, float[]> wynik = osobnik.FunkcjaDopasowania(reprezentacjaRozwiazania);
            ReprezentacjaRozwiazania tmpReprezentacjaRozwiazanie = new ReprezentacjaRozwiazania(tmpGenotyp);
            ushort[][] dostepnePrzedmioty = (ushort[][])problemOptymalizacyjny.ZwrocDostepnePrzedmioty().Clone();

            Dictionary<string, float[]> tmpWynik = wynik;
            double wspolczynnik = wynik["min"][0] / problemOptymalizacyjny.ZwrocOgraniczeniaProblemu()[0];

            do
            {
                poprawy = 0;

                if(stan == "kp")
                {
                    // problem plecakowy
                    for (int i = 1; i < tmpGenotyp.Length - 1; i++)
                    {
                        for (int j = 1; j < tmpGenotyp[i].Length; j++)
                        {
                            if (dostepnePrzedmioty[i][j - 1] == 1)
                            {
                                if (wspolczynnik < 1)
                                {
                                    tmpGenotyp[i][j] = 1;
                                }
                                else
                                {
                                    tmpGenotyp[i][j] = 0;
                                }

                                tmpReprezentacjaRozwiazanie.ZmienGenotyp(tmpGenotyp);
                                tmpWynik = osobnik.FunkcjaDopasowania(tmpReprezentacjaRozwiazanie);
                                wspolczynnik = tmpWynik["min"][0] / problemOptymalizacyjny.ZwrocOgraniczeniaProblemu()[0];

                                if(wspolczynnik > 1) tmpGenotyp[i][j] = 0;
                            }
                        }
                    }

                    stan = "tsp";
                }

                if (stan == "tsp")
                {
                    // problem komiwojażera
                    for (int i = 1; i < tmpGenotyp.Length - 1; i++)
                    {
                        int los = losowy.Next(2, tmpGenotyp.Length - 2);

                        ushort[] tmp = (ushort[])tmpGenotyp[i].Clone();
                        tmpGenotyp[i] = (ushort[])tmpGenotyp[los].Clone();
                        tmpGenotyp[los] = (ushort[])tmp.Clone();
                    }

                    tmpReprezentacjaRozwiazanie.ZmienGenotyp(tmpGenotyp);
                    tmpWynik = osobnik.FunkcjaDopasowania(tmpReprezentacjaRozwiazanie);

                    stan = "kp";
                }

                if (tmpWynik["min"][0] > problemOptymalizacyjny.ZwrocOgraniczeniaProblemu()[0])
                {
                    tmpGenotyp = (ushort[][])genotyp.Clone();

                    if(marginesBledu > 0)
                    {
                        poprawy++;
                        marginesBledu--;
                    }
                }
                else
                {
                    if (tmpWynik["max"][0] > wynik["max"][0])
                    {
                        wynik = tmpWynik;
                        genotyp = (ushort[][])tmpGenotyp.Clone();
                        reprezentacjaRozwiazania.ZmienGenotyp(genotyp);
                        poprawy++;
                        marginesBledu = 50;
                    } else
                    {
                        if (marginesBledu > 0)
                        {
                            poprawy++;
                            marginesBledu--;
                        }
                    }
                }
            } while (poprawy > 0);
        }
    }
}

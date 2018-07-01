﻿using AlgorytmyDoTTP.Struktura.Algorytmy.Abstrakcyjny;
using AlgorytmyDoTTP.Struktura.Algorytmy.Abstrakcyjny.Analityka;
using AlgorytmyDoTTP.Struktura.Algorytmy.Ewolucyjny.Rekombinacja;
using AlgorytmyDoTTP.Struktura.Algorytmy.Ewolucyjny.Selekcja;
using System;
using System.Collections.Generic;

namespace AlgorytmyDoTTP.Struktura.Algorytmy.Ewolucyjny
{
    /// <summary>
    /// Klasa konkretna Algorytmu Ewolucyjnego.
    /// Pozwala na wyszukanie rozwiązania optymalnego danego problemu wg zasad działania Algorytmów Ewolucyjnych
    /// </summary>
    class SEA : IAlgorytm
    {
        private short iloscPokolen; // ilość iteracji algorytmu
        private double pwoKrzyzowania; // prawdopodobieństwo, że zostanie stworzony nowy osobnik
        private ARekombinacja rekombinacja; // klasa odpowiedzialna za tworzenie nowych osobników
        private ASelekcja selekcja; // klasa odpowiedzialna za wybieranie najlepszych osobników do krzyżowania
        private AnalizaEwolucyjny analityka; // klasa odpowiedzialna za analizę rozwiązań
        private ReprezentacjaRozwiazania[] populacjaBazowa; // klasa zarządzająca populcją osobników

        public SEA()
        {
            throw new Exception(); // błąd, nie zbudowano kontekstu pod wybrany problem optymalizacyjny
        }

        public SEA(ASelekcja selekcja, ARekombinacja rekombinacja, AnalizaEwolucyjny analityka, ReprezentacjaRozwiazania[] populacjaBazowa, short iloscPokolen, double pwoKrzyzowania)
        {
            this.selekcja = selekcja;
            this.rekombinacja = rekombinacja;
            this.analityka = analityka;
            this.iloscPokolen = iloscPokolen;
            this.populacjaBazowa = populacjaBazowa;
            this.pwoKrzyzowania = pwoKrzyzowania;
        }
        
        public Dictionary<string, string[]> Start()
        {
            int iloscOsobnikowPopulacji = (int)(populacjaBazowa.Length * 2 * pwoKrzyzowania);
            ReprezentacjaRozwiazania[] nowaPopulacja = new ReprezentacjaRozwiazania[iloscOsobnikowPopulacji];
            Dictionary<string, string[]> zwracanyTekst = new Dictionary<string, string[]>();
            
            analityka.RozpocznijPomiarCzasu(); // rozpoczęcie pomiaru czasu
            // iterując przez wszystkie pokolenia
            while (iloscPokolen >= 0)
            {
                // wczytujemy pewną ilość osobników z populacji
                for (int i = 0; i < iloscOsobnikowPopulacji; i+=2)
                {
                    // zależną od prawdopodobieństwa kzyżowania
                    // i przeprowadzamy operację tworzenia nowych osobników, pobierając rodziców z populacji
                    ReprezentacjaRozwiazania mama = selekcja.WybierzOsobnika(populacjaBazowa, iloscPokolen),
                                            tata = selekcja.WybierzOsobnika(populacjaBazowa, iloscPokolen),
                                            dziecko1 = rekombinacja.Krzyzowanie(mama, tata), // tworząc 1 dziecko
                                            dziecko2 = rekombinacja.Krzyzowanie(tata, mama); // oraz 2 dziecko
                        
                    // dzieci dodajemy do nowej populacji
                    nowaPopulacja[i] = dziecko1;
                    nowaPopulacja[i+1] = dziecko2;

                    // sprawdzając czy nie stworzyliśmy najlepszego rozwiązania do tej pory
                    analityka.ZmienWartoscNiebo(dziecko1);
                    analityka.ZmienWartoscNiebo(dziecko2);
                }

                // wymieniamy starą populację na nową populację
                populacjaBazowa = (ReprezentacjaRozwiazania[])nowaPopulacja.Clone();
                
                --iloscPokolen; // zmiejszając ilość pokoleń
            }

            analityka.ZakonczPomiarCzasu(); // zakończenie pomiaru czasu

            // pobieramy wartości statystyczne / analityczne z naszego badania
            double czasDzialania = analityka.ZwrocCzasDzialaniaAlgorytmu(),
                   srednia = analityka.SredniaPopulacji(populacjaBazowa),
                   mediana = analityka.MedianaPopulacji(populacjaBazowa),
                   odchylenieStadowe = analityka.OdchylenieStandardowePopulacji(populacjaBazowa, srednia);

            ReprezentacjaRozwiazania wartoscNiebo = analityka.ZwrocNajlepszyGenotyp();

            // zwracamy raport z badań w formie czytelnej dla człowieka
            zwracanyTekst["dziedzina"] = new string[] { "Najlepszy genotyp", analityka.ZwrocNajlepszeRowziazanie() };
            zwracanyTekst["maxWartosc"] = new string[] { "Najlepsza funkcja przystosowania (max)", analityka.ZwrocWartoscNiebo()["max"][0].ToString() };
            zwracanyTekst["minWartosc"] = new string[] { "Najlepsza funkcja przystosowania (min)", analityka.ZwrocWartoscNiebo()["min"][0].ToString() };
            zwracanyTekst["sredniaWartosc"] = new string[] { "Średnia funkcji przystosowania z populacji", srednia.ToString() };
            zwracanyTekst["medianaWartosci"] = new string[] { "Mediana funkcji przystosowania z populacji", mediana.ToString() };
            zwracanyTekst["odchstdWartosci"] = new string[] { "Odchylenie standardowe funkcji przystosowania z populacji", odchylenieStadowe.ToString() };
            zwracanyTekst["czasDzialania"] = new string[] { "Czas dzialania algorytmu", czasDzialania.ToString() };

            return zwracanyTekst;
        }
    }
}
﻿using AlgorytmyDoTTP.Struktura.ProblemyOptymalizacyjne.TSP;
using System;
using System.Collections;
using System.Collections.Generic;
using AlgorytmyDoTTP.Struktura.ProblemyOptymalizacyjne.Abstrakcyjny;

namespace AlgorytmyDoTTP.Struktura.Algorytmy.Ewolucyjny.Osobnik
{
    class OsobnikTSP : AOsobnik
    {
        public OsobnikTSP(ProblemOptymalizacyjny problemOptymalizacyjny) : base(problemOptymalizacyjny)
        {
        }

        /// <summary>
        /// Metoda zwraca wektor osobnika opisanego genotypem
        /// </summary>
        /// <param name="genotyp">Tablica definiująca dziedzinę rozwiązania</param>
        /// <returns>Dziedzinę wartości funkcji celu</returns>
        public override ArrayList Fenotyp(ushort[] genotyp)
        {
            ArrayList wynik = new ArrayList();
            wynik.AddRange(genotyp);

            return wynik;
        }
    }
}

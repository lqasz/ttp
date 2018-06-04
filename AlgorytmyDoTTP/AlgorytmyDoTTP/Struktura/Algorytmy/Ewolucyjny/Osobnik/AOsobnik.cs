﻿using AlgorytmyDoTTP.Struktura.Algorytmy.Ewolucyjny.Genotyp;
using AlgorytmyDoTTP.Struktura.ProblemyOptymalizacyjne.Abstrakcyjny;
using System;
using System.Collections;
using System.Collections.Generic;

namespace AlgorytmyDoTTP.Struktura.Algorytmy.Ewolucyjny.Osobnik
{
    abstract class AOsobnik
    {
        protected ProblemOptymalizacyjny problemOptymalizacyjny;

        public AOsobnik(ProblemOptymalizacyjny problemOptymalizacyjny)
        {
            this.problemOptymalizacyjny = problemOptymalizacyjny;
        }

        /// <summary>
        /// Metoda zwraca wektor osobnika opisanego genotypem
        /// </summary>
        /// <param name="genotyp">Tablica definiująca dziedzinę rozwiązania</param>
        public abstract ArrayList Fenotyp(ReprezentacjaGenotypu genotyp);

        //public abstract Dictionary<string, ushort[][]> Fenotyp(ReprezentacjaGenotypu genotyp);

        /// <summary>
        /// Metoda zwraca wartość funkcji celu
        /// </summary>
        /// <param name="genotyp">Tablica definiująca dziedzinę rozwiązania</param>
        /// <returns>Wartość / Wartości funkcji celu</returns>
        public Dictionary<String, double[]> FunkcjaDopasowania(ReprezentacjaGenotypu genotyp)
        {
            return problemOptymalizacyjny.ObliczZysk(Fenotyp(genotyp));
        }

        /// <summary>
        /// Metoda zwraca instancje problemu optymalizacyjnego
        /// </summary>
        /// <returns>Instancję problemu optymalizacyjnego</returns>
        public ProblemOptymalizacyjny ZwrocInstancjeProblemu()
        {
            return problemOptymalizacyjny;
        }
    }
}

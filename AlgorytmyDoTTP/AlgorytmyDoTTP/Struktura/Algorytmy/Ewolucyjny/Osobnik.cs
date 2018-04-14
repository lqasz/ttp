﻿using AlgorytmyDoTTP.Struktura.ProblemyOptymalizacyjne.KP;
using System.Collections;

namespace AlgorytmyDoTTP.Struktura.Algorytmy.Ewolucyjny
{
    class Osobnik
    {
        private ProblemPlecakowy problemPlecakowy;

        public Osobnik(ProblemPlecakowy problemPlecakowy)
        {
            this.problemPlecakowy = problemPlecakowy;
        }

        public ArrayList Fenotyp(ushort[] genotyp)
        {
            return problemPlecakowy.ZwrocWybraneElementy(genotyp);
        }

        public double[] FunkcjaDopasowania(ushort[] genotyp)
        {
            return problemPlecakowy.ObliczZysk(Fenotyp((genotyp)));
        }

        public ProblemPlecakowy ZwrocProblemPlecakowy()
        {
            return problemPlecakowy;
        }
    }
}

﻿using BiPA.Struktura.ProblemyOptymalizacyjne.Abstrakcyjny;
using System.Collections.Generic;

namespace BiPA.Struktura.Algorytmy.Abstrakcyjny.Osobnik
{
    /// <summary>
    /// Klasa konkretna reprezentująca osobnika Problemu Komiwojażera
    /// </summary>
    class OsobnikTSP : AOsobnik
    {
        public OsobnikTSP(ProblemOptymalizacyjny problemOptymalizacyjny) : base(problemOptymalizacyjny){}

        public override string DekodujRozwiazanie(ReprezentacjaRozwiazania reprezentacjaGenotypu)
        {
            // kolejność wyboru miast zapisywana jest po spacjach
            return string.Join(" ", reprezentacjaGenotypu.ZwrocGenotyp1Wymiarowy());
        }

        public override IElement[] Fenotyp(ushort[] genotyp)
        {
            return problemOptymalizacyjny.ZwrocWybraneElementy(genotyp);
        }

        public override Dictionary<string, ushort[][]> Fenotyp(ushort[][] genotyp)
        {
            throw new System.NotImplementedException();
        }

        public override Dictionary<string, float[]> FunkcjaDopasowania(ReprezentacjaRozwiazania reprezentacjaGenotypu)
        {
            return problemOptymalizacyjny.ObliczZysk(Fenotyp(reprezentacjaGenotypu.ZwrocGenotyp1Wymiarowy()));
        }
    }
}

﻿using BiPA.Struktura.Algorytmy.Abstrakcyjny.Osobnik;

namespace BiPA.Struktura.Algorytmy.Abstrakcyjny.Analityka
{
    /// <summary>
    /// Klasa analityczna.
    /// Rozszerzenie podstawowej klasy analitycznej, dla Algorytmu Ewolucyjnego.
    /// </summary>
    class AnalizaEwolucyjny : AAnalityka
    {
        public AnalizaEwolucyjny(AOsobnik rozwiazanie, short liczbaIteracji, short czasDzialania) : base(rozwiazanie, liczbaIteracji, czasDzialania) {}
    }
}

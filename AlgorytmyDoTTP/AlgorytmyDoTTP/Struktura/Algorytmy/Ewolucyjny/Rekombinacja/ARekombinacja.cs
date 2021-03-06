﻿using BiPA.Struktura.Algorytmy.Abstrakcyjny;
using BiPA.Struktura.Algorytmy.Abstrakcyjny.Osobnik;
using System;

namespace BiPA.Struktura.Algorytmy.Ewolucyjny.Rekombinacja
{
    /// <summary>
    /// Klasa abstrakcyjna odpowiedzialna wymianę materiału genetycznego pomiędzy poszczególnymi osobnikami
    /// </summary>
    abstract class ARekombinacja
    {
        protected float pwoMutacji;
        protected AOsobnik rozwiazanie;
        protected string rodzajKrzyzowania;
        protected Random losowy = new Random();
        protected bool czySprawdzacOgraniczenia = true;

        public ARekombinacja(float pwoMutacji, AOsobnik rozwiazanie)
        {
            this.pwoMutacji = pwoMutacji;
            this.rozwiazanie = rozwiazanie;
            rodzajKrzyzowania = "Proste";
        }

        public ARekombinacja(float pwoMutacji, AOsobnik rozwiazanie, string rodzajKrzyzowania)
        {
            this.pwoMutacji = pwoMutacji;
            this.rozwiazanie = rozwiazanie;
            this.rodzajKrzyzowania = rodzajKrzyzowania;
        }

        /// <summary>
        /// Metoda odpowiedzialna za ustalenie, czy sprawdzamy ograniczenia dziedziny
        /// </summary>
        /// <param name="wartosc">Wartość pozwalająca ustalić, czy sprawdzamy ograniczenia</param>
        public void ZmienSprawdzanieOgraniczen(bool wartosc)
        {
            czySprawdzacOgraniczenia = wartosc;
        }

        /// <summary>
        /// Metoda tworząca nowego osobnika za pomocą wymiany materiału genetycznego pomiędzy wybranymi przodkami
        /// </summary>
        /// <param name="przodek1">1 przodek do krzyżowania</param>
        /// <param name="przodek2">2 przodek do krzyżowania</param>
        /// <returns>Osobnik - rozwiązanie</returns>
        public abstract ReprezentacjaRozwiazania Krzyzowanie(ReprezentacjaRozwiazania przodek1, ReprezentacjaRozwiazania przodek2);

        /// <summary>
        /// Metoda zmieniająca losowo wybrany gen w genotypie
        /// </summary>
        /// <param name="genotyp">Genotyp do wykonania na nim operacji mutacji</param>
        /// <returns>Zmieniony lub nie osobnik - rozwiązanie</returns> 
        protected abstract ushort[] Mutacja(ushort[] genotyp);

        /// <summary>
        /// Metoda odpowiedzialna za naprawę osobników naruszających ograniczenia problemu
        /// </summary>
        /// <param name="genotyp">Genotyp osobnika, sprawdzany pod naruszeń ograniczeń</param>
        /// <returns>Naprawiony lub nie osobnik - rozwiązanie</returns> 
        protected abstract ReprezentacjaRozwiazania SprawdzNaruszenieOgraniczen(ReprezentacjaRozwiazania genotyp);
    }
}

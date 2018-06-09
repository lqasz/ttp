﻿using AlgorytmyDoTTP.Struktura.ProblemyOptymalizacyjne.Abstrakcyjny;
using AlgorytmyDoTTP.Struktura.ProblemyOptymalizacyjne.KP;
using AlgorytmyDoTTP.Struktura.ProblemyOptymalizacyjne.TSP;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace AlgorytmyDoTTP.Struktura.ProblemyOptymalizacyjne.TTP
{
    class ProblemPodrozujacegoZlodzieja : ProblemOptymalizacyjny
    {
        private ushort[][] dostepnePrzedmioty;
        private ProblemPlecakowy problemPlecakowy;
        private ProblemKomiwojazera problemKomiwojazera;

        public ProblemPodrozujacegoZlodzieja(string nazwaPakietu)
        {
            Inicjalizacja(nazwaPakietu);
        }

        private void Inicjalizacja(string nazwaPakietu)
        {
            XmlDocument dokument = new XmlDocument();
            dokument.Load("../../Dane/TTP/" + nazwaPakietu + ".xml");
            XmlNode przypadekTSP = dokument.DocumentElement.SelectSingleNode("/korzen/tsp");
            XmlNode przypadekKP = dokument.DocumentElement.SelectSingleNode("/korzen/kp");

            problemPlecakowy = new ProblemPlecakowy(przypadekKP.InnerText);
            problemKomiwojazera = new ProblemKomiwojazera(przypadekTSP.InnerText);

            XmlNodeList rozmieszczeniePrzedmiotow = dokument.DocumentElement.SelectNodes("/korzen/dostepnePrzedmioty/miasto");
            dlugoscGenotypu = (ushort)rozmieszczeniePrzedmiotow.Count;

            dostepnePrzedmioty = new ushort[problemKomiwojazera.ZwrocDlugoscGenotypu()][];
            for(int i = 0; i < problemKomiwojazera.ZwrocDlugoscGenotypu(); i++)
            {
                dostepnePrzedmioty[i] = new ushort[problemPlecakowy.ZwrocDlugoscGenotypu()];
                Instancja obiektTTP = new Instancja(rozmieszczeniePrzedmiotow[i].InnerText, problemPlecakowy.ZwrocDlugoscGenotypu());

                dostepnePrzedmioty[i] = (ushort[])(obiektTTP.ZwrocPrzedmioty().Clone());
            }
        }

        public override ushort[][] ZwrocDostepnePrzedmioty()
        {
            return dostepnePrzedmioty;
        }

        public ProblemPlecakowy ZwrocProblemPlecakowy()
        {
            return problemPlecakowy;
        }

        public ProblemKomiwojazera ZwrocProblemKomiwojazera()
        {
            return problemKomiwojazera;
        }

        public override Dictionary<string, double[]> ObliczZysk(Dictionary<string, ushort[][]> macierz)
        {
            Dictionary<String, double[]> wynik = new Dictionary<String, double[]>();
            wynik["min"] = new double[] { 0, 0 };
            wynik["max"] = new double[] { 0, 0 };

            double sumarycznaWartosc = 0,
                   sumarycznaWaga = 0,
                   czasPodrozy = 0;

            double[] dlugosciTrasy = (double[])(problemKomiwojazera.ZwrocDlugoscTrasy(problemKomiwojazera.ZwrocWybraneElementy(macierz["tsp"][0]), true).Clone());

            for(int i = 0; i < macierz["kp"].Length; i++)
            {
                Dictionary<String, double[]> wynikCzesciowy = problemPlecakowy.ObliczZysk(problemPlecakowy.ZwrocWybraneElementy(macierz["kp"][i]));

                sumarycznaWaga += wynikCzesciowy["min"][0];
                sumarycznaWartosc += wynikCzesciowy["max"][0];

                double predkosc = 1 - sumarycznaWaga * (1 - 0.1) / problemPlecakowy.ZwrocOgraniczeniaProblemu()[0];
                predkosc = (predkosc < 0.1) ? 0.1 : predkosc;

                czasPodrozy += dlugosciTrasy[i] * predkosc;
            }

            wynik["min"][0] = sumarycznaWaga;
            wynik["min"][1] = czasPodrozy;
            wynik["max"][0] = sumarycznaWartosc - 10 * czasPodrozy;
            wynik["max"][1] = sumarycznaWartosc;

            return wynik;
        }

        public override Dictionary<string, ushort[][]> ZwrocWybraneElementy(ushort[][] wybraneElementy)
        {
            Dictionary<string, ushort[][]> wynik = new Dictionary<string, ushort[][]>();

            ushort[][] tsp = new ushort[1][],
                       kp = new ushort[wybraneElementy.Length][];

            tsp[0] = new ushort[wybraneElementy.Length];

            for(int i = 0; i < wybraneElementy.Length; i++)
            {
                tsp[0][i] = wybraneElementy[i][0];
                kp[i] = new ushort[wybraneElementy[0].Length - 1];

                for(int j = 1; j < wybraneElementy[0].Length; j++)
                {
                    kp[i][j - 1] = wybraneElementy[i][j];
                }
            }

            wynik["tsp"] = tsp;
            wynik["kp"] = kp;

            return wynik;
        }

        public override ArrayList ZwrocWybraneElementy(ushort[] wybraneElementy)
        {
            throw new NotImplementedException();
        }

        public override Dictionary<string, double[]> ObliczZysk(ArrayList wektor)
        {
            throw new NotImplementedException();
        }
    }
}
﻿using AlgorytmyDoTTP.Widoki.KonfiguracjaAlgorytmow;
using AlgorytmyDoTTP.Widoki.Walidacja;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace AlgorytmyDoTTP.Widoki.Narzedzia
{
    /// <summary>
    /// Klasa narzędziowa widoku głownego aplikacji
    /// </summary>
    class FormatkaGlowna
    {
        private Random losowy = new Random();
        private AE algorytmEwolucyjny = new AE();
        private Konfiguracja srodowisko = new Konfiguracja();

        /// <summary> 
        /// Metoda odpowiada za wczytanie wszystkich badań z folderu zawierającego zapisane badania
        /// </summary>
        /// <returns>Elementy historycznych zapisów badań</returns>
        public ListViewItem[] WczytajHistoryczneBadania()
        {
            DirectoryInfo sciezka = new DirectoryInfo("./Badania");
            FileInfo[] pliki = sciezka.GetFiles("*.xml");
            ListViewItem[] elementy = new ListViewItem[pliki.Length];

            for (int i = 0; i < pliki.Length; i++)
            {
                XmlDocument dokument = new XmlDocument();
                dokument.Load("./Badania/" + pliki[i].Name);
                XmlNode dataZapisu = dokument.DocumentElement.SelectSingleNode("/badanie/podstawoweDane/dataZapisu");

                string[] wiersz = new string[] { pliki[i].Name.Replace(".xml", ""), dataZapisu.InnerText };
                elementy[i] = new ListViewItem(wiersz);
            }

            return elementy;
        }

        /// <summary>
        /// Metoda odpowiada za wczytanie plików odpowiadających za konfigurację Problemów Optymalizacyjnych
        /// </summary>
        /// <param name="wybranyProblem">Nazwa Problemu Optymalizacyjnego</param>
        /// <returns>Nazwy plików danych pod wybrany Problem Optymalizacyjny</returns>
        public object[] WczytajPlikiDanych(string wybranyProblem)
        {
            string nazwaFolderu = "";

            for (int i = 0; i < srodowisko.PROBLEMY_OPTYMALIZACYJNE.Length; i++)
            {
                if ((string)srodowisko.PROBLEMY_OPTYMALIZACYJNE[i] == wybranyProblem)
                {
                    nazwaFolderu = srodowisko.FOLDERY_Z_DANYMI[i];
                    break;
                }
            }

            DirectoryInfo d = new DirectoryInfo("./Dane/" + nazwaFolderu);
            FileInfo[] files = d.GetFiles("*.xml");
            object[] pliki = new object[files.Length];

            for (int i = 0; i < files.Length; i++)
            {
                pliki[i] = files[i].Name.Replace(".xml", "");
            }

            return pliki;
        }

        /// <summary>
        /// Metoda odpowiada za wczytanie badań i ich parametrów
        /// </summary>
        /// <param name="zaznaczoneElementy">Nazwy badań wybranych do porównania</param>
        /// <returns>Parametry z badań potrzebne do porównania</returns>
        public Dictionary<string, string[]> ZbierzDaneDoPorownania(ListView.CheckedListViewItemCollection zaznaczoneElementy)
        {
            Dictionary<string, string[]> paramentry = new Dictionary<string, string[]>();

            foreach (ListViewItem element in zaznaczoneElementy)
            {
                string nazwa = element.SubItems[0].Text;

                XmlDocument dokument = new XmlDocument();
                dokument.Load("./Badania/" + nazwa + ".xml");

                XmlNode maxWartosc = dokument.DocumentElement.SelectSingleNode("/badanie/podstawoweDane/maxWartosc");
                XmlNode czasDzialania = dokument.DocumentElement.SelectSingleNode("/badanie/podstawoweDane/czasDzialania");
                XmlNode nazwaBadania = dokument.DocumentElement.SelectSingleNode("/badanie/podstawoweDane/nazwaBadania");
                XmlNode plikDanych = dokument.DocumentElement.SelectSingleNode("/badanie/podstawoweDane/plikDanych");

                paramentry[nazwa] = new string[] { czasDzialania.InnerText.Replace(" ms", ""), maxWartosc.InnerText, nazwaBadania.InnerText, plikDanych.InnerText };
            }

            return paramentry;
        }

        public void UsunWybraneBadania(ListView.CheckedListViewItemCollection zaznaczoneElementy)
        {
            foreach (ListViewItem element in zaznaczoneElementy)
            {
                string nazwa = element.SubItems[0].Text + ".xml";

                if (File.Exists(@"./Badania/"+ nazwa))
                {
                    File.Delete(@"./Badania/"+ nazwa);
                }
            }
        }

        public string ZwrocDanePodgladanegoBadania(ListView.SelectedListViewItemCollection wybraneElementy)
        {
            string odpowiedz = "";

            foreach (ListViewItem element in wybraneElementy)
            {
                string nazwa = element.SubItems[0].Text;
                XmlDocument dokument = new XmlDocument();
                dokument.Load("./Badania/" + nazwa + ".xml");

                XmlNode maxWartosc = dokument.DocumentElement.SelectSingleNode("/badanie/podstawoweDane/maxWartosc"),
                        czasDzialania = dokument.DocumentElement.SelectSingleNode("/badanie/podstawoweDane/czasDzialania"),
                        nazwaBadania = dokument.DocumentElement.SelectSingleNode("/badanie/podstawoweDane/nazwaBadania"),
                        plikDanych = dokument.DocumentElement.SelectSingleNode("/badanie/podstawoweDane/plikDanych"),
                        dziedzina = dokument.DocumentElement.SelectSingleNode("/badanie/rozwiazanie/dziedzina");

                XmlNodeList dodatkoweDane = dokument.DocumentElement.SelectNodes("/badanie/dodatkoweDane");

                odpowiedz = "Nazwa Badania: " + nazwaBadania.InnerText + Environment.NewLine +
                            "Plik danych: " + plikDanych.InnerText + Environment.NewLine +
                            "Wartość: " + maxWartosc.InnerText + Environment.NewLine +
                            "Czas działania: " + czasDzialania.InnerText + " ms" + Environment.NewLine +
                            "Rozwiązanie: " + dziedzina.InnerText + Environment.NewLine + Environment.NewLine +
                            "Dane dodatkowe: " + Environment.NewLine;

                foreach (XmlNode dane in dodatkoweDane)
                {
                    foreach (XmlNode atrybut in dane.ChildNodes)
                    {
                        odpowiedz += atrybut.Name +": " + atrybut.InnerText + Environment.NewLine;
                    }
                }
            }

            return odpowiedz;
        }

        /// <summary>
        /// Metoda odpowiada za walidację danych z formatki
        /// </summary>
        /// <param name="parametry">Parametry badania</param>
        /// <exception cref="Exception">Zwraca wyjątek jeżeli jest błąd w formatce</exception>
        public void WalidacjaFormatki(Dictionary<string, string> parametry)
        {
            bool walidacja = new WalidacjaAE().CzyPoprawneCalkowite(parametry, algorytmEwolucyjny.parametryCalkowite) && new WalidacjaAE().CzyPoprawneZmiennoPrzecinkowe(parametry, algorytmEwolucyjny.parametryZmiennoPrzecinkowe);

            if (!walidacja)
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Metoda odpowiada za walidację podstawowych parametrów odpowiadających za badania
        /// </summary>
        /// <param name="parametry">Parametr badania</param>
        /// <exception cref="Exception">Zwraca wyjątek jeżeli jest błąd w formatce</exception>
        public void WalidacjaKluczowychParametrow(string parametr)
        {
            bool walidacja = new WalidacjaAE().CzyPustePoleTekstowe(parametr);

            if (!walidacja)
            {
                throw new Exception("Parametr "+ parametr +" nie może być pusty!");
            }
        }

        /// <summary>
        /// Metoda zwraca instancję konfiguracji środowiskowej
        /// </summary>
        /// <returns>Zwraca instancję konfiguracji środowiska aplikacji</returns>
        public Konfiguracja ZwrocZmiennaSrodowiskowa()
        {
            return srodowisko;
        }

        /// <summary>
        /// Metoda zwraca instancję konfiguracji Algorytmu Ewolucyjnego
        /// </summary>
        /// <returns>Zwraca instancję konfiguracji Algorytmu Ewolucyjnego</returns>
        public AE ZwrocKonfiguracjeAE()
        {
            return algorytmEwolucyjny;
        }

        public string generujDanePodTSP(int liczbaMiast, string mapa)
        {
            string nazwa = "tsp"+ liczbaMiast +"_"+ mapa;
            string[] punkty = mapa.Split("x".ToCharArray());

            XDocument xml = new XDocument();
            XElement xmlMapa = new XElement("mapa");

            int maxX = int.Parse(punkty[0]),
                maxY = int.Parse(punkty[1]);

            List<int[]> wykorzystaneMiasta = new List<int[]>();
            for(int i = 0; i < liczbaMiast; i++)
            {
                int wspX = 0,
                    wspY = 0;

                bool znalezionoMiasto = false;
                XElement miasto = new XElement("miasto");

                do
                {
                    znalezionoMiasto = false;

                    wspX = losowy.Next(maxX);
                    wspY = losowy.Next(maxY);

                    for(int j = 0; j < wykorzystaneMiasta.Count; j++)
                    {
                        if(wykorzystaneMiasta[j][0] == wspX && wykorzystaneMiasta[j][1] == wspY)
                        {
                            znalezionoMiasto = true;
                            break;
                        }
                    }
                } while (znalezionoMiasto) ;

                wykorzystaneMiasta.Add(new int[] { wspX, wspY});

                XElement x = new XElement("x", wspX),
                         y = new XElement("y", wspY);

                miasto.Add(x);
                miasto.Add(y);

                xmlMapa.Add(miasto);
            }

            xmlMapa.Add(new XElement("hash", xmlMapa.GetHashCode()));
            xml.Add(xmlMapa);
            xml.Save("./Dane/TSP/" + nazwa + ".xml");

            return nazwa;
        }

        public string generujDanePodKP(double sumaWagPrzedmiotow, double sumaWartosciPrzedmiotow, int liczbaPrzedmiotow, int procentRozrzutuWartosci)
        {
            int tmpLiczbaPrzedmiotow = liczbaPrzedmiotow;
            string nazwa = "kp" + liczbaPrzedmiotow + "_" + sumaWagPrzedmiotow + "_" + sumaWartosciPrzedmiotow;

            XDocument xml = new XDocument();
            XElement korzen = new XElement("korzen"),
                     przedmioty = new XElement("przedmioty"),
                     sumaWag = new XElement("sumaWagPrzedmiotow", sumaWagPrzedmiotow.ToString()),
                     sumaWartosci = new XElement("sumaWartosciPrzedmiotow", sumaWartosciPrzedmiotow.ToString());

            for (int i = 0; i < liczbaPrzedmiotow; i++)
            {
                XElement przedmiot = new XElement("przedmiot");

                if (i == (liczbaPrzedmiotow - 1))
                {
                    XElement waga = new XElement("waga", sumaWagPrzedmiotow.ToString()),
                             wartosc = new XElement("wartosc", sumaWartosciPrzedmiotow.ToString());

                    przedmiot.Add(waga);
                    przedmiot.Add(wartosc);
                } else
                {
                    double liczba = ObliczLiczbeZPrzedzialu(sumaWagPrzedmiotow, tmpLiczbaPrzedmiotow, procentRozrzutuWartosci);

                    XElement waga = new XElement("waga", liczba.ToString());
                    sumaWagPrzedmiotow -= liczba;

                    liczba = ObliczLiczbeZPrzedzialu(sumaWartosciPrzedmiotow, tmpLiczbaPrzedmiotow, procentRozrzutuWartosci);

                    XElement wartosc = new XElement("wartosc", liczba.ToString());
                    sumaWartosciPrzedmiotow -= liczba;

                    tmpLiczbaPrzedmiotow--;

                    przedmiot.Add(waga);
                    przedmiot.Add(wartosc);
                }

                przedmioty.Add(przedmiot);
            }

            korzen.Add(przedmioty);
            korzen.Add(sumaWag);
            korzen.Add(sumaWartosci);
            korzen.Add(new XElement("hash", korzen.GetHashCode()));

            xml.Add(korzen);
            xml.Save("./Dane/KP/"+nazwa+".xml");
                
            return nazwa;
        }

        public string generujDanePodTTP(int liczbaMiast, string mapa, double sumaWagPrzedmiotow, double sumaWartosciPrzedmiotow, int liczbaPrzedmiotow, int procentRozrzutuWartosci)
        {
            string nazwaKP = generujDanePodKP(sumaWagPrzedmiotow, sumaWartosciPrzedmiotow, liczbaPrzedmiotow, procentRozrzutuWartosci),
                   nazwaTSP = generujDanePodTSP(liczbaMiast, mapa),
                   nazwa = "ttp_"+ nazwaKP +"_"+ nazwaTSP;

            XDocument xml = new XDocument();
            XElement korzen = new XElement("korzen"),
                     dostepnePrzedmioty = new XElement("dostepnePrzedmioty"),
                     kp = new XElement("kp", nazwaKP),
                     tsp = new XElement("tsp", nazwaTSP);

            korzen.Add(kp);
            korzen.Add(tsp);

            for(int i = 0; i < liczbaMiast; i++)
            {
                int losowaDostepnosc = losowy.Next(liczbaPrzedmiotow);
                ArrayList dostepnosc = new ArrayList();
                
                for (int j = 0; j < losowaDostepnosc; j++)
                {
                    int losowaWartosc = losowy.Next(liczbaPrzedmiotow) + 1;
                    
                    if (!dostepnosc.Contains(losowaWartosc))
                    {
                        dostepnosc.Add(losowaWartosc);
                    }
                }

                XElement miasto = new XElement("miasto", string.Join(",", dostepnosc.ToArray()));
                dostepnePrzedmioty.Add(miasto);
            }

            korzen.Add(dostepnePrzedmioty);
            korzen.Add(new XElement("hash", korzen.GetHashCode()));
            xml.Add(korzen);
            xml.Save("./Dane/TTP/" + nazwa + ".xml");

            return nazwa;
        }

        private double ObliczLiczbeZPrzedzialu(double suma, int liczba, int procentRozrzutuWartosci)
        {
            double srednia = suma / liczba,
                   lewaStrona = srednia * ((double)(100 - procentRozrzutuWartosci) / 100),
                   prawaStrona = srednia * ((double)(100 + procentRozrzutuWartosci) / 100);
            
            return losowy.NextDouble() * (prawaStrona - lewaStrona) + lewaStrona;
        }
    }
}

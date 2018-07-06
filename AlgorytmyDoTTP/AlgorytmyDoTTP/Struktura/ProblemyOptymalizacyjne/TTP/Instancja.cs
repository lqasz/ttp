﻿using AlgorytmyDoTTP.Struktura.ProblemyOptymalizacyjne.Abstrakcyjny;

namespace AlgorytmyDoTTP.Struktura.ProblemyOptymalizacyjne.TTP
{
    /// <summary>
    /// Klasa reprezentująca dostępność przedmiotów w danych miastach
    /// </summary>
    class Instancja : IPomocniczy
    {
        public float ZwrocDlugosc()
        {
            throw new System.NotImplementedException();
        }

        public ushort ZwrocDo()
        {
            throw new System.NotImplementedException();
        }

        public ushort ZwrocOd()
        {
            throw new System.NotImplementedException();
        }

        public ushort[] ZwrocPrzedmioty(string dostepnePrzedmioty, ushort iloscPrzedmiotow)
        {
            dostepnePrzedmioty = dostepnePrzedmioty.Replace(" ", "").Trim();
            string[] elementy = dostepnePrzedmioty.Split(',');

            ushort[] przedmioty = new ushort[iloscPrzedmiotow];
            for (ushort i = 0; i < iloscPrzedmiotow; i++)
            {
                for (ushort j = 0; j < elementy.Length; j++)
                {
                    elementy[j] = elementy[j].Replace(" ", "").Trim();
                    if ((i + 1) == ushort.Parse(elementy[j]))
                    {
                        przedmioty[i] = 1;
                        break;
                    }
                    else
                    {
                        przedmioty[i] = 0;
                    }
                }
            }

            return przedmioty;
        }

        public float ZwrocWage()
        {
            throw new System.NotImplementedException();
        }

        public float ZwrocWartosc()
        {
            throw new System.NotImplementedException();
        }
    }
}

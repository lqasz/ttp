﻿using AlgorytmyDoTTP.Widoki;
using AlgorytmyDoTTP.Widoki.Narzedzia;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AlgorytmyDoTTP
{
    /// <summary>
    /// Klasa widoku głownego
    /// </summary>
    public partial class Glowna : Form
    {
        private FormatkaGlowna glowna = new FormatkaGlowna();

        public Glowna()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            WczytajPlikiBadan();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
        }

        /// <summary>
        /// Metoda wywołuje porównywanie wybranych badań
        /// </summary>
        private void porownaj_Click(object sender, EventArgs e)
        {
            int ilosc = daneHistoryczne.CheckedItems.Count;

            if (ilosc <= 10 && ilosc > 1)
            {
                Rectangle ekran = Screen.FromControl(this).Bounds;
                int szerokosc = ekran.Width,
                    wysokosc = ekran.Height - 100;

                FormatkaPorownania porownanieTemp = new FormatkaPorownania();
                if(!porownanieTemp.ZwrocBladPlikuDanych())
                {
                    porownanieTemp.ZwrocRaport(glowna.ZbierzDaneDoPorownania(daneHistoryczne.CheckedItems), szerokosc, wysokosc);
                }
            }
            else if(ilosc < 2)
            {
                MessageBox.Show("Nie wybrano żadnego badania lub wybrano tylko jedno!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Wybrano za dużo elementów do porównania na raz!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void podgladBadania(object sender, EventArgs e)
        {
            podglad.Text = glowna.ZwrocDanePodgladanegoBadania(daneHistoryczne.SelectedItems);
        }

        private void usuniecieBadania_Click(object sender, EventArgs e)
        {
            if(daneHistoryczne.CheckedItems.Count > 0)
            {
                glowna.UsunWybraneBadania(daneHistoryczne.CheckedItems);
                WczytajPlikiBadan();
            }
        }

        private void dodajBadanie_Click(object sender, EventArgs e)
        {
            Badanie widokBadania = new Badanie(this);
            widokBadania.Show();
        }

        private void WczytajPlikiBadan()
        {
            daneHistoryczne.Items.Clear();
            daneHistoryczne.Items.AddRange(glowna.WczytajHistoryczneBadania());
        }
    }
}

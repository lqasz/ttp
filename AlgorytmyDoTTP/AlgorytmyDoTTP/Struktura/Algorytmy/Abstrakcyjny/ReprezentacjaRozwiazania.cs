﻿using System;

namespace AlgorytmyDoTTP.Struktura.Algorytmy.Abstrakcyjny
{
    class ReprezentacjaRozwiazania
    {
        private ushort[] genotyp1Wymiarowy;
        private ushort[][] genotyp2Wymiarowy;

        public ReprezentacjaRozwiazania(){}

        public ReprezentacjaRozwiazania(ushort[] genotyp1Wymiarowy)
        {
            this.genotyp1Wymiarowy = genotyp1Wymiarowy;
        }

        public ReprezentacjaRozwiazania(ushort[][] genotyp2Wymiarowy)
        {
            this.genotyp2Wymiarowy = genotyp2Wymiarowy;
        }

        public void ZmienGenotyp(ushort[] genotyp1Wymiarowy)
        {
            this.genotyp1Wymiarowy = (ushort[])genotyp1Wymiarowy.Clone();
        }

        public void ZmienGenotyp(ushort[][] genotyp2Wymiarowy)
        {
            this.genotyp2Wymiarowy = (ushort[][])genotyp2Wymiarowy.Clone();
        }

        public ushort[] ZwrocGenotyp1Wymiarowy()
        {
            return genotyp1Wymiarowy;
        }

        public ushort[][] ZwrocGenotyp2Wymiarowy()
        {
            return genotyp2Wymiarowy;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dietApp
{
    internal class DaneKalorii
    {
        public static double wspolczynnik, BMR, CPM, dodatek;

        public static string plec;

        public static int wiek, waga, wzrost;

        public static double bialka, tluszcze, weglowodany;


        public static double ObliczKalorie()
        {

            if (plec == "M")
            {
                BMR = 66 + (13.7 * waga) + (5 * wzrost) - (6.8 * wiek);
            }
            else
            {
                BMR = 655 + (9.6 * waga) + (1.8 * wzrost) - (4.7 * wiek);
            }

            CPM = (BMR * wspolczynnik) + dodatek;

            return CPM;
        }

        

        public static double ObliczBialka()
        {
            bialka = (CPM * 0.25) / 4;
            return bialka;
           
        }

        public static double ObliczTluszcze()
        {
            tluszcze = (CPM * 0.25) / 9;
            return tluszcze;
        }

        public static double ObliczWeglowodany()
        {
            weglowodany = (CPM * 0.5) / 4;
            return weglowodany;
        }


    }
}

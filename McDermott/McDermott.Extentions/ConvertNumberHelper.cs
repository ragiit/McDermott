using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Extentions
{
    public static class ConvertNumberHelper
    {
        public static string ConvertNumberToWord(int angka)
        {
            if (angka == 0)
            {
                return "nol";
            }

            string[] satuan = { "satu", "dua", "tiga", "empat", "lima", "enam", "tujuh", "delapan", "sembilan" };
            string[] puluhan = { "sepuluh", "dua puluh", "tiga puluh", "empat puluh", "lima puluh", "enam puluh", "tujuh puluh", "delapan puluh", "sembilan puluh" };
            string[] ratusan = { "seratus", "dua ratus", "tiga ratus", "empat ratus", "lima ratus", "enam ratus", "tujuh ratus", "delapan ratus", "sembilan ratus" };

            string hasil = "";

            if (angka >= 100)
            {
                hasil += ratusan[angka / 100 - 1];
                angka %= 100;
            }

            if (angka >= 10)
            {
                if (angka % 10 == 0)
                {
                    hasil += puluhan[angka / 10 - 1];
                }
                else
                {
                    hasil += puluhan[angka / 10 - 1] + " " + satuan[angka % 10 - 1];
                }
            }
            else
            {
                hasil += satuan[angka - 1];
            }

            return hasil;
        }
    }
}

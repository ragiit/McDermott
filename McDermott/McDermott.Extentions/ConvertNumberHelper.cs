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
                if (angka / 10 == 1)
                {
                    switch (angka % 10)
                    {
                        case 1:
                            hasil += "sebelas";
                            break;
                        case 2:
                            hasil += "dua belas";
                            break;
                        case 3:
                            hasil += "tiga belas";
                            break;
                        case 4:
                            hasil += "empat belas";
                            break;
                        case 5:
                            hasil += "lima belas";
                            break;
                        case 6:
                            hasil += "enam belas";
                            break;
                        case 7:
                            hasil += "tujuh belas";
                            break;
                        case 8:
                            hasil += "delapan belas";
                            break;
                        case 9:
                            hasil += "sembilan belas";
                            break;
                            
                        // Tambahkan kasus untuk angka 13 hingga 19
                        default:
                            hasil += puluhan[angka / 10 - 1] + " " + satuan[angka % 10 - 1];
                            break;
                    }
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

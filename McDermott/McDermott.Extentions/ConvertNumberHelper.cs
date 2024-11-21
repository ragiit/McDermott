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

            string[] satuan = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
            string[] puluhan = { "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };
            string[] ratusan = { "one hundred", "two hundred", "three hundred", "four hundred", "five hundred", "six hundred", "seven hundred", "eight hundred", "nine hundred" };

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
                            hasil += "eleven";
                            break;

                        case 2:
                            hasil += "twelve";
                            break;

                        case 3:
                            hasil += "thirteen";
                            break;

                        case 4:
                            hasil += "fourteen";
                            break;

                        case 5:
                            hasil += "fifteen";
                            break;

                        case 6:
                            hasil += "sixteen";
                            break;

                        case 7:
                            hasil += "seventeen";
                            break;

                        case 8:
                            hasil += "eighteen";
                            break;

                        case 9:
                            hasil += "nineteen";
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfEconomy
{
    class DataGridRow
    { 
        public string LongName { get; set; }
        public string Name { get; set; }

        public string Year0 { get; set; }
        public string Year1 { get; set; }
        public string Year2 { get; set; }
        public string Year3 { get; set; }
        public string Year4 { get; set; }
        public string Year5 { get; set; }
        public string Year6 { get; set; }
        public string Year7 { get; set; }
        public string Year8 { get; set; }
        public string Year9 { get; set; }
        public string Year10 { get; set; }
        public string Year11 { get; set; }
        public string Year12 { get; set; }
        public string Year13 { get; set; }
        public string Year14 { get; set; }
        public string Year15 { get; set; }
        public string Year16 { get; set; }
        public string Year17 { get; set; }
        public string Year18 { get; set; }
        public string Year19 { get; set; }

        public string this[int key]
        {
            get
            {
                switch (key)
                {
                    case 0:
                        return Year0;
                    case 1:
                        return Year1;
                    case 2:
                        return Year2;
                    case 3:
                        return Year3;
                    case 4:
                        return Year4;
                    case 5:
                        return Year5;
                    case 6:
                        return Year6;
                    case 7:
                        return Year7;
                    case 8:
                        return Year8;
                    case 9:
                        return Year9;
                    case 10:
                        return Year10;
                    case 11:
                        return Year11;
                    case 12:
                        return Year12;
                    case 13:
                        return Year13;
                    case 14:
                        return Year14;
                    case 15:
                        return Year15;
                    case 16:
                        return Year16;
                    case 17:
                        return Year17;
                    case 18:
                        return Year18;
                    case 19:
                        return Year19;
                    default:
                        throw new Exception("Year must be >= 0 and < 20");
                }
            }

            set
            {
                switch (key)
                {
                    case 0:
                        Year0 = value;
                        break;
                    case 1:
                        Year1 = value;
                        break;
                    case 2:
                        Year2 = value;
                        break;
                    case 3:
                        Year3 = value;
                        break;
                    case 4:
                        Year4 = value;
                        break;
                    case 5:
                        Year5 = value;
                        break;
                    case 6:
                        Year6 = value;
                        break;
                    case 7:
                        Year7 = value;
                        break;
                    case 8:
                        Year8 = value;
                        break;
                    case 9:
                        Year9 = value;
                        break;
                    case 10:
                        Year10 = value;
                        break;
                    case 11:
                        Year11 = value;
                        break;
                    case 12:
                        Year12 = value;
                        break;
                    case 13:
                        Year13 = value;
                        break;
                    case 14:
                        Year14 = value;
                        break;
                    case 15:
                        Year15 = value;
                        break;
                    case 16:
                        Year16 = value;
                        break;
                    case 17:
                        Year17 = value;
                        break;
                    case 18:
                        Year18 = value;
                        break;
                    case 19:
                        Year19 = value;
                        break;

                    default:
                        throw new Exception("Year must be >= 0 and < 20");
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frogger
{
    public class score
    {
        public int Score { get; set; } = 0;
        public void SetScore(Frogger frogger)
        {
            switch (frogger.froggerRow)
            {
                case 1:
                    Score += 100;
                    break;
                case 2:
                    Score += 10;
                    break;
                case 3:
                    Score += 10;
                    break;
                case 4:
                    Score += 10;
                    break;
                case 5:
                    Score += 10;
                    break;
                case 6:
                    Score += 50;
                    break;
                case 7:
                    Score += 10;
                    break;
                case 8:
                    Score += 10;
                    break;
                case 9:
                    Score += 10;
                    break;
                case 10:
                    Score += 10;
                    break;               
                default:
                    break;
            }
        }

        public void LowerScore(Frogger frogger)
        {
            switch (frogger.froggerRow - 1)
            {
                case 1:
                    Score -= 100;
                    break;
                case 2:
                    Score -= 10;
                    break;
                case 3:
                    Score -= 10;
                    break;
                case 4:
                    Score -= 10;
                    break;
                case 5:
                    Score -= 10;
                    break;
                case 6:
                    Score -= 50;
                    break;
                case 7:
                    Score -= 10;
                    break;
                case 8:
                    Score -= 10;
                    break;
                case 9:
                    Score -= 10;
                    break;
                case 10:
                    Score -= 10;
                    break;
                default:
                    break;
            }
        }
    }
}

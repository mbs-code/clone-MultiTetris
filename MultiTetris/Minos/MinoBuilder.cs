using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTetris.Minos
{
    class MinoBuilder
    {

        public static bool[,] Mino_Null()
        {
            bool[,] mino = new bool[0, 0];
            return mino;
        }

        public static bool[,] Mino_I()
        {
            bool[,] mino = new bool[4, 4];
            mino[0, 2] = true;
            mino[1, 2] = true;
            mino[2, 2] = true;
            mino[3, 2] = true;
            return mino;
        }

        public static bool[,] Mino_O()
        {
            bool[,] mino = new bool[2, 2];
            mino[0, 0] = true;
            mino[0, 1] = true;
            mino[1, 0] = true;
            mino[1, 1] = true;
            return mino;
        }

        public static bool[,] Mino_S()
        {
            bool[,] mino = new bool[3, 3];
            mino[1, 0] = true;
            mino[2, 0] = true;
            mino[0, 1] = true;
            mino[1, 1] = true;
            return mino;
        }

        public static bool[,] Mino_Z()
        {
            bool[,] mino = new bool[3, 3];
            mino[0, 0] = true;
            mino[1, 0] = true;
            mino[1, 1] = true;
            mino[2, 1] = true;
            return mino;
        }

        public static bool[,] Mino_J()
        {
            bool[,] mino = new bool[3, 3];
            mino[1, 0] = true;
            mino[1, 1] = true;
            mino[1, 2] = true;
            mino[0, 2] = true;
            return mino;
        }

        public static bool[,] Mino_L()
        {
            bool[,] mino = new bool[3, 3];
            mino[0, 0] = true;
            mino[0, 1] = true;
            mino[0, 2] = true;
            mino[1, 2] = true;
            return mino;
        }

        public static bool[,] Mino_T()
        {
            bool[,] mino = new bool[3, 3];
            mino[0, 1] = true;
            mino[1, 1] = true;
            mino[2, 1] = true;
            mino[1, 0] = true;
            return mino;
        }


    }
}

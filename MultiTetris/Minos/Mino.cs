using MultiTetris.Maps;
using MultiTetris.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTetris.Minos
{
    enum MinoType { Null, I, O, S, Z, J, L, T};


    class Mino
    {

        public MinoType type { get; }  // ミノの種類
        public Pos pos { get; set; }        // ミノ座標
        public bool[,]  grid { get; set; }  // ミノマップ

        public bool canFix; // 固定フラグ
        public bool doOver; // 最上位突破フラグ

        private int frame;  // 落下フレームカウント



        public Mino(Pos pos, MinoType type)
        {
            // 場所を指定して置く
            this.type = type;
            this.grid = ToMino(type);
            this.pos = pos;
            this.frame = 0;
            this.canFix = false;
            this.doOver = false;
        }

        public Mino(MapManager map, MinoType type, int playerId)
        {
            this.type = type;
            this.grid = ToMino(type);
            this.pos = map.getNewMinoShowPos(this.grid, playerId);
            this.frame = 0;
            this.canFix = false;
            this.doOver = false;
        }





        /// <summary>
        /// 自動落下チェック
        /// </summary>
        /// <param name="map">マップ</param>
        /// <param name="cellPerFrame">1セルあたりの待機フレーム数</param>
        public void CheckFreeFall(MapManager map, int cellPerFrame)
        {
            frame++;
            if (frame >= cellPerFrame)
            {
                Down(map);
            }
        }

        /// <summary>
        /// 移動後は落下タイムをリセットする
        /// </summary>
        private void FrameReset()
        {
            frame = 0;
        }

        /// <summary>
        /// ミノ固定確認
        /// </summary>
        /// <param name="map"></param>
        private void FixCheck(MapManager map)
        {
            pos.y++;
            this.canFix = (map.FieldCheck(this) != CheckType.OK);
            pos.y--;
        }



        /// <summary>
        /// 座標修正
        /// </summary>
        private void Modify(MapManager map)
        {
            while (true)
            {
                CheckType check = map.Check(this);
                //Console.WriteLine("check. {0} {1}", this, check);
                switch (check)
                {
                    case CheckType.OK:
                        goto loop;
                    case CheckType.ERR_LEFT_END:
                        pos.x++;
                        break;
                    case CheckType.ERR_RIGHT_END:
                        pos.x--;
                        break;
                    case CheckType.ERR_OVERLAP:
                        pos.y--;
                        break;
                    case CheckType.ERR_TOP_END:
                        this.doOver = true;
                        goto loop;
                }
            }
            loop:;
        }





        /// <summary>
        /// 右に移動する
        /// </summary>
        /// <param name="map">マップ</param>
        public void RightMove(MapManager map)
        {
            SoundDefine.MinoMoveSound();
            pos.x++;
            if (map.Check(this) != CheckType.OK)
            {
                pos.x--;
            }
            Modify(map);
        }

        /// <summary>
        /// 左に移動する
        /// </summary>
        /// <param name="map">マップ</param>
        public void LeftMove(MapManager map)
        {
            SoundDefine.MinoMoveSound();
            pos.x--;
            if (map.Check(this) != CheckType.OK)
            {
                pos.x++;
            }
            Modify(map);
        }


        /// <summary>
        /// 下に移動する
        /// </summary>
        /// <param name="map">マップ</param>
        public void Down(MapManager map)
        {
            SoundDefine.MinoMoveSound();
            FrameReset();
            pos.y++;
            if (map.Check(this) != CheckType.OK)
            {
                pos.y--;
                FixCheck(map);
            }
            Modify(map);
        }

        /// <summary>
        /// 左回転をする
        /// </summary>
        /// <param name="map">マップ</param>
        public void LeftRotation(MapManager map)
        {
            SoundDefine.MinoRotSound();
            Console.WriteLine("Left rotation.");
            Clockwise();
            Modify(map);
        }

        /// <summary>
        /// 右回転をする
        /// </summary>
        /// <param name="map">マップ</param>
        public void RightRotation(MapManager map)
        {
            SoundDefine.MinoRotSound();
            Console.WriteLine("Right rotation.");
            AntiClockwise();
            Modify(map);
        }














        /// <summary>
        /// ハードドロップ
        /// </summary>
        /// <param name="map"></param>
        public void HardDrop(MapManager map)
        {
            SoundDefine.MinoMoveSound();
            Console.WriteLine("Hard drop.");

            FrameReset();
            GetDropPos(map);
            FixCheck(map);
        }
















        /// <summary>
        /// ドロップ先の座標の取得
        /// </summary>
        public Pos GetDropPos(MapManager map)
        {
            while (true)
            {
                pos.y++;
                if (map.Check(this) != CheckType.OK)
                {
                    pos.y--;
                    return pos;
                }
            }
        }




        /// <summary>
        /// 時計回りの回転
        /// </summary>
        private void Clockwise()
        {
            int sw = grid.GetLength(0);
            int sh = grid.GetLength(1);

            bool[,] _new = new bool[sw, sh];
            for (int j = 0; j < sh; j++)
            {
                for (int i = 0; i < sw; i++)
                {
                    _new[i, sh - 1 - j] = grid[j, i];
                }
            }

            this.grid = _new;
        }

        /// <summary>
        /// 反時計回りの回転
        /// </summary>
        private void AntiClockwise()
        {
            int sw = grid.GetLength(0);
            int sh = grid.GetLength(1);

            bool[,] _new = new bool[sw, sh];
            for (int j = 0; j < sh; j++)
            {
                for (int i = 0; i < sw; i++)
                {
                    _new[sw - 1 - i, j] = grid[j, i];
                }
            }

            this.grid = _new;
        }








        /// <summary>
        /// ミノの種類から配列を取得する
        /// </summary>
        /// <param name="type">ミノの種類</param>
        /// <returns>ミノ配列</returns>
        public static bool[,] ToMino(MinoType type)
        {
            bool[,] grid = MinoBuilder.Mino_Null();

            switch (type)
            {
                case MinoType.I: grid = MinoBuilder.Mino_I(); break;
                case MinoType.O: grid = MinoBuilder.Mino_O(); break;
                case MinoType.S: grid = MinoBuilder.Mino_S(); break;
                case MinoType.Z: grid = MinoBuilder.Mino_Z(); break;
                case MinoType.J: grid = MinoBuilder.Mino_J(); break;
                case MinoType.L: grid = MinoBuilder.Mino_L(); break;
                case MinoType.T: grid = MinoBuilder.Mino_T(); break;
            }

            return grid;
        }








        public override string ToString()
        {
            return string.Format("Mino{{type={0}, id={5}, pos=({1},{2}), fix={3}, cnt={4}}}",
                this.type, pos.x, pos.y, canFix, frame, GetHashCode());
        }

    }
}

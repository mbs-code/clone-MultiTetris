
using DxLibDLL;
using MultiTetris.Events;
using MultiTetris.Maps;
using MultiTetris.Minos;
using MultiTetris.Utilities;
using System;
using System.Collections.Generic;
using static MultiTetris.DrawDefine;

namespace MultiTetris.Maps
{
    class MapManager
    {

        /*
         * ミノを管理しておくクラス.
         * フィールドはw*hの2次元配列で、nullなら存在しない
         * それ以外ならその色のオブジェクトが存在する
         * オブジェクトの要素は、kindofminoとか作ってそこで識別する
         */
         
        public Field field { get; private set; }    // 既に固定されたフィールド
        private Field temp;     // 毎ループ初期化されるフィールド


        private int playerNum;  // プレイヤー数

        private int   hScreen;  // [描画]スクリーンバッファ
        private int[] hMino;    // [描画]ミノ画像ハンドル

        /// <summary>
        /// マップ管理コンストラクタ
        /// </summary>
        /// <param name="width">横セル数(>0)</param>
        /// <param name="height">縦セル数(>0)</param>
        public MapManager(int width, int height, int playerNum)
        {
            field = new Field(width, height);
            temp = new Field(width, height);
            Console.WriteLine("Create Field. {0}x{1}", field.GetWidth(), field.GetHeight());

            this.playerNum = playerNum;

            // ミノ画像ロード
            hMino = new int[MINO_NUM];
            DX.LoadDivGraph(@"src\mino.png",
                MINO_NUM, MINO_NUM, 1, MINO_SIZE, MINO_SIZE, out hMino[0]);


            // スクリーン作成
            int sw = (PAD * 2) + (W_LINE + W_MINO) * width + W_LINE;
            int sh = (PAD * 2) + (W_LINE + W_MINO) * (height- SHOW_LINE) + W_LINE;
            hScreen = DX.MakeScreen(sw, sh, DX.FALSE);
            Console.WriteLine("Create Mino Screen. hscreen={0}, mino=[{1}]", hScreen, string.Join(",", hMino));
        }


        /// <summary>
        /// フィールドを初期化する（イベント発生も）
        /// </summary>
        public void TurnInit()
        {
            // temp の内容を初期化
            temp.Marge(field);
        }




        /// <summary>
        /// 仮変更を適用する
        /// field の内容を temp と同じにする
        /// </summary>
        public void Update()
        {
            Console.WriteLine("Field update.");
            field.Marge(temp);
        }

        /// <summary>
        /// 現在の状態を出力
        /// </summary>
        public void Dump()
        {
            Console.Write("Fact ");
            field.Dump();
            Console.Write("Temp ");
            temp.Dump();
        }



        /// <summary>
        /// フィールドにミノが配置できるか確認
        /// </summary>
        /// <param name="mino">配置するミノ</param>
        /// <returns>成功可否</returns>
        public CheckType Check(Mino mino)
        {
            Pos pos = mino.pos;
            bool[,] grid = mino.grid;

            CheckType type = field.Check(pos, grid);
            if (type == CheckType.OK)
            {
                return temp.Check(pos, grid);
            }

            return type;
        }

        /// <summary>
        /// フィールド上のミノ確認(移動中は認識しない)
        /// </summary>
        /// <param name="mino">配置するミノ</param>
        /// <returns>成功可否</returns>
        public CheckType FieldCheck(Mino mino)
        {
            Pos pos = mino.pos;
            bool[,] grid = mino.grid;
            return field.Check(pos, grid);
        }


        /// <summary>
        /// フィールドにミノを仮配置する
        /// </summary>
        /// <param name="mino">配置するミノ</param>
        /// <returns>成功可否</returns>
        public bool Add(Mino mino)
        {
            return temp.Add(mino);
        }


        /// <summary>
        /// フィールドにミノを本配置する
        /// </summary>
        /// <param name="mino">配置するミノ</param>
        /// <returns>成功可否</returns>
        public bool Drop(Mino mino)
        {
            return field.Add(mino);
        }









        public void SwapLine(int lineNum1, int lineNum2)
        {
            //field.Dump();
            MinoType[,] grid = field.field;
            MinoType tmp;
            for (int i=0; i<field.GetWidth(); i++)
            {
                //Console.WriteLine("  ({0},{1} -> {2},{3})", i, lineNum1, i, lineNum2);
                tmp = grid[i, lineNum1];
                grid[i, lineNum1] = grid[i, lineNum2];
                grid[i, lineNum2] = tmp;
            }
        }



        public void ClearLine(int lineNum)
        {
            MinoType[,] grid = field.field;
            for (int i = 0; i < field.GetWidth(); i ++)
            {
                grid[i, lineNum] = MinoType.Null;
            }
        }









        /// <summary>
        /// ミノの新規出現位置の計算
        /// </summary>
        /// <param name="grid">ミノ配列</param>
        /// <param name="playerId">プレイヤーID</param>
        /// <returns>配置</returns>
        public Pos getNewMinoShowPos(bool[,] grid, int playerId)
        {            
            int sw = field.GetWidth();
            int sh = field.GetHeight();

            // まず、左右の幅を計測(縦に見ていく)
            int w = 0;
            for (int i=0; i<grid.GetLength(0); i++)
            {
                bool show = false;
                for (int j =0; j<grid.GetLength(1); j++)
                {
                    if (grid[i, j])
                    {
                        show = true;
                        break;
                    }
                }
                if (show) w++;
            }

            // 次に一番下の高さを計測(横に見ていく)
            int h = 0;
            for (int j = grid.GetLength(1)-1; j >= 0; j--)
            {
                bool show = false;
                for (int i=0; i<grid.GetLength(0); i++)
                {
                    if (grid[i, j])
                    {
                        show = true;
                        break;
                    }
                }
                if (show)
                {
                    h = j;
                    break;
                }
            }


            // プレイヤー数で割った中央に出す
            int kw = (sw / playerNum);
            int x = (kw - w) / 2 + (kw * playerId);
            Console.WriteLine(kw + " " + x);

            // 高さは上からずらして、出現すべきラインを超えた場所
            int y = APPEND_LINE - h;

            return new Pos(x, y);
        }



        





        /// <summary>
        /// フィールド画像の更新
        /// 描画して返却する
        /// </summary>
        /// <returns>画像ハンドラ</returns>
        public int UpdateFieldGraphics()
        {
            DX.SetDrawScreen(hScreen);
            DX.ClearDrawScreen();

            int sw = (W_LINE + W_MINO) * field.GetWidth() + W_LINE;
            int sh = (W_LINE + W_MINO) * (field.GetHeight() - SHOW_LINE) + W_LINE;


            // 横線の描画
            int c = APPEND_LINE - SHOW_LINE;
            for (int i = 0; i < field.GetHeight() - SHOW_LINE + 1; i++)
            {
                Rect rect = GetRowLine(i);
                uint color = (i != c) ? DX.GetColor(240, 240, 240) : DX.GetColor(240, 0, 0);
                DX.DrawBox(rect.x, rect.y, rect.w, rect.h, color, DX.TRUE);
            }

            // 縦線の描画
            for (int i=0; i<field.GetWidth()+1; i++)
            {
                Rect rect = GetColumnLine(i);
                DX.DrawBox(rect.x, rect.y, rect.w, rect.h, DX.GetColor(240, 240, 240), DX.TRUE);
            }



            // ミノ配置
            int gh = field.GetHeight() - SHOW_LINE;
            for (int j = 0; j < gh; j++)
                for (int i = 0; i < field.GetWidth(); i++)
                {
                    Rect rect = GetCellRect(i, j);
                    MinoType tf = field.field[i, j + SHOW_LINE];
                    MinoType tt = temp.field [i, j + SHOW_LINE];

                    if (tf != MinoType.Null)
                        DX.DrawExtendGraph(rect.x, rect.y, rect.w, rect.h, hMino[(int)tf], DX.TRUE);
                    
                    if (tt != MinoType.Null)
                        DX.DrawExtendGraph(rect.x, rect.y, rect.w, rect.h, hMino[(int)tt], DX.TRUE);
                    
                    //string tmp = "" + (int)tt + (int)tf;
                    //DX.DrawStringF(rect.x, rect.y, tmp, DX.GetColor(255, 255, 255));
                }

            return hScreen;
        }
        
        

    }
}

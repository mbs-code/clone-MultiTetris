using MultiTetris.Maps;
using MultiTetris.Minos;
using MultiTetris.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;
using MultiTetris.Utilities;
using static MultiTetris.DrawDefine;

namespace MultiTetris.Events
{
    class ClearEvent : Event
    {
        //const int actionFrame = 120;    // 処理をするフレーム
        //const int finishFrame = 360;    // 終了フレーム

        private List<int> tgColumns;
        private bool DoEvent;           // イベント実行中はtrue

        private List<Pos> tgCell;   // 対象セル
        

        const int addAlpha = 16;
        const int showFrame = 24;

        public ClearEvent() :base(null)
        {
            this.tgColumns = new List<int>();
            this.DoEvent = false;
        }

        



        public override void Calc(PlayerManager ply, MapManager map, byte[] key)
        {
            base.Calc(ply, map, key);

            // イベント非実行中
            if (!DoEvent)
            {
                LineCheck(map.field);
                if (tgColumns.Count > 0)
                {
                    SoundDefine.LineClearSound();
                    DoEvent = true;
                    frame = 0;
                }
            }

            // イベント処理
            if (DoEvent)
            {
                if (frame > showFrame)
                {
                    LineClear(map);
                    DoEvent = false;
                }
            }
        }

        public override bool CanFinish()
        {
            return false;
        }


        public override void UpdateFieldGraphics(int hMain)
        {
            if (DoEvent)
            {
                DX.SetDrawBlendMode(DX.DX_BLENDMODE_ALPHA, 10 * frame);
                foreach (int col in tgColumns)
                {
                    Rect r = GetRowRect(col);
                    DX.DrawBox(r.x, r.y, r.w, r.h, DX.GetColor(255, 255, 255), DX.TRUE);
                }
                DX.SetDrawBlendMode(DX.DX_BLENDMODE_NOBLEND, 0);
            }
        }













        /// <summary>
        /// 各行の消去確認メソッド
        /// </summary>
        /// <param name="map">ミノマップ</param>
        private void LineCheck(Field map)
        {
            // 各行ごとに確認
            for (int j = 0; j < map.GetHeight(); j++)
            {
                // 各列のミノををカウント
                int cnt = 0;
                for (int i = 0; i < map.GetWidth(); i++)
                {
                    if (map.field[i, j] != MinoType.Null) cnt++;
                }

                // もし一列揃っていたら消去リストに追加
                if (cnt == map.GetWidth())
                {
                    tgColumns.Add(j);
                }
            }
        }

        


        /// <summary>
        /// ミノ消去メソッド
        /// </summary>
        /// <param name="map">ミノマップ</param>
        private void LineClear(MapManager map)
        {
            Console.WriteLine("Clear Lines. line"+ string.Join(",", tgColumns));
            for (int i = tgColumns.Count() - 1; i >= 0; i--)
            {
                int line = tgColumns[i];
                // 対象の行を消す
                //Console.WriteLine("clear {0}", line);
                map.ClearLine(line);

                // 上の行を全てずらす
                while (line - 1 >= 0)
                {
                    //Console.WriteLine("swap {0} to {1}", line, line-1);
                    map.SwapLine(line, line - 1);
                    line--;
                }

                // 対象行を全て1増やす(消した数ほどシフト)
                for (int j = 0; j < tgColumns.Count(); j++)
                {
                    tgColumns[j]++;
                }
            }

            tgColumns.Clear();
        }





    }
}

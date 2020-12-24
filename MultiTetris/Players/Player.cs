using DxLibDLL;
using MultiTetris.Maps;
using MultiTetris.Minos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTetris.Players
{
    class Player
    {
        const int minonum = 5; //現在表示ミノ + スタック


        public string name { get; }
        public int score { get; private set; }

        public List<MinoType> que   { get; private set; }
        public MinoType       hold  { get; private set; }
        public Mino           mino  { get; private set; }
        public FallController cFall { get; } // 落下カウンタ

        private PlayerKeymap keymap;    // キーマップ
        private int playerId;
        private bool lotteryError;      // ミノ出現エラー

        private int keyMoveWaitFrame = 5;   // キー連続入力ウェイト
        private int keyRotWaitFrame = 20;   // キー連続入力ウェイト
        

        public override string ToString()
        {
            return string.Format("Player{{pid={0}, id={1}, name={2}, mino={3},({4}){5}}}",
                playerId, GetHashCode(), name, mino, hold, string.Join(",", que));
        }




        public Player(int playerId, string name, PlayerKeymap keymap)
        {
            this.playerId = playerId;
            this.name = name;
            this.keymap = keymap;
            this.hold = MinoType.Null;

            this.cFall = new FallController(0);

            this.lotteryError = false;


            this.que = new List<MinoType>();
            while (que.Count() < minonum)
            {
                this.que.Add(LotteryMino());
            }
        }


        /// <summary>
        /// 座標計算
        /// ループ毎に一度呼び出す
        /// </summary>
        /// <param name="map">マップ管理クラス</param>
        /// <param name="keymap">入力キー</param>
        public void Calc(MapManager map, byte[] key)
        {
            // ミノが空なら新規ミノを挿入
            if (mino == null || mino.type == MinoType.Null)
            {
                bool result = MinoSetToMap(map, que[0]);
                this.lotteryError = !result;

                que.Remove(que[0]);
                this.que.Add(LotteryMino());
                cFall.Increment();
            }

            // キー移動確認
            KeyCheck(map, key);
        }

        /// <summary>
        /// プレイヤーの終了条件
        /// </summary>
        /// <returns>trueで終了</returns>
        public bool CanFinish()
        {
            if (lotteryError)   return true;
            if (mino != null)   return (mino.doOver);
            return false;
        }


        /// <summary>
        /// ミノの抽選をする
        /// </summary>
        /// <param name="map">マップ</param>
        /// <returns>抽選ミノ</returns>
        private MinoType LotteryMino()
        {
            int rand = DX.GetRand(6) + 1;
            return (MinoType)rand;
        }





        /// <summary>
        /// ミノをマップに出現させる
        /// </summary>
        /// <param name="map">マップ</param>
        /// <param name="type">ミノの種類</param>
        /// <returns>trueで出現成功</returns>
        private bool MinoSetToMap(MapManager map, MinoType type)
        {
            this.mino = new Mino(map, type, playerId);
            Console.WriteLine("[{0}]Mino Append. {1}", name, mino);

            CheckType check = map.Check(mino);
            return check == CheckType.OK;
        }

        /// <summary>
        /// ミノをホールドする
        /// </summary>
        /// <param name="map">マップ</param>
        /// <results>falseでゲーム終了</results>
        private bool HoldMino(MapManager map)
        {
            Mino tmp = mino;
            //if (hold == MinoType.Null)
            //{
            //    hold = que[0];
            //    que.Remove(hold);
            //    que.Add(LotteryMino());
            //}
            if (!MinoSetToMap(map, hold)) return false;
            hold = tmp.type;
            return true;
        }


        /// <summary>
        /// キー入力更新
        /// </summary>
        /// <param name="map">マップ管理クラス</param>
        /// <param name="key">入力キー</param>
        /// <results>falseでゲーム終了</results>
        public bool KeyCheck(MapManager map, byte[] key)
        {
            // 入力確認
            if (key[keymap.left]   % keyMoveWaitFrame == 1) { mino.LeftMove(map); }
            if (key[keymap.right]  % keyMoveWaitFrame == 1) { mino.RightMove(map); }
            if (key[keymap.down]   % keyMoveWaitFrame == 1) { mino.Down(map); }

            if (key[keymap.clock]  % keyRotWaitFrame == 1)  { mino.RightRotation(map); }
            if (key[keymap.uclock] % keyRotWaitFrame == 1)  { mino.LeftRotation(map); }

            if (key[keymap.up] == 1) { mino.HardDrop(map); }
            if (key[keymap.hold] == 1) { if (!HoldMino(map)) return false; }

            // 自動落下確認
            mino.CheckFreeFall(map, cFall.CellPerFrame());

            // マップ固定処理
            if (mino.canFix)
            {
                Console.WriteLine("Fix mino.");
                map.Drop(mino);
                this.mino = null;
            } else {
                map.Add(mino);
            }
            return true;   
        }
    }
}

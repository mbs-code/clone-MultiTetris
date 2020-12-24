using MultiTetris.Maps;
using MultiTetris.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;

namespace MultiTetris.Events
{
    abstract class Event
    {
        public int frame { get; set; }
        public Player player { get; set; }

        public Event(Player player)
        {
            this.player = player;
        }

        /// <summary>
        /// 毎ターン呼び出すカウンタメソッド
        /// </summary>
        /// <param name="map">ミノマップ</param>
        /// <param name="key">キーフィールド</param>
        public virtual void Calc(PlayerManager ply, MapManager map, byte[] key)
        {
            frame++;
        }

        /// <summary>
        /// イベントの終了条件
        /// </summary>
        /// <returns>trueで終了</returns>
        public abstract bool CanFinish();



        /// <summary>
        /// フィールド画像の更新
        /// !!! 描画先を変更する場合は戻すこと !!!
        /// </summary>
        /// <param name="hMain">バッファ裏画面</param>
        /// <returns>画像ハンドラ</returns>
        public abstract void UpdateFieldGraphics(int hMain);




        public override string ToString()
        {
            return string.Format("Event{{type={0}, id={1}, frame={2}, fin={3}}}",
                this.GetType().FullName, GetHashCode(), frame, CanFinish());
        }

    }
}

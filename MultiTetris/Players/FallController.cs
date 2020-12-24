using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTetris.Players
{
    class FallController
    {
        public int level { get; private set; } // 現在のレベル
        public int count { get; private set; } // カウント
        public int next  { get; private set; } // 次のレベルのカウント

        public FallController(int startLevel)
        {
            this.level = startLevel;
            this.count = 0;
            this.next = 10;
        }




        /// <summary>
        /// カウンタを一進める
        /// </summary>
        public void Increment()
        {
            this.count++;
            if (count > next)
            {
                this.level++;
                this.count = 0;
            }
        }

        /// <summary>
        /// 一セル落ちる時間
        /// </summary>
        /// <returns></returns>
        public int CellPerFrame()
        {
            int n = 60 - level * 6;
            if (n < 10) n = 10;
            return n;
        }
    }
}

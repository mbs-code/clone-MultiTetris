using MultiTetris.Maps;
using MultiTetris.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using DxLibDLL;

namespace MultiTetris.Events
{
    class EventManager
    {
        public MapManager map;
        private PlayerManager plm;

        private List<Event>  events;
        public bool run = true;

        public EventManager(MapManager map, PlayerManager plm)
        {
            this.map = map;
            this.plm = plm;
            this.events = new List<Event>();

            this.AddEvent(new ClearEvent());
            this.AddEvent(new FpsEvent());
        }

        public void AddEvent(Event _event) {
            Console.WriteLine("Add event. {0}", _event);
            events.Add(_event);
        }


        /// <summary>
        /// 毎ターンのイベント制御
        /// </summary>
        /// <param name="key">キーマップ</param>
        public void Update(byte[] key)
        {
            // 各イベント操作
            for (int i = events.Count - 1; i >= 0; i--)
            {
                events[i].Calc(plm, map, key);
                if (events[i].CanFinish())
                {
                    Console.WriteLine("Remove event. {0}", events[i]);
                    events.Remove(events[i]);
                }
            }
        }

        /// <summary>
        /// イベントリストが空かどうか
        /// </summary>
        /// <returns>trueで空</returns>
        public bool IsEmpty()
        {
            return events.Count() == 0;
        }

        /// <summary>
        /// フィールド画像の更新
        /// 描画して返却する
        /// </summary>
        /// <returns>画像ハンドラ</returns>
        public int UpdateFieldGraphics(int handler)
        {
            DX.SetDrawScreen(handler);
            foreach (Event e in events)
            {
                e.UpdateFieldGraphics(handler);
            }

            return handler;
        }
    }
}

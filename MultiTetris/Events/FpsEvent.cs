using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiTetris.Players;
using DxLibDLL;

namespace MultiTetris.Events
{
    class FpsEvent : Event
    {
        private long hold;
        const long second = 1 * 1000 * 1000;
        private double fps;
        private int fr;

        public FpsEvent() : base(null)
        {
            hold = DX.GetNowHiPerformanceCount();
            fps = 0.0f;
            fr = 0;
        }

        public override bool CanFinish()
        {
            return false;
        }

        public override void UpdateFieldGraphics(int hMain)
        {
            DX.DrawString(1000, 500, "fps:"+CalcFps(), DX.GetColor(255, 255, 255));
        }

        private double CalcFps()
        {
            long now = DX.GetNowHiPerformanceCount();
            if (now > hold + second)
            {
                long ct = now - hold;
                fps = (double)fr / ct * second;

                hold = now;
                fr = 0;
            }

            fr++;
            return fps;
        }
    }
}

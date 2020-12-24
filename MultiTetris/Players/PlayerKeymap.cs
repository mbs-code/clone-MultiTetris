using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTetris.Players
{
    class PlayerKeymap
    {
        public int up { get; set; }
        public int down { get; set; }
        public int left { get; set; }
        public int right { get; set; }
        public int clock { get; set; }
        public int uclock { get; set; }
        public int hold { get; set; }
        public int item { get; set; }

        public PlayerKeymap(int up, int down, int left, int right, int clock, int uclock, int hold, int item)
        {
            this.up = up;
            this.down = down;
            this.left = left;
            this.right = right;

            this.clock = clock;
            this.uclock = uclock;

            this.hold = hold;
            this.item = item;
        }
    }
}

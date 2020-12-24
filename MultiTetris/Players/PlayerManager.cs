using DxLibDLL;
using MultiTetris.Maps;
using MultiTetris.Minos;
using MultiTetris.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MultiTetris.HmdDefine;

namespace MultiTetris.Players
{
    class PlayerManager
    {
        private MapManager map;

        private List<Player> players;
        public bool run = true;

        private int hHmd; // HMDスクリーン



        public PlayerManager(MapManager map)
        {
            this.map = map;
            this.players = new List<Player>();

            this.AddPlayer(new Player(0, "てすと", new PlayerKeymap(
                DX.KEY_INPUT_UP, DX.KEY_INPUT_DOWN, DX.KEY_INPUT_LEFT, DX.KEY_INPUT_RIGHT,
                DX.KEY_INPUT_LSHIFT, DX.KEY_INPUT_LCONTROL,
                DX.KEY_INPUT_SPACE, DX.KEY_INPUT_I
            )));

            //players.Add(new Player(0, "さとう", new PlayerKeymap(
            //    DX.KEY_INPUT_W, DX.KEY_INPUT_S, DX.KEY_INPUT_A, DX.KEY_INPUT_D,
            //    DX.KEY_INPUT_LSHIFT, DX.KEY_INPUT_LCONTROL,
            //    DX.KEY_INPUT_SPACE, DX.KEY_INPUT_I
            //)));
            //players.Add(new Player(1, "たなか", new PlayerKeymap(
            //    DX.KEY_INPUT_UP, DX.KEY_INPUT_DOWN, DX.KEY_INPUT_LEFT, DX.KEY_INPUT_RIGHT,
            //    DX.KEY_INPUT_RSHIFT, DX.KEY_INPUT_SPACE,
            //    DX.KEY_INPUT_SPACE, DX.KEY_INPUT_I
            //)));

            Rect rect = HmdDefine.GetHMDSize();
            hHmd = DX.MakeScreen(rect.w, rect.h, DX.FALSE);
        }

        public void AddPlayer(Player player)
        {
            Console.WriteLine("Add player. {0}", player);
            players.Add(player);
        }

        /// <summary>
        /// 毎ターンのプレイヤー制御
        /// </summary>
        /// <param name="key">キーマップ</param>
        public void Update(byte[] key)
        {
            // 各プレイヤー操作
            for (int i = players.Count - 1; i >= 0; i--)
            {
                players[i].Calc(map, key);
                if (players[i].CanFinish())
                {
                    Console.WriteLine("Remove player. {0}", players[i]);
                    players.Remove(players[i]);
                }
            }
        }

        /// <summary>
        /// プレイヤーリストが空かどうか
        /// </summary>
        /// <returns>trueで空</returns>
        public bool IsEmpty()
        {
            return players.Count() == 0;
        }



        /// <summary>
        /// フィールド画像の更新
        /// 描画して返却する
        /// </summary>
        /// <returns>画像ハンドラ</returns>
        public int UpdateFieldGraphics(int handler)
        {
            Player p = players[0];
            int x = 0;
            int y = 0;
            int h = 20;

            string mino = (p.mino != null) ? p.mino.ToString() : "null";

            DX.DrawString(x, y+=h, "[now]:"+mino, DX.GetColor(255, 255, 255));
            DX.DrawString(x, y+=h, "[hold]:"+p.hold.ToString(), DX.GetColor(255, 255, 255));

            for (int i=0; i<players[0].que.Count(); i++)
            {
                MinoType type = players[0].que[i];
                DX.DrawString(x, y+=h, "["+(i+1)+"]:"+p.que[i].ToString(), DX.GetColor(255, 255, 255));
            }
            
            DX.DrawString(x, y+=h, "[level]:"+p.cFall.level, DX.GetColor(255, 255, 255));
            DX.DrawString(x, y+=h, "[next]:"+p.cFall.next, DX.GetColor(255, 255, 255));
            DX.DrawString(x, y+=h, "[conut]:"+p.cFall.count, DX.GetColor(255, 255, 255));
            DX.DrawString(x, y+=h, "[fall]:"+p.cFall.CellPerFrame(), DX.GetColor(255, 255, 255));



            Draw(players[0]);




            DX.SetDrawScreen(handler);
            DX.DrawGraph(700, 700, hHmd, DX.TRUE);

            return handler;
        }








        private void Draw(Player p)
        {
            DX.SetDrawScreen(hHmd);
            DX.ClearDrawScreen();

            // 背景を一色に
            Rect rect = HmdDefine.GetHMDSize();
            DX.DrawBox(rect.x, rect.y, rect.w, rect.h, DX.GetColor(255, 255, 255), DX.TRUE);

            // 枠を描画
            Rect tr = HmdDefine.GetOuterFrameRect();
            DX.DrawRoundRect(tr.x, tr.y, tr.w, tr.h, CURVE, CURVE, DX.GetColor(0, 0, 0), DX.TRUE);
            Rect br = HmdDefine.GetInnerFrameRect();
            DX.DrawRoundRect(br.x, br.y, br.w, br.h, CURVE, CURVE, DX.GetColor(255, 255, 255), DX.TRUE);


            Pos def = GetDefaultPad();


            // プレイヤー描画
            Pos nnp = HmdDefine.GetNamePos();
            DX.DrawStringToHandle(nnp.x, nnp.y, p.name, DX.GetColor(255, 0, 0), FONTB);


            //　スコア描画
            int c = PAD + W_LINE + PAD + PAD;
            DX.DrawStringFToHandle(def.x, c, "SCORE", DX.GetColor(255, 0, 0), FONTM);

            Pos sp = GetScorePos(p.score.ToString());
            DX.DrawStringFToHandle(sp.x, sp.y, p.score.ToString(), DX.GetColor(255, 0, 0), FONTM);


            // レベル描画
            c += W_FONTM + PAD;
            DX.DrawStringFToHandle(def.x, c, "LEVEL", DX.GetColor(255, 0, 0), FONTM);

            Pos lp = GetLevelPos(p.cFall.level.ToString());
            DX.DrawStringFToHandle(lp.x, lp.y, p.cFall.level.ToString(), DX.GetColor(255, 0, 0), FONTM);


            // ネクスト描画
            c += W_FONTM + PAD;
            DX.DrawStringFToHandle(def.x, c, "NEXT", DX.GetColor(255, 0, 0), FONTM);

            Pos np = GetNextPos((p.cFall.next-p.cFall.count).ToString());
            DX.DrawStringFToHandle(np.x, np.y, (p.cFall.next - p.cFall.count).ToString(), DX.GetColor(255, 0, 0), FONTM);

            // ホールドみの
            c += W_FONTM + PAD;
            DX.DrawStringToHandle(def.x, c, "HOLD", DX.GetColor(255, 0, 0), FONTS);

            Rect hr = GetHoldRect();
            DX.DrawBox(hr.x, hr.y, hr.w, hr.h, DX.GetColor(0, 255, 0), DX.TRUE);

            // nextみの
            DX.DrawStringToHandle(PLAYER_W - def.x, c, "NEXT", DX.GetColor(255, 0, 0), FONTS);

            Rect nr = GetHoldRect();
            DX.DrawBox(nr.x, nr.y, nr.w, nr.h, DX.GetColor(0, 255, 0), DX.TRUE);


        }

    }
}

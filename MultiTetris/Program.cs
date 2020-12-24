using DxLibDLL;
using MultiTetris.Events;
using MultiTetris.Maps;
using MultiTetris.Players;
using MultiTetris.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MultiTetris.GameDefine;
using static MultiTetris.DrawDefine;

namespace MultiTetris.Mains
{
    class Program
    {
        static void Main(string[] args)
        {
            // アルファチャンネル周りにバグがあるっぽい
            
            DX.SetGraphMode(WINDOW_W, WINDOW_H, 32, 60);
            //DX.SetWindowInitPosition(1366, 0);
            DX.SetWindowInitPosition(1920-1200, 10);
            DX.SetUse3DFlag(DX.FALSE);
            DX.ChangeWindowMode(DX.TRUE);


            if (DX.DxLib_Init() == -1) return;
            int sx;
            int sy;
            int sc;

            DX.GetScreenState(out sx, out sy, out sc);
            Console.WriteLine("DXLib init. size=({0}x{1}), color={2}",sx, sy, sc);






            Console.WriteLine("Roop start");
            Program.MainLoop();
            Console.WriteLine("Roop end");

            
            DX.DxLib_End();
            
        }












        static void MainLoop()
        {
            // スクリーン作成
            int hScreen = DX.MakeScreen(SCREEN_W, SCREEN_H, DX.FALSE);
            Console.WriteLine("Create back screen. h={0}", hScreen);


            // 相互参照
            MapManager map = new MapManager(FIELD_W, FIELD_H, PLAYER_NUM);
            PlayerManager plm = new PlayerManager(map);
            EventManager evm = new EventManager(map, plm);


            Pos minoPos = GetMinoMapPosition();
            
            byte[] key = new byte[256];
            byte[] input = new byte[256];
            while (evm.run && DX.ProcessMessage() == 0)
            {
                //DX.clsDx();

                // マップ初期化
                map.TurnInit();

                // キー取得
                DX.GetHitKeyStateAll(out input[0]);
                for (int i=0; i<key.Length; i++)
                {
                    key[i] = (byte)((input[i] == 0) ? 0 : key[i] + 1);
                }

                // エスケープ処理
                if (key[DX.KEY_INPUT_ESCAPE] > 0)
                {
                    Console.WriteLine("!!!ESCAPE!!!");
                    DX.WaitKey();
                    break;
                }

                // 各種アップデート
                plm.Update(key);
                evm.Update(key);
                if (plm.IsEmpty()) break;


                // [描画]ベースを作成
                int field = map.UpdateFieldGraphics();

                // [描画]コピーハンドルに描画
                DX.SetDrawScreen(hScreen);
                DX.ClearDrawScreen();
                DX.DrawGraph(minoPos.x, minoPos.y, field, DX.TRUE);

                plm.UpdateFieldGraphics(hScreen);
                evm.UpdateFieldGraphics(hScreen);

                // [描画]裏画面に描画
                DX.SetDrawScreen(DX.DX_SCREEN_BACK);
                DX.ClearDrawScreen();
                DX.DrawExtendGraph(0, 0, WINDOW_W, WINDOW_H, hScreen, DX.TRUE);

                // 表画面に描写
                DX.ScreenFlip();
            }

            Console.WriteLine("!!!GAME OVER!!!");
            DX.WaitKey();

            DX.WaitKey();


        }
    }
}

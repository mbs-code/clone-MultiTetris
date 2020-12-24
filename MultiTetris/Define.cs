using MultiTetris.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MultiTetris.GameDefine;
using DxLibDLL;

namespace MultiTetris
{
    class GameDefine
    {
        public static readonly int WINDOW_W = 1920;
        public static readonly int WINDOW_H = 1080;

        public static readonly int SCREEN_W = 1920;
        public static readonly int SCREEN_H = 1080;

        public static readonly int FIELD_W = 10;
        public static readonly int FIELD_H = 30;
        public static readonly int PLAYER_NUM = 1;
    }


    class PointDefine
    {
        //public static readonly int 


    }


    class HmdDefine
    {
        public static readonly int PLAYER_W = 300;
        //public static readonly int PLAYER_H = 100;

        public static readonly int PAD = 15; // [描画] 標準余白

        public static readonly int W_FONTB = 30;  // [描画] フォントサイズ大
        public static readonly int W_FONTM = 25; // [描画] フォントサイズ中
        public static readonly int W_FONTS = 20; // [描画] フォントサイズ小

        public static readonly int W_ICON = 48;  // [描画] アイコンサイズ
        public static readonly int W_LINE = 5;

        public static readonly int CURVE = 5;

        public static readonly int FONTB = DX.CreateFontToHandle(null, W_FONTB, -1 -1);
        public static readonly int FONTM = DX.CreateFontToHandle(null, W_FONTM, -1 - 1);
        public static readonly int FONTS = DX.CreateFontToHandle(null, W_FONTS, -1 - 1);

        public static Pos GetDefaultPad()
        {
            int x = PAD + W_LINE + PAD;
            int y = PAD + W_LINE + PAD + PAD;
            return new Pos(x, y);
        }

        public static Rect GetHMDSize()
        {
            int x = 0;
            int y = 0;
            //int w = PAD + W_LINE + PAD + (FONT_M*3.5) + ;
            int w = PLAYER_W;
            int h = PAD + W_FONTB + PAD + W_FONTB + PAD + W_FONTB + PAD + W_FONTB + PAD
                + W_FONTS + W_ICON + PAD + W_FONTS + W_ICON +PAD + W_LINE + PAD;
            return new Rect(x, y, w, h);
        }
        

        public static Rect GetOuterFrameRect()
        {
            Rect r = GetHMDSize();
            int x = PAD;
            int y = PAD;
            int w = r.w - PAD;
            int h = r.h - PAD;
            return new Rect(x, y, w, h);
        }

        public static Rect GetInnerFrameRect()
        {
            Rect r = GetHMDSize();
            int x = PAD + W_LINE;
            int y = PAD + W_LINE;
            int w = r.w - PAD - W_LINE;
            int h = r.h - PAD - W_LINE;
            return new Rect(x, y, w, h);
        }
        
        public static Pos GetNamePos()
        {
            int x = PAD + W_LINE + PAD;
            int y = 0;
            return new Pos(x, y);
        }

        public static Pos GetScorePos(string mes)
        {
            int w = DX.GetDrawStringWidthToHandle(mes, mes.Length, FONTM);
            int x = PLAYER_W - (PAD + W_LINE + PAD) - w;
            int y = (PAD + W_LINE + PAD + PAD);

            return new Pos(x, y);
        }

        public static Pos GetLevelPos(string mes)
        {
            int w = DX.GetDrawStringWidthToHandle(mes, mes.Length, FONTM);
            int x = PLAYER_W - (PAD + W_LINE + PAD) - w;
            int y = (PAD + W_LINE + PAD + PAD) + (W_FONTM + PAD);
            return new Pos(x, y);
        }

        public static Pos GetNextPos(string mes)
        {
            int w = DX.GetDrawStringWidthToHandle(mes, mes.Length, FONTM);
            int x = PLAYER_W - (PAD + W_LINE + PAD) - w;
            int y = (PAD + W_LINE + PAD + PAD) + (W_FONTM + PAD) + (W_FONTM + PAD);
            return new Pos(x, y);
        }

        public static Rect GetHoldRect()
        {
            int x = (PAD + W_LINE + PAD);
            int y = (PAD + W_LINE + PAD + PAD) + (W_FONTM + PAD) + (W_FONTM + PAD) + (W_FONTM + PAD)
                + W_FONTS;
            int w = x + W_ICON;
            int h = y + W_ICON;
            return new Rect(x, y, w, h);
        }

        public static Rect GetNextRect()
        {
            int x = (PAD + W_LINE + PAD);
            int y = (PAD + W_LINE + PAD + PAD) + (W_FONTM + PAD) + (W_FONTM + PAD) + (W_FONTM + PAD)
                + W_FONTS;
            int w = x + W_ICON;
            int h = y + W_ICON;
            return new Rect(x, y, w, h);
        }
    }



    class DrawDefine
    {
        public static readonly int MINO_NUM     = 8;    // [描画] ミノ種類の数
        public static readonly int MINO_SIZE    = 48;   // [描画] ミノロードサイズ

        public static readonly int W_LINE       = 2;    // [描画]線の幅
        public static readonly int W_MINO       = 40;   // [描画]ミノの幅

        public static readonly int PAD          = 20;   // [描画]マップ周囲余白

        public static readonly int APPEND_LINE  = 10;  // ミノの出現限界ライン
        public static readonly int SHOW_LINE    = 9;   // ミノの描画最高ライン

        /// <summary>
        /// セルの描画範囲(map準拠)
        /// </summary>
        /// <param name="width">横軸のセル番号</param>
        /// <param name="height">縦軸のセル番号</param>
        /// <returns>描画範囲</returns>
        public static Rect GetCellRect(int width, int height)
        {
            int x = PAD + W_LINE + (W_MINO + W_LINE) * width;
            int y = PAD + W_LINE + (W_MINO + W_LINE) * height;
            int w = x + W_MINO;
            int h = y + W_MINO;
            return new Rect(x, y, w, h);
        }

        /// <summary>
        /// 横線の描画範囲(map準拠)
        /// </summary>
        /// <param name="rowId">横線のID</param>
        /// <returns>描画範囲</returns>
        public static Rect GetRowLine(int rowId)
        {
            int ww = (W_LINE + W_MINO) * FIELD_W + W_LINE;
            int hh = (W_LINE + W_MINO) * rowId;

            int x = PAD;
            int y = PAD + hh;
            int w = PAD + ww;
            int h = PAD + hh + W_LINE;
            return new Rect(x, y, w, h);
        }

        /// <summary>
        /// 縦線の描画範囲(map準拠)
        /// </summary>
        /// <param name="colId">縦線のID</param>
        /// <returns>描画範囲</returns>
        public static Rect GetColumnLine(int colId)
        {
            int ww = (W_LINE + W_MINO) * colId;
            int hh = (W_LINE + W_MINO) * (FIELD_H - SHOW_LINE) + W_LINE;

            int x = PAD + ww;
            int y = PAD;
            int w = PAD + ww + W_LINE;
            int h = PAD + hh;
            return new Rect(x, y, w, h);
        }

        /// <summary>
        /// ミノマップの貼り付け先左上座標(sc準拠)
        /// </summary>
        /// <returns>左上座標</returns>
        public static Pos GetMinoMapPosition()
        {
            return new Pos(100, 20);
        }

        /// <summary>
        /// ミノマップの行エリア(sc準拠)
        /// </summary>
        /// <param name="rowId"></param>
        /// <returns>描画範囲</returns>
        public static Rect GetRowRect(int rowId)
        {
            rowId -= SHOW_LINE;

            Pos p = GetMinoMapPosition();
            Rect t = GetRowLine(rowId);
            Rect u = GetRowLine(rowId+1);

            int x = p.x + t.x;
            int y = p.y + t.y;
            int w = p.x + u.w;
            int h = p.y + u.h;
            return new Rect(x, y, w, h);
        }


    }

    class SoundDefine
    {
        private static readonly int hMinoMoveSound;
        private static readonly int hMinoRotSound;
        private static readonly int hLineClearSound;

        static SoundDefine() {
            hMinoMoveSound  = DX.LoadSoundMem(@"src\move.mp3");
            hMinoRotSound   = DX.LoadSoundMem(@"src\rot.mp3");
            hLineClearSound = DX.LoadSoundMem(@"src\lineclear.mp3");
        }

        public static void LineClearSound()
        {
            DX.PlaySoundMem(hLineClearSound, DX.DX_PLAYTYPE_BACK, DX.TRUE);
        }

        public static void MinoMoveSound()
        {
            DX.PlaySoundMem(hMinoMoveSound, DX.DX_PLAYTYPE_BACK, DX.TRUE);
        }

        public static void MinoRotSound()
        {
            //DX.PlaySoundMem(hMinoRotSound, DX.DX_PLAYTYPE_BACK, DX.TRUE);
        }
    }
}

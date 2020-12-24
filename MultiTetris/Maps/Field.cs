using MultiTetris.Minos;
using MultiTetris.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTetris.Maps
{
    enum CheckType { OK, ERR_OVERLAP, ERR_LEFT_END, ERR_RIGHT_END, ERR_TOP_END, ERR_BOTTOM_END };

    class Field
    {
        
        public MinoType[,] field { get; }



        /// <summary>
        /// フィールド本体（初期化あり）
        /// </summary>
        /// <param name="width">横セル数(>0)</param>
        /// <param name="height">縦セル数(デカめに取らないと事故る)(>0)</param>
        public Field(int width, int height)
        {
            if (width < 0 || height < 0)
                throw new System.ArgumentException("マップサイズは正の値である必要があります。 w=" + width + "h=" + height);
            
            field = new MinoType[width, height];
            this.Clear();
        }



        /// <summary>
        /// フィールドを deep copy する
        /// </summary>
        /// <param name="f">コピー元のフィールド</param>
        public void Marge(Field f)
        {
            //Console.WriteLine("Field Marge.");
            
            MinoType[,] of = f.field;
            int width = of.GetLength(0);
            int height = of.GetLength(1);
            
            for (int j = 0; j < height; j++)
                for (int i = 0; i < width; i++)
                    field[i, j] = of[i, j];
        }

        
















        /// <summary>
        /// フィールドにミノを配置する
        /// </summary>
        /// <param name="mino">配置するミノ</param>
        /// <returns>成功可否</returns>
        public bool Add(Mino mino)
        {
            if (mino == null)
            {
                return false;
            }

            if (Check(mino.pos, mino.grid) == CheckType.OK)
            {   
                //Console.WriteLine("Append mino. "+mino);
                this.Set(mino.type, mino.pos, mino.grid);
                return true;
            }

            return false;
        }







        /// <summary>
        /// ミノを配置する(事前にCheckを通すべし！).
        /// </summary>
        /// <param name="type">配置ミノ種類</param>
        /// <param name="pos">基準左上座標</param>
        /// <param name="grid">ミノ配置グリッド</param>
        private void Set(MinoType type, Pos pos, bool[,] grid)
        {
            int width = grid.GetLength(0);
            int height = grid.GetLength(1);

            for (int j = 0; j < height; j++)
                for (int i = 0; i < width; i++)
                    if (grid[i, j])
                    {
                        int x = i + pos.x;
                        int y = j + pos.y;
                        //Console.WriteLine("size:{0}x{1}, pos:{2}x{3}", GetWidth(), GetHeight(), x, y);
                        field[x, y] = type;
                    }
        }





        /// <summary>
        /// ミノを配置できるか確認
        /// </summary>
        /// <param name="pos">基準左上座標</param>
        /// <param name="grid">ミノ配置グリッド</param>
        /// <returns>処理結果</returns>
        public CheckType Check(Pos pos, bool[,] grid)
        {
            int width = grid.GetLength(0);
            int height = grid.GetLength(1);

            for (int j=0; j<height; j++)
                for (int i=0; i<width; i++)
                    if (grid[i, j])
                    {
                        int x = i + pos.x;
                        int y = j + pos.y;
                        //Console.WriteLine("size:{0}x{1}, pos:{2}x{3}",GetWidth(), GetHeight(), x, y);
                        if (x < 0)              return CheckType.ERR_LEFT_END;
                        if (x >= GetWidth())    return CheckType.ERR_RIGHT_END;
                        if (y < 0)              return CheckType.ERR_TOP_END;
                        if (y >= GetHeight())   return CheckType.ERR_BOTTOM_END;
                        if (field[x, y] != MinoType.Null) return CheckType.ERR_OVERLAP;
                    }  

            return CheckType.OK;
        }
        

        /// <summary>
        /// フィールドの初期化
        /// </summary>
        public void Clear()
        {
            int width = field.GetLength(0);
            int height = field.GetLength(1);
            //Console.WriteLine("Field initilized. w="+width+", h="+height);

            for (int i = 0; i < field.Length; i++) {
                field[i%width, i/height] = MinoType.Null;
            }
        }

        /// <summary>
        /// フィールドの仮出力
        /// </summary>
        public void Dump()
        {
            int width = field.GetLength(0);
            int height = field.GetLength(1);
            Console.WriteLine("Field Dump. w=" + width + ", h=" + height);

            for (int j=0; j<height; j++)
            {
                string line = "";
                for (int i=0; i<width; i++)
                {
                    line += field[i, j].ToString()[0];
                }
                Console.WriteLine(line);
            }
        }





        public int GetWidth()
        {
            return field.GetLength(0);
        }

        public int GetHeight()
        {
            return field.GetLength(1);
        }

    }
}

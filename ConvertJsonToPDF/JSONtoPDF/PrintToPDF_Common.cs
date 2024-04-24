using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Drawing.Printing;
using System.Drawing;


namespace ConvertJsonToPDF.JSONtoPDF
{
    static class PrintToPDF_Common
    {
        /// <summary>
        /// 【A4縦共通・デバッグ用】縦横幅20ずつの水色方眼を印字
        /// </summary>
        public static void CreateBaseLine(PrintPageEventArgs e)
        {
            Pen p = new Pen(Color.FromArgb(128, Color.LightSkyBlue));

            //縦線
            for (int x = Const.C_LEFT_M; x <= Const.C_RIGHT_M; x += 20)
            {
                e.Graphics.DrawLine(p, x, Const.C_TOP_M, x, Const.C_BOTTOM_M);
            }
            //横線
            for (int y = Const.C_TOP_M; y <= Const.C_BOTTOM_M; y += 20)
            {
                e.Graphics.DrawLine(p, Const.C_LEFT_M, y, Const.C_RIGHT_M, y);
            }
        }

        /// <summary>
        /// 四角形を描画
        /// </summary>
        /// <param name="e"></param>
        /// <param name="xTopLeft">四角形左上のx座標</param>
        /// <param name="yTopLeft">四角形左上のy座標</param>
        /// <param name="xBottomRight">四角形右下のx座標</param>
        /// <param name="yBottomRight">四角形右下のx座標</param>
        public static　void  CreateSquare(PrintPageEventArgs e, int xLeft, int xRight, int yTop, int yBottom)
        {
            Pen p = new Pen(Color.Black);
            e.Graphics.DrawRectangle(p, GetCoordinate(xLeft, xRight, yTop, yBottom));
        }

        //（画像、文字共通）座標からRectangle型に変換
        //DrawLineの場合は、この枠に沿って線が描画される。
        //DrawStringの場合は、この枠のX軸右端に達すると自動改行される。Y軸の限界を超えると文字は消える
        //また、文字列内に「\r\n」で改行可能。
        public static Rectangle GetCoordinate(int xTopLeft, int xBottomRight, CoordinateData yData)
        {
            return new Rectangle(xTopLeft, yData.CoordinateStart, xBottomRight - xTopLeft, yData.CoordinateEnd - yData.CoordinateStart);
        }
        public static Rectangle GetCoordinate(int xTopLeft, int xBottomRight, int yTopLeft, int yBottomRight)
        {
            return new Rectangle(xTopLeft, yTopLeft, xBottomRight - xTopLeft, yBottomRight - yTopLeft);
        }

    }
}
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Web;

using System.IO;
using System.Drawing;
using ConvertJsonToPDF.Models;
using ConvertJsonToPDF.Tools;

namespace ConvertJsonToPDF.JSONtoPDF
{
    public class PrintPDF_OnamaeSeal
    {
        public enum textType
        {
            //タイトル専用
            tytle,
            //左端の項目
            header1,
            //各固定値
            header2,
            //実データ
            data1,
            //右揃えしたい文字
            rightPrint
        }

        Dictionary<int, Models.JsonObject> OrderDataCollection;

        private int curPageNumber = 1;
        private int curDataNumber = 1;
        private string baseText = "";
        private string stringToPrint = "";
        private bool isNewPage = true;


        /// <summary>
        /// メイン処理
        /// </summary>
        public byte[] PrintTextToPDF()
        {
            //テンプレートとなるファイル（最終的にここは最初から組み込んだほうがいいかも
            PrintDocument doc = GetDocumentSettings();
            //「印刷中..」を出さないようにする→これやると出すよりも時間が数倍になるらしいので、触らないほうが無難？
            doc.PrintController = new System.Drawing.Printing.StandardPrintController();

            //仮データをセット。実際はここでsampleDataListに該当する箇所に渡されてきた伝票情報を格納する
            OrderDataCollection = new ConvertJSONtoPDF().GetJsonSampleList();

            //ページ初期化(DataNumberは2ページに渡っても1として計算するので、必ず「DataNumber<=PageNumber」になる
            curPageNumber = 0;
            curDataNumber = 0;

            //ページ内の記述(実際はPrint()実行する際にデータが作られてるっぽい？）
            isNewPage = true;
            doc.PrintPage += new PrintPageEventHandler(pd_PrintPage);
            doc.Print();

            //↑↑↑↑↑　2024/04/19　↑↑↑↑↑
            //通常のファイル出力できるもの(System.IO.FileStream)なら、MemoryStreamを使うことでメモリ上に疑似的にファイル作成することが可能。
            //つまり、この.Print()の処理のうち、ファイル出力部分が弄るor設定変更できれば、やらうとしていたことが実現可能！？

            //これでバイトにできてるっぽいが、できてる場合中身どうなってる？実データ以外の情報は？これだけでPDFが作り直せるのか？
            //できれば、1回PDF出力をしないでバイナリデータに変換したい
            byte[] bs = System.IO.File.ReadAllBytes(doc.PrinterSettings.PrintFileName);


            //バイナリに変換してしまえば元のファイルは不要
            File.Delete(doc.PrinterSettings.PrintFileName);

            return bs;

            ////検証はこのやり方で良い？→しかし、同じ環境下同プログラム内だったら、byte[]で情報が完結してなかったとしても気づかないのでは…
            //var docPath = @"D:\source\実験\スタイルシート初作成\";
            //var PrintFileName = Path.Combine(docPath, DateTime.Now.ToString("yyyyMMddHHmmss") + "再構築" + ".pdf");
            //System.IO.FileStream fs = new FileStream(PrintFileName, System.IO.FileMode.Create, System.IO.FileAccess.Write);

            //fs.Write(bs, 0, bs.Length);

            ////メモリへの読み書きなので、こっちなら態々実ファイルを作る必要はない
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    //byte[]全部書き込み
            //    ms.Write(bs, 0, bs.Length);
            //}

        }

        /// <summary>
        /// メイン処理(引数あり)
        /// </summary>
        public byte[] PrintTextToPDF(Product[] products)
        {
            //テンプレートとなるファイル（最終的にここは最初から組み込んだほうがいいかも
            PrintDocument doc = GetDocumentSettings();
            //「印刷中..」を出さないようにする→処理が遅いと感じたらこれを解除してもOK
            doc.PrintController = new System.Drawing.Printing.StandardPrintController();

            //データをセット。実際はここでsampleDataListに該当する箇所に渡されてきた伝票情報を格納する
            OrderDataCollection = new ConvertJSONtoPDF().ConvertJsonList(products);

            //ページ初期化(DataNumberは2ページに渡っても1として計算するので、必ず「DataNumber<=PageNumber」になる
            curPageNumber = 0;
            curDataNumber = 0;

            //ページ内の記述(実際はPrint()実行する際にデータが作られてるっぽい？）
            isNewPage = true;
            doc.PrintPage += new PrintPageEventHandler(pd_PrintPage);
            doc.Print();

            LogUtil.Info("PDF" + curPageNumber.ToString() + "ページ作成");

            //↑↑↑↑↑　2024/04/19　↑↑↑↑↑
            //通常のファイル出力できるもの(System.IO.FileStream)なら、MemoryStreamを使うことでメモリ上に疑似的にファイル作成することが可能。
            //つまり、この.Print()の処理のうち、ファイル出力部分が弄るor設定変更できれば、やらうとしていたことが実現可能！？


            //↓一旦ここは処理が重くなるのでコメントアウトする（最終的に復活させる


            //これでバイトにできてるっぽいが、できてる場合中身どうなってる？実データ以外の情報は？これだけでPDFが作り直せるのか？
            //できれば、1回PDF出力をしないでバイナリデータに変換したい
            byte[] bs = System.IO.File.ReadAllBytes(doc.PrinterSettings.PrintFileName);


            //バイナリに変換してしまえば元のファイルは不要
            //File.Delete(doc.PrinterSettings.PrintFileName);

            return bs;

            ////検証はこのやり方で良い？→しかし、同じ環境下同プログラム内だったら、byte[]で情報が完結してなかったとしても気づかないのでは…
            //var docPath = @"D:\source\実験\スタイルシート初作成\";
            //var PrintFileName = Path.Combine(docPath, DateTime.Now.ToString("yyyyMMddHHmmss") + "再構築" + ".pdf");
            //System.IO.FileStream fs = new FileStream(PrintFileName, System.IO.FileMode.Create, System.IO.FileAccess.Write);

            //fs.Write(bs, 0, bs.Length);

            ////メモリへの読み書きなので、こっちなら態々実ファイルを作る必要はない
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    //byte[]全部書き込み
            //    ms.Write(bs, 0, bs.Length);
            //}

        }


        /// <summary>
        /// PDF全体の初期設定
        /// </summary>
        /// <returns></returns>
        private PrintDocument GetDocumentSettings()
        {
            string docPath = @"D:\source\実験\スタイルシート初作成";
            string baseFileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_";
            string fullPath = Path.Combine(baseFileName + ".pdf");   //仮

            //流石に1秒以内で50以上の同時出力はない想定。
            for (int i = 1; i<=50; i++)
            {
                fullPath = Path.Combine(docPath, baseFileName + i.ToString() + ".pdf");
                if (File.Exists(fullPath))
                {
                    continue;
                }
                else
                {
                    break;
                }
            }

            PrintDocument doc = new PrintDocument()
            {
                PrinterSettings = new PrinterSettings()
                {
                    // Windows10 から利用可能
                    PrinterName = "Microsoft Print to PDF",
                    MaximumPage = 1,
                    ToPage = 1,
                    // 縦向き印刷
                    DefaultPageSettings =
                    {
                        Landscape = false,
                    },
                    //出力先をfileに指定する
                    PrintToFile = true,
                    PrintFileName = fullPath,
                }
            };

            // サイズはA4
            doc.DefaultPageSettings.PaperSize = doc.PrinterSettings.PaperSizes.OfType<PaperSize>().First(x => x.Kind == PaperKind.A4);
            // 余白無し
            doc.DefaultPageSettings.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
            return doc;
        }

        //固定値の、線や文字の描画（テスト用ライン含む）
        #region 


        //出力するPDFの、固定された枠の情報を作成
        private void CreateLineAndBox(object sender, PrintPageEventArgs e)
        {
            Pen p = new Pen(Color.Black);

            //上の枠のみ。下の枠や横線は文字列追加しながら描画したほうがいい
            PrintToPDF_Common.CreateSquare(e, Const.C_LEFT_M, Const.C_RIGHT_M, 160, 520);

        }


        private void CreateText(object sender, PrintPageEventArgs e)
        {
            //ここはヘッダ部分。ページ数や日時など変動値はあるが、データの中身は依存しない////////////////////////////////
            var yData = new CoordinateData();

            //タイトル
            yData.CoordinateStart = 40;
            yData.CoordinateEnd = yData.CoordinateStart + 40;
            CreateText(sender, e, "お名前シール 受注明細", PrintToPDF_Common.GetCoordinate(40, 500, yData), textType.tytle);

            //発行日（system日付なので、PDFデータ作成日時になる）
            yData.CoordinateStart = 40;
            yData.CoordinateEnd = yData.CoordinateStart + 20;
            CreateText(sender, e, "発行日： " + DateTime.Now.ToString(), PrintToPDF_Common.GetCoordinate(600, 800, yData), textType.header2);

            //ページ数（ページ数の取得は未実装。そこは現状動作確認ができない為、ここはJSONデータ取込処理実装後になる）
            yData.CoordinateStart = 60;
            yData.CoordinateEnd = yData.CoordinateStart + 20;

            //★★★★★★総ページ数取得の為、改ページが必要な伝票数は先に把握しないといけないのでは！？
            //どうせ3データ超えたら改行するのであれば、3行超えたデータは2ページとして扱えばいい（3枚行くことあるなら別途考える
            var cnt = OrderDataCollection.Count + OrderDataCollection.Count(x => x.Value.bikoList.Count() >= 3);
            CreateText(sender
                     , e
                     , curPageNumber.ToString(GetCountToZeroFormat()) + " / " + cnt.ToString() + "ページ"
                     , PrintToPDF_Common.GetCoordinate(600, 800, yData)
                     , textType.header2);

            //ヘッダここまで////////////////////////////////////////////////////////////////////////////////////////////

            //毎回書くと可読性と拡張性下がるので、このページのJson情報取り出しておく
            var jData = OrderDataCollection[curDataNumber];

            //配送枠の枠内を作成(店舗名だけは枠の外に出る
            CreateTextTransport(sender, e, jData);
            CreateTextBiko(sender, e, jData);
            CreateTextMeisai(sender, e, jData);
        }

        //2ページ目レイアウト
        private void CreateTextAddPage(object sender, PrintPageEventArgs e)
        {
            //ここはヘッダ部分。ページ数や日時など変動値はあるが、データの中身は依存しない////////////////////////////////
            var yData = new CoordinateData();

            //タイトル
            yData.CoordinateStart = 40;
            yData.CoordinateEnd = yData.CoordinateStart + 40;
            CreateText(sender, e, "お名前シール 受注明細(2枚目)", PrintToPDF_Common.GetCoordinate(40, 600, yData), textType.tytle);

            //発行日（system日付なので、PDFデータ作成日時になる）
            yData.CoordinateStart = 40;
            yData.CoordinateEnd = yData.CoordinateStart + 20;
            CreateText(sender, e, "発行日： " + DateTime.Now.ToString(), PrintToPDF_Common.GetCoordinate(600, 800, yData), textType.header2);

            //ページ数（ページ数の取得は未実装。そこは現状動作確認ができない為、ここはJSONデータ取込処理実装後になる）
            yData.CoordinateStart = 60;
            yData.CoordinateEnd = yData.CoordinateStart + 20;

            //★★★★★★総ページ数取得の為、改ページが必要な伝票数は先に把握しないといけないのでは！？
            //どうせ3データ超えたら改行するのであれば、3行超えたデータは2ページとして扱えばいい（3枚行くことあるなら別途考える
            var cnt = OrderDataCollection.Count + OrderDataCollection.Count(x => x.Value.bikoList.Count() >= 3);
            CreateText(sender
                     , e
                     , curPageNumber.ToString(GetCountToZeroFormat()) + " / " + cnt.ToString() + "ページ"
                     , PrintToPDF_Common.GetCoordinate(600, 800, yData)
                     , textType.header2);

            //ヘッダここまで////////////////////////////////////////////////////////////////////////////////////////////

            //データ取得…は、実際受注IDと明細部分さえ取れればいい
            var orderId = OrderDataCollection[curDataNumber].受注ID;
            //最初のページに印字できなかったデータ
            List<MeisaiData> bikoList = OrderDataCollection[curDataNumber].bikoList.Where(x => !x.isPrintFirstPage).ToList();

            if (bikoList?.Count <= 0)
            {
                //2データ未満だと、本来改ページされない
                return;
            }

            //枠外店舗異名
            yData.CoordinateStart = 140;
            yData.CoordinateEnd = yData.CoordinateStart + 20;
            CreateText(sender, e, "■明細  " + orderId, PrintToPDF_Common.GetCoordinate(20, 400, yData), textType.header2);

            //ヘッダ(1行)
            yData.CoordinateStart = 160;
            yData.CoordinateEnd = yData.CoordinateStart + 30;
            CreateText(sender, e, "自社コード/商品名/項目選択肢", PrintToPDF_Common.GetCoordinate(30, 750, yData), textType.header2);
            CreateText(sender, e, "個数", PrintToPDF_Common.GetCoordinate(750, 800, yData), textType.data1);

            //明細(n行)
            //個数(n行)
            int rowCount = 0;
            //明細の数は変動する想定にしておかないといけない
            foreach (MeisaiData bikos in bikoList)
            {
                rowCount++;
                CreateTextMeisaiRow(sender, e, rowCount, bikos.Meisai, bikos.ItemCount,200);
            }


            //外枠の縦幅は、データの数によって動的にしないといけないのでここで設定する
            //160：目次の手前のy座標
            //200：明細欄の開始位置
            //110：1行の明細の縦幅
            // 10：最終行文字列が行でわかり難くなるのを避けるための措置
            PrintToPDF_Common.CreateSquare(e, Const.C_LEFT_M, Const.C_RIGHT_M, 160, 200 + (110 * rowCount) + 10);

        }
        /// <summary>
        /// 取り込んでいるデータの総数から、何文字分0埋めするか判断し、String変換時のフォーマットにする
        /// (用途：ページ数の分子分母の印字）
        /// </summary>
        /// <returns></returns>
        private string GetCountToZeroFormat()
        {
            var len = OrderDataCollection.Count.ToString().Length;
            string zeroFormat = "";
            for (int i = 1; i <= len; i++)
            {
                zeroFormat += "0";
            }
            return zeroFormat;
        }

        /// <summary>
        /// 明細上半分の枠内の情報を構築
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="jData"></param>
        private void CreateTextTransport(object sender, PrintPageEventArgs e, Models.JsonObject jData)
        {
            var yData = new CoordinateData();

            //枠外店舗異名
            yData.CoordinateStart = 110;
            yData.CoordinateEnd = yData.CoordinateStart + 30;

            CreateText(sender, e, jData.店舗名, PrintToPDF_Common.GetCoordinate(20, 200, yData), textType.header1);

            //枠内開始位置
            yData.CoordinateStart = 180;
            yData.CoordinateEnd = yData.CoordinateStart + 20;

            //X軸の点線（二列Ver前半）
            //X軸の点線（二列Ver後半）
            //X軸の点線（一列Ver）
            CoordinateData xData1Col = new CoordinateData() { CoordinateStart = 200, CoordinateEnd = 790 };
            CoordinateData xData2Col1 = new CoordinateData() { CoordinateStart = 200, CoordinateEnd = 490 };
            CoordinateData xData2Col2 = new CoordinateData() { CoordinateStart = 500, CoordinateEnd = 790 };

            CreateText(sender, e, "■受注管理情報", PrintToPDF_Common.GetCoordinate(30, 200, yData), textType.header1);
            CreateText(sender, e, "受注ID：", PrintToPDF_Common.GetCoordinate(200, 300, yData), textType.header2);
            CreateText(sender, e, jData.受注ID, PrintToPDF_Common.GetCoordinate(300, 500, yData), textType.data1);
            CreateText(sender, e, "購入日時：", PrintToPDF_Common.GetCoordinate(500, 600, yData), textType.header2);
            CreateText(sender, e, jData.購入日時, PrintToPDF_Common.GetCoordinate(600, 800, yData), textType.data1);
            CreateRowLine(sender, e, yData.CoordinateEnd, xData2Col1, xData2Col2);

            yData = GetCoordinateData(yData, 1.5);
            CreateText(sender, e, "■配送情報", PrintToPDF_Common.GetCoordinate(30, 200, yData), textType.header1);
            CreateText(sender, e, "配送業者：", PrintToPDF_Common.GetCoordinate(200, 300, yData), textType.header2);
            CreateText(sender, e, jData.宅配業者名, PrintToPDF_Common.GetCoordinate(300, 500, yData), textType.data1);
            CreateText(sender, e, "出荷予定日：", PrintToPDF_Common.GetCoordinate(500, 600, yData), textType.header2);
            CreateText(sender, e, jData.出荷予定日, PrintToPDF_Common.GetCoordinate(600, 800, yData), textType.data1);
            CreateRowLine(sender, e, yData.CoordinateEnd, xData2Col1, xData2Col2);

            yData = GetCoordinateData(yData, 0.5);
            //■配送情報
            CreateText(sender, e, "配送指定日：", PrintToPDF_Common.GetCoordinate(200, 300, yData), textType.header2);
            CreateText(sender, e, jData.お届け予定日, PrintToPDF_Common.GetCoordinate(300, 500, yData), textType.data1);
            CreateText(sender, e, "配達時間帯：", PrintToPDF_Common.GetCoordinate(500, 600, yData), textType.header2);
            CreateText(sender, e, jData.配達時間帯, PrintToPDF_Common.GetCoordinate(600, 800, yData), textType.data1);
            CreateRowLine(sender, e, yData.CoordinateEnd, xData2Col1, xData2Col2);

            yData = GetCoordinateData(yData, 1.5);

            CreateText(sender, e, "■送付先", PrintToPDF_Common.GetCoordinate(30, 200, yData), textType.header1);
            CreateText(sender, e, "氏名：", PrintToPDF_Common.GetCoordinate(200, 300, yData), textType.header2);
            CreateText(sender, e, jData.送付先氏名, PrintToPDF_Common.GetCoordinate(300, 500, yData), textType.data1);
            CreateText(sender, e, "TEL：", PrintToPDF_Common.GetCoordinate(500, 600, yData), textType.header2);
            CreateText(sender, e, jData.送付先電話番号, PrintToPDF_Common.GetCoordinate(600, 800, yData), textType.data1);
            CreateRowLine(sender, e, yData.CoordinateEnd, xData2Col1, xData2Col2);

            yData = GetCoordinateData(yData, 0.5);
            //■送付先
            CreateText(sender, e, "住所：", PrintToPDF_Common.GetCoordinate(200, 300, yData), textType.header2);
            CreateText(sender, e, jData.送付先郵便番号 + " " + jData.送付先住所, PrintToPDF_Common.GetCoordinate(300, 800, yData), textType.data1);
            CreateRowLine(sender, e, yData.CoordinateEnd, xData1Col);

            yData = GetCoordinateData(yData, 1.5);
            CreateText(sender, e, "■梱包指示", PrintToPDF_Common.GetCoordinate(30, 200, yData), textType.header1);
            CreateText(sender, e, "グループ：", PrintToPDF_Common.GetCoordinate(200, 300, yData), textType.header2);
            CreateText(sender, e, jData.グループ, PrintToPDF_Common.GetCoordinate(300, 800, yData), textType.data1);
            CreateRowLine(sender, e, yData.CoordinateEnd, xData1Col);

            yData = GetCoordinateData(yData, 0.5);
            //■梱包指示
            CreateText(sender, e, "シール番号：", PrintToPDF_Common.GetCoordinate(200, 300, yData), textType.header2);
            CreateText(sender, e, jData.シール番号, PrintToPDF_Common.GetCoordinate(300, 800, yData), textType.data1);
            CreateRowLine(sender, e, yData.CoordinateEnd, xData1Col);

            yData = GetCoordinateData(yData, 0.5);
            //■梱包指示
            CreateText(sender, e, "説明書：", PrintToPDF_Common.GetCoordinate(200, 300, yData), textType.header2);
            CreateText(sender, e, jData.説明書, PrintToPDF_Common.GetCoordinate(300, 800, yData), textType.data1);
            CreateRowLine(sender, e, yData.CoordinateEnd, xData1Col);

            yData = GetCoordinateData(yData, 0.5);
            //■梱包指示
            CreateText(sender, e, "その他：", PrintToPDF_Common.GetCoordinate(200, 300, yData), textType.header2);
            CreateText(sender, e, jData.その他, PrintToPDF_Common.GetCoordinate(300, 800, yData), textType.data1);
            CreateRowLine(sender, e, yData.CoordinateEnd, xData1Col);

        }

        /// <summary>
        /// 枠のない備考欄（受注備考、顧客備考）
        /// </summary>
        private void CreateTextBiko(object sender, PrintPageEventArgs e, Models.JsonObject jData)
        {
            var yData = new CoordinateData();
            //枠外店舗異名
            yData.CoordinateStart = 540;
            yData.CoordinateEnd = yData.CoordinateStart + 30;
            CreateText(sender, e, "◆備考", PrintToPDF_Common.GetCoordinate(20, 400, yData), textType.header1);

            yData.CoordinateStart = 600;
            yData.CoordinateEnd = yData.CoordinateStart + 30;
            CreateText(sender, e, "受注備考", PrintToPDF_Common.GetCoordinate(30, 130, yData), textType.header2);
            CreateText(sender, e, jData.受注備考, PrintToPDF_Common.GetCoordinate(130, 410, yData), textType.data1);
            CreateText(sender, e, "顧客備考", PrintToPDF_Common.GetCoordinate(420, 520, yData), textType.header2);
            CreateText(sender, e, jData.顧客備考, PrintToPDF_Common.GetCoordinate(520, 800, yData), textType.data1);

        }

        //明細部分(枠は変動、データ量次第で改行する判断を組み込まないといけない！）
        //とりあえず1パターン改行なしで実装する(4/17)
        private void CreateTextMeisai(object sender, PrintPageEventArgs e, Models.JsonObject jData)
        {
            var yData = new CoordinateData();
            //枠外店舗異名
            yData.CoordinateStart = 700;
            yData.CoordinateEnd = yData.CoordinateStart + 20;
            CreateText(sender, e, "■明細  " + jData.受注ID, PrintToPDF_Common.GetCoordinate(20, 400, yData), textType.header2);

            //ヘッダ(1行)
            yData.CoordinateStart = 720;
            yData.CoordinateEnd = yData.CoordinateStart + 30;
            CreateText(sender, e, "自社コード/商品名/項目選択肢", PrintToPDF_Common.GetCoordinate(30, 750, yData), textType.header2);
            CreateText(sender, e, "個数", PrintToPDF_Common.GetCoordinate(750, 800, yData), textType.data1);

            //最初のページに印字する明細情報のみ取得
            List<MeisaiData> bikoList = OrderDataCollection[curDataNumber].bikoList.Where(x => x.isPrintFirstPage).ToList();
            if (bikoList?.Count <= 0)
            {
                //備考が1件もない場合もある？あった場合でもエラー終了しないように。
                return;
            }

            //明細(n行)
            //個数(n行)
            int rowCount = 0;
            //明細の数は変動する想定にしておかないといけない
            foreach (MeisaiData bikos in bikoList)
            {
                rowCount++;
                CreateTextMeisaiRow(sender, e, rowCount, bikos.Meisai, bikos.ItemCount, 760);
            }

            ////4個以上は1ページに入りきれない。その場合デフォルトではどうなるかを確認してみる。
            /////→を確認するためのサンプル追加行なので普段はコメントアウトでOK
            //string sampleMeisaiString = "※name-seal-exp / 【楽天1位】名前シール 防水 お名前シール なまえシール おなまえシール ネームシール アイロン不要 貼るだけ 食洗機 レンジ 子供 入学 入園 卒園幼稚園 保育園 小学生 ひらがな カタカナ 漢字 英字 最大589枚 300デザイン以上ホビナビ 送料無料 [◆](name-seal-exp) / 配送方法:ネコポス便(追跡番号あり)※5010.えんじいろ ※おおの ここね ※なし ※なし ※シートCタイプ ※ネコポス";
            //jData.bikoList.Add(new MeisaiData(sampleMeisaiString, "44"));
            //MeisaiCollection.Add(new MeisaiData(sampleMeisaiString, "55"));
            //MeisaiCollection.Add(new MeisaiData(sampleMeisaiString, "999"));


            //外枠の縦幅は、データの数によって動的にしないといけないのでここで設定する
            //720：目次の手前のy座標
            //760：明細欄の開始位置
            //110：1行の明細の縦幅
            // 10：最終行文字列が行でわかり難くなるのを避けるための措置
            PrintToPDF_Common.CreateSquare(e, Const.C_LEFT_M, Const.C_RIGHT_M, 720, 760 + (110 * rowCount) + 10);
        }

        /// <summary>
        /// 明細行を描画
        /// </summary>
        private void CreateTextMeisaiRow(object sender, PrintPageEventArgs e, int rowNo, string meisai, string itemCount,int startRow)
        {

            //スタート位置はstartRowで呼び元から取得。
            //行ごとの縦幅変えたかったらここを変える
            var rowSize = 110;

            //Y座標を計算
            var yData = new CoordinateData();
            yData.CoordinateStart = startRow + rowSize * (rowNo - 1);
            yData.CoordinateEnd = startRow + rowSize * rowNo;

            //明細欄用のフォーマット(明細、数量共通）
            Font font = new Font("MSゴシック", 12);
            SolidBrush bush = new SolidBrush(Color.Black);


            if (rowNo != 1)
            {
                //2データ目以降は、まず点線を挟む
                CreateRowLine(sender, e, yData.CoordinateStart, new CoordinateData() { CoordinateStart = 30, CoordinateEnd = 790 });
            }

            //明細内訳の描画
            e.Graphics.DrawString(meisai, font, bush, PrintToPDF_Common.GetCoordinate(30, 750, yData), GetSF(StringAlignment.Center, StringAlignment.Near));
            e.Graphics.DrawString(itemCount, font, bush, PrintToPDF_Common.GetCoordinate(750, 800, yData), GetSF(StringAlignment.Center, StringAlignment.Center));
        }



        public StringFormat GetSF(StringAlignment rowPosition, StringAlignment colPosition)
        {
            StringFormat result = new StringFormat(StringFormat.GenericTypographic);
            result.LineAlignment = rowPosition;
            result.Alignment = colPosition;
            return result;
        }

        /// <summary>
        /// 改行時、y軸の座標を再指定する
        /// </summary>
        /// <param name="cData"></param>
        /// <param name="addRowLine">1行分を1(値としては20)として、指定文字分開始行位置を下げる</param>
        /// <returns></returns>
        private CoordinateData GetCoordinateData(CoordinateData cData, double addRowCount)
        {
            var Result = new CoordinateData();

            //1行=20で計算
            int addRowLine = (int)Math.Round(addRowCount * 20);

            //直前のyの底辺を次の文字の高さのTop+改行分にする
            Result.CoordinateStart = cData.CoordinateEnd + addRowLine;
            //1行分底辺を下げる
            Result.CoordinateEnd = Result.CoordinateStart + 20;
            return Result;
        }
        private void CreateText(object sender, PrintPageEventArgs e, string txt, Rectangle coordinate, bool isTytle)
        {
            var fontsize = 10;
            if (isTytle) { fontsize = 12; }
            Font font = new Font("MSゴシック", fontsize);
            SolidBrush bush = new SolidBrush(Color.Red);
            StringFormat sf = new StringFormat(StringFormat.GenericTypographic);
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            e.Graphics.DrawString(txt, font, bush, coordinate, sf);
        }
        private void CreateText(object sender, PrintPageEventArgs e, string txt, Rectangle coordinate, textType tt)
        {
            var fontsize = 10;
            SolidBrush bush = new SolidBrush(Color.Black);
            StringFormat sf = new StringFormat(StringFormat.GenericTypographic);
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;

            switch (tt)
            {
                case textType.tytle:
                    fontsize = 24;
                    sf.Alignment = StringAlignment.Near;
                    break;
                case textType.header1:
                    fontsize = 12;
                    sf.Alignment = StringAlignment.Near;
                    break;
                case textType.header2:
                    sf.Alignment = StringAlignment.Near;
                    break;
                case textType.data1:
                    //デフォルトにするので処理不要
                    break;
                case textType.rightPrint:
                    sf.Alignment = StringAlignment.Far;
                    break;
            }
            Font font = new Font("MSゴシック", fontsize);

            e.Graphics.DrawString(txt, font, bush, coordinate, sf);
        }


        /// <summary>
        /// 下点線を描画（1行に1列のパターン）
        /// </summary>
        private void CreateRowLine(object sender, PrintPageEventArgs e, int height, CoordinateData xData)
        {
            Pen p = new Pen(Color.Black, 2);
            p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            e.Graphics.DrawLine(p, xData.CoordinateStart, height, xData.CoordinateEnd, height);
        }
        /// <summary>
        /// 下点線を描画（1行に2列あるパターン）
        /// </summary>
        private void CreateRowLine(object sender, PrintPageEventArgs e, int height, CoordinateData xData1, CoordinateData xData2)
        {
            CreateRowLine(sender, e, height, xData1);
            CreateRowLine(sender, e, height, xData2);
        }






        #endregion


        /// <summary>
        /// 印刷データ作成(HasMorePages=Trueの間は次ページを印刷し続ける）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            //一旦改ページは有効にしておく（条件に一致してなければReturn前にFalseにすることで終了できる）
            if (!e.HasMorePages)
            {
                e.HasMorePages = true;
            }

            //取り込まれるデータの存在チェック
            if (!(OrderDataCollection?.Count > 0))
            {
                e.HasMorePages = false;
                return;
            }

            if (isNewPage)
            {
                curPageNumber++;
                curDataNumber++;

                //ループ中の行番号がない場合（連番で作成するのであり得ない筈）、次データに移動
                if (!OrderDataCollection.ContainsKey(curDataNumber))
                {
                    //e.HasMorePages = Trueの状態で終了する為、再度pd_PrintPageの最初から呼ばれ直す
                    return;
                }

                pd_PrintPage_New(sender, e);

                //複数ページ対象のデータだった場合、次はAddPage処理にする
                if (OrderDataCollection[curDataNumber].IsPageOver)
                {
                    isNewPage = false;
                }
            }
            else
            {
                curPageNumber++;
                //データは前ページと同じもの
                //curDataNumber++;

                pd_PrintPage_Add(sender, e);

                //流石に3ページには跨がない筈
                isNewPage = true;
            }

            //データが最後まで行ってる＆次データ(もうない)に行こうとしたら、改ページOFFにすることでイベントのループを停止できる
            //(keysのMAX=DataNumberの時が全体の最終行になる筈）
            if (OrderDataCollection.Keys.Max() <= curDataNumber && isNewPage)
            {
                e.HasMorePages = false;
            }


        }





        private void pd_PrintPage_New(object sender, PrintPageEventArgs e)
        {

            //(動作確認用)縦横のライン作成しておく
            PrintToPDF_Common.CreateBaseLine(e);

            //テキスト描画(JSONデータ取込もこの中で行われる）
            CreateText(sender, e);

            //線描画
            CreateLineAndBox(sender, e);

            //画像が組み込めるか(x軸は四角形に合わせ、y軸300から50*50の描画)
            //e.Graphics.DrawImage(Image.FromFile(@"D:\source\実験\スタイルシート初作成\谷.png"), new Rectangle(20, 300, 50, 50));

        }

        /// <summary>
        /// 1つの明細が2ページ以上に跨った際の、2ページ目の印字機能
        /// ※その為、ここに来ている時点でe.HasMorePages=trueとなっている前提。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pd_PrintPage_Add(object sender, PrintPageEventArgs e)
        {

            ////(動作確認用)縦横のライン作成しておく
            //CreateBaseLine(sender, e);

            //テキスト描画(JSONデータ取込もこの中で行われる）
            CreateTextAddPage(sender, e);

            //線描画
            //CreateLineAndBox(sender, e);

        }





        /// <summary>
        /// 外部ファイルからttf、otfなどのフォントファイルを一時的に取り込む
        /// </summary>
        #region 
        /// <summary>
        /// ttf、otfを取り込むサンプルプログラム
        /// 「https://dobon.net/vb/dotnet/graphics/privatefontcollection.html」
        /// ※この処理は起動時に1回だけ行いたい。文字列印字のたびに呼ぶのはよくない。
        /// </summary>
        private System.Drawing.Font GetPrivateFont(string fontFileName)
        {
            System.Drawing.Text.PrivateFontCollection pfc = new System.Drawing.Text.PrivateFontCollection();

            //ここは固定しないならファイルパスごと引数にすればいい
            string fontDirectory = (@"D:\font\");

            //PrivateFontCollectionにフォントを追加する
            pfc.AddFontFile(fontDirectory + fontFileName);

            //PrivateFontCollectionに追加されているフォントの名前を列挙する
            foreach (System.Drawing.FontFamily ff in pfc.Families)
            {
                Console.WriteLine(ff.Name);
            }

            //PrivateFontCollectionの先頭のフォントのFontオブジェクトを作成する
            System.Drawing.Font f =
                new System.Drawing.Font(pfc.Families[0], 8);

            return f;

            ////後始末
            //pfc.Dispose();
        }

        /// <summary>
        /// ttfを取り込むサンプルプログラム
        /// 「https://dobon.net/vb/dotnet/graphics/privatefontcollection.html」
        /// </summary>
        private System.Drawing.Font GetPrivateFontCollection()
        {
            System.Drawing.Text.PrivateFontCollection pfc = new System.Drawing.Text.PrivateFontCollection();

            //PrivateFontCollectionにフォントを追加する
            pfc.AddFontFile(@"D:\font\gomarice_mukasi_mukasi.ttf");
            //同様にして、複数のフォントを追加できる
            pfc.AddFontFile(@"D:\font\FOT-MatisseElegantoPro-DB.otf");

            //PrivateFontCollectionに追加されているフォントの名前を列挙する
            foreach (System.Drawing.FontFamily ff in pfc.Families)
            {
                Console.WriteLine(ff.Name);
            }

            //PrivateFontCollectionの先頭のフォントのFontオブジェクトを作成する
            System.Drawing.Font f =
                new System.Drawing.Font(pfc.Families[0], 12);

            return f;

            ////後始末
            //pfc.Dispose();
        }
        #endregion 

    }
}
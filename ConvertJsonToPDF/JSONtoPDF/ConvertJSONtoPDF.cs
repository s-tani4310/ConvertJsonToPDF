using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.Json;
using System.IO;
using System.Data.SqlTypes;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ConvertJsonToPDF.Models;
using System.Text.Json.Serialization;
using System.Windows.Markup;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Drawing.Drawing2D;
using NLog;
using ConvertJsonToPDF.Tools;
using System.Security.AccessControl;
using NuGet.Protocol;

namespace ConvertJsonToPDF.JSONtoPDF
{
    public class ConvertJSONtoPDF
    {

        public byte[] ConvertToPDF()
        {

            LogUtil.Debug("★Debugモードでのみ出力されます。実運用版でこれが出る場合はReleaseでコンパイルしなおすこと！★");
            LogUtil.Info("▼▼▼処理開始▼▼▼");
            try
            {
                //★Windows10以降でプリインストールされているPrintToPDFを利用し、PDFを作成する
                var pdf = new PrintPDF_OnamaeSeal();
                var result = pdf.PrintTextToPDF();
                return result;

            }
            finally
            {
                LogUtil.Info("▲▲▲処理終了▲▲▲");
            }
        }
        public byte[] ConvertToPDF(Product[] products)
        {
            LogUtil.Info("▼▼▼処理開始▼▼▼");
            try
            {
                //★Windows10以降でプリインストールされているPrintToPDFを利用し、PDFを作成する
                var pdf = new PrintPDF_OnamaeSeal();
                var result = pdf.PrintTextToPDF(products);
                return result;

            }
            finally 
            {
                LogUtil.Info("▲▲▲処理終了▲▲▲");
            }
        }

        //テスト用サンプルデータ作成処理(GetJsonSampleList)
        #region
        /// <summary>
        /// ★★テスト用サンプルデータ作成★★
        /// 最終的にこのデータを使っている場所では外部からJSONデータを取り込むことになる
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, JsonObject> GetJsonSampleList()
        {
            var result = new Dictionary<int,JsonObject>();
            for (int i = 1;i <= 10; i++)
            {
                if (i % 2 == 1)
                {
                    result.Add(i, GetJsonSampleData("1"));
                }
                else
                {
                    result.Add(i, GetJsonSampleData("2"));
                }
            }
            return result;
        }
        private JsonObject GetJsonSampleData(string id)
        {
            var data = new JsonObject();
            string sampleMeisaiString1 = "name-seal-exp / 【楽天1位】名前シール 防水 お名前シール なまえシール おなまえシール ネームシール アイロン不要 貼るだけ 食洗機 レンジ 子供 入学 入園 卒園幼稚園 保育園 小学生 ひらがな カタカナ 漢字 英字 最大589枚 300デザイン以上ホビナビ 送料無料 [◆](name-seal-exp) / 配送方法:ネコポス便(追跡番号あり)※5010.えんじいろ ※おおの ここね ※なし ※なし ※シートCタイプ ※ネコポス";
            string sampleMeisaiString2 = "name-seal-exp / 【楽天2位】名前シール 防水 お名前シール なまえシール おなまえシール ネームシール アイロン不要 貼るだけ 食洗機 レンジ 子供 入学 入園 卒園幼稚園 保育園 小学生 ひらがな カタカナ 漢字 英字 最大589枚 300デザイン以上ホビナビ 送料無料 [◆](name-seal-exp) / 配送方法:ネコポス便(追跡番号あり)※5010.えんじいろ ※おおの ここね ※なし ※なし ※シートCタイプ ※ネコポス";
            string sampleMeisaiString3 = "name-seal-exp / 【楽天3位】名前シール 防水 お名前シール なまえシール おなまえ\r\nシール ネームシール アイロン不要 貼るだけ 食洗機 レンジ 子供 入学 入園 卒園\r\n幼稚園 保育園 小学生 ひらがな カタカナ 漢字 英字 最大589枚 300デザイン以上\r\nホビナビ 送料無料 [◆](name-seal-exp) / 配送方法:ネコポス便(追跡番号あり)\r\n※602-シンプル ウサギ ※ひらお すずは ※なし ※なし ※シートBタイプ ※ネコポス";
            string sampleMeisaiString4 = "name-seal-exp / 【楽天4位】名前シール 防水 お名前シール なまえシール おなまえシール ネームシール アイロン不要 貼るだけ 食洗機 レンジ 子供 入学 入園 卒園幼稚園 保育園 小学生 ひらがな カタカナ 漢字 英字 最大589枚 300デザイン以上ホビナビ 送料無料 [◆](name-seal-exp) / 配送方法:ネコポス便(追跡番号あり)※5010.えんじいろ ※おおの ここね ※なし ※なし ※シートCタイプ ※ネコポス";
            string sampleMeisaiString5 = "name-seal-exp / 【楽天5位】名前シール 防水 お名前シール なまえシール おなまえシール ネームシール アイロン不要 貼るだけ 食洗機 レンジ 子供 入学 入園 卒園幼稚園 保育園 小学生 ひらがな カタカナ 漢字 英字 最大589枚 300デザイン以上ホビナビ 送料無料 [◆](name-seal-exp) / 配送方法:ネコポス便(追跡番号あり)※5010.えんじいろ ※おおの ここね ※なし ※なし ※シートCタイプ ※ネコポス";
            switch (id)
            {
                case "1":
                    data.OrderID = "664-1978";
                    data.ShopName = "楽天 ホビナビ";
                    data.OrderDate = "2024/3/19  0:24:26";
                    data.InputDate = "2024/4/03";
                    //data.取込日時 = "2024/4/03  10:02:26";
                    //data.請求金額 = "1000";
                    data.GroupCd = "04/03";
                    data.SealNo = "C2";
                    data.Guide = "MB107・MB151";
                    data.Other = "";
                    data.DeliverySupplier = "ネコポス(速達)事務所用";
                    data.DeliveryDate = "";
                    data.DeliveryTime = "";
                    data.CustomerName = "落合　良子";
                    data.CustomerPost = "496-0905";
                    data.CustomerAddress = "愛知県愛西市北一色町北田面284-3";
                    data.CustomerTel = "090-999-9999";
                    //data.記載事項 = "";
                    //data.特記事項 = "";
                    data.OrderBiko = "受注備考に書かれる";
                    data.CustomerBiko = "顧客備考に書かれる";
                    data.ShippingDate = "2024/3/26  10:33:26";

                    //Inがただの配列なので、これは取り込む時に加工したほうがいい
                    data.bikoList = new List<MeisaiData>
                    {
                        //3つめから改ページさせる
                        new MeisaiData(true,sampleMeisaiString1, "1"),
                        new MeisaiData(true,sampleMeisaiString2, "999"),

                        new MeisaiData(false,sampleMeisaiString3, "38"),
                        new MeisaiData(false,sampleMeisaiString4, "999"),
                        new MeisaiData(false,sampleMeisaiString5, "9999"),
                    };

                    break;
                case "2":
                    data.OrderID = "664-1980";
                    data.ShopName = "楽天 ホビナビ";
                    data.OrderDate = "2024/03/19 00:31:29";
                    data.InputDate = "2024/4/03  10:02:26";
                    //data.請求金額 = "1000";
                    data.GroupCd = "04/03";
                    data.SealNo = "B5";
                    data.Guide = "MB107・MB151";
                    data.Other = "";
                    data.DeliverySupplier = "ネコポス(速達)事務所用";
                    data.DeliveryDate = "";
                    data.DeliveryTime = "";
                    data.CustomerName = "平尾 美希";
                    data.CustomerPost = "668-0824";
                    data.CustomerAddress = "兵庫県豊岡市森尾931-14";
                    data.CustomerTel = "090-7752-8054";
                    //data.記載事項 = "";
                    //data.特記事項 = "";
                    data.OrderBiko = "";
                    data.CustomerBiko = "";
                    data.ShippingDate = "2024/03/26 10:33:41";

                    data.bikoList = new List<MeisaiData>
                    {
                        new MeisaiData(true, sampleMeisaiString2, "23")
                    };

                    break;
            }

            return data;
        }

        //外部データを印刷用Dictionaryに変換(とりあえず1データ)
        public Dictionary<int, JsonObject> ConvertJsonList(Product[] products)
        {
            var result = new Dictionary<int, JsonObject>();
            var cnt = 1;
            try
            {
                //取り込む際は配列なので、各データに「何件目のデータか」を持たせる為Dictionaryに切替
                foreach (Product product in products)
                {
                    //髙
                    result.Add(cnt, ConvertJsonData(product));
                    cnt++;
                }

                //最後+1してるので、データ件数はcnt-1
                foreach (var item in result)
                {
                    JsonObject data = item.Value;
                    LogUtil.Info("　受注ID：" + data.OrderID );
                    LogUtil.Info("　顧客名：" + data.CustomerName);
                }
                LogUtil.Info((cnt - 1).ToString() + "件のデータ取込");

                return result;
            }
            catch (Exception ex)
            {
                LogUtil.Error("ConvertJsonListでエラー：" + ex.ToString());
                return result;
            }
        }
        //外部データを印刷用Dictionaryに変換(とりあえず1データ)
        public JsonObject ConvertJsonData(Product product)
        {
            var data = new JsonObject();

            data.OrderID = product.受注ID;
            data.ShopName = product.店舗名;
            data.OrderDate = product.購入日時;
            data.InputDate = data.InputDate;
            //data.請求金額 = data.請求金額;
            data.GroupCd = product.グループ;
            //data.シール番号 = product.シール番号;
            //data.説明書 = product.説明書;
            //data.その他 = product.その他;
            data.SealNo = string.Join('・', product.シール番号);
            data.Guide = string.Join('・', product.説明書);
            data.Other = string.Join('・', product.その他);

            data.DeliverySupplier = product.宅配業者名;
            data.DeliveryDate = product.お届け予定日;
            data.DeliveryTime = product.配達時間帯;
            data.CustomerName = product.送付先氏名;
            data.CustomerPost = product.送付先郵便番号;
            data.CustomerAddress = "〒"+ product.送付先住所;
            data.CustomerTel = product.送付先電話番号;
            //data.記載事項 = //product.記載事項;
            //data.特記事項 = //product.特記事項;
            data.OrderBiko = product.受注備考;
            data.CustomerBiko = product.顧客備考;
            data.ShippingDate = product.出荷予定日;

            data.bikoList = new List<MeisaiData>();
            if (product?.bikos?.Count > 0)
            {
                bool firstPageFlg = true;

                foreach (var row in product.bikos)
                {
                    //明細欄3個まで(0開始なのでindexは2まで)は1ページに収まり、
                    //4個超えると次ページに自動で遷移させる
                    firstPageFlg = (data.bikoList.Count() <= 2);

                    data.bikoList.Add(new MeisaiData(firstPageFlg, row[0].ToString(), row[1].ToString()));
                }
            };

            return data;
        }

        #endregion

        public string GetStringBuilder()
        {
            string result = string.Empty;

            // ファイルの存在チェック
            if (System.IO.File.Exists(@"D:\source\実験\スタイルシート初作成\baseHTML_NameSeal.txt"))
            {

                // StreamReaderでファイルを読み込む
                System.IO.StreamReader reader = (new System.IO.StreamReader(@"D:\source\実験\スタイルシート初作成\baseHTML_NameSeal.txt", System.Text.Encoding.GetEncoding("UTF-8")));
                //System.IO.StreamReader reader = (new System.IO.StreamReader(@"D:\source\実験\スタイルシート初作成\baseHTML_NameSeal.txt", System.Text.Encoding.GetEncoding("shift_jis")));

                // ファイルの最後まで読み込む
                result = reader.ReadToEnd();

                // 閉じる (オブジェクトの破棄)
                reader.Close();
            }

            return result;
        }
    }

}
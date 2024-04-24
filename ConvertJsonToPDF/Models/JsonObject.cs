using ConvertJsonToPDF.JSONtoPDF;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConvertJsonToPDF.Models
{
    [Serializable] // DeepCopy対象では Serializable 属性が必須
    public class JsonObject
    {

        //result.Add("p1", "664-1978");
        //    result.Add("p2", "ネコポス（速達）事務所用");


        //* @param {Array<{
        // * 受注ID: string,
        // * 店舗名: string,
        // * 購入日時: string,

        // * 取込日時: string,
        // * 請求金額: string,
        // * グループ: string,
        // * シール番号: string,
        // * 説明書: string,
        // * その他: string,
        // * "宅配業者名": string,
        // * "お届け予定(指定)日": string,
        // * "配達時間帯": string,
        // * "送付先氏名": string,
        // * "送付先郵便番号": string,
        // * "送付先住所": string,
        // * "送付先電話番号": string,
        // * "記載事項": string,
        // * "特記事項": string,
        // * "受注備考": string,
        // * "顧客備考": string,
        // * bikos: Array<[備考, 個数]>,
        // * "出荷予定日": string
        // *
        //    }>}
        //data


        [JsonProperty("受注ID")] public string 受注ID { get; set; }
        [JsonProperty("店舗名")] public string 店舗名 { get; set; }
        [JsonProperty("購入日時")] public string 購入日時 { get; set; }
        [JsonProperty("取込日時")] public string 取込日時 { get; set; }
        [JsonProperty("請求金額")] public string 請求金額 { get; set; }
        [JsonProperty("グループ")] public string グループ { get; set; }
        [JsonProperty("シール番号")] public string シール番号 { get; set; }
        [JsonProperty("説明書")] public string 説明書 { get; set; }
        [JsonProperty("その他")] public string その他 { get; set; }
        [JsonProperty("宅配業者名")] public string 宅配業者名 { get; set; }
        [JsonProperty("お届け予定(指定)日")] public string お届け予定日 { get; set; }
        [JsonProperty("配達時間帯")] public string 配達時間帯 { get; set; }
        [JsonProperty("送付先氏名")] public string 送付先氏名 { get; set; }
        [JsonProperty("送付先郵便番号")] public string 送付先郵便番号 { get; set; }
        [JsonProperty("送付先住所")] public string 送付先住所 { get; set; }
        [JsonProperty("送付先電話番号")] public string 送付先電話番号 { get; set; }
        [JsonProperty("記載事項")] public string 記載事項 { get; set; }
        [JsonProperty("特記事項")] public string 特記事項 { get; set; }
        [JsonProperty("受注備考")] public string 受注備考 { get; set; }
        [JsonProperty("顧客備考")] public string 顧客備考 { get; set; }

        [JsonProperty("出荷予定日")] public string 出荷予定日 { get; set; }

        //bikos: Array<[備考, 個数]>
        [JsonProperty("bikos")] public List<MeisaiData> bikoList { get; set; }

        /// <summary>
        /// 次ページ対象の備考が1件でもあれば、2ページに渡って印字する必要あり
        /// </summary>

        public bool IsPageOver
        {
            get
            {
                return (bikoList.Where(x => !x.isPrintFirstPage).Any());
            }
        }
    }

}



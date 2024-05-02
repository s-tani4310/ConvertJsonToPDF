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


        [JsonProperty("受注ID")] public string OrderID { get; set; }
        [JsonProperty("店舗名")] public string ShopName { get; set; }
        [JsonProperty("購入日時")] public string OrderDate { get; set; }
        [JsonProperty("取込日時")] public string InputDate { get; set; }
        [JsonProperty("請求金額")] public string OrderPrice { get; set; }
        [JsonProperty("グループ")] public string GroupCd { get; set; }
        [JsonProperty("シール番号")] public string SealNo { get; set; }
        [JsonProperty("説明書")] public string Guide { get; set; }
        [JsonProperty("その他")] public string Other { get; set; }
        [JsonProperty("宅配業者名")] public string DeliverySupplier { get; set; }
        [JsonProperty("お届け予定(指定)日")] public string DeliveryDate { get; set; }
        [JsonProperty("配達時間帯")] public string DeliveryTime { get; set; }
        [JsonProperty("送付先氏名")] public string CustomerName { get; set; }
        [JsonProperty("送付先郵便番号")] public string CustomerPost { get; set; }
        [JsonProperty("送付先住所")] public string CustomerAddress { get; set; }
        [JsonProperty("送付先電話番号")] public string CustomerTel { get; set; }
        [JsonProperty("記載事項")] public string KisaiText { get; set; }
        [JsonProperty("特記事項")] public string TokkiText { get; set; }
        [JsonProperty("受注備考")] public string OrderBiko { get; set; }
        [JsonProperty("顧客備考")] public string CustomerBiko { get; set; }

        [JsonProperty("出荷予定日")] public string ShippingDate { get; set; }

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



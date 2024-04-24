using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConvertJsonToPDF.JSONtoPDF
{
    public class MeisaiData
    {
        public bool isPrintFirstPage;
        public String Meisai;
        public String ItemCount;
        //コンストラクタ
        public MeisaiData(bool pIsPrintFirstPage, string pMeisai, string pItemCount)
        {
            isPrintFirstPage = pIsPrintFirstPage;
            Meisai = pMeisai;
            ItemCount = pItemCount;
        }
    }
}
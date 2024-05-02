using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ConvertJsonToPDF.Data;
using ConvertJsonToPDF.Models;
using Humanizer;
using ConvertJsonToPDF.JSONtoPDF;
using Newtonsoft.Json.Linq;
using ConvertJsonToPDF.Tools;
using System.Text.Json;
using Newtonsoft.Json;

namespace ConvertJsonToPDF.Controllers
{
    //テスト用。個の記述だと「～test/Products」「～api/Products」両方接続できる
    //[Route("test/[controller]")]

    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ConvertJsonToPDFContext _context;

        public ProductsController(ConvertJsonToPDFContext context)
        {
            _context = context;
        }

        //[HttpGet] 属性に対する引数はあってもなくてもいい
        #region
        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProduct()
        {
            Console.WriteLine("うごいた");

            LogUtil.Info("◆引数なしGET◆");
            //return this.File(bs, "application/pdf");

            ConvertJSONtoPDF JSONtoPDF = new ConvertJSONtoPDF();
            //引数を使わず、PG内でサンプルデータを作成して印刷
            byte[] bs = JSONtoPDF.ConvertToPDF();

            // content-type:PDFドキュメントデータ
            //exeみたいにダウンロード式にする場合は、「application/octet-stream」に変更する
            return this.File(bs, "application/pdf");
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            LogUtil.Info("◆引数なしGETid指定◆");
            var product = await _context.Product.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }


            return product;
        }
        #endregion

        //[HttpPost] 要引数指定
        //※[HttpPost](おそらくGetとか他のも）は、1箇所しか書けない。これを見てアクセスしているため。
        #region
        //Inをフォーマットの決まっているJSON型データの配列指定に固定。
        //もし送信時点でのフォーマットを自由にできて受信後に変換可能か判断したければ
        //取込の型をobjectにすれば一旦取込は可能（その場合どのデータが何に該当するかは判断処理を書かないといけない）


        //[HttpPost]
        //public async Task<IActionResult> PostProduct(RequestJson products)
        //{
        //    //PDF作成処理取ってきた。但し非同期対応できてないので、データ量が大量になる場合は一部改良が必要になる。
        //    ConvertJSONtoPDF JSONtoPDF = new ConvertJSONtoPDF();

        //    //引数の情報を元に、PDFをバイナリデータで取得
        //    byte[] bs = JSONtoPDF.ConvertToPDF(products.data);

        //    // content-type:PDFドキュメントデータ
        //    //exeみたいにダウンロード式にする場合は、「application/octet-stream」に変更する
        //    return this.File(bs, "application/pdf");
        //}

        //配列で渡されるVer
        [HttpPost]
        public async Task<IActionResult> PostProduct(Product[] products)
        {
            //PDF作成処理取ってきた。但し非同期対応できてないので、データ量が大量になる場合は一部改良が必要になる。
            ConvertJSONtoPDF JSONtoPDF = new ConvertJSONtoPDF();

            //引数の情報を元に、PDFをバイナリデータで取得
            byte[] bs = JSONtoPDF.ConvertToPDF(products);

            // content-type:PDFドキュメントデータ
            //exeみたいにダウンロード式にする場合は、「application/octet-stream」に変更する
            return this.File(bs, "application/pdf");
        }

        #endregion

        //[HttpPut] 要属性に対する引数指定
        #region
        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutProduct(string id, Product product)
        //{
        //    if (id != product.OrderId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(product).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ProductExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}
        #endregion
        //[HttpDelete] 要属性に対する引数指定
        #region
        // DELETE: api/Products/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteProduct(int id)
        //{
        //    var product = await _context.Product.FindAsync(id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Product.Remove(product);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}
        #endregion




    }
}

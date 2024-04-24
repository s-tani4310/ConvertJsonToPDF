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

namespace ConvertJsonToPDF.Controllers
{
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
            return await _context.Product.ToListAsync();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
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
        //POST: api/Products
        //To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        #region

        //[HttpPost]
        //public async Task PostProduct(JsonObject product)
        //{
        //    //PDF作成処理取ってきた。但し非同期対応できてない
        //    ConvertJSONtoPDF JSONtoPDF = new ConvertJSONtoPDF();
        //    JSONtoPDF.ConvertToPDF();

        //    //以下、サンプルにあった分

        //    //_context.Product.Add(product);
        //    //この中でDB接続エラーが出てる。でも、今回別にDB接続するわけではないので解決する必要ない？
        //    //本処理でまだawaitにできてないのでとりあえず作成しているだけ。
        //    await _context.SaveChangesAsync();

        //    //ここにPDF作成処理つくればいける？
        //    //戻り値ありの場合、Task<Result>に変えればOK
        //}







        ////これは動いた（引数はあるけどデータは独自に作る）
        //[HttpPost]
        //public async Task PostProduct(Product product)
        //{
        //    //PDF作成処理取ってきた。但し非同期対応できてない
        //    ConvertJSONtoPDF JSONtoPDF = new ConvertJSONtoPDF();
        //    //JSONtoPDF.ConvertToPDF();

        //    //引数の値のPDFを作成してみる(とりあえす1レコード。複数の場合どうすればよいかは考えないといけない）
        //    JSONtoPDF.ConvertToPDF(product);

        //    //以下、サンプルにあった分

        //    //_context.Product.Add(product);
        //    //この中でDB接続エラーが出てる。でも、今回別にDB接続するわけではないので解決する必要ない？
        //    //本処理でまだawaitにできてないのでとりあえず作成しているだけ。
        //    await _context.SaveChangesAsync();

        //    //ここにPDF作成処理つくればいける？
        //    //戻り値ありの場合、Task<Result>に変えればOK
        //}

        //Inをフォーマットの決まっているJSON型データの配列指定に固定。
        //もし送信時点でのフォーマットを自由にできて受信後に変換可能か判断したければ
        //取込の型をobjectにすれば一旦取込は可能（その場合どのデータが何に該当するかは判断処理を書かないといけない）
        [HttpPost]
        public async Task PostProduct(Product[] products)
        {
            //PDF作成処理取ってきた。但し非同期対応できてない
            ConvertJSONtoPDF JSONtoPDF = new ConvertJSONtoPDF();
            //JSONtoPDF.ConvertToPDF();

            //引数の値のPDFを作成してみる(とりあえす1レコード。複数の場合どうすればよいかは考えないといけない）
            JSONtoPDF.ConvertToPDF(products);

            //byte[] bs = JSONtoPDF.ConvertToPDF(products);


            //以下、サンプルにあった分


            //_context.Product.Add(product);

            //この中でDB接続エラーが出てる。でも、今回別にDB接続するわけではないので解決する必要ない？
            //本処理でまだawaitにできてないのでとりあえず作成しているだけ。
            await _context.SaveChangesAsync();

            //ここにPDF作成処理つくればいける？
            //戻り値ありの場合、Task<Result>に変えればOK
            //return this.File(bs, "application/octet-stream");
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

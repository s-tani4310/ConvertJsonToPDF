using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ConvertJsonToPDF.Models;

namespace ConvertJsonToPDF.Data
{
    public class ConvertJsonToPDFContext : DbContext
    {
        public ConvertJsonToPDFContext (DbContextOptions<ConvertJsonToPDFContext> options)
            : base(options)
        {
        }

        public DbSet<ConvertJsonToPDF.Models.Product> Product { get; set; } = default!;
    }
}

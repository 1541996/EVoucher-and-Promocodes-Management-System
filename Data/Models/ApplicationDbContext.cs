using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Models
{
    //public class ApplicationDbContext : IDisposable
    //{
    //    public MySqlConnection Connection { get; }

    //    public ApplicationDbContext(string connectionString)
    //    {
    //        Connection = new MySqlConnection(connectionString);
    //    }

    //    public void Dispose() => Connection.Dispose();
    //}
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        //public MySqlConnection Connection { get; }

        //public ApplicationDbContext(string connectionString)
        //{
        //    Connection = new MySqlConnection(connectionString);
        //}
        //public void Dispose() => Connection.Dispose();

        public virtual DbSet<tbvoucher> vouchers { get; set; }
        public virtual DbSet<tbcustomer> customers { get; set; }
        public virtual DbSet<tbpurchase> purchases { get; set; }
        public virtual DbSet<tbpaymentmethod> paymentmethods { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<tbvoucher>().Ignore(t => t.PhotoUrl);

        }


    }
}

using Core.Interface;
using Data.Models;
using Infra.Repository;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Infra.UnitOfWork
{
    public class UnitOfWork
    {    
        private IRepository<tbvoucher> _voucherRepo;      
        private IRepository<tbcustomer> _customerRepo;
        private IRepository<tbpurchase> _purchaseRepo;
       
        private ApplicationDbContext _ctx;
        public UnitOfWork(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }
        public IRepository<tbcustomer> customerRepo
        {
            get
            {
                if (_customerRepo == null)
                {
                    _customerRepo = new Repository<tbcustomer>(_ctx);
                }
                return _customerRepo;
            }
        }
        public IRepository<tbvoucher> voucherRepo
        {
            get
            {
                if (_voucherRepo == null)
                {
                    _voucherRepo = new Repository<tbvoucher>(_ctx);
                }
                return _voucherRepo;
            }
        }

        public IRepository<tbpurchase> purchaseRepo
        {
            get
            {
                if (_purchaseRepo == null)
                {
                    _purchaseRepo = new Repository<tbpurchase>(_ctx);
                }
                return _purchaseRepo;
            }
        }


    }
}

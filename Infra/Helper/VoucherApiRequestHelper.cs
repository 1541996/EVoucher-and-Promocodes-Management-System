using Data.Models;
using Data.ViewModels;
using Infra.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace Infra.Helper
{
    public class VoucherApiRequestHelper
    {
        public static async Task<PagedListClient<VoucherViewModel>> List(int pagesize = 10, int page = 1, string token = null)
        {
            string url = string.Format("api/EVoucher/GetVoucherList?Pagesize={0}&Page={1}", pagesize, page);
            var data = await ApiRequest<PagedListServer<VoucherViewModel>>.GetRequest(url,token);

            var model = new PagedListClient<VoucherViewModel>();
            var pagedList = new StaticPagedList<VoucherViewModel>(data.Results, page, pagesize, data.TotalCount);
            model.Results = pagedList;
            model.TotalCount = data.TotalCount;
            model.TotalPages = data.TotalPages;
            return model;
        }
      
        public static async Task<tbvoucher> GetById(int ID, string token = null)
        {
            string url = string.Format("api/EVoucher/GetVoucherDetail?ID={0}", ID);
            tbvoucher result = await ApiRequest<tbvoucher>.GetRequest(url, token);
            return result;
        }

        public static async Task<PaymentViewModel> GetCheckOutData(int ID, string token = null)
        {
            string url = string.Format("api/EVoucher/GetCheckOutData?ID={0}", ID);
            PaymentViewModel result = await ApiRequest<PaymentViewModel>.GetRequest(url, token);
            return result;
        }

        public static async Task<List<tbpaymentmethod>> GetPaymentMethodList(string token = null)
        {
            string url = string.Format("api/EVoucher/GetPaymentMethodList");
            List<tbpaymentmethod> result = await ApiRequest<List<tbpaymentmethod>>.GetRequest(url, token);
            return result;
        }

        public static async Task<tbvoucher> SetActive(int ID, string token = null)
        {
            string url = string.Format("api/EVoucher/SetActive?ID={0}", ID);
            tbvoucher result = await ApiRequest<tbvoucher>.GetRequest(url, token);
            return result;
        }


        public static async Task<tbvoucher> UpSert(tbvoucher data,string token)
        {
            var url = "api/EVoucher/SaveVoucher";
            tbvoucher result = await ApiRequest<tbvoucher>.PostRequest(url, data, token);
            return result;
        }


        public static async Task<ReturnPaymentViewModel> makePayment(PaymentViewModel data, string token)
        {
            var url = "api/EVoucher/MakePayment";
            ReturnPaymentViewModel result = await ApiRequest<PaymentViewModel, ReturnPaymentViewModel>.PostDiffRequest(url, data, token);
            return result;
        }


        public static async Task<ReturnPaymentViewModel> makeActualPayment(PaymentViewModel data, string token)
        {
            var url = "api/EVoucher/makeActualPayment";
            ReturnPaymentViewModel result = await ApiRequest<PaymentViewModel, ReturnPaymentViewModel>.PostDiffRequest(url, data, token);
            return result;
        }


    }
}

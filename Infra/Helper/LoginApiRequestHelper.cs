using Data.Models;
using Data.ViewModels;
using Infra.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace Infra.Helper
{
    public class LoginApiRequestHelper
    {
        public static async Task<AuthResponseModel> authenticate(AuthModel model)
        {          
            var url = "api/Authenticate/authenticate";
            AuthResponseModel result = await ApiRequest<AuthModel, AuthResponseModel>.PostDiffRequest(url, model);
            return result;
        }


    }
}

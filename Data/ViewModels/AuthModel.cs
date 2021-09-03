using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.ViewModels
{
    public class AuthModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class AuthResponseModel
    {
        public string Token { get; set; }
    }

    public class FileUploadViewModel
    {
        public string filepath { get; set; }
        public string photo { get; set; }
    }
}

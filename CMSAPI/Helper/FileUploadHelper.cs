using Data.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CMSWeb.Helper
{
    public static class FileUploadHelper
    {
        public static string upload(FileUploadViewModel fvm)
        {

            String result;
          //  String path = HttpContext.Current.Server.MapPath(fvm.filepath); //Path          
            //Check if directory exist

          
            if (!System.IO.Directory.Exists(fvm.filepath))
            {
                System.IO.Directory.CreateDirectory(fvm.filepath); //Create directory if it doesn't exist
            }

            if (fvm.photo != null)
            {
                result = uploadPhoto(fvm.photo, fvm.filepath);
            }
            else
            {
                result = null;
            }

            return result;


        }

        public static string uploadPhoto(string stringInBase64, string path)
        {
            try
            {
                string imageName = Guid.NewGuid().ToString() + ".jpg";
                //set the image path
                string imgPath = Path.Combine(path, imageName);
                List<string> bs64List = stringInBase64.Split(',').ToList();
                if (bs64List.Count() > 0)
                {
                    stringInBase64 = bs64List[1];
                }
                byte[] imageBytes = Convert.FromBase64String(stringInBase64);
                System.IO.File.WriteAllBytes(imgPath, imageBytes);

                return imageName;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}

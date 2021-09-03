using Microsoft.EntityFrameworkCore;
//using PagedList;
//using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace Infra.Services
{
    public class PagingService<T>
    {
        public static async Task<Model<T>> getPaging(int page, int pageSize, IQueryable<T> result)
        {
            try
            {
                var totalCount = result.Count();
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
                var dataList = await result.Skip(pageSize * (page - 1)).Take(pageSize).ToListAsync();
              
                Model<T> model = new Model<T>();
                model.Results = dataList;
                model.TotalCount = totalCount;
                model.TotalPages = totalPages;
                return model;
            }
            catch (Exception ex)
            {

            }

            return null;

        }
    }
    public class PagedListServer<T>
    {
        public int TotalCount { get; set; }
        public int TotalPages
        {
            get
            {
                if (Pagesize != 0)
                {
                    return (int)Math.Ceiling((double)TotalCount / Pagesize);
                }
                return 0;
            }
        }
        //public string prevLink { get; set; }
        //public string nextLink { get; set; }
        public IEnumerable<T> Results { get; set; }
        public int Pagesize { get; set; }
        public PagedListServer(IEnumerable<T> results, int totalcount, int pagesize = 0)
        {
            TotalCount = totalcount;
            Pagesize = pagesize;
            Results = results;
        }

    }
   

    public class PagedListClient<T>
    {
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        //public string prevLink { get; set; }
        //public string nextLink { get; set; }
        public StaticPagedList<T> Results { get; set; }

    }

    public class Model<T>
    {
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public string prevLink { get; set; }
        public string nextLink { get; set; }
        public IEnumerable<T> Results { get; set; }

    }
}

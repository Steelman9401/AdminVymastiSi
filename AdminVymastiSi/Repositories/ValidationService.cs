using AdminVymastiSi.Data;
using AdminVymastiSi.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminVymastiSi.Repositories
{
    public class ValidationService
    {
        public bool VideoExists(string url)
        {
            using (var db = new myDb())
            {
                var urls = db.Videos.OrderByDescending(x => x.Id).Select(p => p.Url).Take(10).ToList();
                if (urls.Contains(url))
                    return true;
                else
                    return false;
            }
        }
        public bool CategoryExists(CategoryDTO cat)
        {
            using (var db = new myDb())
            {
                int check = db.Categories
                    .Where(x => x.Name == cat.Name || x.Name_en == cat.Name_en).Count();
                if (check > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool ImageExists(string image, IEnumerable<string> Images)
        {
            var array = image.Split('/');
            string trimmedUrl = "";
            for (int i = 1; i <= 4; i++)
            {
                trimmedUrl = trimmedUrl.Insert(0, array[array.Length - i]);
            }
            if (trimmedUrl.Contains('?'))
            {
                trimmedUrl = trimmedUrl.Substring(0, trimmedUrl.IndexOf("?"));
            }
            int count = Images.Where(x => x == trimmedUrl).Count();
            if (count != 0)
                return true;
            else
                return false;
        }
    }
}
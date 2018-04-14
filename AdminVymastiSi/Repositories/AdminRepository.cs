﻿using AdminVymastiSi.Data;
using AdminVymastiSi.DTO;
using AdminVymastiSi.Repositories;
using DotVVM.Framework.Controls;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AdminVymastiSi.Repositories
{
    public class AdminRepository:ValidationService
    {
        public async Task<GridViewDataSetLoadedData<VideoListAdminDTO>> GetAllVideosAdmin(IGridViewDataSetLoadOptions gridViewDataSetLoadOptions)
        {
            using (var db = new myDb())
            {
                var query = db.Videos.Select(x => new VideoListAdminDTO
                {
                    Id = x.Id,
                    Img = "/Previews/" + x.Img,
                    Title_en = x.Title_en,
                    Title = x.Title,
                    Preview = "/Previews/" + x.Preview,
                }).OrderByDescending(p => p.Id).AsQueryable();
                return query.GetDataFromQueryable(gridViewDataSetLoadOptions);
            }
        }
        public async Task<VideoAdminDTO> GetVideoById(int Id)
        {
            using (var db = new myDb())
            {
                return await db.Videos.
                    Where(x => x.Id == Id)
                    .Select(p => new VideoAdminDTO
                    {
                        Id = p.Id,
                        Description = p.Description,
                        Img = p.Img,
                        Preview = p.Preview,
                        Title = p.Title,
                        Url = p.Url,
                        Categories = p.Categories
                       .Select(a => a.Name)
                    }).FirstOrDefaultAsync();
            }
        }
        public async Task AddPorn(VideoAdminDTO vid)
        {
            var downloadPath = GetDownloadPath();
            var previewfileName = GetVideoFileName(vid.Preview);
            var imgfileName = GetVideoFileName(vid.Img);
            if (imgfileName.Contains("?"))
            {
                imgfileName = imgfileName.Substring(0, imgfileName.IndexOf("?"));
            }
            if (previewfileName.Contains("?"))
            {
                previewfileName = previewfileName.Substring(0, previewfileName.IndexOf("?"));
            }
            using (var client = new WebClient())
            {
                client.DownloadFile(vid.Preview, downloadPath + "/" + previewfileName);
                client.DownloadFile(vid.Img, downloadPath + "/" + imgfileName);
            }
            Video video = new Video();
            video.Description = vid.Description;
            video.Title = vid.Title;
            video.Title_en = vid.Title_en;
            video.Url = vid.Url;
            video.Img = imgfileName;
            video.Date = DateTime.Now;
            video.Preview = previewfileName;
            video.HD = vid.HD;
            video.Duration = vid.Duration;
            List<Category> listCat = new List<Category>();
            foreach (string item in vid.Categories)
            {
                Category cat = new Category();
                cat.Name = item;
                listCat.Add(cat);
            }
            using (var db = new myDb())
            {
                foreach (Category item in listCat)
                {
                    Category tag = await db.Categories
                  .Where(x => x.Name == item.Name)
                  .FirstOrDefaultAsync();
                    if (tag == null)
                    {
                        video.Categories.Add(item);
                    }
                    else
                    {
                        video.Categories.Add(tag);
                    }
                }
                db.Videos.Add(video);
                await db.SaveChangesAsync();

            }
        }
        public async Task UpdateVideo(VideoAdminDTO video)
        {
            using (var db = new myDb())
            {
                var dbVideo = db.Videos.Find(video.Id);
                dbVideo.Title = video.Title;
                dbVideo.Description = video.Description;
                dbVideo.Img = video.Img;
                dbVideo.Preview = video.Preview;
                dbVideo.Url = video.Url;
                dbVideo.Categories.Clear();
                foreach (string item in video.Categories)
                {
                    var category = await db.Categories.Where(x => x.Name == item).FirstOrDefaultAsync();
                    if (category != null)
                    {
                        dbVideo.Categories.Add(category);
                    }
                    else
                    {
                        Category dbCat = new Category();
                        dbCat.Name = item;
                        dbVideo.Categories.Add(dbCat);
                    }
                }
                await db.SaveChangesAsync();
            }
        }
        public async Task RemoveVideo(int Id)
        {
            using (var db = new myDb())
            {
                var video = await db.Videos.FindAsync(Id);
                db.Comments.RemoveRange(video.Comments);
                db.Videos.Remove(video);
                await db.SaveChangesAsync();
            }
        }
        public async Task AddCategory(CategoryDTO category)
        {
            using (var db = new myDb())
            {
                Category dbCat = new Category();
                dbCat.Name = category.Name;
                dbCat.Name_en = category.Name_en;
                db.Categories.Add(dbCat);
                await db.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<string>> GetCategories()
        {
            using (var db = new myDb())
            {
                return await db.Categories
                    .Select(x => x.Name).ToListAsync();
            }
        }

        private string GetVideoFileName(string url)
        {
            string[] array = url.Split('/');
            string trimmedUrl = "";
            for (int i = 1; i <= 4; i++)
            {
                trimmedUrl = trimmedUrl.Insert(0, array[array.Length - i]);
            }
            return trimmedUrl;
        }
        private string GetDownloadPath()
        {
            var virtualPath = Path.Combine(HttpContext.Current.Request.ApplicationPath, "Previews");
            var finalPath = HttpContext.Current.Request.MapPath(virtualPath);
            if (!Directory.Exists(finalPath))
            {
                Directory.CreateDirectory(finalPath);
            }
            return finalPath;
        }

    }

}
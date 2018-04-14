using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdminVymastiSi.DTO;
using AdminVymastiSi.Repositories;
using AdminVymastiSi.SupportClasses;
using DotVVM.Framework.Controls;
using DotVVM.Framework.Storage;
using DotVVM.Framework.ViewModel;
using HtmlAgilityPack;

namespace AdminVymastiSi.ViewModels.administration
{
    public class SpiderViewModel : MasterPageViewModel
    {
        public List<VideoListAdminDTO> Videos { get; set; } = new List<VideoListAdminDTO>();
        public SpiderRepository SpiderRep { get; set; } = new SpiderRepository();
        public int SwitchWebsite { get; set; } = 0;
        public string ImageName { get; set; }
        public VideoAdminDTO Video { get; set; } = new VideoAdminDTO();
        public AdminRepository AdminRep { get; set; } = new AdminRepository();
        public List<WebCategory> WebCategories { get; set; } = new List<WebCategory>
        {
            new WebCategory() { Name = "Hlavní stránka", Url = ""},
            new WebCategory() { Name = "Anál", Url = "anal"},
            new WebCategory() { Name = "Amatérky", Url = "amateur"},
            new WebCategory() { Name = "Asiatky", Url = "asian"},
            new WebCategory() { Name = "Blondýny", Url = "blonde"},
            new WebCategory() { Name = "Èernošky", Url = "ebony"},
            new WebCategory() { Name = "Zrzky", Url = "redhead"},
            new WebCategory() { Name = "MILF", Url = "milf"},
            new WebCategory() { Name = "Gay", Url = "gay"},
            new WebCategory() { Name = "Brunetky", Url = "brunette"},
            new WebCategory() { Name = "Lesbièky", Url = "lesbian"},

        };
        public string SelectedWebCategory { get; set; } = "";

        public async override Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                SpiderRep.GetRedTubeVideos(Videos, string.Empty);
            }
            if (SwitchWebsite == 0)
                ImageName = "../../Content/redtube.png";
            else if (SwitchWebsite == 1)
                ImageName = "../../Content/xhamster.png";
            else if (SwitchWebsite == 2)
                ImageName = "../../Content/drtuber.png";

            await base.PreRender();
        }

        public async Task OpenModal(VideoListAdminDTO vid)
        {
            Video.Duration = vid.Duration;
            Video.HD = vid.HD;
            Video.Img = vid.Img;
            Video.Preview = vid.Preview;
            Video.Title = vid.Title;
            Video.Title_en = vid.Title_en;
            Video.Url = vid.Url;
            Video.Id = vid.Id;
            Video.DatabaseCategories = await AdminRep.GetCategories();
            if (SwitchWebsite == 0)
                SpiderRep.GetCategoriesRedTube(Video);
            else if (SwitchWebsite == 1)
                SpiderRep.GetCategoriesXhamster(Video);
            else if (SwitchWebsite == 2)
                SpiderRep.GetCategoriesDrTuber(Video);
        }
        public void ChangeCategoryList()
        {
            Videos = new List<VideoListAdminDTO>();
            if (SwitchWebsite == 1)
                SpiderRep.GetXhamsterVideos(Videos, SelectedWebCategory);
            else if (SwitchWebsite == 0)
                SpiderRep.GetRedTubeVideos(Videos, SelectedWebCategory);
            else if (SwitchWebsite == 2)
                SpiderRep.GetDrTuberVideos(Videos, SelectedWebCategory);
        }
        public void SwitchToRedTube()
        {
            SwitchWebsite = 0;
            Videos = new List<VideoListAdminDTO>();
            SpiderRep.GetRedTubeVideos(Videos, SelectedWebCategory);
        }
        public async Task AddVideo()
        {
            await Video.AddVideo();
            Videos.RemoveAll(x => x.Id == Video.Id);
        }
        public async Task AddCategory()
        {
            if (await Video.AddCategoryToDatabase())
            {
                Video.NewCategory.Name = "";
                Video.NewCategory.Name_en = "";
                Video.DatabaseCategories = await AdminRep.GetCategories();
            }
        }
        public void CreateCustomVideo()
        {
            Video = new VideoAdminDTO();
            Video.IsCustom = true;
            List<string> list = new List<string>();
            for (int i = 0; i < 3; i++)
            {
                list.Add("");
            }
            Video.Categories = list;
        }
        public void SwitchToXhamster()
        {
            SwitchWebsite = 1;
            Videos = new List<VideoListAdminDTO>();
            SpiderRep.GetXhamsterVideos(Videos, SelectedWebCategory);
        }
        public void SwitchToDrTuber()
        {
            SwitchWebsite = 2;
            Videos = new List<VideoListAdminDTO>();
            SpiderRep.GetDrTuberVideos(Videos, SelectedWebCategory);
        }
        public void CloseModal()
        {
            Video = new VideoAdminDTO();
        }
        public void DoStuff()
        {

        }
    }
}


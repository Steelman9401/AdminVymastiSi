using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdminVymastiSi.DTO;
using AdminVymastiSi.Repositories;
using AdminVymastiSi.SupportClasses;
using DotVVM.Framework.Controls;
using DotVVM.Framework.Runtime.Filters;
using DotVVM.Framework.Storage;
using DotVVM.Framework.ViewModel;
using HtmlAgilityPack;

namespace AdminVymastiSi.ViewModels.administration
{
    [Authorize()]
    public class SpiderViewModel : MasterPageViewModel
    {
        public List<VideoListAdminDTO> Videos { get; set; } = new List<VideoListAdminDTO>();
        public SpiderRepository SpiderRep { get; set; } = new SpiderRepository();
        public int SwitchWebsite { get; set; } = 3;
        public string ImageName { get; set; }
        public VideoAdminDTO Video { get; set; } = new VideoAdminDTO();
        public AdminRepository AdminRep { get; set; } = new AdminRepository();
        public List<WebCategory> WebCategories { get; set; } = new List<WebCategory>
        {
            new WebCategory() { Name = "Hlavn� str�nka", Url = ""},
            new WebCategory() { Name = "An�l", Url = "anal"},
            new WebCategory() { Name = "Amat�rky", Url = "amateur"},
            new WebCategory() { Name = "Asiatky", Url = "asian"},
            new WebCategory() { Name = "Blond�ny", Url = "blonde"},
            new WebCategory() { Name = "�erno�ky", Url = "ebony"},
            new WebCategory() { Name = "Zrzky", Url = "redhead"},
            new WebCategory() { Name = "MILF", Url = "milf"},
            new WebCategory() { Name = "Gay", Url = "gay"},
            new WebCategory() { Name = "Brunetky", Url = "brunette"},
            new WebCategory() { Name = "Lesbi�ky", Url = "lesbian"},
            new WebCategory() { Name = "Teenky", Url = "teen"},
            new WebCategory() { Name = "�e�ky", Url = "czech"},
            new WebCategory() { Name = "BDSM", Url = "bdsm"},
            new WebCategory() { Name = "Hentai", Url = "hentai"},

        };
        public string SelectedWebCategory { get; set; } = "";

        public async override Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                SpiderRep.GetPornHubVideos(Videos, string.Empty);
            }
            if (SwitchWebsite == 0)
                ImageName = "../../Content/redtube.png";
            else if (SwitchWebsite == 1)
                ImageName = "../../Content/xhamster.png";
            else if (SwitchWebsite == 2)
                ImageName = "../../Content/drtuber.png";
            else if (SwitchWebsite == 3)
                ImageName = "../../Content/pornhub.png";

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
            if(SelectedWebCategory=="gay")
            {
                Video.AllowMain = false;
                Video.CheckBoxEnabled = false;
            }
            if (Video.DatabaseCategories.Count() == 0)
            {
                Video.DatabaseCategories = await AdminRep.GetCategoriesAsync();
            }
            if (SwitchWebsite == 0)
                SpiderRep.GetCategoriesRedTube(Video);
            else if (SwitchWebsite == 1)
                SpiderRep.GetCategoriesXhamster(Video);
            else if (SwitchWebsite == 2)
                SpiderRep.GetCategoriesDrTuber(Video);
            else if (SwitchWebsite == 3)
                SpiderRep.GetCategoriesPornHub(Video);
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
            else if (SwitchWebsite == 3)
                SpiderRep.GetPornHubVideos(Videos, SelectedWebCategory);
        }
        public void SwitchToRedTube()
        {
            SwitchWebsite = 0;
            Videos = new List<VideoListAdminDTO>();
            SpiderRep.GetRedTubeVideos(Videos, SelectedWebCategory);
        }
        public void SwitchToPornHub()
        {
            SwitchWebsite = 3;
            Videos = new List<VideoListAdminDTO>();
            SpiderRep.GetPornHubVideos(Videos, SelectedWebCategory);
        }
        public async Task AddVideo()
        {
            await Video.AddVideo(CurrentUser);
            Videos.RemoveAll(x => x.Id == Video.Id);
        }
        public async Task AddCategory()
        {
            if (await Video.AddCategoryToDatabase(CurrentUser))
            {
                Video.NewCategory.Name = "";
                Video.NewCategory.Name_en = "";
                Video.DatabaseCategories = await AdminRep.GetCategoriesAsync();
            }
        }
        public async Task CreateCustomVideo()
        {
            if (Video.DatabaseCategories.Count() == 0)
            {
                Video.DatabaseCategories = await AdminRep.GetCategoriesAsync();
            }
            Video.IsCustom = true;
            List<CategoryDTO> list = new List<CategoryDTO>();
            for (int i = 0; i < 3; i++)
            {
                CategoryDTO cat = new CategoryDTO();
                list.Add(cat);
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
            Video.Categories = new List<CategoryDTO>();
            Video.CategoryAdded = false;
            Video.ErrorMessage = "";
            Video.IsCustom = false;
            Video.IsEdited = false;
            Video.NewCategory = new CategoryDTO();
            Video.ShowCategoryOption = false;
            Video.Success = false;
            Video.Url = null;
            Video.AllowMain = true;
            Video.CheckBoxEnabled = true;
            Video.LoadError = false;
        }
    }
}


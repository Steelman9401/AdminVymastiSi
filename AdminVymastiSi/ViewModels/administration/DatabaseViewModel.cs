using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdminVymastiSi.DTO;
using AdminVymastiSi.Repositories;
using DotVVM.Framework.Controls;
using DotVVM.Framework.ViewModel;

namespace AdminVymastiSi.ViewModels.administration
{
    public class DatabaseViewModel : MasterPageViewModel
    {
        public AdminRepository AdminRep { get; set; } = new AdminRepository();
        public GridViewDataSet<VideoListAdminDTO> Videos { get; set; } = new GridViewDataSet<VideoListAdminDTO>()
        {
            PagingOptions = { PageSize = 8 }
        };
        public VideoAdminDTO Video { get; set; }
        public override Task PreRender()
        {
            Videos.OnLoadingDataAsync = option => AdminRep.GetAllVideosAdmin(option,CurrentUser);
            return base.PreRender();
        }
        public async Task EditVideo(VideoListAdminDTO vid)
        {
            Video = await AdminRep.GetVideoById(vid.Id);
            Video.DatabaseCategories = await AdminRep.GetCategories();
            Video.IsEdited = true;
        }
        public async Task UpdateVideo()
        {
            await Video.UpdateVideo();
            var editedVideo = Videos.Items
                 .Where(x => x.Id == Video.Id)
                 .First();
            editedVideo.Title = Video.Title; 
        }
        public void CloseModal()
        {
            
        }
        public async Task AddCategory()
        {
            if (await Video.AddCategoryToDatabase(CurrentUser))
            {
                Video.NewCategory.Name = "";
                Video.NewCategory.Name_en = "";
                Video.DatabaseCategories = await AdminRep.GetCategories();
            }
        }
    }
}


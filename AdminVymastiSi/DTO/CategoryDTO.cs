using AdminVymastiSi.Repositories;
using DotVVM.Framework.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AdminVymastiSi.DTO
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Name_en { get; set; }
        public async Task RemoveCategory(GridViewDataSet<CategoryDTO> Categories)
        {
            AdminRepository AdminRep = new AdminRepository();
            Categories.Items.Remove(this);
            await AdminRep.RemoveCategoryAsync(Id);
        }
    }
}
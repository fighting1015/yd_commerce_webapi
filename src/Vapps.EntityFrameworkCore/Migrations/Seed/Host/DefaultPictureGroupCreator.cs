using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vapps.EntityFrameworkCore;
using Vapps.Media;

namespace Vapps.Migrations.Seed.Host
{

    public class DefaultPictureGroupCreator
    {
        private readonly VappsDbContext _context;

        public DefaultPictureGroupCreator(VappsDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateEditions();
            CreatePictureIfNotExist((int)DefaultGroups.ProfilePicture, "头像");
            CreatePictureIfNotExist((int)DefaultGroups.ProductPicture, "商品");
        }

        private void CreateEditions()
        {
        }

        private void CreatePictureIfNotExist(int groupId, string name)
        {
            var defaultGroup = _context.PictureGroups.IgnoreQueryFilters().FirstOrDefault(ef => ef.Id == groupId && ef.Name == name);

            if (defaultGroup == null)
            {
                _context.PictureGroups.Add(new PictureGroup
                {
                    Name = name,
                    IsSystemGroup = true,
                });
            }
        }
    }
}

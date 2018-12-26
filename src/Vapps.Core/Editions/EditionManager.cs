using Abp;
using Abp.Application.Editions;
using Abp.Application.Features;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vapps.Configuration;
using Vapps.Features;

namespace Vapps.Editions
{
    public class EditionManager : AbpEditionManager
    {
        public const string DefaultEditionName = "Standard";

        protected IRepository<SubscribableEdition> _subscribableEditionRepository { get; set; }

        public IQueryable<SubscribableEdition> SubscribableEditions => _subscribableEditionRepository.GetAll();

        private readonly ISettingManager _settingManager;

        public EditionManager(
            IRepository<Edition> editionRepository,
            IRepository<SubscribableEdition> subscribableEditionRepository,
            IAbpZeroFeatureValueStore featureValueStore,
            ISettingManager settingManager)
            : base(
                editionRepository,
                featureValueStore
            )
        {
            this._subscribableEditionRepository = subscribableEditionRepository;
            this._settingManager = settingManager;
        }

        public async Task<int?> GetDefaultEditionIdAsync()
        {
            var defaultEditionIdValue = await _settingManager.GetSettingValueForApplicationAsync(AppSettings.TenantManagement.DefaultEdition);
            int? defaultEditionId = null;

            if (!string.IsNullOrEmpty(defaultEditionIdValue) && (await FindByIdAsync(Convert.ToInt32(defaultEditionIdValue)) != null))
            {
                defaultEditionId = Convert.ToInt32(defaultEditionIdValue);
            }

            return defaultEditionId;
        }

        public async Task<SubscribableEdition> GetDefaultEditionAsync()
        {
            var defaultEditionIdValue = await _settingManager.GetSettingValueForApplicationAsync(AppSettings.TenantManagement.DefaultEdition);
            int? defaultEditionId = null;

            if (!string.IsNullOrEmpty(defaultEditionIdValue) && (await FindByIdAsync(Convert.ToInt32(defaultEditionIdValue)) != null))
            {
                defaultEditionId = Convert.ToInt32(defaultEditionIdValue);
            }

            return (SubscribableEdition)await GetByIdAsync(defaultEditionId.Value);
        }

        /// <summary>
        /// 获取最高版本Id
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetHighestEditionIdAsync()
        {
            var top = await SubscribableEditions.OrderByDescending(s => s.MonthlyPrice).FirstOrDefaultAsync();
            return top.Id;
        }

        /// <summary>
        /// 获取最高版本
        /// </summary>
        /// <returns></returns>
        public async Task<SubscribableEdition> GetHighestEditionAsync()
        {
            var top = await SubscribableEditions.OrderByDescending(s => s.MonthlyPrice).FirstOrDefaultAsync();
            return top;
        }

        public async Task<List<Edition>> GetAllAsync()
        {
            return await EditionRepository.GetAllListAsync();
        }

        public virtual async Task<IReadOnlyList<FeatureValue>> GetFeatureNestValuesAsync(int editionId)
        {
            var values = new List<FeatureValue>();
            var features = FeatureManager.GetAll().ToList();
            foreach (var feature in features)
            {
                if (feature.Parent != null)
                {
                    var parentValue = values.FirstOrDefault(v => v.Name == feature.Parent.Name);
                    if (parentValue == null)
                    {
                        parentValue = new FeatureValue(feature.Parent.Name, await GetFeatureValueOrNullAsync(editionId, feature.Parent.Name) ?? feature.Parent.DefaultValue);
                    }

                    if (parentValue.Childs == null)
                    {
                        parentValue.Childs = new List<NameValue>();
                    }

                    parentValue.Childs.Add(new NameValue(feature.Name, await GetFeatureValueOrNullAsync(editionId, feature.Name) ?? feature.DefaultValue));
                }
                else
                {
                    values.Add(new FeatureValue(feature.Name, await GetFeatureValueOrNullAsync(editionId, feature.Name) ?? feature.DefaultValue));
                }
            }

            return values;
        }

    }
}

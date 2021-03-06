﻿using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Vapps.Consts;

namespace Vapps.States
{
    [Table("StateCitys")]
    public class City : FullAuditedEntity, IPassivable
    {
        /// <summary>
        /// 省份Id
        /// </summary>
        public int ProvinceId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [StringLength(EntityConsts.MaxEntityNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// 显示顺序
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// 启用
        /// </summary>
        public bool IsActive { get; set; }
    }
}

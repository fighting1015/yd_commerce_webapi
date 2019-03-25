using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Vapps.Auditing.Dto;
using Vapps.Dto;

namespace Vapps.Auditing
{
    public interface IAuditLogAppService : IApplicationService
    {
        /// <summary>
        /// 获取审计日志
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<AuditLogListDto>> GetAuditLogs(GetAuditLogsInput input);

        /// <summary>
        /// 导入审计日志到Excel
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<FileDto> GetAuditLogsToExcel(GetAuditLogsInput input);

        /// <summary>
        /// 获取实体变更
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<EntityChangeListDto>> GetEntityChanges(GetEntityChangeInput input);

        /// <summary>
        /// 导出实体变更到Excel
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<FileDto> GetEntityChangesToExcel(GetEntityChangeInput input);

        /// <summary>
        /// 获取字段变更
        /// </summary>
        /// <param name="entityChangeId"></param>
        /// <returns></returns>
        Task<List<EntityPropertyChangeDto>> GetEntityPropertyChanges(long entityChangeId);

        /// <summary>
        /// 获取实体记录对象类型
        /// </summary>
        /// <returns></returns>
        List<NameValueDto> GetEntityHistoryObjectTypes();
    }
}
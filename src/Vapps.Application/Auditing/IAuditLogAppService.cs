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
        /// ��ȡ�����־
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<AuditLogListDto>> GetAuditLogs(GetAuditLogsInput input);

        /// <summary>
        /// ���������־��Excel
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<FileDto> GetAuditLogsToExcel(GetAuditLogsInput input);

        /// <summary>
        /// ��ȡʵ����
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<EntityChangeListDto>> GetEntityChanges(GetEntityChangeInput input);

        /// <summary>
        /// ����ʵ������Excel
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<FileDto> GetEntityChangesToExcel(GetEntityChangeInput input);

        /// <summary>
        /// ��ȡ�ֶα��
        /// </summary>
        /// <param name="entityChangeId"></param>
        /// <returns></returns>
        Task<List<EntityPropertyChangeDto>> GetEntityPropertyChanges(long entityChangeId);

        /// <summary>
        /// ��ȡʵ���¼��������
        /// </summary>
        /// <returns></returns>
        List<NameValueDto> GetEntityHistoryObjectTypes();
    }
}
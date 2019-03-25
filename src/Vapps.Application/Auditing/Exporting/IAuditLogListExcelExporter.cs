using System.Collections.Generic;
using Vapps.Auditing.Dto;
using Vapps.Dto;

namespace Vapps.Auditing.Exporting
{
    public interface IAuditLogListExcelExporter
    {
        FileDto ExportToFile(List<AuditLogListDto> auditLogListDtos);

        FileDto ExportChangeToFile(List<EntityChangeListDto> entityChangeListDtos);
    }
}

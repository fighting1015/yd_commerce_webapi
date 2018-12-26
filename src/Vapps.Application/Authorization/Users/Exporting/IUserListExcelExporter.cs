using System.Collections.Generic;
using Vapps.Authorization.Users.Dto;
using Vapps.Dto;

namespace Vapps.Authorization.Users.Exporting
{
    public interface IUserListExcelExporter
    {
        FileDto ExportToFile(List<UserListDto> userListDtos);
    }
}
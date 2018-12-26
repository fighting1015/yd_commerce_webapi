using Abp.Runtime.Validation;
using Vapps.Dto;

namespace Vapps.Authorization.Users.Dto
{
    public class GetLinkedUsersInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Username";
            }
        }
    }
}
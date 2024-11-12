using System;

namespace QTimes.core.common.Dto
{
    public class RoleDto
    {
        public RoleDto(Guid roleId, string roleName)
        {
            RoleId = roleId;
            RoleName = roleName;
        }

        public Guid RoleId { get; private set; }
        public string RoleName { get; private set; }
    }
}

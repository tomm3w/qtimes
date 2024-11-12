using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatQ.core.common.Dto
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

using TLGames.Core.Enums;

namespace TLGames.Core.Entities
{
    public class RoleModel
    {
        public int RoleId { get; private set; }
        public string RoleName { get; private set; }
        public EActiveStatus Status { get; private set; }

        public RoleModel() { }

        public RoleModel(int roleId, string roleName, EActiveStatus status)
        {
            RoleId = roleId;
            RoleName = roleName;
            Status = status;
        }

        public void SetRoleId(int id) { RoleId = id; }
        public void SetRoleName(string name) { RoleName = name; }
        public void SetStatus(EActiveStatus status) { Status = status; }

    }
}

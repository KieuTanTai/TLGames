namespace TLGames.Models
{
    internal class RoleModel
    {
        public int RoleId { get; private set; }
        public string RoleName { get; private set; }

        public RoleModel() { }

        public RoleModel(int roleId, string roleName)
        {
            RoleId = roleId;
            RoleName = roleName;
        }

        public void SetRoleId(int id) { RoleId = id; }
        public void SetRoleName(string name) { RoleName = name; }
    }
}

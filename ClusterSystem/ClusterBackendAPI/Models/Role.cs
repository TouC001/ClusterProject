namespace ClusterBackendAPI.Models
{
    public class Role
    {
        public int Id { get; set; }

        required public string Name { get; set; }

        public List<UserRole> userRoles { get; set; }
    }
}

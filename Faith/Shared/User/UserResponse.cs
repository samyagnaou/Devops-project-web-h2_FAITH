namespace Faith.Shared.User;

public static class UserResponse
{
    public class GetIndex
    {
        public List<UserDTO.Index> Users { get; set; } = new();
    }
    public class GetDetail
    {
        public UserDTO.Detail User { get; set; }
    }
    public class GetRoleByUserId
    {
        public string Role { get; set; }
    }
    public class GetUsersByRole
    {
        public List<UserDTO.Index> Users { get; set; } = new();
    }
    public class Create
    {
        public string Auth0UserId { get; set; }
    }
    public class Edit
    {
        public int UserId { get; set; }
    }
    public class Delete
    {

    }
}
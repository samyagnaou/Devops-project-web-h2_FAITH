namespace Faith.Shared.User;

public static class UserRequest
{
    public class GetIndex
    {

    }

    public class GetDetail
    {
        public int UserId { get; set; }
    }
    public class GetRoleByUserId
    {
        public string UserId { get; set; }
    }
    public class GetUsersByRole
    {
        public string Role { get; set; }
    }


    public class Create
    {
        public UserDTO.Mutate User { get; set; }
    }
    public class Edit
    {
        public int UserId { get; set; }
        public UserDTO.Mutate User { get; set; }
    }
    public class Delete
    {
        public int UserId { get; set; }
    }
}
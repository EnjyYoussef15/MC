using System.ComponentModel.DataAnnotations;

namespace MCSHiPPERS_Task.DTO
{
    public class LoginDTO
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [StringLength(15, ErrorMessage = "Your Password is limited to {2} to {1} character"), MinLength(6)]
        public string Password { get; set; }
    }
    public class UserDTO : LoginDTO
    {
        [Required]
        public string UserName { get; set; }


    }

    public class newpasswordDTO
    {
        [Required]
        public string password { get; set; }
        [Required]
        public string email { get; set; }

        public string? Token { get; set; }
    }

}


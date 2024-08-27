using System.ComponentModel.DataAnnotations;

namespace CoreApiInNet.Users
{
    public class ApiUserDto
    {
        [Required]
        public string nick {  get; set; }
        [Required]
        public string password { get; set; }
    }
}

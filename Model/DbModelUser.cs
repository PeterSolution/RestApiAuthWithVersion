using System.ComponentModel.DataAnnotations;

namespace CoreApiInNet.Model
{
    public class DbModelUser
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string password { get; set; }

    }
}

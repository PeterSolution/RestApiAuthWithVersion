using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreApiInNet.Model
{
    public class DbModelData
    {
        [Key]
        public int IdData { get; set; }
        public string data { get; set; }
        [ForeignKey(nameof(IdUser))]
        public int IdUser { get; set; } //id of user who created this
    }
}

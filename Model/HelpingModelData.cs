using System.ComponentModel.DataAnnotations.Schema;

namespace CoreApiInNet.Model
{
    public class HelpingModelData
    {
        public string data { get; set; }
        public int IdUser { get; set; } //id of user who created this
    }
}

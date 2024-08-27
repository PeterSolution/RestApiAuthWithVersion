using System.ComponentModel.DataAnnotations;

namespace CoreApiInNet.Model
{
    public class FullDataModel:ClassId
    {
        public string data { get; set; }
        public int IdUser { get; set; }
    }
}

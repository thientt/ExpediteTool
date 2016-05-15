
using System.Collections.Generic;
namespace ExpediteTool.Model.DataTransfer
{
    public class BuDto
    {
        public int BuId { get; set; }
        public string BuName { get; set; }
        public string Description { get; set; }
        public List<LotExpediteDto> HotLotData { get; set; }
    }
}

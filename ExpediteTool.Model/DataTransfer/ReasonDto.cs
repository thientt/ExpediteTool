using System;

namespace ExpediteTool.Model.DataTransfer
{
    public class ReasonDto
    {
        public int IdHotLotData { get; set; }
        public string LotId { get; set; }
        public string Reason { get; set; }
        public DateTime RequestOutDate { get; set; }
        public string UserNameRequest { get; set; }
    }
}

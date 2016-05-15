using System;

namespace ExpediteTool.Model.DataTransfer
{
    /// <summary>
    /// 
    /// </summary>
    public class LotExpediteDto : DataTransferBase
    {
        /// <summary>
        /// Gets or sets the lot identifier.
        /// </summary>
        /// <value>
        /// The lot identifier.
        /// </value>
        public string LotId { get; set; }

        /// <summary>
        /// Gets or sets the reason.
        /// </summary>
        /// <value>
        /// The reason.
        /// </value>
        public string Reason { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public StatusType Status { get; set; }

        /// <summary>
        /// Gets or sets the request out date.
        /// </summary>
        /// <value>
        /// The request out date.
        /// </value>
        public DateTime RequestOutDate { get; set; }
        
        /// <summary>
        /// Gets or sets the device.
        /// </summary>
        /// <value>
        /// The device.
        /// </value>
        public string Device { get; set; }
        /// <summary>
        /// Gets or sets the requestor.
        /// </summary>
        /// <value>
        /// The requestor.
        /// </value>
        public string Requestor { get; set; }

        /// <summary>
        /// Gets or sets the bu.
        /// </summary>
        /// <value>
        /// The bu.
        /// </value>
        public BuDto Bu { get; set; }

        /// <summary>
        /// Gets or sets the bu identifier.
        /// </summary>
        /// <value>
        /// The bu identifier.
        /// </value>
        public int BUId { get; set; }

        /// <summary>
        /// Gets or sets the owner.
        /// </summary>
        /// <value>
        /// The owner.
        /// </value>
        public string Owner { get; set; }

        /// <summary>
        /// Gets or sets the SCM end date.
        /// </summary>
        /// <value>
        /// The SCM end date.
        /// </value>
        public DateTime? ScmEndDate { get; set; }

        /// <summary>
        /// Gets or sets the platform.
        /// </summary>
        /// <value>
        /// The platform.
        /// </value>
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the platform.
        /// </summary>
        /// <value>
        /// The platform.
        /// </value>
        public string Platform { get; set; }

        /// <summary>
        /// Gets or sets the current operation.
        /// </summary>
        /// <value>
        /// The current operation.
        /// </value>
        public string CurrentOperation { get; set; }

        /// <summary>
        /// Gets or sets the lot priority.
        /// </summary>
        /// <value>
        /// The lot priority.
        /// </value>
        public int LotPriority { get; set; }

        /// <summary>
        /// Gets the action.
        /// </summary>
        /// <value>
        /// The action.
        /// </value>
        public string Action
        {
            get
            {
                string result = "Make Active";
                switch (Status)
                {
                    case StatusType.ACTIVED:
                        result = "Close Lot";
                        break;
                    case StatusType.PENDING:
                        result = "Make Active";
                        break;
                }
                return result;
            }
        }
    }
}

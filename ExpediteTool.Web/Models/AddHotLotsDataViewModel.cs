using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebForm.ModelBinder;

namespace ExpediteTool.Web.Models
{
    [RaiseBindingError]
    public class AddHotLotsDataViewModel
    {
        [Required]
        [ValueSource(FromProperty.Text)]
        public string txtLotId { get; set; }

        [ValueSource(FromProperty.Text)]
        public string txtReason { get; set; }

        [TypeConvert]
        [ValueSource(FromProperty.SelectedValue)]
        public int cboBu { get; set; }

        [ValueSource(FromProperty.Text)]
        public string txtOwner { get; set; }

        [ValueSource(FromProperty.Text)]
        public string txtComment { get; set; }

        [ValueSource(FromProperty.Text)]
        public string txtPlatform { get; set; }

        [ValueSource(FromProperty.Text)]
        public string txtCurrentOperation { get; set; }

        [ValueSource(FromProperty.Text)]
        public string txtDevice { get; set; }

        [ValueSource(FromProperty.Text)]
        public string txtRequestor { get; set; }
    }
}
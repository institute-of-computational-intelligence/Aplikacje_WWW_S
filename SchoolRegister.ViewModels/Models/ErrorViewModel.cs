using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolRegister.ViewModels.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}

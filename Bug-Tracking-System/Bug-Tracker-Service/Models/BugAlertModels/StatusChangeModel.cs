using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bug_Tracker_Service.Models.BugAlertModels
{
    public class StatusChangeModel
    {
        public int id { get; set; }

        public int developerId { get; set; }

        public int assignedBy { get; set; }

        public string bugAlertResolutionDescription { get; set; }
    }
}
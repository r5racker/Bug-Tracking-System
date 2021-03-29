using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bug_Tracker_Service.Tests.Models
{
    public class BugAlert
    {
        int _bugId = 0;
        string _title = "bug alert";
        string _description = "";
        int _createdBy = 0;
        int _categoryId = 0;
        BugAlertStatus _status = BugAlertStatus.None;
        string _resolutionDescription = "";
        string _reportPath = "";
        DateTime _createdOn = DateTime.Now;
        DateTime? _assignedOn = DateTime.MinValue;
        DateTime? _resolvedOn = DateTime.MinValue;

        public int BugId
        {
            get { return _bugId; }
            set { _bugId = value; }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public int CreatedBy
        {
            get { return _createdBy; }
            set { _createdBy = value; }
        }

        public int CategoryId
        {
            get { return _categoryId; }
            set { _categoryId = value; }
        }

        public BugAlertStatus Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public string ResolutionDescription
        {
            get { return _resolutionDescription; }
            set { _resolutionDescription = value; }
        }

        public string ReportPath
        {
            get { return _reportPath; }
            set { _reportPath = value; }
        }

        public DateTime CreatedOn
        {
            get { return _createdOn; }
            set { _createdOn = value; }
        }

        public DateTime? AssignedOn
        {
            get { return _assignedOn; }
            set { _assignedOn = value; }
        }

        public DateTime? ResolvedOn
        {
            get { return _resolvedOn; }
            set { _resolvedOn = value; }
        }
    }
}

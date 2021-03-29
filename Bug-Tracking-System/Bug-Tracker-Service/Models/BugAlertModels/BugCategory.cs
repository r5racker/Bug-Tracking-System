using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bug_Tracker_Service.Tests.Models
{
    public class BugCategory
    {
        int _categoryId = 0;
        string _title = "";
        string _description = "";
        int _createdBy = 0;
        int _alertCount = 0;
        int _alertCountUnresolved = 0;

        public int CategoryId
        {
            get { return _categoryId; }
            set { _categoryId = value; }
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

        
        public int AlertCount
        {
            get { return _alertCount; }
            set { _alertCount = value; }
        }

        
        public int AlertCountUnresolved
        {
            get { return _alertCountUnresolved; }
            set { _alertCountUnresolved = value; }
        }
    }
}

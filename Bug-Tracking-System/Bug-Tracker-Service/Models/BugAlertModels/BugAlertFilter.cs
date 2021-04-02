using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bug_Tracker_Service.Models
{
    public enum BugAlertFilter
    {
        All,
        AllUnresolved,
        AllByTester,
        AllByDeveloper,
        UnresolvedByTester,
        UnresolvedByDeveloper,
        ResolvedByDeveloper
    }
}

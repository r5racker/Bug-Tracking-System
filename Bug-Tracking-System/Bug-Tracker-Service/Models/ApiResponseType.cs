using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bug_Tracker_Service.Models
{
    public enum ApiResponseType
    {
        None,
        UserCreate,
        UserUpdate,
        UserDelete,
        UserActionError,
        BugCreate,
        BugUpdate,
        BugDelete,
        BugActionError
    }
}
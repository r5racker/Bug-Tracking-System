using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bug_Tracker_Service.Models
{
    public class ApiResponseFactory
    {
        public string Generate(ApiResponseType rType)
        {
            switch (rType)
            {
                case ApiResponseType.None:
                    return "";
                case ApiResponseType.BugCreate:
                    return "Bug Alert Record added Successfully.";
                case ApiResponseType.BugUpdate:
                    return "Bug Alert Record Updated Successfully.";
                case ApiResponseType.BugDelete:
                    return "Bug Alert Assignment Record Deleted Successfully.";
                case ApiResponseType.BugActionError:
                    return "Error occured while working with Bug Alert";
                case ApiResponseType.UserCreate:
                    return "User added Successfully.";
                case ApiResponseType.UserUpdate:
                    return "User updated Successfully.";
                case ApiResponseType.UserDelete:
                    return "User deleted Successfully.";
                case ApiResponseType.UserActionError:
                    return "Error occured while working with User";
                default:
                    return "";
            }
        }
    }
}
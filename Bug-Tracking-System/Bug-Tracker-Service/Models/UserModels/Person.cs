using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bug_Tracker_Service.Models
{
    public class Person
    {
        int _personId = 0;
        string _name = "";
        string _email = "";
        string _contact = "";
        string _password = "";
        int _createdBy = 1;
        UserRole _role = UserRole.Any;

        public int PersonId
        {
            get { return _personId; }
            set { _personId = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public string Contact
        {
            get { return _contact; }
            set { _contact = value; }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public int CreaedBy
        {
            get { return _createdBy; }
            set { _createdBy = value; }
        }

        public UserRole Role
        {
            get { return _role; }
            set { _role = value; }
        }
    }
}
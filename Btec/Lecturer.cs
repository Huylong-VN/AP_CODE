using System;

namespace Btec
{
    internal class Lecturer : Person
    {
        public string Department { set; get; }

        public Lecturer()
        { }

        public Lecturer(Guid id, string name, DateTime dob, string email, string address, string department)
        {
            Id = id;
            Name = name;
            Dob = dob;
            Email = email;
            Address = address;
            Department = department;
        }
    }
}
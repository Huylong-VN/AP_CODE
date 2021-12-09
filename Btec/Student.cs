using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Btec
{
    internal class Student : Person
    {
        public string Batch { set; get; }

        public Student()
        {
        }

        public Student(Guid id, string name, DateTime dob, string email, string address, string batch)
        {
            Id = id;
            Name = name;
            Dob = dob;
            Email = email;
            Address = address;
            Batch = batch;
        }

        public override string ToString()
        {
            return "Id: " + Id + "\n" + "Name: " + Name + "\n" + "Dob: " + Dob + "\n" + "Email: " + Email + "\n" + "Address: " + Address + "\n" + "Batch: " + Batch;
        }
    }
}
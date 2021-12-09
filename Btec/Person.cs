using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Btec
{
    public class Person
    {
        public Guid Id { set; get; }

        public string Name { set; get; }
        public DateTime Dob { set; get; }
        public string Email { set; get; }
        public string Address { set; get; }
    }
}
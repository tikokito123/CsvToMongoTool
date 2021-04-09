using System;
using System.Collections.Generic;
using System.Text;

namespace Mongo
{
    class Employee
    {
        public string first_name, last_name, job;
        public int age { get; set; }
        public DateTime joined { get; set; }
    }
}

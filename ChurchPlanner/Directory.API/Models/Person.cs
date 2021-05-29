using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace Directory.API.Models
{
    public class Person
    {
        public Person()
        {
            FamilyRoles = new HashSet<FamilyPerson>();
        }

        public Guid PersonId { get; set; }

        public String Name { get; set; }

        [DataType(DataType.PhoneNumber)] public String PhoneNumber { get; set; }
        [DataType(DataType.EmailAddress)] public String EmailAddress { get; set; }

        public String Notes { get; set; }

        public ICollection<FamilyPerson> FamilyRoles { get; set; }
    }
}
using System;

namespace Directory.API.Models
{
    public class FamilyPerson
    {
        public Guid PersonId { get; set; }

        public PersonType Type { get; set; }

        public Guid FamilyId { get; set; }
    }
}
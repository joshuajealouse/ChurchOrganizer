using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Directory.API.Models
{
    public class Family
    {
        public Family()
        {
            FamilyMembers = new HashSet<FamilyPerson>();
        }

        [Key] public Guid FamilyId { get; set; }

        public string Notes { get; set; }

        public ICollection<FamilyPerson> FamilyMembers { get; set; }
    }
}
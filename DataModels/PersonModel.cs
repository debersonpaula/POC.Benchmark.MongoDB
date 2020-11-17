using System;
using System.Collections.Generic;

namespace DataModels
{
    public class PersonModel
    {
        public Guid Id { get; set; }
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JobArea { get; set; }
        public string JobDescriptor { get; set; }
        public string JobTitle { get; set; }
        public string JobType { get; set; }
        public IList<OrderModel> Orders { get; set; }
    }
}

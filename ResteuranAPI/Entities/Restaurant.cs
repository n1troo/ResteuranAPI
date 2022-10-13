﻿ namespace ResteuranAPI.Entities
{
    public class Restaurant
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public bool HasDelivery { get; set; }
        public string ConcactEmail { get; set; }
        public string ConcactNumber { get; set; }
        public int AddressId { get; set; }
        public DateTime AddDate { get; set; }

        public int? CreatedById { get; set; }

        public virtual User CreatedBy { set; get; }

        public virtual Address Address { get; set; }
        public virtual List<Dish> Dishes { get; set; }

    }
}

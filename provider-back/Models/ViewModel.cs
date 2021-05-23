using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace provider_back.Models
{
    public class ProviderViewModel
    {
        public int ProviderID { get; set; }
        public int ProviderOrder { get; set; }
        public string ProviderName { get; set; }
        public string ProviderBusiness_Name { get; set; }
        public string ProviderNIT { get; set; }
        public string ProviderAddress { get; set; }
        public string ProviderPhoneNumber { get; set; }
        public DateTime ProviderCreation_Date { get; set; }
        public DateTime? ProviderModification_Date { get; set; }
        public decimal ProviderRating_Number { get; set; }
    }
}

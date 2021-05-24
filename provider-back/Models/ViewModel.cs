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
        public string ProviderBusinessName { get; set; }
        public string ProviderNIT { get; set; }
        public string ProviderAddress { get; set; }
        public string ProviderPhoneNumber { get; set; }
        public DateTime ProviderCreationDate { get; set; }
        public DateTime? ProviderLastModificationDate { get; set; }
        public string ProviderRatingNumber { get; set; }
    }
}

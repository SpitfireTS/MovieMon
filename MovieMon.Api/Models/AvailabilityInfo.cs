using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace MovieMon.Api.Models
{
    public class AvailabilityInfo
    {
        public string ProviderName { get; set; }
        public string DeliveryFormat { get; set; }
        public DateTime? AvailableFrom { get; set; }
        public DateTime? AvailableTo { get; set; }
        public List<string> Addresses { get; set; }
    }
}
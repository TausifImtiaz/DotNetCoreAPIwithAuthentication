using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DotNetCoreAPIwithAuthentication.Models
{
    public class OrderDetail
    {
        [Key]
        public int DetailId { get; set; }
        public int OrderId { get; set; }
        [JsonIgnore]
        public OrderMaster OrderMaster { get; set; }
        public int PlantId { get; set; }
        public Plant Plant { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
    }
}

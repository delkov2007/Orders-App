using System.Xml.Serialization;

namespace Orders.Models
{
    public class ItemModel
    {
        public string ID { get; set; }
        public int ProductCode { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Price { get; set; }
        public string Image { get; set; }
    }

}

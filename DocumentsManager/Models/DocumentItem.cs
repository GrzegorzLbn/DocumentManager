namespace DocumentsManager.Models
{
    public class DocumentItem
    {
        public const string TableName = "DocumentItems";

        public int Id { get; set; }
        public string ArtName { get; set; }
        public int Quantity { get; set; }
        public double NetPrice { get; set; }
        public double GrossPrice { get; set; }
    }
}

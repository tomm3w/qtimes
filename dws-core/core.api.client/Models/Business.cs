namespace core.api.client.Models
{
    public class Business
    {
        public int BusinessId { get; set; }

        public string Name { get; set; }

        public Business(int businessId, string name)
        {
            this.BusinessId = businessId;
            this.Name = name;
        }
    }
}

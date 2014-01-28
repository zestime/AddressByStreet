namespace AddressByStreet.Model
{
    public class Address
    {
        public string Code { get; set; }
        public string Post { get; set; }
        // TODO - Combine common part of address
        public string Street{ get; set; } 
        public string LandLot{ get; set; } 
    }
}
using Newtonsoft.Json;

namespace Vapps.Helpers
{
    public class Longitude2Address
    {
        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("result")]
        public AddressResult Result { get; set; }

        public class AddressResult
        {
            [JsonProperty("address")]
            public string Address { get; set; }

            [JsonProperty("formatted_addresses")]
            public FormattedAddressResult FormattedAddress { get; set; }

            public class FormattedAddressResult
            {
                [JsonProperty("recommend")]
                public string Recommend { get; set; }

                [JsonProperty("rough")]
                public string Rough { get; set; }
            }
        }
    }
}

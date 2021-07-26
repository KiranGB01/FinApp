using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinApp.Data
{
    public class Customers
    {
        [JsonProperty(PropertyName="id")]
       // public string Id { get; set; }
        public string Accountnumber { get; set; }
        public string Name { get; set; }
        public string Email{ get; set; }
        public int Balance { get; set; }
        public string Password { get; set; }
        public string Cityname { get; set; }
    }
}

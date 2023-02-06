using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class AdCompany
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string StreetAdress { get; set; }
        public string ZipCode { get; set; }
        public string PhoneNumber { get; set; }
        public override string ToString()
        {
            return CompanyName;
        }
    }

    public class AdCompanyVM : AdCompany
    {
        public string City { get; set; }
        public string State { get; set; }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WebApplication1.Models
{
    public class customerModel
    {
        [Key]
        public int customerId { get; set; }

        public string Name { get; set; }

        [EmailAddress]
        public string eMail { get; set; }

        public companyModell company { get; set; }

  
    }

    public class companyModell
    {
        [Key]
        public int companyId { get; set; }
        
        public string companyOrgNr { get; set; }

        public string compayPgNr { get; set; }

        public string companyName { get; set; }

        public List<suplierModel> suplierModels { get; set; }
       
    }
    public class suplierModel
    {
        [Key]
        public int suplierId { get; set; }

        public string suplierName { get; set; }

        public string suplierOrgNr { get; set; }

        public string suplierPgNr { get; set; }

        public string suplierBgNr { get; set; }

        public string currencyCode { get; set; }

        public string suplierOrgNrFormat { get; set; }

        public string suplierPgNrFormat { get; set; }

        public string suplierBgNrFormat { get; set; }

        public string currencyFormat { get; set; }

        public string purchOrderNoFormat { get; set; }

        public string invoiceDateFormat { get; set; }

        public string suplierOrcNrFormat { get; set; }


    }

}

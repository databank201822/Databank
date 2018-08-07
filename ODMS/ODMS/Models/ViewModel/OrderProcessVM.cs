using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ODMS.Models.ViewModel
{
    public class OrderProcessIvm
    {
        public int Psrid { get; set; }
        public string PsrName { get; set; }
        public int DBid { get; set; }
        public int NoOfOutlet { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
    }
}
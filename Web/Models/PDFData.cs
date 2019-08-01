using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Web;

namespace Web.Models
{
    public class PDFData
    {
        private string _tin = string.Empty;
        private string _customerSince = string.Empty;
        private string _pointBalance = string.Empty;
        private string _tinFormatted = string.Empty;
        private string _customerSinceFormatted = string.Empty;
        private string _pointBalanceFormatted = string.Empty;


      [DisplayName("First")]  public string FirstName { get; set; }
      [DisplayName("Middle")] public string MiddleInitial { get; set; }
      [DisplayName("Last")] public string LastName { get; set; }
      [DisplayName("Street")] public string Street { get; set; }
      [DisplayName("City")] public string City { get; set; }
      [DisplayName("State")] public string State { get; set; }
      [DisplayName("Zip")] public string Zip { get; set; }
      [DisplayName("Customer Since")] public string CustomerSince { get => _customerSince; set => _customerSince = processCustomerSince(value); }
      [DisplayName("Active")] public bool Active { get; set; }
      [DisplayName("Point Balance")] public string PointBalance { get => _pointBalance ;   set => _pointBalance = processPointBalance(value);}
      [DisplayName("TIN")] public string TIN { get => _tin; set => _tin = processTIN(value); }


      string processPointBalance(string pointBalance)
      {
          int result;





          if (string.IsNullOrEmpty(pointBalance) ||  !int.TryParse(pointBalance.Replace(",", ""), out result))
              pointBalance = string.Empty;


          _pointBalanceFormatted = pointBalance;

            _pointBalance = pointBalance.Replace(",", "");

          return pointBalance.Replace(",","");
      }
      string processCustomerSince(string customerSince)
      {
          DateTime result;
          if (!DateTime.TryParse(customerSince, out result))
              customerSince = string.Empty;

          _customerSinceFormatted = customerSince;

          return customerSince.Replace("/","");
      }
      string processTIN(string tin)
      {

          if (!Regex.Match(tin.Replace("-",""), @"^[0-9]{9}$").Success)
              tin = string.Empty;


          _tinFormatted = tin;

            tin =  tin.Replace("-", "");
            


          return tin;
      }
    }
}
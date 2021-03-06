﻿using System.ComponentModel;

namespace Web.Models
{
    public class PDFData
    {
      [DisplayName("First")]  public string FirstName { get  ; set; }
      [DisplayName("Middle")] public string MiddleInitial { get; set; }
      [DisplayName("Last")] public string LastName { get; set; }
      [DisplayName("Street")] public string Street { get; set; }
      [DisplayName("City")] public string City { get; set; }
      [DisplayName("State")] public string State { get; set; }
      [DisplayName("Zip")] public string Zip { get; set; }
      [DisplayName("Customer Since")] public string CustomerSince { get; set; }
      [DisplayName("Active")] public bool Active { get; set; }
      [DisplayName("Point Balance")] public string PointBalance { get; set; }
      [DisplayName("TIN")] public string TIN { get; set; }
    }
}
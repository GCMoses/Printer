﻿using System.ComponentModel.DataAnnotations;


namespace Printer.Core.Models;

public class Company
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Display(Name = "Street Address")]
    public string? StreetAddress { get; set; }
    public string? City { get; set; }
    [Display(Name = "Province")]
    public string? State { get; set; }
    [Display(Name = "Postal Code")]
   public string? PostalCode { get; set; }
    [Display(Name = "Phone Number")]
    public string? PhoneNumber { get; set; }
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }

}

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Printer.Core.Models;
public class Product
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    public string Description { get; set; }
    [Display(Name = "List Price")]
    [Required]
    [Range(1, 10000)]
    public double Price { get; set; }
	[ValidateNever]
	public string ImageUrl { get; set; }
	[ValidateNever]
	
    [Display(Name = "Category")]
    [Required]
    public int CategoryId { get; set; }
    [ForeignKey("CategoryId")]
    [ValidateNever]
    public Category Category { get; set; }
    [Display(Name = "DigiPix Cover")]
    [Required]
    public int DigiPixCoverId { get; set; }
    [ValidateNever]
    public DigiPixCover DigiPixCover { get; set; }
}

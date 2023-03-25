using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Printer.Core.Models;

public class DigiPixCover
{
    [Key]
    public int Id { get; set; }

    [Display(Name = "DigiPix Cover")]
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }
}

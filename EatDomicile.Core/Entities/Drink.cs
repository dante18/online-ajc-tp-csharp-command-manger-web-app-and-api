using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EatDomicile.Core.Entities;

[Table("Drink")]
public class Drink : Product
{

    [Required]
    public bool Fizzy { get; set; }

    [Required]
    public decimal KCal {  get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace EatDomicile.Core.Entities;

public class Pasta : Food
{
    [Required]
    public int Type { get; set; }

    [Required]
    public decimal KCal { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace EatDomicile.Core.Entities;

public class Food : Product
{
    [Required]
    public bool Vegetarian { get; set; }
}
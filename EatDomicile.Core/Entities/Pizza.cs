﻿using System.ComponentModel.DataAnnotations;

namespace EatDomicile.Core.Entities;

public class Pizza : Food
{
    [Required]
    public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();

    public int DoughsId { get; set; }

    [Required]
    public Doughs Doughs { get; set; }
}


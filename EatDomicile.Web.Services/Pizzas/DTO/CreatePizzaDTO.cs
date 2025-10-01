using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EatDomicile.Web.Services.Pizzas.DTO;

public class CreatePizzaDTO
{
    public int? Id { get; set; }

    public string Name { get; set; }

    public decimal Price { get; set; }

    public int DoughsId { get; set; }

    public bool Vegetarian { get; set; }
}

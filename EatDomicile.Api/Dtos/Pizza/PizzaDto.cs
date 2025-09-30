using System.ComponentModel.DataAnnotations;

namespace EatDomicile.Api.Dtos.Pizza
{
    public class PizzaDto
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public int Dough { get; set; }
    }
}

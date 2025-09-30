using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EatDomicile.Api.Dtos.Dough
{
    public class DoughDto
    {
        public int? Id { get; set; }

        public string Name { get; set; }
    }
}

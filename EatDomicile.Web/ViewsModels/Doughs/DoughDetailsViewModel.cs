using System.ComponentModel;

namespace EatDomicile.Web.ViewsModels.Doughs;

public class DoughDetailsViewModel
{
    public int? Id { get; init; }

    [DisplayName("Nom")]
    public string Name { get; init; }
}

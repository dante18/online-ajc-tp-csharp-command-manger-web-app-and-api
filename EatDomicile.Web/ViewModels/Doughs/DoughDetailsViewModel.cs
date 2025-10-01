using System.ComponentModel;

namespace EatDomicile.Web.ViewModels.Doughs;

public class DoughDetailsViewModel
{
    public int? Id { get; init; }

    [DisplayName("Nom")]
    public string Name { get; init; }
}

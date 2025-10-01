using System.ComponentModel;

namespace EatDomicile.Web.ViewModels.Doughs;

public class DoughViewModel
{
    public int? Id { get; set; }

    [DisplayName("Nom")]
    public string Name { get; set; }
}


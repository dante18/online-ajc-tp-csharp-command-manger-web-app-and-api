using System.ComponentModel;

namespace EatDomicile.Web.ViewsModels.Doughs;

public class DoughViewModel
{
    public int? Id { get; set; }

    [DisplayName("Nom")]
    public string Name { get; set; }
}


using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace EatDomicile.Web.ViewsModels.Doughs;

public class DoughEditViewModel
{
    [HiddenInput]
    public int Id { get; set; }

    [DisplayName("Nom")]
    public string Name { get; set; }
}

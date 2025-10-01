using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace EatDomicile.Web.ViewModels.Doughs;

public class DoughEditViewModel
{
    [HiddenInput]
    public int Id { get; set; }

    [DisplayName("Nom")]
    public string Name { get; set; }
}

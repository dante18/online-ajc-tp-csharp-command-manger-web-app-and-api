using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EatDomicile.Web.ViewsModels.Users;

public class UsersCreateViewModel
{
    [DisplayName("Prénom")]
    public string FirstName { get; set; }

    [DisplayName("Nom")]
    public string LastName { get; set; }

    [DisplayName("Téléphone")]
    public string Phone { get; set; }

    [DisplayName("Email")]
    public string Mail { get; set; }

    [DisplayName("Rue")]
    public string Street { get; set; }

    [DisplayName("Ville")]
    public string City { get; set; }

    [DisplayName("Région")]
    public string State { get; set; }

    [DisplayName("Code postal")]
    public string Zip { get; set; }

    [DisplayName("Pays")]
    public string Country { get; set; }

    [DisplayName("Mot de passe de l'utilisateur")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [ValidateNever]
    public List<SelectListItem> Roles { get; set; }

    [DisplayName("Rôle de l'utilisateur")]
    public string RoleId { get; set; }
}

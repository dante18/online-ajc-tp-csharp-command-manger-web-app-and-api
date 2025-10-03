using System.ComponentModel;

namespace EatDomicile.Web.ViewModels.Users;

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
}

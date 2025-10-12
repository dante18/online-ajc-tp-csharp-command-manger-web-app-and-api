namespace EatDomicile.Web.ViewsModels.Security;

using System.ComponentModel.DataAnnotations;

public class RegisterViewModel
{
    [Required(ErrorMessage = "L'email est requis")]
    [EmailAddress(ErrorMessage = "Format d'email invalide")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Le mot de passe est requis")]
    [MinLength(6, ErrorMessage = "Le mot de passe doit contenir au moins 6 caractères")]
    [DataType(DataType.Password)]
    [Display(Name = "Mot de passe")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "La confirmation est requise")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirmer le mot de passe")]
    [Compare("Password", ErrorMessage = "Les mots de passe ne correspondent pas")]
    public string ConfirmPassword { get; set; } = string.Empty;
}

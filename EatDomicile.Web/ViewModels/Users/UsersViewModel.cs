using System.ComponentModel;

namespace EatDomicile.Web.ViewModels.Users
{
    public class UsersViewModel
    {
        public int? Id { get; set; }

        [DisplayName("FirstName")]
        public string FirstName { get; set; }

        [DisplayName("LastName")]
        public string LastName { get; set; }

        [DisplayName("Phone")]
        public string Phone { get; set; }

        [DisplayName("Mail")]
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
}

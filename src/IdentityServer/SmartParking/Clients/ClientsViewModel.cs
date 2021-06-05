using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerHost.SmartParking.UI
{
    public class ClientsViewModel
    {
        [Required(AllowEmptyStrings =false, ErrorMessage = "Please input Client Id")]
        [RegularExpression("[A-Za-z0-9]{16,16}", ErrorMessage = "Invalid ClientId. ClientId should be 16 charactors")]
        public string ClientId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please input Client Name")]
        public string ClientName { get; set; }
        //[RegularExpression("[A-Z0-9a-z]{8}", ErrorMessage = "Invalid ClientSecret. ClientSecret should be grreater than 8 charactors")]
        public string ClientSecret { get; set; }
        public IEnumerable<string> AllowedGrantTypes { get; set; }
        public IEnumerable<string> RedirectUris { get; set; }
        public bool RequirePkce { get; set; }
        public IEnumerable<string> AllowedScopes { get; set; }
        public IEnumerable<string> AllowedCorsOrigins { get; set; }
        public bool RequireClientSecret { get; set; }
        public IEnumerable<string> PostLogoutRedirectUris { get; set; }
        public bool RequireConsent { get; set; }
        public int AccessTokenLifetime { get; set; }
    }
}

using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ravi.learn.identity.mvc.Services
{
    public class ProfileClaimsTransformation : IClaimsTransformation
    {
        private readonly IProfileService _profileService;

        public ProfileClaimsTransformation(IProfileService profileService)
        {
            this._profileService = profileService;
        }
        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var identity = principal.Identities.FirstOrDefault(x => x.IsAuthenticated);
            if (identity == null)
            {
                return principal;
            }

            var idClaim = identity.FindFirst("name");
            if (idClaim == null)
            {
                return principal;
            }
            var profile = await _profileService.GetUserProfileAsync(idClaim.Value);
            if (profile == null)
            {
                return principal;
            }
            var claims = new List<Claim>
            {
                idClaim,
                new Claim(ClaimTypes.GivenName, profile.FirstName,ClaimValueTypes.String,"ProfileClaimsTransformation"),
                new Claim(ClaimTypes.Surname,profile.LastName, ClaimValueTypes.String,"ProfileClaimsTransformation"),
                new Claim(ClaimTypes.Name, $"{profile.FirstName} {profile.LastName}",ClaimValueTypes.String,"ProfileClaimsTransformation")
            };
            claims.AddRange(profile.Roles.Select(x => new Claim(ClaimTypes.Role, x, ClaimValueTypes.String, "ProfileClaimsTransformation")));

            var claimsIdentity = new ClaimsIdentity(claims, identity.AuthenticationType);
            return new ClaimsPrincipal(claimsIdentity);
        }
    }
}

using DAL;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Security.Claims;

namespace Panel.Extention
{
    public static class Util
    {

        public static Contractor GetContractor(this ClaimsPrincipal user)
        {
            try
            {
                if (user.Identity.IsAuthenticated)
                {
                    var strings = user.Identities.First(c => c.IsAuthenticated && c.HasClaim(e => e.Type == ClaimTypes.UserData)).FindFirst(ClaimTypes.UserData).Value;
                    var data = JsonConvert.DeserializeObject<Contractor>(strings);
                    return data;
                }
                return null;
            }

            catch (Exception)
            {
                ;

            }
            return null;
        }

    }
}

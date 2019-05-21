using DAL;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Security.Claims;

namespace DriverArea.Util
{
    public static class Util
    {

        public static Driver GetDriver(this ClaimsPrincipal user)
        {
            try
            {
                if (user.Identity.IsAuthenticated)
                {
                    var strings = user.Identities.First(c => c.IsAuthenticated && c.HasClaim(e => e.Type == ClaimTypes.UserData)).FindFirst(ClaimTypes.UserData).Value;
                    var data = JsonConvert.DeserializeObject<Driver>(strings);
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

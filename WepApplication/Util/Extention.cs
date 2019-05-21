using DAL;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Security.Claims;

namespace WepApplication.Util
{
    public static class Util
    {

        public static StudentParent Getparrent(this ClaimsPrincipal user)
        {
            try
            {
                if (user.Identity.IsAuthenticated)
                {
                    var strings = user.Identities.First(c => c.IsAuthenticated && c.HasClaim(e => e.Type == ClaimTypes.UserData)).FindFirst(ClaimTypes.UserData).Value;
                    var data = JsonConvert.DeserializeObject<StudentParent>(strings);
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

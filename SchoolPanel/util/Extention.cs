using DAL;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Security.Claims;

namespace Panel.Extention
{
    public static class Util
    {

        public static Academy GetAcademy(this ClaimsPrincipal user)
        {
            try
            {
                if (user.Identity.IsAuthenticated)
                {
                    var strings = user.Identities.First(c => c.IsAuthenticated && c.HasClaim(e => e.Type == ClaimTypes.UserData)).FindFirst(ClaimTypes.UserData).Value;
                    var data = JsonConvert.DeserializeObject<Academy>(strings);
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




        public static string ToPersionMount(this int value)
        {
            switch (value)
            {
                case 1: return "فروردین";
                case 2: return "اردیبهشت";
                case 3: return "خرداد";
                case 4: return "خرداد";
                case 5: return "مرداد";
                case 6: return "شهریور";
                case 7: return "مهر";
                case 8: return "آبان";
                case 9: return "آذر";
                case 10: return "دی";
                case 11: return "بهمن";
                case 12: return "اسفند";
                default: return "";
            }
        }

    }
}

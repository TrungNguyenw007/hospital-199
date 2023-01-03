using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;
using Newtonsoft.Json;

namespace Core.Extension
{
    public static class ObjectExtensions
    {
        public static T Clone<T>(this T source)
        {
            var serialized = JsonConvert.SerializeObject(source);
            return JsonConvert.DeserializeObject<T>(serialized);
        }
        
    }

    public static class ClaimPrincipalExtensions
    {
        public static bool HasClaim(this ClaimsPrincipal source,string type,string value,bool demo)
        {
            var claim = source.Claims.FirstOrDefault(x =>x.Type ==type);
            if (claim == null) return false;
         
            var values = claim.Value.Split(new []{','}).ToList();
            if (values.FirstOrDefault(x=>x.Contains(value)) !=null) return true;
            return false;
        }
    }
}
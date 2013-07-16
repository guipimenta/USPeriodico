using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace USPeriodico.Controllers
{
    public class Utilitarios
    {
        public static bool TestEmailRegex(string emailAddress)
        {
            /*
             * Retorna TRUE caso seja um email vÃ¡lido (sintaticamente) da USP e FALSE caso contrÃ¡rio
             */
            // Email esperado "*@usp.br"
            if(emailAddress.Length <= 7)
                return false;
            // EMAIL VALIDATION
            //                string patternLenient = @"\w([.]\w)@\w([.]\w)\.\w([-.]\w)*";
            //                Regex reLenient = new Regex(patternLenient);
            string patternStrict = @"^(([^<>()[\]\\.,;:\s@\""]" 
                 +  @"(\.[^<>()[\]\\.,;:\s@\""])*)|(\"".\""))@"
                 +  @"((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}"
                 +  @"\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]\.)"
                 +  @"[a-zA-Z]{2,}))$";
            Regex reStrict = new Regex(patternStrict);

            //                      bool isLenientMatch = reLenient.IsMatch(emailAddress);
            //                      return isLenientMatch;

            bool isStrictMatch = reStrict.IsMatch(emailAddress);

            // DOMAIN VALIDATION
            if (isStrictMatch) // email bem formado
                return emailAddress.Substring(emailAddress.LastIndexOf('@'), 7).CompareTo("@usp.br").Equals(true);
            else
                return false; // email mal formado
        }


    }
}
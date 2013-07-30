using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using USPeriodico.Models;

namespace USPeriodico.Controllers
{
    public class Utilitarios
    {
        public static bool TestEmailRegex(string emailAddress)
        {
            /*
             * Retorna TRUE caso seja um email vÃ¡lido (sintaticamente) e FALSE caso contrÃ¡rio
             */
            // EMAIL VALIDATION
            //                string patternLenient = @"\w([.]\w)@\w([.]\w)\.\w([-.]\w)*";
            //                Regex reLenient = new Regex(patternLenient);
            string patternStrict = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                                + "@"
                                + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";
            Regex reStrict = new Regex(patternStrict);

            //                      bool isLenientMatch = reLenient.IsMatch(emailAddress);
            //                      return isLenientMatch;

            bool isStrictMatch = reStrict.IsMatch(emailAddress);

            // DOMAIN VALIDATION
            if (isStrictMatch) // email bem formado
                return true;
            else
                return false; // email mal formado
        }

        public static bool VerificaEmailUSP(string emailAddress)
        {
            return emailAddress.Substring(emailAddress.LastIndexOf('@'), 7).Equals("@usp.br");
        }

        public static int VerificaUsuario(int role, string login)
        {
            //verifica se é um usuário logado
            if (login.Equals(""))
                return -1;

            usperiodicoEntities aux = new usperiodicoEntities();
            Usuarios usuario = aux.Usuarios.First(Usuario => Usuario.email == login);
            //verifica se não é um outro tipo de usuario
            if (usuario.role != 1 && usuario.role != role)
                return 0;

            return 1;
        }
    }
}
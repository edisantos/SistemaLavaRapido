using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace SkywalkerAutoWash.BLL
{
    public class Util
    {
       

        public static void Alertar(string mensagem)
        {
            //HttpContext.Current.Session["alert"] = mensagem;
        }
        public static void EnviarEmail(string assunto, string mensagem, string destinatario)
        {
            var smtp = new SmtpClient("smtp.live.com", 587);
            smtp.EnableSsl = true;
            var msg = new MailMessage("edinaldolemos@yahoo.com.br", destinatario, assunto, mensagem);
            msg.IsBodyHtml = true;
            var cred = new NetworkCredential("edi.santos", "lemos318");
            smtp.Credentials = cred;
            smtp.Send(msg);
        }
        public static string GerarSenhaAleatoria(int tamanho)
        {
            string caracterPermitidos = "abcdefghijkmnopqrstuwxyzABCDEFGHJKLMNOPQSTUVWXYZ0123456789!@$?_-*#+";
            Char[] senhaGerada = new char[tamanho];
            Random rd = new Random();
            for (int i = 0; i < tamanho; i++)
            {
                senhaGerada[i] = caracterPermitidos[rd.Next(0, caracterPermitidos.Length)];

            }
            return new string(senhaGerada);
        }
    }
}

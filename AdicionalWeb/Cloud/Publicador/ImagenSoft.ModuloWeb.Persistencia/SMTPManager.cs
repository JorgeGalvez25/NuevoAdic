using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ImagenSoft.ModuloWeb.Persistencia
{
    public class SMTPManager
    {
        public bool Send(SMTPParameter p, ref string msj)
        {
            if (p.Destinatary.Count <= 0)
            {
                msj = "No hay destinatario(s).";
                return false;
            }

            string _innerMessage = string.Empty;

            TaskCompletionSource<bool> _task = new TaskCompletionSource<bool>(p);
            Task.Run(() =>
            {
                using (MailMessage message = new MailMessage())
                {
                    try
                    {
                        MailStringBuilder mb = new MailStringBuilder(ConfigurationManager.AppSettings["CfgMail"]);

                        message.From = new MailAddress(mb.User);
                        message.To.Add(p.Destinatary[0].Trim());

                        for (int i = 1; i < p.Destinatary.Count; i++)
                        {
                            message.CC.Add(p.Destinatary[i].Trim());
                        }

                        message.Subject = p.Subject;
                        message.IsBodyHtml = true;
                        message.BodyEncoding = System.Text.Encoding.UTF8;
                        message.Body = p.Message;

                        using (SmtpClient cliente = new SmtpClient(mb.Host, mb.Port))
                        {
                            cliente.EnableSsl = mb.UseSsl.HasValue ? mb.UseSsl.Value : false;
                            cliente.Credentials = new System.Net.NetworkCredential(mb.User, mb.Pass);
                            cliente.DeliveryMethod = SmtpDeliveryMethod.Network;
                            cliente.Send(message);
                            _task.TrySetResult(true);
                        }
                    }
                    catch (Exception e)
                    {
                        _innerMessage = e.Message;
                        _task.TrySetResult(false);
                    }
                }
            }).ConfigureAwait(false);

            bool result = _task.Task.Result;
            if (!result)
            {
                msj = _innerMessage;
            }
            return result;
        }

        private string CombinePath(char[] split, params string[] param)
        {
            List<string> aux = new List<string>();

            for (int i = 0; i < param.Length; i++)
            {
                aux.AddRange(param[i].Split(split, StringSplitOptions.RemoveEmptyEntries));
            }

            return aux.Aggregate((x, y) => x.Trim() + @"\\" + y.Trim());
        }

        public string LoadTemplate(string path)
        {
            string result = string.Empty;
            path = CombinePath(new char[] { '\\' }, Environment.CurrentDirectory, "templates", path);

            try
            {
                if (File.Exists(path))
                {
                    result = File.ReadAllText(path);
                }
            }
            catch { }
            return result;
        }

        private class MailStringBuilder : IDisposable
        {
            public MailStringBuilder()
                : this(string.Empty)
            {

            }

            public MailStringBuilder(string data)
            {
                this.Process(data);
            }

            private void Process(string data)
            {
                List<string> items = data.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                string[] aux = null;
                char[] spltChar = new char[] { '=' };

                items.ForEach(p =>
                {
                    p = p.Trim();
                    aux = p.Split(spltChar, StringSplitOptions.RemoveEmptyEntries);

                    if (aux.Length == 2)
                    {
                        aux[0] = aux[0].Trim();

                        switch (aux[0].ToLower())
                        {
                            case "host":
                                this.Host = aux[1];
                                break;
                            case "port":
                                int port = 0;
                                this.Port = int.TryParse(aux[1], out port) ? port : 25;
                                break;
                            case "user":
                                this.User = aux[1];
                                break;
                            case "pass":
                                this.Pass = aux[1];
                                break;
                            case "ssl":
                                this.UseSsl = aux[1].Equals("Si", StringComparison.OrdinalIgnoreCase);
                                break;
                        }
                    }
                });
            }

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                if (!string.IsNullOrEmpty(this.Host))
                {
                    sb.Append(string.Format("Host:{0}", this.Host));
                }

                if (this.Port > 0)
                {
                    sb.Append(string.Format("Port:{0}", this.Port));
                }

                if (!string.IsNullOrEmpty(this.User))
                {
                    sb.Append(string.Format("User:{0}", this.User));
                }

                if (!string.IsNullOrEmpty(this.Pass))
                {
                    sb.Append(string.Format("Pass:{0}", this.Pass));
                }

                if (this.UseSsl != null)
                {
                    sb.Append(string.Format("Ssl:{0}", (this.UseSsl.HasValue && this.UseSsl.Value) ? "Si" : "No"));
                }

                return base.ToString();
            }

            public string Host { get; set; }
            public int Port { get; set; }
            public string User { get; set; }
            public string Pass { get; set; }
            public bool? UseSsl { get; set; }

            #region IDisposable Members

            public void Dispose()
            {
                this.Dispose(true);
                GC.SuppressFinalize(this);
            }

            private void Dispose(bool dispose)
            {
                if (dispose)
                {
                    this.Host = null;
                    this.Pass = null;
                    this.Port = default(int);
                    this.User = null;
                    this.UseSsl = default(bool?);
                }
            }
            ~MailStringBuilder()
            {
                this.Dispose(false);
            }

            #endregion
        }
    }

    public class SMTPParameter : IDisposable
    {
        public SMTPParameter()
        {
            this.Destinatary = new List<string>();
            this.Message = string.Empty;
            this.Subject = string.Empty;
        }

        public string Message { get; set; }
        public List<string> Destinatary { get; set; }
        public string Subject { get; set; }

        #region IDisposable Members

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool dispose)
        {
            if (dispose)
            {
                this.Message = null;
                this.Subject = null;

                if (this.Destinatary != null)
                {
                    this.Destinatary.Clear();
                    this.Destinatary = null;
                }
            }
        }

        ~SMTPParameter()
        {
            this.Dispose(false);
        }

        #endregion
    }
}

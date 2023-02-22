using ImagenSoft.ModuloWeb.Entidades;
using ImagenSoft.Extensiones;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace ImagenSoft.ModuloWeb.Persistencia.Utilidades
{
    public delegate void SendMailEventHandler(object sender, MailEventArgs e);

    public class ServicioCorreo : IDisposable
    {
        private SmtpClient servicioHost;
        private MailMessage sendMessage;
        private Action<AsyncCompletedEventArgs> fnAction;

        private MailServerConfiguration MailServerConfiguration { get; set; }

        public event SendMailEventHandler AfterSendMail;
        public event SendMailEventHandler BeforeSendMail;

        public ServicioCorreo()
        {
            this.servicioHost = new SmtpClient();
            this.sendMessage = new MailMessage();
        }
        public ServicioCorreo(string host, string user, string password, bool ssl)
            : this()
        {
            this.SetHost(host, user, password, ssl);
            this.servicioHost.SendCompleted += this.servicioHost_SendCompleted;
        }

        #region Eventos

        private void OnSendMail(MailEventArgs e)
        {
            if (BeforeSendMail != null)
            {
                BeforeSendMail(this, e);
            }
        }
        private void OnCompleteSendMail(MailEventArgs e)
        {
            if (AfterSendMail != null)
            {
                AfterSendMail(this, e);
            }
        }

        private void servicioHost_SendCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (fnAction != null)
            {
                fnAction(e);
            }
        }

        #endregion

        public void Attachments(List<string> attachs)
        {
            for (int i = 0; i < attachs.Count; i++)
            {
                this.sendMessage.Attachments.Add(new Attachment(attachs[i]));
            }
        }
        public void Attachments(Attachment[] attachs)
        {
            for (int i = 0; i < attachs.Length; i++)
            {
                this.sendMessage.Attachments.Add(attachs[i]);
            }
        }
        public void Attachments(Dictionary<string, Stream> attachs)
        {
            string nombreArchivo = string.Empty;
            Collection<Attachment> attachList = new Collection<Attachment>();

            foreach (KeyValuePair<string, Stream> item in attachs)
            {
                nombreArchivo = Path.GetFileName(item.Key);
                attachList.Add(new Attachment(item.Value, nombreArchivo));
            }
            this.Attachments(attachList.ToArray());
        }

        public void SendMail()
        {
            this.SendMail(false);
        }
        public void SendMail(bool async)
        {
            this.SendMail(async, null);
        }
        public void SendMail(bool async, Action<AsyncCompletedEventArgs> events)
        {
            if (this.MailServerConfiguration == null)
            {
                this.MailServerConfiguration = new MailServerConfiguration();
            }

            if (this.MailServerConfiguration.Message_Id && !this.sendMessage.Headers.AllKeys.Contains("Message-Id"))
            {
                this.sendMessage.Headers.Add("Message-Id", string.Format("<{0}{1}>", Guid.NewGuid(), this.sendMessage.From.Address.Substring(this.sendMessage.From.Address.IndexOf('@'))));
            }

            if (this.MailServerConfiguration.Precedence && !this.sendMessage.Headers.AllKeys.Contains("Precedence"))
            {
                this.sendMessage.Headers.Add("Precedence", "bulk");
            }

            foreach (string item in this.sendMessage.Headers.AllKeys)
            {
                MensajesRegistros.Informacion(item, this.sendMessage.Headers[item]);
            }

            this.OnSendMail(new MailEventArgs(this.sendMessage, this.servicioHost));

            if (events != null)
            {
                this.fnAction = events;
            }

            Exception aux = null;

            try
            {
                if (!async)
                {
                    this.servicioHost.Send(this.sendMessage);
                }
                else
                {
                    this.servicioHost.SendAsync(this.sendMessage, this.servicioHost);
                }
            }
            catch (Exception e)
            {
                aux = e;
                if (fnAction != null)
                {
                    fnAction(new AsyncCompletedEventArgs(e, true, this));
                }
            }
            finally
            {
                this.OnCompleteSendMail(new MailEventArgs(aux, this.sendMessage, this.servicioHost));
            }
        }

        public void SetHost(string host, string user, string password)
        {
            this.SetHost(host, 0, new NetworkCredential(user, password));
        }
        public void SetHost(string host, NetworkCredential credential)
        {
            this.SetHost(host, 0, credential);
        }
        public void SetHost(string host, int port, string user, string password)
        {
            this.SetHost(host, port, new NetworkCredential(user, password));
        }
        public void SetHost(string host, int port, NetworkCredential credential)
        {
            this.SetHost(host, port, credential, false);
        }

        public void SetHost(string host, string user, string password, bool ssl)
        {
            this.SetHost(host, 0, new NetworkCredential(user, password), ssl);
        }
        public void SetHost(string host, NetworkCredential credential, bool ssl)
        {
            this.SetHost(host, 0, credential, ssl);
        }
        public void SetHost(string host, int port, string user, string password, bool ssl)
        {
            this.SetHost(host, port, new NetworkCredential(user, password), ssl);
        }
        public void SetHost(string host, int port, NetworkCredential credential, bool ssl)
        {
            this.servicioHost.Host = host;
            this.servicioHost.Port = port <= 0 ? 25 : port;

            if (credential != null)
            {
                this.servicioHost.UseDefaultCredentials = false;
                this.servicioHost.Credentials = credential;
            }
            else
            {
                this.servicioHost.UseDefaultCredentials = true;
            }

            //this.servicioHost.DeliveryMethod = SmtpDeliveryMethod.Network;
        }

        public void SetFrom(string from)
        {
            this.SetFrom(new MailAddress(from));
        }
        public void SetFrom(MailAddress from)
        {
            this.sendMessage.From = from;
        }
        public void SetFrom(string name, string from)
        {
            this.SetFrom(new MailAddress(from, name));
        }

        public void SetTo(string to)
        {
            this.sendMessage.To.Add(to);
        }
        public void SetTo(List<string> to)
        {
            MailAddressCollection collection = new MailAddressCollection();
            to.ForEach(p => collection.Add(new MailAddress(p)));
            this.SetTo(collection);
        }
        public void SetTo(MailAddressCollection to)
        {
            for (int i = 0; i < to.Count; i++)
            {
                this.sendMessage.To.Add(to[i]);
            }
        }
        public void SetTo(Dictionary<string, string> to)
        {
            MailAddressCollection collection = new MailAddressCollection();
            foreach (KeyValuePair<string, string> item in to)
            {
                collection.Add(new MailAddress(item.Value, item.Key));
            }
            this.SetTo(collection);
        }

        public void AddCopy(List<string> destiny)
        {
            MailAddressCollection collection = new MailAddressCollection();
            destiny.ForEach(p => collection.Add(new MailAddress(p)));

            this.AddCopy(collection);
        }
        public void AddCopy(MailAddressCollection destiny)
        {
            foreach (MailAddress item in destiny)
            {
                this.sendMessage.CC.Add(item);
            }
        }
        public void AddCopy(Dictionary<string, string> destiny)
        {
            MailAddressCollection collection = new MailAddressCollection();
            foreach (KeyValuePair<string, string> item in destiny)
            {
                collection.Add(new MailAddress(item.Value, item.Key));
            }

            this.AddCopy(collection);
        }
        public void AddCopy(string destiny, params char[] separator)
        {
            this.AddCopy(this.GetMailsFromString(destiny, separator));
        }

        public void AddSecretCopy(List<string> destiny)
        {
            MailAddressCollection collection = new MailAddressCollection();
            destiny.ForEach(p => collection.Add(new MailAddress(p)));

            this.AddSecretCopy(collection);
        }
        public void AddSecretCopy(MailAddressCollection destiny)
        {
            foreach (MailAddress item in destiny)
            {
                this.sendMessage.Bcc.Add(item);
            }
        }
        public void AddSecretCopy(Dictionary<string, string> destiny)
        {
            MailAddressCollection collection = new MailAddressCollection();
            foreach (KeyValuePair<string, string> item in destiny)
            {
                collection.Add(new MailAddress(item.Value, item.Key));
            }

            this.AddSecretCopy(collection);
        }
        public void AddSecretCopy(string destiny, params char[] separator)
        {
            this.AddSecretCopy(this.GetMailsFromString(destiny, separator));
        }

        public void AddToAndCopy(List<string> destiny)
        {
            MailAddressCollection collection = new MailAddressCollection();
            destiny.ForEach(p => collection.Add(new MailAddress(p.Trim())));

            this.AddToAndCopy(collection);
        }
        public void AddToAndCopy(MailAddressCollection destiny)
        {
            if (destiny.Count > 0)
            {
                this.sendMessage.To.Add(destiny[0]);

                for (int i = 1; i < destiny.Count; i++)
                {
                    this.sendMessage.CC.Add(destiny[i]);
                }
            }
        }
        public void AddToAndCopy(Dictionary<string, string> destiny)
        {
            MailAddressCollection collection = new MailAddressCollection();
            foreach (KeyValuePair<string, string> item in destiny)
            {
                collection.Add(new MailAddress(item.Value.Trim(), item.Key));
            }

            this.AddToAndCopy(collection);
        }
        public void AddToAndCopy(string destiny, params char[] separator)
        {
            this.AddToAndCopy(this.GetMailsFromString(destiny, separator));
        }

        public void SetMessage(string message)
        {
            this.SetMessage(message, false);
        }
        public void SetMessage(string message, bool isHtml)
        {
            this.SetMessage(message, isHtml, null);
        }
        public void SetMessage(string message, Dictionary<string, object> replacements)
        {
            this.SetMessage(message, false, replacements);
        }
        public void SetMessage(string message, bool isHtml, Dictionary<string, object> replacements)
        {
            if (replacements != null)
            {
                foreach (KeyValuePair<string, object> item in replacements)
                {
                    message = message.ReplaceEx(item.Key, item.Value.ToString());
                }
            }

            this.sendMessage.BodyEncoding = Encoding.UTF8;
            this.sendMessage.IsBodyHtml = isHtml;
            this.sendMessage.Body = message;
        }

        public void SetMessageTemplate(string fromTemplate, bool isHtml)
        {
            this.SetMessageTemplate(fromTemplate, false, new Dictionary<string, string>(0), null);
        }
        public void SetMessageTemplate(string fromTemplate, bool isHtml, Dictionary<string, string> resources, Dictionary<string, object> replacements)
        {
            Dictionary<string, Stream> lstResource = null;
            if (resources != null)
            {
                lstResource = new Dictionary<string, Stream>(resources.Count);
                FileInfo f = null;
                foreach (KeyValuePair<string, string> item in resources)
                {
                    f = new FileInfo(item.Value);
                    if (f.Exists)
                    {
                        lstResource.Add(item.Key, f.Open(FileMode.Open, FileAccess.Read));
                    }
                }
            }
            this.SetMessageTemplate(fromTemplate, isHtml, lstResource, replacements);
        }
        public void SetMessageTemplate(string fromTemplate, bool isHtml, Dictionary<string, Stream> resources, Dictionary<string, object> replacements)
        {
            FileInfo f = new FileInfo(fromTemplate);
            if (f.Exists)
            {
                string template = f.OpenText().ReadToEnd();

                if (replacements != null)
                {
                    foreach (KeyValuePair<string, object> item in replacements)
                    {
                        template = template.ReplaceEx(item.Key, item.Value.ToString());
                    }
                }

                this.sendMessage.BodyEncoding = Encoding.UTF8;
                this.sendMessage.IsBodyHtml = isHtml;
                this.sendMessage.Body = template;

                if (resources != null)
                {
                    AlternateView view = AlternateView.CreateAlternateViewFromString(this.sendMessage.Body, Encoding.UTF8, "text/html");
                    {
                        LinkedResource rex = null;
                        foreach (KeyValuePair<string, Stream> item in resources)
                        {
                            rex = new LinkedResource(item.Value);
                            {
                                rex.ContentId = item.Key;
                                view.LinkedResources.Add(rex);
                                this.sendMessage.AlternateViews.Add(view);
                            }
                        }
                    }
                }
            }
        }

        public void SetResources(string[] resources, Dictionary<string, object> replacements)
        {
            List<LinkedResource> rex = new List<LinkedResource>();
            for (int i = 0; i < resources.Length; i++)
            {
                rex.Add(new LinkedResource(resources[i]));
            }

            this.SetResources(rex.ToArray(), replacements);
        }
        public void SetResources(Stream[] resources, Dictionary<string, object> replacements)
        {
            List<LinkedResource> rex = new List<LinkedResource>();
            for (int i = 0; i < resources.Length; i++)
            {
                rex.Add(new LinkedResource(resources[i]));
            }

            this.SetResources(rex.ToArray(), replacements);
        }
        public void SetResources(LinkedResource[] resources, Dictionary<string, object> replacements)
        {
            foreach (KeyValuePair<string, object> item in replacements)
            {
                this.sendMessage.Body = this.sendMessage.Body.ReplaceEx(item.Key, item.Value.ToString());
            }

            AlternateView view = AlternateView.CreateAlternateViewFromString(this.sendMessage.Body, Encoding.UTF8, "text/html");
            {
                for (int i = 0; i < resources.Length; i++)
                {
                    this.sendMessage.AlternateViews.Clear();
                    view.LinkedResources.Clear();
                    view.LinkedResources.Add(resources[i]);
                    this.sendMessage.AlternateViews.Add(view);
                }
            }
        }

        public void ConfigureMessage(string subject)
        {
            this.ConfigureMessage(subject, MailPriority.Normal);
        }
        public void ConfigureMessage(string subject, MailPriority priority)
        {
            this.sendMessage.SubjectEncoding = Encoding.UTF8;
            this.sendMessage.Subject = subject;
            this.sendMessage.Priority = priority;
        }

        internal List<string> GetMailsFromString(string mails, params char[] separator)
        {
            return mails.Split(separator, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public void CleanMessage()
        {
            if (this.sendMessage != null)
            {
                this.sendMessage.AlternateViews.Clear();
                this.sendMessage.Attachments.Clear();
                this.sendMessage.Bcc.Clear();
                this.sendMessage.Body = string.Empty;
                this.sendMessage.CC.Clear();
                this.sendMessage.Headers.Clear();
                this.sendMessage.Dispose();
                this.sendMessage = new MailMessage();
            }
        }

        #region IDisposable Members

        private void Dispose(bool disposable)
        {
            if (disposable)
            {
                this.CleanMessage();
                if (this.sendMessage != null)
                {
                    this.sendMessage.Dispose();
                    this.sendMessage = null;
                }

                if (this.servicioHost != null)
                {
                    this.servicioHost.SendCompleted -= this.servicioHost_SendCompleted;
                    this.servicioHost = null;
                }

                if (this.AfterSendMail != null)
                {
                    this.AfterSendMail = null;
                }

                if (this.BeforeSendMail != null)
                {
                    this.BeforeSendMail = null;
                }

                if (this.fnAction != null)
                {
                    this.fnAction = null;
                }
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.Collect();
            GC.WaitForFullGCComplete();
        }

        ~ServicioCorreo()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }

    public class MailEventArgs : EventArgs
    {
        public MailEventArgs(MailMessage mensaje, SmtpClient cliente)
            : this(null, mensaje, cliente)
        {

        }
        public MailEventArgs(Exception excepcion, MailMessage mensaje, SmtpClient cliente)
        {
            this.Mensaje = mensaje;
            this.Excepcion = excepcion;
            this.Cliente = cliente;
        }

        public SmtpClient Cliente { get; private set; }
        public MailMessage Mensaje { get; private set; }
        public Exception Excepcion { get; private set; }
    }

    public class MailStringBuilder
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

            items.ForEach(p =>
                {
                    p = p.Trim();
                    aux = p.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);

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
                                int.TryParse(aux[1], out port);
                                this.Port = port;
                                break;
                            case "user":
                                this.User = aux[1];
                                break;
                            case "pass":
                                this.Pass = aux[1];
                                break;
                            case "ssl":
                                this.UseSsl = aux[1].Equals("Si", StringComparison.CurrentCultureIgnoreCase);
                                break;
                        }
                    }
                });
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (string.IsNullOrEmpty(this.Host))
            {
                sb.Append(string.Format("Host:{0}", this.Host));
            }

            if (this.Port > 0)
            {
                sb.Append(string.Format("Port:{0}", this.Port));
            }

            if (string.IsNullOrEmpty(this.User))
            {
                sb.Append(string.Format("User:{0}", this.User));
            }

            if (string.IsNullOrEmpty(this.Pass))
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
    }

    public class MailServerConfiguration
    {
        public MailServerConfiguration()
            : this(ConfigurationManager.AppSettings["CfgServerMail"])
        {
        }

        public MailServerConfiguration(string data)
        {
            this.Process(data);
        }

        private void Process(string data)
        {
            if (!string.IsNullOrWhiteSpace(data))
            {
                List<string> items = data.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                string[] aux = null;

                items.ForEach(p =>
                    {
                        p = p.Trim();
                        aux = p.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);

                        if (aux.Length == 2)
                        {
                            aux[0] = aux[0].Trim();

                            switch (aux[0].ToLower())
                            {
                                //Message-Id
                                case "message-id":
                                    this.Message_Id = aux[1].Equals("true", StringComparison.OrdinalIgnoreCase);
                                    break;
                                //Precedence
                                case "precedence":
                                    this.Precedence = aux[1].Equals("true", StringComparison.OrdinalIgnoreCase);
                                    break;
                            }
                        }
                    });
            }
        }

        public bool Message_Id { get; set; }

        public bool Precedence { get; set; }

        public override string ToString()
        {
            return string.Format("Message-Id={0};Precedence={1}", this.Message_Id, this.Precedence);
        }
    }
}

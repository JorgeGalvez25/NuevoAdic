using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Diagnostics;
using System.Web.Services;

namespace EstandarCliente.CargadorVistas.Services
{
    public class ServicioRed
    {
        public static bool ServiceExists(string url, bool throwExceptions, out string errorMessage)
        {
            try
            {
                errorMessage = string.Empty;
                // try accessing the web service directly via it's URL        

                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.Timeout = 30000;

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.OK:
                            break;
                        case HttpStatusCode.RequestTimeout:
                            throw new Exception("Error se excedio el tiempo de espera.");
                        default:
                            throw new Exception("Error al localizar el servicio.");
                    }
                }
                // try getting the WSDL?        
                // asmx lets you put "?wsdl" to make sure the URL is a web service        
                // could parse and validate WSDL here    
                WebService ws = new WebService();
            }
            catch (WebException ex)
            {
                // decompose 400- codes here if you like        
                errorMessage = string.Format("Error testing connection to web service at \"{0}\":\r\n{1}", url, ex);
                Trace.TraceError(errorMessage);
                if (throwExceptions)
                    throw new Exception(errorMessage, ex);
            }
            catch (Exception ex)
            {
                errorMessage = string.Format("Error testing connection to web service at \"{0}\":\r\n{1}", url, ex);
                Trace.TraceError(errorMessage);
                if (throwExceptions)
                    throw new Exception(errorMessage, ex);
                return false;
            }
            return true;
        }
    }
}

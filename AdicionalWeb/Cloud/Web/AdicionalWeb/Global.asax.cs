using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace AdicionalWeb
{
    public static class StreamReaderExtension
    {
        /// <summary>
        /// Minify the HTML code
        /// </summary>
        /// <param name="reader">The StreamReader.</param>
        /// <param name="features">Any features to enable / disable.</param>
        /// <returns>The minified HTML code.</returns>
        public static string MinifyHtmlCode(this StreamReader reader, Features features)
        {
            return MinifyHtmlCode(reader.ReadToEnd(), features);
        }

        /// <summary>
        /// Minifies the HTML code
        /// </summary>
        /// <param name="htmlCode">The HTML as a string</param>
        /// <param name="features">Any features to enable / disable.</param>
        /// <returns>The minified HTML code.</returns>
        public static string MinifyHtmlCode(string htmlCode, Features features)
        {
            string contents;

            // Minify the contents
            contents = MinifyHtml(htmlCode, features);

            // Ensure that the max length is less than 65K characters
            contents = EnsureMaxLength(contents, features);

            // Re-add the @model declaration
            contents = ReArrangeDeclarations(contents);

            return string.Format("{0} ", contents);
        }

        /// <summary>
        /// Find any occurences of the particular Razor keywords
        /// and add a new line or move to the top of the view.
        /// </summary>
        /// <param name="fileContents">The contents of the file</param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ReArrangeDeclarations(string fileContents)
        {
            // A list of all the declarations
            Dictionary<string, bool> declarations = new Dictionary<string, bool>();
            declarations.Add("@model ", true);
            declarations.Add("@using ", false);
            declarations.Add("@inherits ", false);

            // Loop through the declarations
            foreach (var declaration in declarations)
            {
                fileContents = ReArrangeDeclaration(fileContents, declaration.Key, declaration.Value);
            }

            return fileContents;
        }

        /// <summary>
        /// Re-arranges the razor syntax on its own line.
        /// It seems to break the razor engine if this isnt on
        /// it's own line in certain cases.
        /// </summary>
        /// <param name="fileContents">The file contents.</param>
        /// <param name="declaration">The declaration keywords that will cause a new line split.</param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string ReArrangeDeclaration(string fileContents, string declaration, bool bringToTop)
        {
            // Find possible multiple occurences in the file contents
            MatchCollection matches = Regex.Matches(fileContents, declaration);

            // Loop through the matches
            int alreadyMatched = 0;
            foreach (Match match in matches)
            {
                int position = declaration.Length;
                int declarationPosition = match.Index;

                // If we have more than one match, we need to keep the counter moving everytime we add a new line
                if (matches.Count > 1 && alreadyMatched > 0)
                {
                    // Cos we added one or more new line break \n\r
                    declarationPosition += (2 * alreadyMatched);
                }

                while (declarationPosition >= 0)
                {
                    // Move one forward
                    position += 1;
                    if (position > fileContents.Length) break;
                    string substring = fileContents.Substring(declarationPosition, position);

                    // Check if it contains a whitespace at the end
                    if (substring.EndsWith(" ") || substring.EndsWith(">"))
                    {
                        if (bringToTop)
                        {
                            // First replace the occurence
                            fileContents = fileContents.Replace(substring, string.Empty);

                            // Next move it to the top on its own line
                            fileContents = substring + Environment.NewLine + fileContents;
                            break;
                        }
                        else
                        {
                            // Add a line break afterwards
                            fileContents = fileContents.Replace(substring, substring + Environment.NewLine);
                            alreadyMatched++;
                            break;
                        }
                    }
                }
            }

            return fileContents;
        }

        /// <summary>
        /// Minifies the given HTML string.
        /// </summary>
        /// <param name="htmlContents"> The html to minify.</param>
        /// <param name="features">The features</param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string MinifyHtml(string htmlContents, Features features)
        {
            // First, remove all JavaScript comments
            if (!features.IgnoreJsComments)
            {
                htmlContents = RemoveJavaScriptComments(htmlContents);
            }

            // Remove special keys
            htmlContents = htmlContents.Replace("/*", "{{{SLASH_STAR}}}");

            // Minify the string
            htmlContents = Regex.Replace(htmlContents, @"/\*([^*]|[\r\n]|(\*+([^*/]|[\r\n])))*\*+/", string.Empty);

            // Replace line comments
            htmlContents = Regex.Replace(htmlContents, @"// (.*?)\r?\n", string.Empty, RegexOptions.Singleline);

            // Replace spaces between quotes
            htmlContents = Regex.Replace(htmlContents, @"\s+", " ");

            // Replace line breaks
            htmlContents = Regex.Replace(htmlContents, @"\s*\n\s*", "\n");

            // Replace spaces between brackets
            htmlContents = Regex.Replace(htmlContents, @"\s*\>\s*\<\s*", "><");
            // Replace comments
            if (!features.IgnoreHtmlComments)
            {
                if (features.IgnoreKnockoutComments)
                {
                    htmlContents = Regex.Replace(htmlContents, @"<!--(?!(\[|\s*#include))(?!ko .*)(?!\/ko)(.*?)-->", string.Empty);
                }
                else
                {
                    htmlContents = Regex.Replace(htmlContents, @"<!--(?!(\[|\s*#include))(.*?)-->", string.Empty);
                }
            }

            // single-line doctype must be preserved
            var firstEndBracketPosition = htmlContents.IndexOf(">", StringComparison.Ordinal);
            if (firstEndBracketPosition >= 0)
            {
                htmlContents = htmlContents.Remove(firstEndBracketPosition, 1);
                htmlContents = htmlContents.Insert(firstEndBracketPosition, ">");
            }

            // Put back special keys
            htmlContents = htmlContents.Replace("{{{SLASH_STAR}}}", "/*");
            return htmlContents.Trim();
        }

        /// <summary>
        /// Removes any JavaScript Comments in a script block
        /// </summary>
        /// <param name="javaScriptComments"></param>
        /// <returns>A string with all JS comments removed</returns>
        public static string RemoveJavaScriptComments(string javaScriptComments)
        {
            // Remove JavaScript comments
            Regex extractScripts = new Regex(@"<script[^>]*>[\s\S]*?</script>");

            // Loop through the script blocks
            foreach (Match match in extractScripts.Matches(javaScriptComments))
            {
                var scriptBlock = match.Value;

                javaScriptComments = javaScriptComments.Replace(scriptBlock, Regex.Replace(scriptBlock, @"[^:|""|']//(.*?)\r?\n", string.Empty));

            }

            return javaScriptComments;
        }

        /// <summary>
        /// Ensure that the max character count is less than 65K.
        /// If so, break onto the next line.
        /// </summary>
        /// <param name="htmlContents">The minified HTML</param>
        /// <param name="features">The features</param>
        /// <returns>A html string</returns>
        public static string EnsureMaxLength(string htmlContents, Features features)
        {
            if (features.MaxLength > 0)
            {
                int htmlLength = htmlContents.Length;
                int currentMaxLength = features.MaxLength;
                int position;

                while (htmlLength > currentMaxLength)
                {
                    position = htmlContents.LastIndexOf("><", currentMaxLength);
                    htmlContents = htmlContents.Substring(0, position + 1) + "\r\n" + htmlContents.Substring(position + 1);
                    currentMaxLength += features.MaxLength;
                }
            }
            return htmlContents;
        }
    }

    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            HttpApplication app = sender as HttpApplication;
            if (app.Context.CurrentHandler == null)
                return;

            bool isJs = app.Request.RawUrl.Contains(".js");
            bool isCss = app.Request.RawUrl.Contains(".css");
            bool isJson = (app.Request.Headers["Content-type"] ?? string.Empty).Contains("application/json");

            if (!isJs && !isCss && !isJson)
            {
                if (!(app.Context.CurrentHandler is System.Web.UI.Page ||
                    app.Context.CurrentHandler.GetType().Name == "SyncSessionlessHandler") ||
                    app.Request["HTTP_X_MICROSOFTAJAX"] != null)
                    return;
            }

            string acceptEncoding = app.Request.Headers["Accept-Encoding"];

            if (acceptEncoding == null || acceptEncoding.Length == 0)
                return;

            acceptEncoding = acceptEncoding.ToLower();
            BufferedStream prevUncompressedStream = new BufferedStream(app.Response.Filter, short.MaxValue);

            if (acceptEncoding.Contains("gzip"))
            {
                // gzip
                app.Response.AppendHeader("Content-Encoding", "gzip");
                app.Response.Filter = new GZipStream(prevUncompressedStream, CompressionMode.Compress);
            }
            else if (acceptEncoding.Contains("deflate") || acceptEncoding == "*")
            {
                // deflate
                app.Response.AppendHeader("Content-Encoding", "deflate");
                app.Response.Filter = new DeflateStream(prevUncompressedStream, CompressionMode.Compress);
            }
        }

        //protected void Application_PreRequestHandlerExecute(object src, EventArgs e)
        //{
        //    //if (AdicionalWeb.Persistencia.EstacionesAdicionalPersistencia.ListaEstaciones.Count <= 0)
        //    //{
        //    //    DateTime fecha = DateTime.Now.AddSeconds(30);

        //    //    if (this.Context.Cache.Get("MensajeTtl") == null)
        //    //    {
        //    //        this.Context.Cache.Add("MensajeTtl", "Aplicación", null, DateTime.MaxValue, fecha.TimeOfDay, System.Web.Caching.CacheItemPriority.Normal, null);
        //    //    }

        //    //    if (this.Context.Cache.Get("Mensaje") == null)
        //    //    {
        //    //        this.Context.Cache.Add("Mensaje", "Aplicación expiró.", null, DateTime.MaxValue, fecha.TimeOfDay, System.Web.Caching.CacheItemPriority.Normal, null);
        //    //    }

        //    //    if (this.Context.Handler is System.Web.UI.Page)
        //    //    {
        //    //        this.Context.Server.Transfer("~/pages/mensajes/mensaje.aspx", false);
        //    //    }
        //    //}
        //}

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs
            Exception ex = Server.GetLastError();
            Application["TheException"] = ex; //store the error for later
            Server.ClearError(); //clear the error so we can continue onwards
            //Response.Redirect("~/pages/Error.aspx"); //direct user to error page
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }

        internal class WhitespaceStream : MemoryStream
        {
            private readonly BufferedStream FilterStream = null;

            public WhitespaceStream(Stream stream)
            {
                this.FilterStream = new BufferedStream(stream);
            }

            public override void Write(byte[] buffer, int Offset, int Count)
            {
                byte[] data = new byte[Count];
                Buffer.BlockCopy(buffer, Offset, data, 0, Count);

                string source = StreamReaderExtension.MinifyHtml(Encoding.Default.GetString(data), new Features(new List<string>().ToArray())) + Environment.NewLine;

                //StreamReader reader = new StreamReader(new MemoryStream(data));
                //string source = reader.MinifyHtmlCode(new Features(new List<string>().ToArray())) + Environment.NewLine;

                byte[] output = Encoding.Default.GetBytes(source);
                this.FilterStream.Write(output, Offset, output.GetLength(0));
                this.FilterStream.Flush();
            }
        }
    }

    public class Features
    {
        /// <summary>
        /// Check the arguments passed in to determine if we should enable or disable any features.
        /// </summary>
        /// <param name="args">The arguments passed in.</param>
        public Features(string[] args)
        {
            if (args.Contains("ignorehtmlcomments"))
            {
                IgnoreHtmlComments = true;
            }

            if (args.Contains("ignorejscomments"))
            {
                IgnoreJsComments = true;
            }

            if (args.Contains("ignoreknockoutcomments"))
            {
                IgnoreKnockoutComments = true;
            }

            int maxLength = 0;

            // This is a check to see if the args contain an optional parameter for the max line length
            if (args != null && args.Length > 1)
            {
                // Try and parse the value sent through
                int.TryParse(args[1], out maxLength);
            }

            MaxLength = maxLength;
        }

        /// <summary>
        /// Should we ignore the JavaScript comments and not minify?
        /// </summary>
        public bool IgnoreJsComments { get; set; }

        /// <summary>
        /// Should we ignore the html comments and not minify?
        /// </summary>
        public bool IgnoreHtmlComments { get; set; }

        /// <summary>
        /// Should we ignore knockout comments?
        /// </summary>
        public bool IgnoreKnockoutComments { get; set; }

        /// <summary>
        /// Property for the max character count
        /// </summary>
        public int MaxLength { get; private set; }
    }
}
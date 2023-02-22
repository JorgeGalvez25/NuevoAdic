using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using iTextSharp.tool.xml.css;
using iTextSharp.tool.xml.html;
using iTextSharp.tool.xml.parser;
using iTextSharp.tool.xml.pipeline.css;
using iTextSharp.tool.xml.pipeline.end;
using iTextSharp.tool.xml.pipeline.html;
using AdicionalWeb.Persistencia;
using System.Threading;

namespace AdicionalWeb.Code
{
    public class WebHtmlToPdf
    {
        //private static readonly object _lock = new object();

        //private static string _pdfHtmlToPdfExePath;
        //private static string PdfHtmlToPdfExePath
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(_pdfHtmlToPdfExePath))
        //        {
        //            _pdfHtmlToPdfExePath = LogExceptionManager.GetCombinedPath(AppDomain.CurrentDomain.BaseDirectory, @"\bin\wkhtmltox\wkhtmltopdf.exe"); // HttpContext.Current.Server.MapPath(@"~\bin\wkhtmltox\wkhtmltopdf.exe");
        //        }
        //        return _pdfHtmlToPdfExePath;
        //    }
        //}

        private static string _templatePath;
        private static string TemplatePath
        {
            get
            {
                if (string.IsNullOrEmpty(_templatePath))
                {
                    _templatePath = AdicionalUtils.CombinePaths(AppDomain.CurrentDomain.BaseDirectory, @"\template\reportes");
                }
                return _templatePath;
            }
        }

        private string GetFormatoFecha(TemplateConfiguration config)
        {
            switch (config.Id)
            {
                case TemplateConstants.Reportes.VENTAS_COMBUSTIBLE: return string.Format("{0:dddd}, {0:dd} de {0:MMMM} de {0:yyyy}", config.Date);
            }

            return config.Date.ToString("dd/MMM/yyy");
        }

        private string CreateFromTemplate(TemplateConfiguration config)
        {
            string templateBase = AdicionalUtils.CombinePaths(TemplatePath, TemplateConstants.TEMPLATE_BASE_FILE);

            //LogExceptionManager.LogMessage("Plantilla", templateBase);
            //LogExceptionManager.LogMessage("Pagina Plantilla", config.TemplatePage);

            string iTBase = File.ReadAllText(templateBase);
            string iTPage = File.ReadAllText(config.TemplatePage);

            string formatoFecha = GetFormatoFecha(config);
            iTBase = iTBase.Replace(TemplateConstants.CONTENT, iTPage);

            string template = iTBase.Replace(TemplateConstants.COMERCIAL_NAME, HttpUtility.HtmlEncode(config.ComercialName))
                                    .Replace(TemplateConstants.DATE, formatoFecha)
                                    .Replace(TemplateConstants.REPORT_NAME, HttpUtility.HtmlEncode(config.ReportName))
                                    .Replace(TemplateConstants.IMAGE, config.Image)
                                    .Replace(TemplateConstants.TABLE_DATA, config.TableData)
                                    .Replace(TemplateConstants.TABLE_FOOT, config.TableFooter)
                                    .Replace(TemplateConstants.TABLE_INFO, config.TableInfo);


            return template;
            //File.WriteAllText(config.HTMLFileName, template);
        }

        public string HtmlToPdf(TemplateConfiguration config)
        {
            ManualResetEvent await = new ManualResetEvent(false);
            string result = string.Empty;
            Exception exception = null;

            ThreadPool.QueueUserWorkItem(_ =>
                {
                    string outputFolder = AdicionalUtils.CombinePaths(AppDomain.CurrentDomain.BaseDirectory, @"\files\temp\");
                    DirectoryInfo dInfo = new DirectoryInfo(outputFolder);

                    string fileName = config.ReportName
                                            .Replace("Á", "A")
                                            .Replace("á", "a")
                                            .Replace("É", "E")
                                            .Replace("é", "e")
                                            .Replace("Í", "I")
                                            .Replace("í", "i")
                                            .Replace("Ó", "O")
                                            .Replace("ó", "o")
                                            .Replace("Ú", "U")
                                            .Replace("ú", "u")
                                            .Replace("ñ", "n")
                                            .Replace("Ñ", "N")
                                            .Replace(" ", "_") + "_" + Guid.NewGuid().ToString();
                    string htmlOut = fileName + ".html";
                    string pdfOut = fileName + ".PDF";

                    try
                    {
                        if (!dInfo.Exists)
                        {
                            try
                            {
                                dInfo.Create();
                                dInfo.Refresh();
                            }
                            catch (Exception exc)
                            {
                                //LogExceptionManager.LogException("Crear carpeta", exc);
                                exception = new Exception("Se produjo un error al generar el PDF", exc);
                                await.Set();
                                return;
                            };
                        }

                        config.HTMLFileName = AdicionalUtils.CombinePaths(outputFolder, htmlOut);
                        config.PDFFileName = AdicionalUtils.CombinePaths(outputFolder, pdfOut);

                        string allHtml = CreateFromTemplate(config);

                        using (FileStream ms = new FileStream(config.PDFFileName, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                        {
                            using (BufferedStream bsIn = new BufferedStream(ms))
                            {
                                //string allHtml = File.ReadAllText(config.HTMLFileName, System.Text.Encoding.UTF8);

                                SimpleParser simpleParser = new SimpleParser(config.HTMLFileName);
                                simpleParser.Parse(bsIn, allHtml, config);
                            }
                        }
                        result = config.PDFFileName;
                        await.Set();
                    }
                    catch (Exception exc)
                    {
                        //LogExceptionManager.LogException("Process", exc);
                        exception = new Exception("Se produjo un error al generar el PDF", exc);
                        await.Set();
                    }
                    finally
                    {
                        try
                        {
                            FileInfo info = new FileInfo(config.HTMLFileName);
                            if (info.Exists)
                            {
                                info.Delete();
                            }
                        }
                        catch { }
                        try
                        {
                            DateTime lasDate = DateTime.Now.AddDays(5).Date;
                            foreach (FileInfo f in dInfo.GetFiles().Where(p => p.LastAccessTime.Date >= lasDate))
                            {
                                try { f.Delete(); }
                                catch { }
                            }
                        }
                        catch { }
                    }
                });

            await.WaitOne();

            if (exception != null) throw exception;
            return result;
        }

        #region HtmlToPDF
        // resolve URIs for LinkProvider & ImageProvider
        public class UriHelper
        {
            /* IsLocal; when running in web context:
             * [1] give LinkProvider http[s] scheme; see CreateBase(string baseUri)
             * [2] give ImageProvider relative path starting with '/' - see:
             *     Join(string relativeUri)
             */
            public bool IsLocal { get; set; }
            public HttpContext HttpContext { get; private set; }
            public Uri BaseUri { get; private set; }

            public UriHelper(string baseUri) : this(baseUri, true) { }
            public UriHelper(string baseUri, bool isLocal)
            {
                IsLocal = isLocal;
                HttpContext = HttpContext.Current;
                BaseUri = CreateBase(baseUri);
            }

            /* get URI for IImageProvider to instantiate iTextSharp.text.Image for 
             * each <img> element in the HTML.
             */
            public string Combine(string relativeUri)
            {
                /* when running in a web context, the HTML is coming from a MVC view 
                 * or web form, so convert the incoming URI to a **local** path
                 */
                if (HttpContext != null && !BaseUri.IsAbsoluteUri && IsLocal)
                {
                    // Combine() checks directory traversal exploits
                    return HttpContext.Server.MapPath(VirtualPathUtility.Combine(BaseUri.ToString(), relativeUri));
                }
                return BaseUri.Scheme == Uri.UriSchemeFile
                    ? Path.Combine(BaseUri.LocalPath, relativeUri)
                    // for this example we're assuming URI.Scheme is http[s]
                    : new Uri(BaseUri, relativeUri).AbsoluteUri;
            }

            private Uri CreateBase(string baseUri)
            {
                if (HttpContext != null)
                {   // running on a web server; need to update original value  
                    HttpRequest req = HttpContext.Request;
                    baseUri = IsLocal
                        // IImageProvider; absolute virtual path (starts with '/')
                        // used to convert to local file system path. see:
                        // Combine(string relativeUri)
                        ? req.ApplicationPath
                        // ILinkProvider; absolute http[s] URI scheme
                        : req.Url.GetLeftPart(UriPartial.Authority) + HttpContext.Request.ApplicationPath;
                }

                Uri uri;
                if (Uri.TryCreate(baseUri, UriKind.RelativeOrAbsolute, out uri)) return uri;

                throw new InvalidOperationException("cannot create a valid BaseUri");
            }
        }

        // make hyperlinks with relative URLs absolute
        public class LinkProvider : ILinkProvider
        {
            // rfc1738 - file URI scheme section 3.10
            public const char SEPARATOR = '/';
            public string BaseUrl { get; private set; }

            public LinkProvider(UriHelper uriHelper)
            {
                Uri uri = uriHelper.BaseUri;
                /* simplified implementation that only takes into account:
                 * Uri.UriSchemeFile || Uri.UriSchemeHttp || Uri.UriSchemeHttps
                 */
                BaseUrl = uri.Scheme == Uri.UriSchemeFile
                    // need trailing separator or file paths break
                                ? uri.AbsoluteUri.TrimEnd(SEPARATOR) + SEPARATOR
                    // assumes Uri.UriSchemeHttp || Uri.UriSchemeHttps
                                : BaseUrl = uri.AbsoluteUri;
            }

            public string GetLinkRoot()
            {
                return BaseUrl;
            }
        }

        // handle <img> elements in HTML  
        public class ImageProvider : iTextSharp.tool.xml.pipeline.html.IImageProvider
        {
            private UriHelper _uriHelper;
            // see Store(string src, Image img)
            private Dictionary<string, iTextSharp.text.Image> _imageCache = new Dictionary<string, iTextSharp.text.Image>();

            public virtual float ScalePercent { get; set; }
            public virtual Regex Base64 { get; set; }

            public ImageProvider(UriHelper uriHelper) : this(uriHelper, 67f) { }
            //              hard-coded based on general past experience ^^^
            // but call the overload to supply your own
            public ImageProvider(UriHelper uriHelper, float scalePercent)
            {
                _uriHelper = uriHelper;
                ScalePercent = scalePercent;
                // rfc2045, section 6.8 (alphabet/padding)
                Base64 = new Regex(@"^data:image/[^;]+;base64,(?<data>[a-z0-9+/]+={0,2})$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            }

            public virtual iTextSharp.text.Image ScaleImage(iTextSharp.text.Image img)
            {
                img.ScalePercent(ScalePercent);
                return img;
            }

            public virtual iTextSharp.text.Image Retrieve(string src)
            {
                if (_imageCache.ContainsKey(src)) return _imageCache[src];

                try
                {
                    if (Regex.IsMatch(src, "^https?://", RegexOptions.IgnoreCase))
                    {
                        return ScaleImage(iTextSharp.text.Image.GetInstance(src));
                    }

                    Match match;
                    if ((match = Base64.Match(src)).Length > 0)
                    {
                        return ScaleImage(iTextSharp.text.Image.GetInstance(Convert.FromBase64String(match.Groups["data"].Value)));
                    }

                    string imgPath = _uriHelper.Combine(src);
                    return ScaleImage(iTextSharp.text.Image.GetInstance(imgPath));
                }
                // not implemented to keep the SO answer (relatively) short
                catch (BadElementException) { return null; }
                catch (IOException) { return null; }
                catch (Exception) { return null; }
            }

            /*
             * always called after Retrieve(string src):
             * [1] cache any duplicate <img> in the HTML source so the image bytes
             *     are only written to the PDF **once**, which reduces the 
             *     resulting file size.
             * [2] the cache can also **potentially** save network IO if you're
             *     running the parser in a loop, since Image.GetInstance() creates
             *     a WebRequest when an image resides on a remote server. couldn't
             *     find a CachePolicy in the source code
             */
            public virtual void Store(string src, iTextSharp.text.Image img)
            {
                if (!_imageCache.ContainsKey(src)) _imageCache.Add(src, img);
            }

            /* XMLWorker documentation for ImageProvider recommends implementing
             * GetImageRootPath():
             * 
             * http://demo.itextsupport.com/xmlworker/itextdoc/flatsite.html#itextdoc-menu-10
             * 
             * but a quick run through the debugger never hits the breakpoint, so 
             * not sure if I'm missing something, or something has changed internally 
             * with XMLWorker....
             */
            public virtual string GetImageRootPath() { return null; }
            public virtual void Reset() { }
        }

        /* a simple parser that uses XMLWorker and XMLParser to handle converting 
 * (most) images and hyperlinks internally
 */
        public class SimpleParser
        {
            public virtual ILinkProvider LinkProvider { get; set; }
            public virtual iTextSharp.tool.xml.pipeline.html.IImageProvider ImageProvider { get; set; }

            public virtual HtmlPipelineContext HtmlPipelineContext { get; set; }
            public virtual ITagProcessorFactory TagProcessorFactory { get; set; }
            public virtual ICSSResolver CssResolver { get; set; }

            /* overloads simplfied to keep SO answer (relatively) short. if needed
             * set LinkProvider/ImageProvider after instantiating SimpleParser()
             * to override the defaults (e.g. ImageProvider.ScalePercent)
             */
            public SimpleParser() : this(null) { }
            public SimpleParser(string baseUri)
            {
                LinkProvider = new LinkProvider(new UriHelper(baseUri, false));
                ImageProvider = new ImageProvider(new UriHelper(baseUri, true));

                HtmlPipelineContext = new HtmlPipelineContext(null);

                // another story altogether, and not implemented for simplicity 
                TagProcessorFactory = Tags.GetHtmlTagProcessorFactory();
                CssResolver = XMLWorkerHelper.GetInstance().GetDefaultCssResolver(true);
            }

            /*
             * when sending XHR via any of the popular JavaScript frameworks,
             * <img> tags are **NOT** always closed, which results in the 
             * infamous iTextSharp.tool.xml.exceptions.RuntimeWorkerException:
             * 'Invalid nested tag a found, expected closing tag img.' a simple
             * workaround.
             */
            private string SimpleAjaxImgFix(string xHtml)
            {
                return Regex.Replace(xHtml, "(?<image><img[^>]+)(?<=[^/])>", new MatchEvaluator(match => match.Groups["image"].Value + " />"), RegexOptions.IgnoreCase | RegexOptions.Multiline);
            }

            public virtual void Parse(Stream outStream, string xHtml, TemplateConfiguration config)
            {
                xHtml = SimpleAjaxImgFix(xHtml);

                Document document = new Document((config.Horizontal ? iTextSharp.text.PageSize.A4.Rotate()
                                                                    : iTextSharp.text.PageSize.A4), 10f, 10f, 10f, 10f);

                // CSS
                string css = GetCss();
                document.HtmlStyleClass = css;

                //if (string.IsNullOrWhiteSpace(config.Image))
                //{
                //    document.Open();

                //    PdfPTable table = new PdfPTable(1);
                //    table.WidthPercentage = 100;
                //    PdfPCell cell = new PdfPCell() { Border = 0 };

                //    foreach (IElement e in XMLWorkerHelper.ParseToElementList(xHtml, css))
                //    {
                //        cell.AddElement(e);
                //    }

                //    table.AddCell(cell);
                //    document.Add(table);

                //    outStream.FlushAsync().Wait();
                //    document.Close();
                //}
                //else
                //{
                PdfWriter writer = PdfWriter.GetInstance(document, outStream);
                writer.InitialLeading = 12;
                document.Open();

                //CssResolver.AddCss(css.ToString(), "utf-8", true);
                //DirectoryInfo dInfo = new DirectoryInfo(AdicionalUtils.CombinePaths(AppDomain.CurrentDomain.BaseDirectory, @"\css"));
                //foreach (var item in dInfo.GetFiles("*.css", SearchOption.AllDirectories))
                //{
                //    CssResolver.AddCssFile(item.FullName, true);
                //}
                CssResolver.AddCss(".tblTitle{background:#808080 !important;color:#fff;}", "utf-8", true);
                CssResolver.AddCss(".tblTitle>th{background:#808080 !important;color:#fff;}", "utf-8", true);

                // HTML
                HtmlPipelineContext.SetAcceptUnknown(true)
                                   .SetTagFactory(TagProcessorFactory)
                                   .SetLinkProvider(LinkProvider)
                                   .SetImageProvider(ImageProvider)
                                   .AutoBookmark(true)
                                   .CharSet(Encoding.UTF8);

                // Pipelines
                PdfWriterPipeline pdfWriterPipeline = new PdfWriterPipeline(document, writer);
                HtmlPipeline htmlPipeline = new HtmlPipeline(HtmlPipelineContext, pdfWriterPipeline);
                CssResolverPipeline cssResolverPipeline = new CssResolverPipeline(CssResolver, htmlPipeline);

                // XML Worker
                XMLWorker worker = new XMLWorker(cssResolverPipeline, true);
                XMLParser parser = new XMLParser(true, worker, Encoding.UTF8);

                using (MemoryStream htmlStream = new MemoryStream(Encoding.UTF8.GetBytes(xHtml)))
                {
                    using (BufferedStream buff = new BufferedStream(htmlStream))
                    {
                        parser.Parse(buff, Encoding.UTF8);
                        writer.CloseStream = false;
                        writer.Flush();
                        outStream.Flush();

                        document.Close();
                    }
                }
                //}
            }

            private string GetCss()
            {
                string[] lstCss = new string[3]
                    {
                        AdicionalUtils.CombinePaths(AppDomain.CurrentDomain.BaseDirectory, @"\css\normalize.css"),
                        AdicionalUtils.CombinePaths(AppDomain.CurrentDomain.BaseDirectory, @"\css\bootstrap.min.css"),
                        AdicionalUtils.CombinePaths(AppDomain.CurrentDomain.BaseDirectory, @"\css\adicional.css")
                    };

                StringBuilder css = new StringBuilder();
                {
                    ICssFile cssFile = null;
                    string cssData = string.Empty;
                    foreach (var item in lstCss)
                    {
                        cssData = File.ReadAllText(item);
                        css.Append(cssData);
                        cssFile = XMLWorkerHelper.GetCSS(new MemoryStream(Encoding.UTF8.GetBytes(File.ReadAllText(item))));
                        CssResolver.AddCss(cssFile);
                    }
                }

                return css.ToString();
            }
        }

        #endregion
    }

    public class TemplateConstants
    {
        public const string COMERCIAL_NAME = "{comercial_name}";

        public const string REPORT_NAME = "{report_name}";

        public const string DATE = "{date}";

        public const string CONTENT = "{content}";

        public const string IMAGE = "{image}";

        public const string TABLE_INFO = "{tblInfo}";

        public const string TABLE_FOOT = "{tblFoot}";

        public const string TABLE_DATA = "{tblData}";

        public const string TEMPLATE_BASE_FILE = "report-base-template.html";

        public const string TEMPLATE_VENTAS_COMBUSTIBLE = "tmplVentasCombustible.html";

        public class Reportes
        {
            public const string VENTAS_COMBUSTIBLE = "rptVentasComb";
        }
    }

    public class TemplateConfiguration
    {
        public TemplateConfiguration()
        {
            this.ComercialName = string.Empty;
            this.Content = string.Empty;
            this.HTMLFileName = string.Empty;
            this.ReportName = string.Empty;
            this.Page = string.Empty;
            this.TableInfo = string.Empty;
            this.TableData = string.Empty;
            this.Image = string.Empty;
            this.NoEstacion = string.Empty;
            this.Horizontal = false;
            this.TemplatePage = string.Empty;
            this.PDFFileName = string.Empty;
        }

        public string Page { get; set; }

        public bool Horizontal { get; set; }

        public string Id { get; set; }

        public string ComercialName { get; set; }

        public string ReportName { get; set; }

        public DateTime Date { get; set; }

        public string Content { get; set; }

        public string NoEstacion { get; set; }

        public string TableInfo { get; set; }

        public string TableFooter { get; set; }

        public string TableData { get; set; }

        public string Image { get; set; }

        public string TemplatePage { get; set; }

        public string HTMLFileName { get; set; }

        public string PDFFileName { get; set; }
    }
}

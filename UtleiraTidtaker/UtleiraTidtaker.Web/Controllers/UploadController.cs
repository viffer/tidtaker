using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.AccessControl;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.UI;
using UtleiraTidtaker.DataReader.Repository;
using UtleiraTidtaker.Lib.Model;
using UtleiraTidtaker.Lib.Repository;
using UtleiraTidtaker.Web.Models;

namespace UtleiraTidtaker.Web.Controllers
{
    [System.Web.Http.RoutePrefix("api/v1/upload")]
    public class UploadController : ApiController
    {
        private const string ProjectUploadFolder = @"c:\temp\UtleiraTidtaker.Web";
        private static readonly string ServerUploadFolder = Path.GetTempPath();
        private Config _config = new Config
                                 {
                                     StartNumbers =
                                         new Dictionary<int, int>
                                         {
                                             {10000, 200},
                                             {5000, 1},
                                             {2000, 400},
                                             {600, 450},
                                             {4999, 500}
                                         },
                                     StartTimeOffset =
                                         new Dictionary<int, int>
                                         {
                                             {10000, 75},
                                             {5000, 90},
                                             {2000, 15},
                                             {600, 0},
                                             {4999, 90}
                                         }
                                 };

        [System.Web.Http.Route("file")]
        [System.Web.Http.HttpPost]
        [ValidateMimeMultipartContentFilter]
        public JsonResult UploadFile()
        {
            if (!Directory.Exists(ProjectUploadFolder)) Directory.CreateDirectory(ProjectUploadFolder);

            //var streamProvider = new bytera(ServerUploadFolder);
            var data = Request.Content.ReadAsFormDataAsync().Result;
            //var messages = UploadFiles(streamProvider, false);
            var filename = Request.GetQueryNameValuePairs().First().Value;
            filename = string.Format(@"{0}\{1}_{2}", ProjectUploadFolder, DateTime.Now.Ticks, filename);
            var html = data.Keys[0];
            html = html.Substring(html.IndexOf("base64,") + "base64,".Length).Trim().Replace(' ', '+');
            try
            {
                if (html.Length % 4 > 0) html = html.PadRight(html.Length + 4 - html.Length % 4, '=');
                var image64 = Convert.FromBase64String(html);
                File.WriteAllBytes(filename, image64);
            }
            catch (Exception exception)
            {
                html = exception.ToString();
                //throw;
            }

            var racejson = "";
            var athletejson = "";
            try
            {
                using (var excelRepository = new ExcelRepository(filename))
                {
                    foreach (var sheetName in excelRepository.GetSheetNames())
                    {
                        var exceldata = excelRepository.Load(sheetName);
                        var athleteRepository = new AthleteRepository(exceldata, new DateTime(), _config);
                        var races = athleteRepository.GetRaces().ToList();
                        racejson = Newtonsoft.Json.JsonConvert.SerializeObject(races);
                        var raceAthletes = new RaceAthletes(athleteRepository.GetAthletes(), DateTime.Now, excelRepository.GetFiletime(), _config);
                        athletejson = Newtonsoft.Json.JsonConvert.SerializeObject(raceAthletes);
                        break;
                    }
                }
            }
            catch (Exception exception)
            {
                racejson = exception.Message;
                athletejson = exception.ToString();
            }
            //foreach (var message in messages)
            //{
            //    if (!string.IsNullOrEmpty(html)) html += "<br>";
            //    html += string.Format("<a href=\"{0}\">{0}</a>", message.Second);
            //}

            var rr = new JsonResult
            {
                Data = new jsondata
                {
                    races = racejson,
                    athletes = athletejson
                }
            };
            return rr;


            //var response = new HttpResponseMessage
            //{
            //    Content = new StringContent(string.Format("<p>{0}</p><p>{1}</p>", racejson, athletejson))
            //};
            //response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            //return response;
            
        }

        class jsondata
        {
            public string races { get; set; }
            public string athletes { get; set; }
        }

        [System.Web.Http.Route("files")]
        [System.Web.Http.HttpPost]
        [ValidateMimeMultipartContentFilter]
        public async Task<IHttpActionResult> UploadSingleFile()
        {
            var streamProvider = new MultipartFormDataStreamProvider(ServerUploadFolder);
            await Request.Content.ReadAsMultipartAsync(streamProvider);
            return Ok(UploadFiles(streamProvider, true));
        }

        [System.Web.Http.Route("fileshtml")]
        [System.Web.Http.HttpPost]
        [ValidateMimeMultipartContentFilter]
        public async Task<HttpResponseMessage> UploadSingleFileHtml()
        {
            var streamProvider = new MultipartFormDataStreamProvider(ServerUploadFolder);
            await Request.Content.ReadAsMultipartAsync(streamProvider);
            var messages = UploadFiles(streamProvider, false);
            var html = "";
            foreach (var message in messages)
            {
                if (!string.IsNullOrEmpty(html)) html += "<br>";
                html += string.Format("<a href=\"{0}\">{0}</a>", message.Second);
            }

            var response = new HttpResponseMessage
            {
                Content = new StringContent(html)
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;
        }

        private List<Pair> UploadFiles(MultipartFormDataStreamProvider streamProvider, bool addparameters)
        {
            var messages = new List<Pair>();

            // Add input parameters (application & project)
            var application = "";
            var project = "";
            foreach (string formkey in streamProvider.FormData)
            {
                var key = formkey;
                var value = streamProvider.FormData[key];
                switch (formkey.ToLowerInvariant())
                {
                    case "application":
                        application = value;
                        break;
                    case "project":
                        project = streamProvider.FormData[key];
                        break;
                    default:
                        key = string.Format("Unknown parameter: '{0}'", key);
                        break;
                }
                if (!addparameters) continue;
                messages.Add(new Pair(key, value));
            }

            if (!Directory.Exists(ProjectUploadFolder)) Directory.CreateDirectory(ProjectUploadFolder);

            foreach (var fileData in streamProvider.FileData)
            {
                //var filename = GetFilename(fileData.Headers.ContentDisposition.FileName);
                var filename = string.Format("{0}{1}", GetFilename(fileData.LocalFileName),
                    GetExtension(fileData.Headers.ContentDisposition.FileName));
                var newfilename = Path.Combine(ProjectUploadFolder, filename);
                if (File.Exists(newfilename))
                {
                    File.Move(newfilename, Path.Combine(ProjectUploadFolder, string.Format("{0}_{1}", DateTime.Now.Ticks, filename)));
                }
                File.Move(fileData.LocalFileName, newfilename);
                AddFileSecurity(newfilename, "Everyone", FileSystemRights.FullControl, AccessControlType.Allow);
                var protocol = "http";
                if (Request.RequestUri.Port != 80) protocol = "https";
                var url = string.Format("{0}://{1}/{2}/api/v1/attachment/get?file={3}",
                                    protocol,
                                    Request.RequestUri.Host,
                                    GetApplicationname(HttpContext.Current.Request),
                                    filename);
                messages.Add(new Pair("url", url));
            }
            return messages;
        }

        private static string GetExtension(string fileName)
        {
            var pos = fileName.LastIndexOf(".", StringComparison.Ordinal);
            if (pos < 0) return "";
            var ext = fileName.Substring(pos);
            if (ext.EndsWith("\"")) ext = ext.Trim('"');
            return ext;
        }

        private static string GetApplicationname(HttpRequest request)
        {
            var applicationname = AppDomain.CurrentDomain.BaseDirectory;
            if (applicationname.EndsWith("\\"))
            {
                applicationname = applicationname.Substring(0, applicationname.Length - 1);
            }
            var pos = applicationname.LastIndexOf("\\");
            if (pos > 0)
            {
                applicationname = applicationname.Substring(pos + 1);
            }

            var basePath = request.Path.Substring(0, request.Path.IndexOf(applicationname, StringComparison.OrdinalIgnoreCase));
            if (basePath.StartsWith("/")) basePath = basePath.Substring(1);
            return string.Format("{0}{1}", basePath, applicationname);
        }

        private static string GetFilename(string filename)
        {
            if (filename.StartsWith("\"") && filename.EndsWith("\""))
            {
                filename = filename.Trim('"');
            }
            if (filename.Contains(@"/") || filename.Contains(@"\"))
            {
                filename = Path.GetFileName(filename);
            }
            return filename;
        }

        private static void AddFileSecurity(string fileName, string account, FileSystemRights rights, AccessControlType controlType)
        {
            // Get a FileSecurity object that represents the
            // current security settings.
            var fSecurity = File.GetAccessControl(fileName);

            // Add the FileSystemAccessRule to the security settings.
            fSecurity.AddAccessRule(new FileSystemAccessRule(account, rights, controlType));

            // Set the new access settings.
            File.SetAccessControl(fileName, fSecurity);
        }
    }
}

using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace UtleiraTidtaker.Web.Controllers
{
    [RoutePrefix("api/v1/attachment")]
    public class AttachmentController : ApiController
    {
        private const string ProjectUploadFolder = @"c:\temp\UtleiraTidtaker.Web";

        [Route("get")]
        [HttpGet]
        public HttpResponseMessage Get([FromUri] string file)
        {
            if (!Directory.Exists(ProjectUploadFolder)) Directory.CreateDirectory(ProjectUploadFolder);

            var filepath = Path.Combine(ProjectUploadFolder, file);
            if (!File.Exists(filepath)) return null;

            var response = new HttpResponseMessage
            {
                Content = new StreamContent(new FileStream(filepath, FileMode.Open, FileAccess.Read))
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(filepath));
            return response;
        }
    }
}

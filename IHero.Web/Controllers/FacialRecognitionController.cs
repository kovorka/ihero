using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using IHero.Web.ViewModels;
using IHero.Web.Services;
using System.IO;
using System.Threading.Tasks;

namespace IHero.Web.Controllers
{
    public class FacialRecognitionController : ApiController
    {
        // GET: api/FacialRecognition/5
        public string Get(int id)
        {
            return id.ToString();
        }



        // http://localhost:12546/api/FacialRecognition/GetFacialRecognitionViewModel
        [HttpGet]
        public FacialRecognitionViewModel GetFacialRecognitionViewModel()
        {
            var vm = new FacialRecognitionViewModel();

            var dataService = new DataService();
            PhotoFace photoFace = dataService.GetTestPhotoFace();//get a real photo from Azure.
            vm.Face = new PhotoFaceViewModel(photoFace, vm.MaxWidthHeight);

            return vm;
        }

        //uploads an image to be searched by only the primary NAA/SLQ face group.
        public async Task<IHttpActionResult> UploadFile()
        {
            var result = await UploadFileProcess(false);
            return result;
        }

       //uploads an image to be searched by all cortana face groups.
        public async Task<IHttpActionResult> UploadFileDeep()
        {
            var result = await UploadFileProcess(true);
            return result;
        }

        private async Task<IHttpActionResult> UploadFileProcess(bool deepSearch)
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return StatusCode(HttpStatusCode.UnsupportedMediaType);
            }

            var filesReadToProvider = await Request.Content.ReadAsMultipartAsync();

            if (filesReadToProvider.Contents.Count == 1)
            {
                byte[] photo = await filesReadToProvider.Contents[0].ReadAsByteArrayAsync();

                var facialRecognitionService = new FacialRecognitionService();
                var vm = await facialRecognitionService.DetectFaceFindSimilar(photo, deepSearch);
                //vm.ResultFaces.ForEach(f => f.Photo.PhotoUrl = f.Photo.PhotoUrl.Replace(",","%2c"));
                
                return Ok(vm);
            }
            else
            {
                return new System.Web.Http.Results.BadRequestResult(this);
            }
        }

       
    }
}

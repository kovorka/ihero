using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IHero.Web.Services;
using System.IO;
using IHero.Web.ViewModels;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace IHero.Tests
{
    [TestClass]
    public class FacialRecognitionTests
    {
        private static string ImageVernonPath = @"..\..\..\IHero.Web\images\NAA\uz_72157628242532489\vernon-lieutenant-colonel-hugh-venables_6433573159_o.jpg";
        private static string ImageBiddlePath = @"..\..\TestImages\Biddle.JPG";
        private static string ImageArmstrongPath = @"..\..\TestImages\Armstrong.JPG";

        

        [TestMethod]
        public void DetectSimilarFacesArmstrongTest()
        {
            DetectFaceViewModel vm = DetectSimilarFace(ImageArmstrongPath, false);
            Assert.IsNotNull(vm);
        }

        [TestMethod]
        public void DetectSimilarFacesBiddleTest()
        {//10 o r8 seconds.
            DetectFaceViewModel vm = DetectSimilarFace(ImageBiddlePath, true);
            Assert.IsNotNull(vm);
        }



        [TestMethod]
        public void DetectSimilarFacesVernonTest()
        {
            DetectFaceViewModel vm = DetectSimilarFace(ImageVernonPath,false);
            Assert.IsNotNull(vm);
        }

        [TestMethod]
        public void SimilarFaceDatabaseSearch()
        {
            var service = new FacialRecognitionService();
            DetectFaceViewModel vm = new DetectFaceViewModel(); ;
            vm.SearchFaces = new List<PhotoFaceViewModel>
            {
                 new PhotoFaceViewModel
                 {
                      Gender = "Female",
                      Age = 0
                 }
            };
            var task = service.FindSimilarDatabaseResults(vm);
            task.Wait();
            Assert.IsTrue(vm.ResultFaces.Count > 0);
        }

      
        private static DetectFaceViewModel DetectSimilarFace(string imagePath, bool doDeepSearch)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            byte[] image = File.ReadAllBytes(imagePath);
            var service = new FacialRecognitionService();           //This face ID exists in the primary face group.
            var task = service.DetectFaceFindSimilar(image, doDeepSearch);
            task.Wait();
            var vm = task.Result;

            string timeElapsed = string.Format("{0:dd} days, {0:hh} hours, {0:mm} minutes, {0:ss} seconds", stopwatch.Elapsed);
            return vm;
        }
    }
}

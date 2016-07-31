using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IHero.Web.Services;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace IHero.Tests
{
    [TestClass]
    public class CortanaTests
    {
        private string _cortanaSubscriptionKey = CortanaService.SubscriptionKey;

        [TestMethod]
        public void GetFaceLists()
        {
            var cortanaService = new CortanaService();
            var faceList = cortanaService.ListFaceLists();
            faceList.Wait();
            var result = faceList.Result;
            
            string faceListStrings = string.Join("," + Environment.NewLine, result.OrderBy(f => f.Name).Select(f => string.Format(@"new FaceListMetadata {{ FaceListId = ""{0}"", Name = ""{1}"", UserData = ""{2}"" }}", f.FaceListId, f.Name, f.UserData)));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetPersonFacesInFaceList()
        {
            var cortanaService = new CortanaService();
            var faceList = cortanaService.GetFaceList("bb7bc96f-c0ca-4c55-b7fe-499ade353bca");
            faceList.Wait();
            var result = faceList.Result;
            Assert.IsNotNull(result);
        }
      
        
        [TestMethod]
        public void TestFaceDetection()
        {
            var cortanaService = new CortanaService();
            var fileStream = File.OpenRead(@"D:\Projects\Samples\FacialRecognition_ProjectOxford-ClientSDK-master\FacePhotos\images3.jpg");


            ////use a generic face list to add this image to. All it does it return the unique face ID that it matches.
            //var addImageToFaceListTask = cortanaService.AddImageToFaceList(_cortanaSubscriptionKey, "8bd4c772-a104-4100-a50d-715046c70755", fileStream);
            //addImageToFaceListTask.Wait();
            //var result = addImageToFaceListTask.Result;
        }

        
    }
}

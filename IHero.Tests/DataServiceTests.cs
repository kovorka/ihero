using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IHero.Web.Services;
using IHero.Web;

namespace IHero.Tests
{
    [TestClass]
    public class DataServiceTests
    {
        [TestMethod]
        public void GetAPhotoTest()
        {
            var dataService = new DataService();
            Photo photo = dataService.GetTestPhoto();
            Assert.IsNotNull(photo);
        }

        //[TestMethod]
        //public void GetPrimaryDetectionGroupPhotoFace()
        //{
        //    var dataService = new DataService();
        //    var photoFace = dataService.GetPrimaryFaceGroupTestPhoto();
        //} 
    }
}

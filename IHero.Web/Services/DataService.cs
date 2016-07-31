using IHero.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace IHero.Web.Services
{
    public class DataService
    {
        public Photo GetTestPhoto()
        {
            var context = new GovHack2016Entities();
            Photo photo = context.Photos.Where(p => p.HostingSite == "NAA").First();
            return photo;
        }

        //public PhotoFace GetPrimaryFaceGroupTestPhoto()
        //{
        //    var primaryGroup = new Guid("bb7bc96f-c0ca-4c55-b7fe-499ade353bca");
        //    var context = new GovHack2016Entities();
        //    PhotoFace photoFace = context.PhotoFaces.Where(p => p.FaceGroupId == primaryGroup).First();
        //    return photoFace;
        //}



        public PhotoFace GetTestPhotoFace()
        {
            var context = new GovHack2016Entities();
            var photoResult = context.PhotoFaces.Where(pf => pf.Photo.HostingSite == "NAA")
                                                .Select(pf => new { PhotoFace = pf, pf.Photo })
                                                .First();

            return photoResult.PhotoFace;
        }

        public List<PhotoFace> GetPhotoFacesByGender(string gender, double age)
        {
            var context = new GovHack2016Entities();
            if (age <= 0)
            {
                return new List<PhotoFace>();
            }
            List<PhotoFace> photoFaceList = (from faces in context.PhotoFaces
                                             where faces.Gender == gender
                                            select new { faces, faces.Photo, AgeDifference = faces.Age.HasValue ? Math.Abs(faces.Age.Value - age) : 1000 })
                                            .OrderBy(a => a.AgeDifference)
                                            .Take(10)
                                            .Select(a => a.faces)
                                            .ToList();

            return photoFaceList;
        }

        public async Task<List<PhotoFace>> PopulateDetectedFaceList(List<Guid> cortanaFaceIdList)
        {
            var context = new GovHack2016Entities();           
            var photoResult = context.PhotoFaces.Where(pf => cortanaFaceIdList.Contains(pf.CortanaFaceId))
                                                .Select(pf => new {  PhotoFace = pf, pf.Photo})
                                                .ToList();

            var facesOnly = photoResult.Select(p => p.PhotoFace).ToList();
            return facesOnly;
        }
    }
}
using IHero.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace IHero.Web.Services
{
    public class FacialRecognitionService
    {
        //public async Task<DetectFaceViewModel> DetectFace(byte[] imageBytes)
        //{
        //    var vm = new DetectFaceViewModel();
        //    var cs = new CortanaService();
        //    var imageStream = new MemoryStream(imageBytes);
        //    var faceResult = await cs.DetectFaces(imageStream);//call out to cortana!

        //    System.Drawing.Image image = System.Drawing.Image.FromStream(new MemoryStream(imageBytes));//convert to image to get the width/height.

        //    vm.ResultFaces = faceResult.Select(f => new PhotoFaceViewModel(f, image, vm.MaxWidthHeight)).ToList();
        //    return vm;
        //}

        //public async Task<DetectFaceViewModel> DetectSimilarFacesPrimaryGroupSearch(Guid faceId)
        //{
        //    var cs = new CortanaService();

        //    //Search cortana for similar faces.
        //    var findSimilarFaceResult = await cs.FindSimilarFaces(cs.PrimaryFaceGroup.FaceListId, faceId);

        //    var vm = new DetectFaceViewModel();

        //    List<Guid> cortanaFaceIdList = findSimilarFaceResult.Select(f => f.PersistedFaceId).ToList();
        //    var ds = new DataService();
        //    List<PhotoFace> photoFaceList = await ds.PopulateDetectedFaceList(cortanaFaceIdList);
        //    vm.ResultFaces = photoFaceList.Select(pf => new PhotoFaceViewModel(pf, vm.MaxWidthHeight)).ToList();

        //    //set the match confidence
        //    vm.ResultFaces.ForEach(f => f.MatchConfidence = findSimilarFaceResult.First(s => s.PersistedFaceId == f.CortanaFaceId).Confidence);
        //    return vm;
        //}


        public async Task<DetectFaceViewModel> DetectFaceFindSimilar(byte[] imageBytes, bool doDeepSearch = false)
        {
            var vm = new DetectFaceViewModel();
            await PopulateSearchFaces(imageBytes, vm);

            if (vm.SearchFaces.Count == 0)
            {
                return vm;
            }
            else
            {
                await PopulateResultFaces(vm, doDeepSearch);

                if (vm.ResultFaces.Count > 0)
                {
                    vm.IsCortanaResults = true;
                    return vm;
                }
                else
                {
                    vm.IsCortanaResults = false;
                    await FindSimilarDatabaseResults(vm);
                    return vm;
                }
            }
        }

        public async Task FindSimilarDatabaseResults(DetectFaceViewModel vm)
        {
            PhotoFaceViewModel face = vm.SearchFaces.First();

            DataService ds = new DataService();
            List<PhotoFace> similarFaces = ds.GetPhotoFacesByGender(face.Gender.ToLower(), face.Age.HasValue? face.Age.Value : 0);

            similarFaces = similarFaces.OrderBy(x => (x.Age - face.Age)).Take(10).ToList();
            vm.ResultFaces = similarFaces.Select(pf => new PhotoFaceViewModel(pf, vm.MaxWidthHeight)).ToList();
        
        }

        private static async Task PopulateSearchFaces(byte[] imageBytes, DetectFaceViewModel vm)
        {
            var cs = new CortanaService();
            var imageStream = new MemoryStream(imageBytes);
            var faceResult = await cs.DetectFaces(imageStream);//call out to cortana!

            System.Drawing.Image image = System.Drawing.Image.FromStream(new MemoryStream(imageBytes));//convert to image to get the width/height.

            //results of the input image.
            vm.SearchFaces = faceResult.Select(f => new PhotoFaceViewModel(f, image, vm.MaxWidthHeight)).ToList();
        }

        private static async Task PopulateResultFaces(DetectFaceViewModel vm, bool doDeepSearch)
        {
            var cs = new CortanaService();

            //got search faces, try to find similar faces.
            Guid faceIdToSearch = vm.SearchFaces.First().CortanaFaceId;//just use the first face.
            //Search cortana for similar faces.
            List<Microsoft.ProjectOxford.Face.Contract.SimilarPersistedFace> findSimilarFaceResult = null;
            if (doDeepSearch)
                findSimilarFaceResult = await cs.FindSimilarFacesInAllFaceLists(faceIdToSearch);
            else findSimilarFaceResult = await cs.FindSimilarFacesInFacelist(cs.PrimaryFaceGroup.FaceListId, faceIdToSearch);

            List<Guid> cortanaFaceIdList = findSimilarFaceResult.Select(f => f.PersistedFaceId).ToList();
            var ds = new DataService();
            List<PhotoFace> photoFaceList = await ds.PopulateDetectedFaceList(cortanaFaceIdList);
            vm.ResultFaces = photoFaceList.Select(pf => new PhotoFaceViewModel(pf, vm.MaxWidthHeight)).ToList();

            //set the match confidence
            vm.ResultFaces.ForEach(f => f.MatchConfidence = findSimilarFaceResult.First(s => s.PersistedFaceId == f.CortanaFaceId).Confidence);
            vm.ResultFaces = vm.ResultFaces.OrderByDescending(f => f.MatchConfidence).ToList();
        }
    }
}
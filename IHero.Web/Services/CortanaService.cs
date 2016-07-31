using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace IHero.Web.Services
{
    public class CortanaService
    {

        public static string SubscriptionKey = "EnterYourCortanaAnalyticsFaceApiKeyHere";

        #region FaceListConstants


        public FaceListMetadata PrimaryFaceGroup { get { return FaceGroupList.First(f => f.FaceListId == "bb7bc96f-c0ca-4c55-b7fe-499ade353bca"); } }

        /// <summary>
        /// This group is just to test if we can match a face for Frederick Biddle 
        /// </summary>
      //  public FaceListMetadata BiddleFaceGroup {  get { return FaceGroupList.First(f => f.FaceListId == "BB7BC96F-C0CA-4C55-B7FE-499ADE353BCA");  } }

        public List<FaceListMetadata> FaceGroupList { get; set; }
        #endregion

        public CortanaService()
        {
            SetupFaceGroupList();
        }

        private void SetupFaceGroupList()
        {
            FaceGroupList = new List<FaceListMetadata>
            {
                new FaceListMetadata { FaceListId = "bb7bc96f-c0ca-4c55-b7fe-499ade353bca", Name = "NAA/SLQ", UserData = " 481 x National Archives of Australia (NAA) and 519 x State Library of Queensland (SLQ)" },
                new FaceListMetadata { FaceListId = "2d60173c-99da-46c4-b1f1-80eabc81141b", Name = "AWM 1", UserData = "Australian War Memorial #1" },
                new FaceListMetadata { FaceListId = "bd110cc2-810c-4b34-abac-6357dcc6b4de", Name = "AWM 2", UserData = "Australian War Memorial #2" },
                new FaceListMetadata { FaceListId = "913faeb9-fda1-42f4-83a6-beaaa4f61079", Name = "AWM 3", UserData = "Australian War Memorial #3" },
                new FaceListMetadata { FaceListId = "2b35e1c6-c5c4-4b54-98af-81f293c760ae", Name = "AWM 4", UserData = "Australian War Memorial #4" },
                new FaceListMetadata { FaceListId = "1a8300ce-8259-4c37-83b3-af2bf2658bac", Name = "AWM 5", UserData = "Australian War Memorial #5" },
                new FaceListMetadata { FaceListId = "6c1d5adf-1bc8-407d-8902-7f293ce808e0", Name = "AWM 6", UserData = "Australian War Memorial #6" },
                new FaceListMetadata { FaceListId = "71899ca8-4019-4e13-af09-3add20431d4e", Name = "AWM 7", UserData = "Australian War Memorial #7" },
                new FaceListMetadata { FaceListId = "42d146a4-2b8b-449e-8d1c-219dc237cdb9", Name = "AWM 8", UserData = "Australian War Memorial #8" },
                new FaceListMetadata { FaceListId = "e4ca0912-7c06-40fb-a759-3481b945005f", Name = "AWM 9", UserData = "Australian War Memorial #9" },
                new FaceListMetadata { FaceListId = "c25cbf64-52b4-4a65-873f-5d4f97f07d50", Name = "AWM 10", UserData = "Australian War Memorial #10" },
                new FaceListMetadata { FaceListId = "dc7c7da8-a8ad-40e0-b77f-83f76599c81e", Name = "SLWA 1", UserData = "State Library of Western Australia" }
            };
        }

        /// <summary>
        /// Get the list of faces group guids that can be used with the 'FindSimilarFaces' function.
        /// </summary>
        /// <returns></returns>
        public async Task<List<FaceListMetadata>> ListFaceLists()
        {
            var faceServiceClient = new FaceServiceClient(SubscriptionKey);
            var taskList = await faceServiceClient.ListFaceListsAsync();

            List<FaceListMetadata> faceListResult = taskList.ToList();
            return faceListResult;
        }


        /// <summary>
        /// Gets all the person faces associated with a face list.
        /// </summary>
        /// <returns></returns>
        public async Task<FaceList> GetFaceList(string faceListId)
        {
            var faceServiceClient = new FaceServiceClient(SubscriptionKey);
            var faceList = await faceServiceClient.GetFaceListAsync(faceListId);

            return faceList;
        }

        /// <summary>
        /// Detects face with Guid. Use this for finding similar faces or for finding interesting facial information.
        /// </summary>        
        public async Task<List<Face>> DetectFaces(Stream imageStream)
        {
            var faceServiceClient = new FaceServiceClient(SubscriptionKey);
            Face[] faces = await faceServiceClient.DetectAsync(imageStream, true, true, new FaceAttributeType[] { FaceAttributeType.Gender, FaceAttributeType.Age, FaceAttributeType.Smile, FaceAttributeType.Glasses });
            return faces.ToList();
        }

        /// <summary>
        /// Using a face ID, find similar faces in the group.
        /// </summary>                
        /// <param name="faceId">The faceId returned from 'DetectFace'</param>
        /// <returns></returns>
        public async Task<List<SimilarPersistedFace>> FindSimilarFacesInFacelist(string faceListId, Guid faceId)
        {
            var faceServiceClient = new FaceServiceClient(SubscriptionKey);
            var results = await faceServiceClient.FindSimilarAsync(faceId, faceListId, 5);
            return results.ToList();
        }

        public async Task<List<SimilarPersistedFace>> FindSimilarFacesInAllFaceLists(Guid faceId)
        {
            var faceServiceClient = new FaceServiceClient(SubscriptionKey);
            var similarPersistedFacelist = new List<SimilarPersistedFace>();
            var faceGroupsToProcess = FaceGroupList.ToList();
            List<Task> tasks = new List<Task>();
            foreach (FaceListMetadata faceGroup in FaceGroupList)
            {
                SimilarPersistedFace[] persistedFaceResults = await faceServiceClient.FindSimilarAsync(faceId, faceGroup.FaceListId, 5);
                similarPersistedFacelist.AddRange(persistedFaceResults);
            }

            return similarPersistedFacelist;
        }

        //Do all searches in paralell
        //public async Task<List<SimilarPersistedFace>> FindSimilarFacesInAllFaceLists(Guid faceId)
        //{
        //    var faceServiceClient = new FaceServiceClient(SubscriptionKey);
        //    var similarPersistedFacelist = new List<SimilarPersistedFace>();
        //    List<Task> tasks = new List<Task>();
        //    foreach (FaceListMetadata faceGroup in FaceGroupList)
        //    {
        //        tasks.Add(Task.Factory.StartNew(
        //            async (faceListIdObj) =>
        //            {
        //                SimilarPersistedFace[] persistedFaceResults = await faceServiceClient.FindSimilarAsync(faceId, faceListIdObj.ToString(), 5);
        //                return persistedFaceResults.ToList();
        //            }, faceGroup.FaceListId)
        //            .Unwrap().ContinueWith((persistedFaceTask) =>
        //            {
        //                var persistedFaceList = persistedFaceTask.Result;
        //                similarPersistedFacelist.AddRange(persistedFaceList);
        //            }));                                      
        //    }

        //    await Task.WhenAll(tasks);

        //    return similarPersistedFacelist;
        //}

        //List<Task> tasks = new List<Task>();
        //foreach (var img in Directory.EnumerateFiles(dlg.SelectedPath, "*.jpg", SearchOption.AllDirectories))
        //        {
        //            tasks.Add(Task.Factory.StartNew(
        //                async(obj) =>
        //                {
        //                    var imgPath = obj as string;

        //                    // Call detection
        //                    using (var fStream = File.OpenRead(imgPath))
        //                    {
        //                        try
        //                        {
        //                            var faces = await faceServiceClient.AddFaceToFaceListAsync(_faceListName, fStream);
        //                            return new Tuple<string, ClientContract.AddPersistedFaceResult>(imgPath, faces);
        //                        }
        //                        catch (FaceAPIException)
        //                        {
        //                            // Here we simply ignore all detection failure in this sample
        //                            // You may handle these exceptions by check the Error.Error.Code and Error.Message property for ClientException object
        //                            return new Tuple<string, ClientContract.AddPersistedFaceResult>(imgPath, null);
        //                        }
        //                    }
        //                },
        //                img).Unwrap().ContinueWith((detectTask) =>
        //                {
        //                    var res = detectTask.Result;
        //                    if (res.Item2 == null)
        //                    {
        //                        return;
        //                    }

        //                    // Update detected faces on UI
        //                    this.Dispatcher.Invoke(
        //                        new Action<ObservableCollection<Face>, string, ClientContract.AddPersistedFaceResult>(UIHelper.UpdateFace),
        //                        FacesCollection,
        //                        res.Item1,
        //                        res.Item2);
        //                }));
        //            processCount++;

        //            if (processCount >= SuggestionCount && !forceContinue)
        //            {
        //                var continueProcess = System.Windows.Forms.MessageBox.Show("The images loaded have reached the recommended count, may take long time if proceed. Would you like to continue to load images?", "Warning", System.Windows.Forms.MessageBoxButtons.YesNo);
        //                if (continueProcess == System.Windows.Forms.DialogResult.Yes)
        //                {
        //                    forceContinue = true;
        //                }
        //                else
        //                {
        //                    break;
        //                }
        //            }
        //        }

        //        await Task.WhenAll(tasks);
    }
}
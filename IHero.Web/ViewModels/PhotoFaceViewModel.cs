using IHero.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IHero.Web.ViewModels
{
    public class PhotoFaceViewModel
    {
        private double? _age;
        private string _gender;
        private string _glasses;
        private double _matchConfidence;

        /// <summary>
        /// 1.0 = 100% confident
        /// 0.5 = 50% confident
        /// 0.0 = 0.0% confident
        /// </summary>
        public double MatchConfidence
        {
            get
            {
                return Math.Round(_matchConfidence*100);
            }
            set
            {
                _matchConfidence = value;
            }
        }
       
        public int Id { get; set; }
        
        public Guid CortanaFaceId { get; set; }
        public double? Age {
            get
            {
                return _age == null? _age: Math.Round(_age ?? 0);
            }
            set
            {
                _age = value;
            }
        }
        public string Gender
        {
            get
            {
                return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(_gender);
            }
            set
            {
                _gender = value;
            }
        }
        public string Glasses
        {
            get
            {
                switch (_glasses)
                {
                    case "NoGlasses": return "No Glasses";
                    case "Sunglasses": return "Sunglasses";
                    case "ReadingGlasses": return "Reading Glasses";
                    case "SwimmingGoggles": return "Swimming Goggles";
                    default: return _glasses;
                } 
            }
            set
            {
                _glasses = value;
            }
        }
        public Nullable<double> SmileCalculation { get; set; }

        public Nullable<bool> HasSmile { get; set; }

        /// <summary>
        /// The location of the face for a full sized image.
        /// </summary>
        public FaceRectangleViewModel FaceRectangle { get; set; } = new FaceRectangleViewModel();

        /// <summary>
        /// The location of the face for a resized image. (eg if the max width or height rendered on the web page is less then the actual width or height).
        /// </summary>
        public FaceRectangleViewModel FaceRectangleResized { get; set; } = new FaceRectangleViewModel();

        public PhotoViewModel Photo { get; set; }


        public PhotoFaceViewModel() { }

        public PhotoFaceViewModel(PhotoFace photoFace, int? maxImageWidthHeight = null)
        {
            Id = photoFace.Id;
            CortanaFaceId = photoFace.CortanaFaceId;
            Age = photoFace.Age;
            Gender = photoFace.Gender;
            Glasses = photoFace.Glasses;
            SmileCalculation = photoFace.SmileCalculation;
            HasSmile = photoFace.HasSmile;
            FaceRectangle = new FaceRectangleViewModel { Top = photoFace.FaceRectangleTop, Left = photoFace.FaceRectangleLeft, Height = photoFace.FaceRectangleHeight, Width = photoFace.FaceRectangleWidth };

            Photo = new PhotoViewModel(photoFace.Photo, maxImageWidthHeight);

            if (maxImageWidthHeight.HasValue)
                PhotoProcessor.CalculateResizedFaceRectangle(this, maxImageWidthHeight.Value);
        }

        public PhotoFaceViewModel(Microsoft.ProjectOxford.Face.Contract.Face face, System.Drawing.Image image, int? maxImageWidthHeight = null)
        {
            CortanaFaceId = face.FaceId;
            Age = face.FaceAttributes.Age;
            Gender = face.FaceAttributes.Gender;
            Glasses = face.FaceAttributes.Glasses.ToString();
            SmileCalculation = face.FaceAttributes.Smile;
            HasSmile = SmileCalculation > 0.5;
            FaceRectangle = new FaceRectangleViewModel { Top = face.FaceRectangle.Top, Left = face.FaceRectangle.Left, Height = face.FaceRectangle.Height, Width = face.FaceRectangle.Width };

            Photo = new PhotoViewModel(image, maxImageWidthHeight);

            if (maxImageWidthHeight.HasValue)
                PhotoProcessor.CalculateResizedFaceRectangle(this, maxImageWidthHeight.Value);
        }
    }
}
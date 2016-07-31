using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IHero.Web.ViewModels
{
    public class DetectFaceViewModel
    {
        public int MaxWidthHeight { get; set; } = 300;//300 pixels is either the maximum height of the maximum width.

        public bool IsCortanaResults { get; set; }

        public List<PhotoFaceViewModel> SearchFaces { get; set; } = new List<PhotoFaceViewModel>();

        public List<PhotoFaceViewModel> ResultFaces { get; set; } = new List<PhotoFaceViewModel>();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IHero.Web.ViewModels
{
    public class FacialRecognitionViewModel
    {
        public int MaxWidthHeight { get; set; } = 300;//300 pixels is either the maximum height of the maximum width.

        public PhotoFaceViewModel Face { get; set; }
    }
}
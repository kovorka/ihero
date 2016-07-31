using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IHero.Web.ViewModels;

namespace IHero.Web.Services
{
    public class PhotoProcessor
    {

        public static void CalculateScaledImageWidthHeight(PhotoViewModel photo, int maxImageWidthHeight)
        {            
            float ratio = (float)photo.Width / photo.Height;
            
            if (ratio > 1.0)
            {
                photo.ScaledWidth = maxImageWidthHeight;
                photo.ScaledHeight = (int)(maxImageWidthHeight / ratio);
            }
            else
            {
                photo.ScaledHeight = maxImageWidthHeight;
                photo.ScaledWidth = (int)(ratio * maxImageWidthHeight);
            }            
        }

        public static void CalculateResizedFaceRectangle(PhotoFaceViewModel photoFace,int maxImageWidthHeight)
        {
            var imageWidth = photoFace.Photo.Width;
            var imageHeight = photoFace.Photo.Height;
            float ratio = (float)imageWidth / imageHeight;
            int uiWidth = 0;
            int uiHeight = 0;
            if (ratio > 1.0)
            {
                uiWidth = maxImageWidthHeight;
                uiHeight = (int)(maxImageWidthHeight / ratio);
            }
            else
            {
                uiHeight = maxImageWidthHeight;
                uiWidth = (int)(ratio * uiHeight);
            }

            //int uiXOffset = (maxImageWidthHeight - uiWidth) / 2;
            //int uiYOffset = (maxImageWidthHeight - uiHeight) / 2;
            float scale = (float)uiWidth / imageWidth;


            //photoFace.FaceRectangleResized.Left = (int)((photoFace.FaceRectangle.Left * scale) + uiXOffset);
            //photoFace.FaceRectangleResized.Top = (int)((photoFace.FaceRectangle.Top * scale) + uiYOffset);
            photoFace.FaceRectangleResized.Left = (int)(photoFace.FaceRectangle.Left * scale);
            photoFace.FaceRectangleResized.Top = (int)(photoFace.FaceRectangle.Top * scale);
            photoFace.FaceRectangleResized.Height = (int)(photoFace.FaceRectangle.Height * scale);
            photoFace.FaceRectangleResized.Width = (int)(photoFace.FaceRectangle.Width * scale);            
        }
    }
}
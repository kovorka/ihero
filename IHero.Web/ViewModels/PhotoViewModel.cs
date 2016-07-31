using IHero.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IHero.Web.ViewModels
{
    public class PhotoViewModel
    {
        public string Description { get; set; }
        public string PhotoUrl { get; set; }

        public string ContentUrl { get; set; }

        public int Width { get; set; }

        /// <summary>
        /// The width that has been scaled down from the original to maintain the aspect ratio.
        /// </summary>
        public int ScaledWidth { get; set; }

        /// <summary>
        /// eg State Library of Western Australia
        /// </summary>
        public string HostingSite { get; set; }

        public string WarConflicts { get; set; }

        public int Height { get; set; }

        /// <summary>
        /// The height that has been scaled down from the original to maintain the aspect ratio.
        /// </summary>
        public int ScaledHeight { get; set; }

        public PhotoViewModel() { }

        public PhotoViewModel(Photo photo, int? maxImageWidthHeight = 0)
        {
            Description = photo.Description;
            PhotoUrl = photo.PhotoUrl;
            ContentUrl = photo.ContentUrl;
            Width = photo.Width;
            Height = photo.Height;
            WarConflicts = photo.WarConflicts;
            HostingSite = GetHostingSiteDescriptionFromCode(photo.HostingSite);

            if (maxImageWidthHeight.HasValue)
                PhotoProcessor.CalculateScaledImageWidthHeight(this, maxImageWidthHeight.Value);
        }

        private string GetHostingSiteDescriptionFromCode(string hostingSiteCode)
        {
            switch (hostingSiteCode)
            {
                case "AWM": return "Australian War Memorial";
                case "NAA": return "National Archives of Australia";
                case "SLQ": return "State Library of Queensland";
                case "SLWA": return "State Library of Western Australia";
                default:
                    return hostingSiteCode;
            }
        }

        public PhotoViewModel(System.Drawing.Image image, int? maxImageWidthHeight = 0)
        {
            Width = image.Width;
            Height = image.Height;

            if (maxImageWidthHeight.HasValue)
                PhotoProcessor.CalculateScaledImageWidthHeight(this, maxImageWidthHeight.Value);
        }
    }
}
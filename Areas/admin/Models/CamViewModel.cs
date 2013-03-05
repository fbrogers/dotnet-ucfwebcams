using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using SDES___Webcams.Models;

namespace SDES___Webcams.Areas.admin.Models
{
    public class CamViewModel
    {
        public int camId { get; set; }

        [DisplayName("Name")]
        [Required]
        public string camName { get; set; }

        [DisplayName("Short Description (100 Characters)")]
        [DataType(DataType.MultilineText)]
        [Required]
        [StringLength(100, ErrorMessage = "Must Not Exceed 100 Characters")]
        public string camShortDesc { get; set; }

        [DisplayName("Longer Description (Unlimited Characters)")]
        [DataType(DataType.MultilineText)]
        [Required]
        public string camLongDesc { get; set; }

        [DisplayName("Location (Building)")]
        [Required]
        public string camLocationName { get; set; }

        [DisplayName("Location (UCF Map ID)")]
        [Required]
        public string camLocationId { get; set; }

        [DisplayName("Location (Latitude)")]
        [DisplayFormat(DataFormatString = "{0:n6}", ApplyFormatInEditMode = true)]
        public decimal camLat { get; set; }

        [DisplayName("Location (Longitude)")]
        [DisplayFormat(DataFormatString = "{0:n6}", ApplyFormatInEditMode = true)]
        public decimal camLong { get; set; }

        [DisplayName("Stream Refresh Rate (Seconds)")]
        [Required]
        public byte camInterval { get; set; }

        [DisplayName("URI to Rendered JPG")]
        [Required]
        public string camJpegUri { get; set; }

        [DisplayName("Image Quality")]
        [Required]
        public byte camJpegQuality { get; set; }
        public SelectList jpegQualityList { get; set; }

        [DisplayName("Camera Enabled")]
        [Required]
        public bool isEnabled { get; set; }

        [DisplayName("Image Blurred")]
        [Required]
        public bool isBlurred { get; set; }

        public DateTime created { get; set; }

        public CamViewModel()
        {
            var options = new []
            {
                new {ID = 20, Name = "Low"},
                new {ID = 40, Name = "Medium"},
                new {ID = 60, Name = "High"},
                new {ID = 90, Name = "Maximum"}
            };
            jpegQualityList = new SelectList(options, "ID", "Name");
        }

        public static CamViewModel ModelConvert(cam input)
        {
            return new CamViewModel
            {
                camId = input.camId,
                camName = input.camName,
                camShortDesc = input.camShortDesc,
                camLongDesc = input.camLongDesc,
                camLocationName = input.camLocationName,
                camLocationId = input.camLocationId,
                camLat = input.camLat,
                camLong = input.camLong,
                camInterval = input.camInterval,
                camJpegUri = input.camJpegUri,
                camJpegQuality = input.camJpegQuality,
                isEnabled = input.isEnabled,
                isBlurred = input.isBlurred,
                created = input.created
            };
        }

        public static cam ModelConvert(CamViewModel input)
        {
            return new cam
            {
                camId = input.camId,
                camName = input.camName,
                camShortDesc = input.camShortDesc,
                camLongDesc = input.camLongDesc,
                camLocationName = input.camLocationName,
                camLocationId = input.camLocationId,
                camLat = input.camLat,
                camLong = input.camLong,
                camInterval = input.camInterval,
                camJpegUri = input.camJpegUri,
                camJpegQuality = input.camJpegQuality,
                isEnabled = input.isEnabled,
                isBlurred = input.isBlurred,
                created = input.created
            };
        }
    }
}
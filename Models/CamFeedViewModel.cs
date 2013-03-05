using System.Collections.Generic;
using System.Linq;

namespace SDES___Webcams.Models
{
    public class CamFeedViewModel
    {
        public int camId { get; set; }
        public string camName { get; set; }
        public string camShortDesc { get; set; }
        public string camLongDesc { get; set; }
        public string camLocationName { get; set; }
        public string camLocationId { get; set; }
        public decimal camLat { get; set; }
        public decimal camLong { get; set; }
        public string camUri { get; set; }
        public string frameUri { get; set; }
        public string streamUri { get; set; }

        public static IEnumerable<CamFeedViewModel> Convert(IEnumerable<cam> derived)
        {
            var output = derived.AsEnumerable().Select(input => new CamFeedViewModel
            {
                camId = input.camId,
                camName = input.camName,
                camShortDesc = input.camShortDesc,
                camLongDesc = input.camLongDesc,
                camLocationName = input.camLocationName,
                camLocationId = input.camLocationId,
                camLat = input.camLat,
                camLong = input.camLong,
                camUri = string.Format("http://webcams.sdes.ucf.edu/webcam/Details/{0}", input.camId),
                frameUri = string.Format("http://webcams.sdes.ucf.edu/webcam/Frame/{0}", input.camId),
                streamUri = string.Format("http://webcams.sdes.ucf.edu/webcam/Stream/{0}", input.camId)
            });

            return output;
        }
    }
}
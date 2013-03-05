using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Xml;
using SDES___Webcams.Code;
using SDES___Webcams.Models;

namespace SDES___Webcams.Controllers
{
    public class webcamController : Controller
    {
        //
        // GET: /webcam/

        public ActionResult Index()
        {
            var db = new IT_WebcamsEntities();
            var all = db.cams.Where(x => x.isEnabled).OrderBy(x => x.camName).ToList();
            return View(all);
        }

        //
        // GET: /webcam/Details/5

        public ActionResult Details(int id)
        {
            try
            {
                //forward if browser is IE
                var browser = Request.Browser;
                if (browser.Browser == "IE")
                {
                    return RedirectToAction("DetailsIE", new {id});
                }

                var db = new IT_WebcamsEntities();
                var single = db.cams.Find(id);
                return View(single);
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        //
        // GET: /webcam/DetailsIE/5

        public ActionResult DetailsIE(int id)
        {
            try
            {
                var db = new IT_WebcamsEntities();
                var single = db.cams.Find(id);
                return View(single);
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        //
        // GET: /webcam/Frame/5

        public void Frame(int id)
        {
            var db = new IT_WebcamsEntities();
            var single = db.cams.Find(id);
            Generic.GetFrame(single.camJpegUri, single.camJpegQuality);  
        }

        //
        // GET: /webcam/Stream/5

        public void Stream(int id)
        {
            //forward if browser is IE
            var browser = Request.Browser;
            if (browser.Browser == "IE")
            {
                Frame(id);
                return;
            }

            var db = new IT_WebcamsEntities();
            var single = db.cams.Find(id);
            Generic.GetStream(single.camJpegUri, single.camJpegQuality, single.isBlurred, single.camInterval * 1000);
        }

        //
        // GET: /webcam/feed/

        [OutputCache(Duration = 86400)]
        public ActionResult feed()
        {
            var db = new IT_WebcamsEntities();
            var cameras = db.cams.Where(x => x.isEnabled).OrderBy(x => x.camName);
            var json = CamFeedViewModel.Convert(cameras);

            //return json
            return Json(json, "application/json", Encoding.UTF8, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /webcam/kml/

        [OutputCache(Duration = 300)]
        public void kml()
        {
            //init
            Response.ClearHeaders();
            Response.ContentType = "application/vnd.google-earth.kml+xml";

            var xml = new XmlDocument {PreserveWhitespace = false};
            xml.AppendChild(xml.CreateXmlDeclaration("1.0", "UTF-8", null));

            var root = (XmlElement)xml.AppendChild(xml.CreateElement("kml"));
            root.SetAttribute("xmlns", "http://www.opengis.net/kml/2.2");
            root.SetAttribute("xmlns:gx", "http://www.google.com/kml/ext/2.2");

            var document = (XmlElement)root.AppendChild(xml.CreateElement("Document"));

            var name = (XmlElement)document.AppendChild(xml.CreateElement("name"));
            name.InnerText = "UCF Campus Webcams";

            var Snippet = (XmlElement)document.AppendChild(xml.CreateElement("Snippet"));
            Snippet.SetAttribute("maxLines", "1");
            Snippet.AppendChild(xml.CreateCDataSection("http://webcams.sdes.ucf.edu/"));

            var open = (XmlElement)document.AppendChild(xml.CreateElement("open"));
            open.InnerText = "1";

            var folder = (XmlElement)document.AppendChild(xml.CreateElement("Folder"));

            var fname = (XmlElement)folder.AppendChild(xml.CreateElement("name"));
            fname.AppendChild(xml.CreateCDataSection("KMLFeed"));

            var visibility = (XmlElement)folder.AppendChild(xml.CreateElement("visibility"));
            visibility.InnerText = "1";

            var fopen = (XmlElement)folder.AppendChild(xml.CreateElement("open"));
            fopen.InnerText = "0";

            using (var db = new IT_WebcamsEntities())
            {
                var cameras = db.cams.Where(x => x.isEnabled).OrderBy(x => x.camName);
                foreach (var camera in cameras)
                {
                    var placemark = (XmlElement)folder.AppendChild(xml.CreateElement("Placemark"));

                    var pname = (XmlElement)placemark.AppendChild(xml.CreateElement("name"));
                    pname.AppendChild(xml.CreateCDataSection(camera.camName));

                    //lookat
                    var lookat = (XmlElement)placemark.AppendChild(xml.CreateElement("LookAt"));

                    var longitude = (XmlElement)lookat.AppendChild(xml.CreateElement("longitude"));
                    longitude.InnerText = camera.camLong.ToString();

                    var latitude = (XmlElement)lookat.AppendChild(xml.CreateElement("latitude"));
                    latitude.InnerText = camera.camLat.ToString();

                    var range = (XmlElement)lookat.AppendChild(xml.CreateElement("range"));
                    range.InnerText = "1000";

                    var altitudeMode = (XmlElement)lookat.AppendChild(xml.CreateElement("altitudeMode"));
                    altitudeMode.InnerText = "relativeToGround";

                    var tilt = (XmlElement)lookat.AppendChild(xml.CreateElement("tilt"));
                    tilt.InnerText = "0";

                    var heading = (XmlElement)lookat.AppendChild(xml.CreateElement("heading"));
                    heading.InnerText = "0";

                    //point
                    var point = (XmlElement)placemark.AppendChild(xml.CreateElement("Point"));

                    var altitudeMode2 = (XmlElement)point.AppendChild(xml.CreateElement("altitudeMode"));
                    altitudeMode2.InnerText = "clampToGround";

                    var extrude = (XmlElement)point.AppendChild(xml.CreateElement("extrude"));
                    extrude.InnerText = "0";

                    var coordinates = (XmlElement)point.AppendChild(xml.CreateElement("coordinates"));
                    coordinates.InnerText = string.Format("{0},{1},{2}", camera.camLong, camera.camLat, "0");

                    //description
                    var description = (XmlElement)placemark.AppendChild(xml.CreateElement("description"));
                    var descriptionInner = camera.camLongDesc.Replace(Environment.NewLine, string.Empty) +
                        string.Format(
                            "<br />" +
                            "<a href=\"http://webcams.sdes.ucf.edu/webcam/Details/{0}\">" +
                            "<img src=\"http://webcams.sdes.ucf.edu/webcam/Frame/{0}\" alt=\"Frame\" width=\"300\" height=\"200\"/>" +
                            "</a><br />" +
                            "<a href=\"http://webcams.sdes.ucf.edu/webcam/Details/{0}\">View Stream</a>",
                        camera.camId);
                    description.AppendChild(xml.CreateCDataSection(descriptionInner));
                }
            }

            //finish
            Response.BinaryWrite(Encoding.UTF8.GetPreamble());
            Response.Write(xml.InnerXml.Trim());
            Response.End();
        }
    }
}

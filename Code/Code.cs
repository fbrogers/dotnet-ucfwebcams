using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using Kaliko.ImageLibrary;
using Kaliko.ImageLibrary.Filters;

namespace SDES___Webcams.Code
{
    public static class Generic
    {
        public static void GetFrame(string uri, int quality, bool isBlurry = false)
        {
            //prep
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ContentType = "image/jpeg";

            //get and prep image data
            var client = new WebClient();
            SetBasicAuthenticationCredentials(client, uri);
            var data = client.DownloadData(uri);
            var image = Image.FromStream(new MemoryStream(data));

            //write image data
            if (isBlurry)
            {
                image = BlurImage(image);
            }

            //compress image
            var compressed = CompressImage(image, quality);

            //write to response
            HttpContext.Current.Response.AddHeader("Content-Length", compressed.Length.ToString());
            compressed.WriteTo(HttpContext.Current.Response.OutputStream);
            compressed.Dispose();

            //send to client
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
            client.Dispose();
        }

        public static void GetStream(string uri, int quality, bool isBlurry = false, int interval = 1000)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ContentType = "multipart/x-mixed-replace; boundary=--myboundary";

            //get and prep image data
            var client = new WebClient();
            SetBasicAuthenticationCredentials(client, uri);
           
            while (true)
            {
                var data = client.DownloadData(uri);
                var image = Image.FromStream(new MemoryStream(data));

                //write image data
                if (isBlurry)
                {
                    image = BlurImage(image);
                }

                //compress image
                var compressed = CompressImage(image, quality);

                //build boundary string
                var boundary = Encoding.ASCII.GetBytes("\r\n--myboundary\r\nContent-Type: image/jpeg\r\n\r\n");
                var mem = new MemoryStream(boundary);

                //write to response
                mem.WriteTo(HttpContext.Current.Response.OutputStream);
                compressed.WriteTo(HttpContext.Current.Response.OutputStream);

                //add extra padding to fix Firefox clipping
                HttpContext.Current.Response.BinaryWrite(Encoding.ASCII.GetBytes(string.Empty.PadRight(1000)));

                //send data to the browser
                HttpContext.Current.Response.Flush();
            
                //pause
                Thread.Sleep(interval);
            }
        }

        public static void SetBasicAuthenticationCredentials(WebClient client, string uri)
        {
            if (uri.IndexOf("@") == -1) return;
            var start = uri.IndexOf("//") + 2;
            var end = uri.IndexOf("@");
            var userpass = uri.Substring(start, end - start).Split(':');
            var basic_auth = new NetworkCredential(userpass[0], userpass[1]);
            client.Credentials = basic_auth;
        }

        public static MemoryStream CompressImage(Image source, int quality)
        {
            var blob = new KalikoImage(source);
            var output = new MemoryStream();
            blob.SaveJpg(output, quality);
            blob.Dispose();

            return output;
        }

        public static Image BlurImage(Image source)
        {
            var blob = new KalikoImage(source);
            blob.ApplyFilter(new GaussianBlurFilter(1, 1));

            var output = new MemoryStream();
            blob.SaveJpg(output, 100);
            blob.Dispose();

            return Image.FromStream(output);
        }
    }
}
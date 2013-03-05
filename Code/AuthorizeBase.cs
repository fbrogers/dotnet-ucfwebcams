using System.Web.Mvc;

namespace SDES___Webcams.Code
{
    [Authorize(Roles = @"SDES\Web-IT UCF Webcams Admin Access")]
    public abstract class AuthorizeBaseController : Controller
    {

    }
}
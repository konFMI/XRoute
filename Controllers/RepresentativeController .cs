using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace XRoute.Controllers
{
    [Authorize(Roles = "Representative")]
    public class RepresentativeController : Controller
    {
        // Actions restricted to representatives
    }

}

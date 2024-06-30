using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace XRoute.Controllers
{
    [Authorize(Roles = "Normal")]
    public class UserController : Controller
    {
        // Actions restricted to normal users
    }
}

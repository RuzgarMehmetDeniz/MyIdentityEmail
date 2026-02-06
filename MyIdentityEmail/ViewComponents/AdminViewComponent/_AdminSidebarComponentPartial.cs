using Microsoft.AspNetCore.Mvc;
using MyIdentityEmail.Context;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityEmail.ViewComponents
{
    public class _AdminSidebarComponentPartial : ViewComponent
    {

        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
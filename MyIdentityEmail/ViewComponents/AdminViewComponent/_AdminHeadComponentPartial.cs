using Microsoft.AspNetCore.Mvc;

namespace MyIdentityEmail.ViewComponents.AdminViewComponent
{
    public class _AdminHeadComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}

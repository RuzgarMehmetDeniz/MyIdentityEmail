using Microsoft.AspNetCore.Mvc;

namespace MyIdentityEmail.ViewComponents.AdminViewComponent
{
    public class _AdminScriptComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}

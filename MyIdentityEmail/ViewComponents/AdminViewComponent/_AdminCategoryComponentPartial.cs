using Microsoft.AspNetCore.Mvc;
using MyIdentityEmail.Context;

namespace MyIdentityEmail.ViewComponents.AdminViewComponent
{
    public class _AdminCategoryComponentPartial : ViewComponent
    {
        private readonly EmailContext _emailContext;

        public _AdminCategoryComponentPartial(EmailContext emailContext)
        {
            _emailContext = emailContext;
        }

        public IViewComponentResult Invoke()
        {
            var values = _emailContext.Categories.ToList();
            return View(values);
        }
    }
}

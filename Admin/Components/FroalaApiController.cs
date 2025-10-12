using Microsoft.AspNetCore.Mvc;

namespace Admin.Components
{
    [ViewComponent(Name = "InitFloara")]
    public class InitFloaraViewComponent : ViewComponent
    {
        public InitFloaraViewComponent()
        {
            
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var script = "<script id='spark-fe'>try{(function (k){localStorage.FEK=k;t=document.getElementById('spark-fe');t.parentNode.removeChild(t);})('fPELe1OMFCIe2LUH1IT==')}catch(e){}</script>";
            return View("Default", script);
        }
    }

   
}
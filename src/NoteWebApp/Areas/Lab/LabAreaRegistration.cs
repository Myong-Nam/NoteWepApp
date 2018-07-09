using System.Web.Mvc;

namespace NoteWebApp.Areas.Lab
{
    public class LabAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Lab";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Lab_default",
                "Lab/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
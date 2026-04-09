using System.Web;
using System.Web.Mvc;

namespace MajorProject_HRMS_APP25
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}

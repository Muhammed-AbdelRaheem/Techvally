using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Web;

namespace Utility
{
    public static class HtmlHelper

    {
        public static string DecodeHtmlHelper(this IHtmlHelper helper, IHtmlContent text)
        {
            using (var writer = new System.IO.StringWriter())
            {
                text.WriteTo(writer, HtmlEncoder.Default);
                HtmlString msg = new HtmlString(writer.ToString());
                // return msg;
                var test = msg.Value.ToString();
                return HttpUtility.HtmlDecode(test);
            }

        }
        public static string IsSelected(this IHtmlHelper htmlHelper, string controllers, string actions, string id = null, string cssClass = "active")
        {
            string currentId = htmlHelper.ViewContext.RouteData.Values["id"] as string;
            string currentAction = htmlHelper.ViewContext.RouteData.Values["action"] as string;
            string currentController = htmlHelper.ViewContext.RouteData.Values["controller"] as string;

            IEnumerable<string> acceptedId = id != null ? (id ?? currentId).Split(',') : null;
            IEnumerable<string> acceptedActions = (actions ?? currentAction).Split(',');
            IEnumerable<string> acceptedControllers = (controllers ?? currentController).Split(',');
            if (!String.IsNullOrWhiteSpace(id))
            {
                return acceptedId.Contains(currentId) && acceptedActions.Contains(currentAction) && acceptedControllers.Contains(currentController) ?
              cssClass : String.Empty;
            }
            return acceptedActions.Contains(currentAction) && acceptedControllers.Contains(currentController) ?
                    cssClass : String.Empty;
        }
        public static string? Language(this IHtmlHelper htmlHelper)
        {
            string language = htmlHelper.ViewContext.RouteData.Values["language"] as string ?? "";
            return language;
        }

        public static string? GetId(this IHtmlHelper htmlHelper, string id)
        {
            string idValue = htmlHelper.ViewContext.RouteData.Values[id] as string ?? "";
            return idValue;
        }


        public static string? Controller(this IHtmlHelper htmlHelper)
        {
            string controller = htmlHelper.ViewContext.RouteData.Values["controller"] as string ?? "";
            return controller;
        }
        public static string? Action(this IHtmlHelper htmlHelper)
        {
            string action = htmlHelper.ViewContext.RouteData.Values["action"] as string ?? "";
            return action;
        }

        public static string IsArabic(this IHtmlHelper htmlHelper, string language, string cssClass = "arabic-lang")
        {
            string currentVideo = htmlHelper.ViewContext.RouteData.Values["language"] as string ?? "";
            return currentVideo.Equals(language) ? cssClass : String.Empty;
        }

        public static string Isfrench(this IHtmlHelper htmlHelper, string language, string cssClass = "french-lang")
        {
            string currentVideo = htmlHelper.ViewContext.RouteData.Values["language"] as string ?? "";
            return currentVideo.Equals(language) ? cssClass : String.Empty;
        }


        public static string slidercarousel(this IHtmlHelper htmlHelper, string language, string cssClass = "slider-carousel")
        {
            string currentVideo = htmlHelper.ViewContext.RouteData.Values["language"] as string;
            if (currentVideo.Equals("ar"))
            {
                return "slider-carousel-ar";
            }
            return cssClass;
        }
        public static string carousel(this IHtmlHelper htmlHelper, string language, string cssClass = "ltr")
        {
            string currentVideo = htmlHelper.ViewContext.RouteData.Values["language"] as string;
            if (currentVideo.Equals("ar"))
            {
                return "rtl";
            }
            return cssClass;
        }
        public static string client(this IHtmlHelper htmlHelper, string language, string cssClass = "client-items")
        {
            string currentVideo = htmlHelper.ViewContext.RouteData.Values["language"] as string;
            if (currentVideo.Equals("ar"))
            {
                return "client-items-ar";
            }
            return cssClass;
        }
        public static string Testimonials(this IHtmlHelper htmlHelper, string language, string cssClass = "testimonials")
        {
            string currentVideo = htmlHelper.ViewContext.RouteData.Values["language"] as string;
            if (currentVideo.Equals("ar"))
            {
                return "testimonials-ar";
            }
            return cssClass;
        }
        public static string workitems(this IHtmlHelper htmlHelper, string language, string cssClass = "work-items")
        {
            string currentVideo = htmlHelper.ViewContext.RouteData.Values["language"] as string;
            if (currentVideo.Equals("ar"))
            {
                return "work-items-ar";
            }
            return cssClass;
        }
        public static string productitems(this IHtmlHelper htmlHelper, string language, string cssClass = "product-items")
        {
            string currentVideo = htmlHelper.ViewContext.RouteData.Values["language"] as string;
            if (currentVideo.Equals("ar"))
            {
                return "product-items-ar";
            }
            return cssClass;
        }
        public static string productitem(this IHtmlHelper htmlHelper, string language, string cssClass = "packaging-items")
        {
            string currentVideo = htmlHelper.ViewContext.RouteData.Values["language"] as string;
            if (currentVideo.Equals("ar"))
            {
                return "packaging-items-ar";
            }
            return cssClass;
        }
        public static string sliderfor(this IHtmlHelper htmlHelper, string language, string cssClass = "slider-for")
        {
            string currentVideo = htmlHelper.ViewContext.RouteData.Values["language"] as string;
            if (currentVideo.Equals("ar"))
            {
                return "slider-for-ar";
            }
            return cssClass;
        }
        public static string slidernav(this IHtmlHelper htmlHelper, string language, string cssClass = "slider-nav")
        {
            string currentVideo = htmlHelper.ViewContext.RouteData.Values["language"] as string;
            if (currentVideo.Equals("ar"))
            {
                return "slider-nav-ar";
            }
            return cssClass;
        }
        public static string poultryitems(this IHtmlHelper htmlHelper, string language, string cssClass = "poultry-items")
        {
            string currentVideo = htmlHelper.ViewContext.RouteData.Values["language"] as string;
            if (currentVideo.Equals("ar"))
            {
                return "poultry-items-ar";
            }
            return cssClass;
        }
        public static string grid(this IHtmlHelper htmlHelper, string language, string cssClass = "grid")
        {
            string currentVideo = htmlHelper.ViewContext.RouteData.Values["language"] as string;
            if (currentVideo.Equals("ar"))
            {
                return "grid-ar";
            }
            return cssClass;
        }

    }

}

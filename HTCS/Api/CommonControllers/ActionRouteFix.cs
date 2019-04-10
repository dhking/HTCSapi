using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;

namespace API.CommonControllers
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class ActionRouteFixAttribute : Attribute, IDirectRouteFactory, IHttpRouteInfoProvider
    {
        const string TOKEN = "{token}/";
        /// <summary>
        /// 
        /// </summary>
		public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public int Order
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Template
        {
            get;
            private set;
        }
        /// <summary>Initializes a new instance of the <see cref="T:System.Web.Http.RouteAttribute" /> class. </summary>
        public ActionRouteFixAttribute()
        {
            this.Template = TOKEN + string.Empty;
        }
        /// <summary>Initializes a new instance of the <see cref="T:System.Web.Http.RouteAttribute" /> class. </summary>
        /// <param name="template">The route template describing the URI pattern to match against.</param>
        public ActionRouteFixAttribute(string template)
        {
            if (template == null)
            {
                throw new ArgumentNullException("template");
            }
            this.Template = TOKEN + template;
        }
        RouteEntry IDirectRouteFactory.CreateRoute(DirectRouteFactoryContext context)
        {
            IDirectRouteBuilder directRouteBuilder = context.CreateBuilder(this.Template);
            directRouteBuilder.Name = this.Name;
            directRouteBuilder.Order = this.Order;
            return directRouteBuilder.Build();
        }
    }
  
   
  
}

using Microsoft.Owin; 
using Owin;
 
[assembly: OwinStartupAttribute(typeof(HeadManagementSystem.Startup))]

namespace HeadManagementSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        { 
        }
    }
}

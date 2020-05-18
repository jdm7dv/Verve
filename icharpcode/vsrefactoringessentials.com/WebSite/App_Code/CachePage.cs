using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


public static class CachePage
{
    //
    // Inspired by Mads' https://github.com/madskristensen/vswebessentials.com/blob/master/Website/Views/_Layout.cshtml
    //
    public static void WhenNotLocal(HttpContextBase context)
    {
        if (!context.Request.IsLocal)
        {
            context.Response.Cache.SetValidUntilExpires(true);
            context.Response.Cache.SetExpires(DateTime.Now.AddDays(1));
            context.Response.Cache.SetCacheability(HttpCacheability.Public);

            string file = context.Server.MapPath(context.Request.CurrentExecutionFilePath);
            context.Response.AddFileDependency(file);
            context.Response.Cache.SetLastModifiedFromFileDependencies();
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Routing;
using Carrotware.CMS.Core;
using System.Web.Caching;
/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/


namespace Carrotware.CMS.UI.Base {
	public class VirtualDirectory : IRouteHandler {

		public VirtualDirectory(string virtualPath) {
			this.VirtualPath = virtualPath;
		}


		public string VirtualPath { get; private set; }

		private static string ContentKey = "cms_RegisterRoutes";
		public static bool HasRegisteredRoutes {
			get {
				bool c = false;
				try { c = (bool)HttpContext.Current.Cache[ContentKey]; } catch { }
				return c;
			}
			set {
				HttpContext.Current.Cache.Insert(ContentKey, value, null, DateTime.Now.AddMinutes(5), Cache.NoSlidingExpiration);
			}
		}


		public static void RegisterRoutes(RouteCollection routes) {
			RegisterRoutes(routes, false);
		}


		public static void RegisterRoutes(RouteCollection routes, bool OverrideRefresh) {

			try {
				if (!HasRegisteredRoutes || OverrideRefresh) {

					List<string> listFiles = SiteNavHelper.GetSiteDirectoryPaths();
					int iRoute = 0;
					routes.Clear();

					foreach (string fileName in listFiles) {

						VirtualDirectory vd = new VirtualDirectory(fileName);
						Route r = new Route(fileName.Substring(1, fileName.LastIndexOf("/")), vd);

						routes.Add("Route" + iRoute.ToString(), r);

						iRoute++;
					}

					HasRegisteredRoutes = true;
				}

			} catch (Exception ex) {
				//assumption is database is probably empty / needs updating, so trigger the under construction view
				if (DatabaseUpdate.SystemNeedsChecking(ex) || DatabaseUpdate.AreCMSTablesIncomplete()) {
					routes.Clear();
					HasRegisteredRoutes = false;
				} else {
					//something bad has gone down, toss back the error
					throw;
				}
			}
		}



		#region IRouteHandler Members

		public IHttpHandler GetHttpHandler(RequestContext requestContext) {

			IHttpHandler p = new VirtualFileSystem();

			return p;
		}

		#endregion
	}
}
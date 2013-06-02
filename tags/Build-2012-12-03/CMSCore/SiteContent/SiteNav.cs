﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Carrotware.CMS.Data;
using System.Text.RegularExpressions;
/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/


namespace Carrotware.CMS.Core {

	public class SiteNav : IDisposable, ISiteContent {
		private CarrotCMSDataContext db = CarrotCMSDataContext.GetDataContext();
		//private CarrotCMSDataContext db = CompiledQueries.dbConn;


		public SiteNav() {
//#if DEBUG
//            db.Log = new DebugTextWriter();
//#endif
		}


		public ContentPage GetContentPage() {
			ContentPage cp = null;
			if (SiteData.IsPageSampler) {
				cp = ContentPageHelper.GetSamplerView();
			} else {
				using (ContentPageHelper cph = new ContentPageHelper()) {
					cp = cph.FindContentByID(this.SiteID, this.Root_ContentID);
				}
			}
			return cp;
		}


		public string PageTextSummary {
			get {
				string txt = !string.IsNullOrEmpty(PageText) ? PageText : "";

				if (txt.Length > 512) {
					return txt.Substring(0, 500) + "........";
				} else {
					return txt;
				}
			}
		}

		public string PageTextPlainSummary {
			get {
				string txt = !string.IsNullOrEmpty(PageText) ? PageText : "";
				txt = Regex.Replace(txt, @"<(.|\n)*?>", " ");

				if (txt.Length > 512) {
					return txt.Substring(0, 500) + "........";
				} else {
					return txt;
				}
			}
		}


		public string TemplateFolderPath {
			get {
				if (!string.IsNullOrEmpty(TemplateFile)) {
					if (TemplateFile.LastIndexOf("/") >= 2) {
						return TemplateFile.Substring(0, TemplateFile.LastIndexOf("/") + 1);
					} else {
						return "/";
					}
				} else {
					return "/";
				}
			}
		}


		public Guid ContentID { get; set; }
		public DateTime EditDate { get; set; }
		public Guid? EditUserId { get; set; }
		public DateTime CreateDate { get; set; }
		public string NavMenuText { get; set; }
		public int? NavOrder { get; set; }
		public string PageHead { get; set; }
		public string PageText { get; set; }
		public Guid? Parent_ContentID { get; set; }
		public Guid Root_ContentID { get; set; }
		public string TitleBar { get; set; }

		public string FileName { get; set; }
		public bool PageActive { get; set; }
		public Guid SiteID { get; set; }

		public string TemplateFile { get; set; }


		#region IDisposable Members


		public override bool Equals(Object obj) {
			//Check for null and compare run-time types.
			if (obj == null || GetType() != obj.GetType()) return false;
			if (obj is SiteNav) {
				SiteNav p = (SiteNav)obj;
				return (this.ContentID == p.ContentID)
						&& (this.SiteID == p.SiteID)
						&& (this.FileName.ToLower() == p.FileName.ToLower());
			} else {
				return false;
			}
		}

		public override int GetHashCode() {
			return ContentID.GetHashCode() ^ SiteID.GetHashCode() ^ Root_ContentID.GetHashCode() ^ FileName.GetHashCode();
		}


		public void Dispose() {
			if (db != null) {
				db.Dispose();
			}
		}

		#endregion

	}

}
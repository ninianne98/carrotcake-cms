﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using Carrotware.CMS.UI.Base;
using Carrotware.CMS.UI.Controls;
/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/


namespace Carrotware.CMS.UI.Admin.c3_admin {
	public partial class ucSiteMap : AdminBaseUserControl {
		List<ContentPage> lstSite = new List<ContentPage>();


		protected void Page_Load(object sender, EventArgs e) {

			if (!IsPostBack) {

				rpSub.ItemTemplate = rpTop.ItemTemplate;

				lstSite = (from c in pageHelper.GetAllLatestContentList(SiteID)
						   orderby c.TemplateFile
						   select c).ToList();

				var topPages = (from l in lstSite
								orderby l.NavOrder, l.NavMenuText
								where l.Parent_ContentID == null
								select l).ToList();

				GeneralUtilities.BindRepeater(rpTop, topPages);
			}
		}

		public string MakeStar(bool bFlag) {
			if (bFlag) {
				return "";
			} else {
				return "*";
			}
		}


		public string ReturnNavImage(bool bFlag) {
			if (bFlag) {
				return hdnNavShow.Value;
			} else {
				return hdnNavHide.Value;
			}
		}

		public string ReturnImage(bool bFlag) {
			if (bFlag) {
				return hdnActive.Value;
			} else {
				return hdnInactive.Value;
			}
		}

		protected void rpMap_ItemDataBound(object sender, RepeaterItemEventArgs e) {
			PlaceHolder ph = (PlaceHolder)e.Item.FindControl("ph");

			if (ph != null) {
				var d = (ContentPage)e.Item.DataItem;
				var lst = (from l in lstSite
						   orderby l.NavOrder, l.NavMenuText
						   where l.Parent_ContentID == d.Root_ContentID
						   select l).ToList();

				if (lst.Count > 0) {
					Repeater rp = new Repeater();

					rp.HeaderTemplate = rpSub.HeaderTemplate;
					rp.ItemTemplate = rpSub.ItemTemplate;
					rp.FooterTemplate = rpSub.FooterTemplate;
					ph.Controls.Add(rp);

					rp.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.rpMap_ItemDataBound);

					GeneralUtilities.BindRepeater(rp, lst);
				}
			}

		}

		protected void btnSave_Click(object sender, EventArgs e) {
			using (SiteMapOrderHelper orderHelper = new SiteMapOrderHelper()) {
				var lst = orderHelper.CreateSiteMapList(txtMap.Text);
				orderHelper.UpdateSiteMap(SiteID, lst);
			}

			Response.Redirect(SiteData.CurrentScriptName);
		}

		protected void btnFixOrphan_Click(object sender, EventArgs e) {
			using (SiteMapOrderHelper orderHelper = new SiteMapOrderHelper()) {
				orderHelper.FixOrphanPages(SiteID);
			}

			Response.Redirect(SiteData.CurrentScriptName);
		}
	}
}
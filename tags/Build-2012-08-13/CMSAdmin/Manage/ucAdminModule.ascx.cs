﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.UI.Base;
using Carrotware.CMS.Core;
using Carrotware.CMS.Interface;
/*
* CarrotCake CMS
* http://carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.UI.Admin {
	public partial class ucAdminModule : BaseUserControl {

		Guid ModuleID = Guid.Empty;
		string pf = string.Empty;

		public string SelMenu = "0";

		public bool UseAjax { get; set; }

		protected bool bLoadModule = false;

		protected void Page_Load(object sender, EventArgs e) {
			if (!IsPostBack) {
				if (cmsHelper.AdminModules.Count > 0) {
					rpModuleList.DataSource = cmsHelper.AdminModules;
					rpModuleList.DataBind();
				} else {
					rpModuleList.Visible = false;
				}
			}

			if (ModuleID != Guid.Empty) {
				pnlSetter.Visible = true;
				int x = 0;
				foreach (var row in cmsHelper.AdminModules) {
					if (ModuleID.ToString().ToLower() == row.PluginID.ToString().ToLower()) {
						SelMenu = x.ToString();
						break;
					}
					x++;
				}
			}
		}


		protected override void OnInit(EventArgs e) {
			if (!bLoadModule) {
				LoadModule();
			}
			base.OnInit(e);
		}


		public void LoadModule() {

			if (Request.QueryString["pi"] != null) {
				try { ModuleID = new Guid(Request.QueryString["pi"].ToString()); } catch { }
			}

			if (Request.QueryString["pf"] != null) {
				pf = Request.QueryString["pf"].ToString();

				CMSAdminModule mod = (from m in cmsHelper.AdminModules
									  where m.PluginID == ModuleID
									  select m).FirstOrDefault();

				var cc = (from m in mod.PluginMenus
						  where m.PluginParm == pf
						  select m).FirstOrDefault();

				UseAjax = cc.UseAjax;

				Control c = Page.LoadControl(cc.ControlFile);
				phAdminModule.Controls.Add(c);

				if (c is IAdminModule) {
					var w = (IAdminModule)c;
					w.SiteID = SiteID;
					w.ModuleID = ModuleID;
					w.ModuleName = pf;
					w.QueryStringFragment = "pf=" + pf + "&pi=" + ModuleID.ToString();
					w.QueryStringPattern = "pf={0}&pi=" + ModuleID.ToString();
				}
			}

			bLoadModule = true;
		}

		protected string MarkSelected(string sID, string sParm) {

			if (sID == ModuleID.ToString().ToLower() && sParm == pf) {
				return " class=\"selectedModule\" ";
			} else {
				return " ";
			}


		}

		protected void rpModuleList_ItemDataBound(object sender, RepeaterItemEventArgs e) {

			Repeater rpModuleContents = (Repeater)e.Item.FindControl("rpModuleContents");
			HiddenField hdnID = (HiddenField)e.Item.FindControl("hdnID");

			if (rpModuleContents != null) {
				var d = (CMSAdminModule)e.Item.DataItem;

				rpModuleContents.DataSource = d.PluginMenus.Where(z => z.IsVisible == true).OrderBy(x => x.SortOrder).ToList();
				rpModuleContents.DataBind();

			}
		}



	}
}
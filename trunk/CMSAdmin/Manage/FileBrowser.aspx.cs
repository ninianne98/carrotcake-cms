﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.UI.Base;
using Carrotware.CMS.Core;

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
	public partial class FileBrowser : AdminBasePage {
		public string sQueryPath = "";
		public string sQueryMode = "1";
		protected string sMapPath = "";

		protected FileDataHelper helpFile = new FileDataHelper();

		protected void Page_Load(object sender, EventArgs e) {

			sQueryPath = Request.QueryString["fldrpath"];
			try { sQueryMode = Request.QueryString["useTiny"]; } catch { }

			if (sQueryMode != "1") {
				sQueryMode = "0";
				pnlTiny.Visible = false;
				pnlTiny2.Visible = false;
				litFileMgr.Visible = true;
			} else {
				sQueryMode = "1";
				litFileMgr.Visible = false;
			}

			if (!string.IsNullOrEmpty(sQueryPath)) {
				if (sQueryPath.Length == 1) {
					sQueryPath = "";
				}
			}

			if (string.IsNullOrEmpty(sQueryPath)) {
				sMapPath = SetSitePath("/");
				sQueryPath = "/";
				lnkUp.Visible = false;
			} else {
				sMapPath = SetSitePath(sQueryPath);
				lnkUp.Visible = true;
			}
			sQueryPath = sQueryPath.StartsWith(@"/") ? sQueryPath : @"/" + sQueryPath;
			sQueryPath.Replace(@"//", @"/").Replace(@"//", @"/");

			if (lnkUp.Visible) {
				lnkUp.NavigateUrl = Request.ServerVariables["script_name"] + "?useTiny=" + sQueryMode + "&fldrpath=" + sQueryPath.Substring(0, sQueryPath.Substring(0, sQueryPath.Length - 2).LastIndexOf('/')) + @"/";
			}
			if (!IsPostBack) {
				LoadLists();
			}
			lblPath.Text = sQueryPath;
		}

		protected void LoadLists() {
			var fldr = helpFile.GetFolders(sMapPath, sQueryPath);
			var fls = helpFile.GetFiles(sMapPath, sQueryPath);

			rpFolders.DataSource = fldr;
			rpFolders.DataBind();

			rpFiles.DataSource = fls;
			rpFiles.DataBind();
		}

		public string CreateFileLink(string sPath) {
			return string.Format("javascript:SetFile('{0}');", sPath);
		}


		protected void btnUpload_Click(object sender, EventArgs e) {
			if (upFile.HasFile) {
				var sPath = SetSitePath(sQueryPath);
				lblWarning.Text = "";

				if ((from b in helpFile.BlockedTypes
					 where upFile.FileName.ToLower().Contains("." + b.ToLower())
					 select b).Count() < 1) {

					upFile.SaveAs(Path.Combine(sPath, upFile.FileName));
					lblWarning.Text = upFile.FileName + " uploaded!";
					lblWarning.Attributes["style"] = "color: #009900;";
				} else {
					lblWarning.Text = upFile.FileName + " is a blocked filetype";
					lblWarning.Attributes["style"] = "color: #990000;";
				}

				LoadLists();
			}
		}

		protected void btnRemove_Click(object sender, EventArgs e) {
			var sPath = SetSitePath(sQueryPath);
			foreach (RepeaterItem f in rpFiles.Items) {
				CheckBox chkRemove = (CheckBox)f.FindControl("chkRemove");
				if (chkRemove != null) {
					if (chkRemove.Checked) {
						var fname = chkRemove.Attributes["value"];
						File.Delete(Path.Combine(sPath, fname));
					}
				}
			}
			LoadLists();
		}

		protected string SetSitePath(string sPath) {
			var wwwpath = Server.MapPath("~/");
			//var wwwpath = Server.MapPath("/");
			string _path = wwwpath + "/" + sPath;
			_path = _path.Replace(@"\", @"/").Replace(@"//", @"/");
			return _path;
		}

	}
}

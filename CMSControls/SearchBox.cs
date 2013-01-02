﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using Carrotware.CMS.Interface;
using Carrotware.Web.UI.Controls;
/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/


namespace Carrotware.CMS.UI.Controls {
	[ToolboxData("<{0}:SearchBox runat=server></{0}:SearchBox>")]
	public class SearchBox : BaseServerControl, INamingContainer {

		[PersistenceMode(PersistenceMode.InnerProperty)]
		[TemplateInstance(TemplateInstance.Single)]
		[MergableProperty(false)]
		[Browsable(false)]
		[TemplateContainer(typeof(SearchBox))]
		public ITemplate SearchTemplate { get; set; }

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string OverrideTextboxName {
			get {
				String s = (String)ViewState["OverrideTextboxName"];
				return ((s == null) ? String.Empty : s);
			}
			set {
				ViewState["OverrideTextboxName"] = value;
			}
		}

		protected PlaceHolder phEntry = new PlaceHolder();
		protected List<Control> EntryFormControls = new List<Control>();

		protected string JS_SearchName {
			get {
				return "CarrotCakeSiteSearch_" + this.ClientID;
			}
		}
		protected string JS_EnterSearch {
			get {
				return "CarrotCakeSiteSearchEnter_" + this.ClientID;
			}
		}

		protected override void OnInit(EventArgs e) {

			base.OnInit(e);

			if (SearchTemplate == null) {
				SearchTemplate = new DefaultSearchBoxForm();
			}

		}

		protected override void Render(HtmlTextWriter writer) {
			base.BaseRender(writer);
		}

		protected override void RenderContents(HtmlTextWriter writer) {
			base.BaseRenderContents(writer);
		}


		protected override void CreateChildControls() {

			if (SearchTemplate != null) {
				this.Controls.Clear();
			}

			phEntry.Visible = true;
			phEntry.Controls.Clear();
			if (SearchTemplate != null) {
				SearchTemplate.InstantiateIn(phEntry);
			}

			FindEntryFormCtrls(phEntry);

			phEntry.Controls.Add(new jsHelperLib());

			TextBox txtSearchText = null;
			if (string.IsNullOrEmpty(OverrideTextboxName)) {
				txtSearchText = (TextBox)GetEntryFormControl("SearchText");

				if (txtSearchText == null) {
					txtSearchText = (TextBox)GetEntryFormControl(typeof(TextBox));
				}
			} else {
				txtSearchText = new TextBox();
				txtSearchText.ID = "over_" + OverrideTextboxName;
			}

			string sScript = ControlUtilities.GetManifestResourceStream("Carrotware.CMS.UI.Controls.SearchBoxJS.txt");

			if (txtSearchText != null) {
				sScript = sScript.Replace("{SEARCH_FUNC}", JS_SearchName);
				sScript = sScript.Replace("{SEARCH_ENTERFUNC}", JS_EnterSearch);

				if (string.IsNullOrEmpty(OverrideTextboxName)) {
					sScript = sScript.Replace("{SEARCH_TEXT}", this.ClientID + "_" + txtSearchText.ID);
				} else {
					sScript = sScript.Replace("{SEARCH_TEXT}", OverrideTextboxName);
				}

				sScript = sScript.Replace("{SEARCH_URL}", SiteData.CurrentSite.SiteSearchPath);

				phEntry.Controls.Add(new Literal { Text = sScript });
			}

			this.Controls.Add(phEntry);

			base.CreateChildControls();
		}


		protected Control GetEntryFormControl(string ControlName) {

			return (from x in EntryFormControls
					where x.ID != null
					&& x.ID.ToLower() == ControlName.ToLower()
					select x).FirstOrDefault();
		}

		protected Control GetEntryFormControl(Type type) {

			return (from x in EntryFormControls
					where x.ID != null
					&& x.GetType() == type
					select x).FirstOrDefault();
		}

		private void FindEntryFormCtrls(Control X) {

			foreach (Control c in X.Controls) {
				EntryFormControls.Add(c);

				if (c is LiteralControl) {
					LiteralControl z = (LiteralControl)c;
					z.Text = z.Text.Replace("{EXEC_SEARCH_FUNCTION}", "return " + JS_SearchName + "()");
					z.Text = z.Text.Replace("{EXEC_SEARCH_FUNCTION_ENTER}", "return " + JS_EnterSearch + "(event)");
				}

				if (c is TextBox && c.ID != null) {
					TextBox z = (TextBox)c;
					if (z.ID.ToLower().Contains("search")) {
						z.Attributes["onkeypress"] = "return " + JS_EnterSearch + "()";
					}
				}

				if (c is Button && c.ID != null) {
					Button z = (Button)c;
					if (z.ID.ToLower().Contains("search")) {
						z.OnClientClick = "return " + JS_SearchName + "()";
					}
				}

				FindEntryFormCtrls(c);
			}
		}

	}
}
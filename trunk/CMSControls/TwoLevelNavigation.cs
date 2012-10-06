﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
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

	[ToolboxData("<{0}:TwoLevelNavigation runat=server></{0}:TwoLevelNavigation>")]
	public class TwoLevelNavigation : BaseServerControl {

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string OverrideCSS {
			get {
				string s = (string)ViewState["OverrideCSS"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["OverrideCSS"] = value;
			}
		}


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string CSSSelected {
			get {
				string s = (string)ViewState["CSSSelected"];
				return ((s == null) ? "selected" : s);
			}
			set {
				ViewState["CSSSelected"] = value;
			}
		}


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string CSSULClassTop {
			get {
				string s = (string)ViewState["ULClassTop"];
				return ((s == null) ? "parent" : s);
			}
			set {
				ViewState["ULClassTop"] = value;
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string CSSULClassLower {
			get {
				string s = (string)ViewState["ULClassLower"];
				return ((s == null) ? "children" : s);
			}
			set {
				ViewState["ULClassLower"] = value;
			}
		}

		[DefaultValue(false)]
		[Themeable(false)]
		public override bool EnableViewState {
			get {
				String s = (String)ViewState["EnableViewState"];
				bool b = ((s == null) ? false : Convert.ToBoolean(s));
				base.EnableViewState = b;
				return b;
			}

			set {
				ViewState["EnableViewState"] = value.ToString();
				base.EnableViewState = value;
			}
		}


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue(false)]
		[Localizable(true)]
		public bool AutoStylingDisabled {
			get {
				String s = (String)ViewState["AutoStylingDisabled"];
				return ((s == null) ? false : Convert.ToBoolean(s));
			}

			set {
				ViewState["AutoStylingDisabled"] = value.ToString();
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public Unit MenuWidth {
			get {
				Unit s = new Unit("940px");
				try { s = new Unit(ViewState["MenuWidth"].ToString()); } catch { }
				return s;
			}
			set {
				ViewState["MenuWidth"] = value;
			}
		}


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public Unit FontSize {
			get {
				Unit s = new Unit("14px");
				try { s = new Unit(ViewState["FontSize"].ToString()); } catch { }
				return s;
			}
			set {
				ViewState["FontSize"] = value;
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public Unit MenuHeight {
			get {
				Unit s = new Unit("60px");
				try { s = new Unit(ViewState["MenuHeight"].ToString()); } catch { }
				return s;
			}
			set {
				ViewState["MenuHeight"] = value;
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public Unit SubMenuWidth {
			get {
				Unit s = new Unit("300px");
				try { s = new Unit(ViewState["SubMenuWidth"].ToString()); } catch { }
				return s;
			}
			set {
				ViewState["SubMenuWidth"] = value;
			}
		}


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string TopBackgroundStyle {
			get {
				String s = (String)ViewState["TopBackgroundStyle"];
				return ((s == null) ? String.Empty : s);
			}
			set {
				ViewState["TopBackgroundStyle"] = value;
			}
		}


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string ItemBackgroundStyle {
			get {
				String s = (String)ViewState["ItemBackgroundStyle"];
				return ((s == null) ? String.Empty : s);
			}
			set {
				ViewState["ItemBackgroundStyle"] = value;
			}
		}


		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public override Color ForeColor {
			get {
				string s = (string)ViewState["ForeColor"];
				return ((s == null) ? ColorTranslator.FromHtml("#758569") : ColorTranslator.FromHtml(s));
			}
			set {
				ViewState["ForeColor"] = ColorTranslator.ToHtml(value);
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public override Color BackColor {
			get {
				string s = (string)ViewState["BackColor"];
				return ((s == null) ? ColorTranslator.FromHtml("#DDDDDD") : ColorTranslator.FromHtml(s));
			}
			set {
				ViewState["BackColor"] = ColorTranslator.ToHtml(value);
			}
		}

		private List<SiteNav> lstTwoLevelNav = new List<SiteNav>();

		protected List<SiteNav> GetTopNav() {
			return lstTwoLevelNav.Where(ct => ct.Parent_ContentID == null).OrderBy(ct => ct.NavMenuText).OrderBy(ct => ct.NavOrder).ToList();
		}

		protected List<SiteNav> GetChildren(Guid rootContentID) {
			return lstTwoLevelNav.Where(ct => ct.Parent_ContentID == rootContentID).OrderBy(ct => ct.NavMenuText).OrderBy(ct => ct.NavOrder).ToList();
		}

		protected void LoadData() {

			lstTwoLevelNav = navHelper.GetTwoLevelNavigation(SiteData.CurrentSiteID, !SecurityData.IsAuthEditor);

		}

		protected override void OnInit(EventArgs e) {
			this.Controls.Clear();

			base.OnInit(e);

			LoadData();
		}


		protected override void RenderContents(HtmlTextWriter output) {

			int indent = output.Indent;
			output.Indent = indent + 3;

			List<SiteNav> lstNav = GetTopNav();
			SiteNav pageNav = GetParentPage();

			string sParent = pageNav.FileName.ToLower();

			string sCSS = "";
			if (!string.IsNullOrEmpty(CssClass)) {
				sCSS = string.Format(" class=\"{0}\"", CssClass);
			}

			output.WriteLine();
			output.WriteLine("<div" + sCSS + " id=\"" + this.ClientID + "\">");
			output.Indent++;
			output.WriteLine("<div id=\"" + this.ClientID + "-inner\">");
			output.Indent++;
			output.WriteLine("<ul class=\"" + CSSULClassTop + "\">");
			output.Indent++;

			int indent2 = output.Indent;

			foreach (SiteNav c1 in lstNav) {
				output.Indent = indent2;
				List<SiteNav> cc = GetChildren(c1.Root_ContentID);
				IdentifyLinkAsInactive(c1);
				if (SiteData.IsFilenameCurrentPage(c1.FileName) || AreFilenamesSame(c1.FileName, sParent)) {
					output.WriteLine("<li class=\"level-1 " + CSSSelected + "\"><a href=\"" + c1.FileName + "\">" + c1.NavMenuText + "</a>");
				} else {
					output.WriteLine("<li class=\"level-1\"><a href=\"" + c1.FileName + "\">" + c1.NavMenuText + "</a>");
				}

				output.Indent++;
				if (cc.Count > 0) {
					int indent3 = output.Indent;
					output.WriteLine("<ul class=\"" + CSSULClassLower + "\">");
					output.Indent++;
					foreach (SiteNav c2 in cc) {
						IdentifyLinkAsInactive(c2);
						if (SiteData.IsFilenameCurrentPage(c2.FileName)) {
							output.WriteLine("<li class=\"level-2 " + CSSSelected + "\"><a href=\"" + c2.FileName + "\">" + c2.NavMenuText + "</a></li>");
						} else {
							output.WriteLine("<li class=\"level-2\"><a href=\"" + c2.FileName + "\">" + c2.NavMenuText + "</a></li>");
						}
					}
					output.Indent = indent3;
					output.WriteLine("</ul>");
				}
				output.Indent--;
				output.WriteLine("</li>");
			}

			output.Indent--;
			output.WriteLine("</ul>");
			output.Indent--;
			output.WriteLine("</div>");
			output.Indent--;
			output.WriteLine("</div>");
			output.WriteLine();

			output.Indent = indent;
		}


		protected override void OnPreRender(EventArgs e) {
			try {


				if (PublicParmValues.Count > 0) {

					string sTmp = "";

					OverrideCSS = GetParmValue("OverrideCSS", "");

					sTmp = GetParmValue("CSSSelected", "");
					if (!string.IsNullOrEmpty(sTmp)) {
						CSSSelected = sTmp;
					}

					sTmp = GetParmValue("CSSULClassTop", "");
					if (!string.IsNullOrEmpty(sTmp)) {
						CSSULClassTop = sTmp;
					}

					sTmp = GetParmValue("CSSULClassLower", "");
					if (!string.IsNullOrEmpty(sTmp)) {
						CSSULClassLower = sTmp;
					}

					sTmp = GetParmValue("MenuWidth", "");
					if (!string.IsNullOrEmpty(sTmp)) {
						MenuWidth = new Unit(sTmp);
					}

					sTmp = GetParmValue("MenuHeight", "");
					if (!string.IsNullOrEmpty(sTmp)) {
						MenuHeight = new Unit(sTmp);
					}

					sTmp = GetParmValue("SubMenuWidth", "");
					if (!string.IsNullOrEmpty(sTmp)) {
						SubMenuWidth = new Unit(sTmp);
					}

					sTmp = GetParmValue("FontSize", "");
					if (!string.IsNullOrEmpty(sTmp)) {
						FontSize = new Unit(sTmp);
					}

					sTmp = GetParmValue("ForeColor", "");
					if (!string.IsNullOrEmpty(sTmp)) {
						ForeColor = ColorTranslator.FromHtml(sTmp);
					}

					sTmp = GetParmValue("BackColor", "");
					if (!string.IsNullOrEmpty(sTmp)) {
						BackColor = ColorTranslator.FromHtml(sTmp);
					}
				}
			} catch (Exception ex) {
			}


			if (string.IsNullOrEmpty(OverrideCSS) && !AutoStylingDisabled) {
				string sCSS = String.Empty;
				string sCSS7 = String.Empty;

				Assembly _assembly = Assembly.GetExecutingAssembly();

				using (StreamReader oTextStream = new StreamReader(_assembly.GetManifestResourceStream("Carrotware.CMS.UI.Controls.TopMenu.txt"))) {
					sCSS = oTextStream.ReadToEnd();
				}

				using (StreamReader oTextStream = new StreamReader(_assembly.GetManifestResourceStream("Carrotware.CMS.UI.Controls.TopMenu7.txt"))) {
					sCSS7 = oTextStream.ReadToEnd();
				}

				sCSS = sCSS.Replace("{FORE_HEX}", ColorTranslator.ToHtml(ForeColor));
				sCSS = sCSS.Replace("{BG_HEX}", ColorTranslator.ToHtml(BackColor));

				sCSS7 = sCSS7.Replace("{FORE_HEX}", ColorTranslator.ToHtml(ForeColor));
				sCSS7 = sCSS7.Replace("{BG_HEX}", ColorTranslator.ToHtml(BackColor));

				sCSS = sCSS.Replace("{MENU_WIDTH}", MenuWidth.Value.ToString() + "px");
				sCSS7 = sCSS7.Replace("{MENU_WIDTH}", MenuWidth.Value.ToString() + "px");

				sCSS = sCSS.Replace("{MENU_HEIGHT}", MenuHeight.Value.ToString() + "px");
				sCSS7 = sCSS7.Replace("{MENU_HEIGHT}", MenuHeight.Value.ToString() + "px");

				sCSS = sCSS.Replace("{FONT_SIZE}", FontSize.Value.ToString() + "px");
				sCSS7 = sCSS7.Replace("{FONT_SIZE}", FontSize.Value.ToString() + "px");

				sCSS = sCSS.Replace("{LINK_PAD1}", Convert.ToInt16(FontSize.Value * .45).ToString() + "px");
				sCSS7 = sCSS7.Replace("{LINK_PAD1}", Convert.ToInt16(FontSize.Value * .45).ToString() + "px");

				sCSS = sCSS.Replace("{LINK_PAD2}", Convert.ToInt16(FontSize.Value * 1.40).ToString() + "px");
				sCSS7 = sCSS7.Replace("{LINK_PAD2}", Convert.ToInt16(FontSize.Value * 1.40).ToString() + "px");

				sCSS = sCSS.Replace("{SUB_MENU_WIDTH}", SubMenuWidth.Value.ToString() + "px");
				sCSS7 = sCSS7.Replace("{SUB_MENU_WIDTH}", SubMenuWidth.Value.ToString() + "px");

				sCSS = sCSS.Replace("{MENU_LINE_HEIGHT}", Convert.ToInt16(MenuHeight.Value / 3).ToString() + "px");
				sCSS7 = sCSS7.Replace("{MENU_LINE_HEIGHT}", Convert.ToInt16(MenuHeight.Value / 3).ToString() + "px");

				sCSS = sCSS.Replace("{MENU_TOP_PAD}", Convert.ToInt16((MenuHeight.Value + 5) / 4).ToString() + "px");
				sCSS7 = sCSS7.Replace("{MENU_TOP_PAD}", Convert.ToInt16((MenuHeight.Value + 5) / 4).ToString() + "px");

				sCSS = sCSS.Replace("{MENU_TOP_2_PAD}", Convert.ToInt16(FontSize.Value * .5).ToString() + "px");
				sCSS7 = sCSS7.Replace("{MENU_TOP_2_PAD}", Convert.ToInt16(FontSize.Value * .5).ToString() + "px");

				sCSS = sCSS.Replace("{MENU_TOP_3_PAD}", Convert.ToInt16(FontSize.Value * .8).ToString() + "px");
				sCSS7 = sCSS7.Replace("{MENU_TOP_3_PAD}", Convert.ToInt16(FontSize.Value * .8).ToString() + "px");

				sCSS = sCSS.Replace("{MENU_SELECT_CLASS}", CSSSelected);
				sCSS7 = sCSS7.Replace("{MENU_SELECT_CLASS}", CSSSelected);

				sCSS = sCSS.Replace("{MENU_UL_TOP}", CSSULClassTop);
				sCSS7 = sCSS7.Replace("{MENU_UL_TOP}", CSSULClassTop);

				sCSS = sCSS.Replace("{MENU_UL_LOWER}", CSSULClassLower);
				sCSS7 = sCSS7.Replace("{MENU_UL_LOWER}", CSSULClassLower);

				if (!string.IsNullOrEmpty(TopBackgroundStyle)) {
					TopBackgroundStyle = TopBackgroundStyle.Replace(";", "");
					sCSS = sCSS.Replace("{TOP_BACKGROUND_STYLE}", "background: " + TopBackgroundStyle + ";");
					sCSS7 = sCSS7.Replace("{TOP_BACKGROUND_STYLE}", "background: " + TopBackgroundStyle + ";");
				} else {
					sCSS = sCSS.Replace("{TOP_BACKGROUND_STYLE}", "");
					sCSS7 = sCSS7.Replace("{TOP_BACKGROUND_STYLE}", "");
				}

				if (!string.IsNullOrEmpty(ItemBackgroundStyle)) {
					ItemBackgroundStyle = ItemBackgroundStyle.Replace(";", "");
					sCSS = sCSS.Replace("{ITEM_BACKGROUND_STYLE}", "background: " + ItemBackgroundStyle + ";");
					sCSS7 = sCSS7.Replace("{ITEM_BACKGROUND_STYLE}", "background: " + ItemBackgroundStyle + ";");
				} else {
					sCSS = sCSS.Replace("{ITEM_BACKGROUND_STYLE}", "");
					sCSS7 = sCSS7.Replace("{ITEM_BACKGROUND_STYLE}", "");
				}

				sCSS = sCSS.Replace("{MENU_ID}", "#" + this.ClientID + "-inner");
				sCSS = sCSS.Replace("{MENU_WRAPPER_ID}", "#" + this.ClientID + "");
				sCSS = "<style type=\"text/css\">" + sCSS + "</style>";

				sCSS7 = sCSS7.Replace("{MENU_ID}", "#" + this.ClientID + "-inner");
				sCSS7 = sCSS7.Replace("{MENU_WRAPPER_ID}", "#" + this.ClientID + "");
				sCSS7 = "<!--[if IE 7]> <style type=\"text/css\">" + sCSS7 + "</style> <![endif]-->";

				Literal link = new Literal();
				link.Text = sCSS;
				Page.Header.Controls.Add(link);
				Literal link7 = new Literal();
				link7.Text = sCSS7;
				Page.Header.Controls.Add(link7);
			} else {
				if (!string.IsNullOrEmpty(OverrideCSS)) {
					HtmlLink link = new HtmlLink();
					link.Href = OverrideCSS;
					link.Attributes.Add("rel", "stylesheet");
					link.Attributes.Add("type", "text/css");
					Page.Header.Controls.Add(link);
				}
			}


			base.OnPreRender(e);
		}



	}
}

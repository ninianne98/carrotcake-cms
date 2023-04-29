﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;
using Carrotware.CMS.Interface;
using Carrotware.Web.UI.Controls;
using System.Web;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.UI.Controls {

	public abstract class BaseNavHeaded : BaseNav, IHeadedList, IWidgetLimitedProperties {

		public BaseNavHeaded()
			: base() {
			this.ItemCount = -1;
			this.MetaDataTitle = string.Empty;
			this.HeadWrapTag = TagType.H2;
		}

		public int ItemCount { get; set; }

		[Category("Appearance")]
		[DefaultValue("")]
		public string MetaDataTitle {
			get {
				string s = (string)ViewState["MetaDataTitle"];
				return ((s == null) ? "" : s);
			}
			set {
				ViewState["MetaDataTitle"] = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue("H2")]
		[Widget(WidgetAttribute.FieldMode.DropDownList, "lstTagType")]
		public TagType HeadWrapTag {
			get {
				String s = (String)ViewState["HeadWrapTag"];
				TagType c = TagType.H2;
				if (!string.IsNullOrEmpty(s)) {
					c = (TagType)Enum.Parse(typeof(TagType), s, true);
				}
				return c;
			}

			set {
				ViewState["HeadWrapTag"] = value.ToString();
			}
		}

		[Widget(WidgetAttribute.FieldMode.DictionaryList)]
		public Dictionary<string, string> lstTagType {
			get {
				Dictionary<string, string> _dict = new Dictionary<string, string>();

				_dict = EnumHelper.ToList<TagType>().OrderBy(x => x.Text).ToDictionary(k => k.Text, v => v.Description);

				return _dict;
			}
		}

		protected override void WriteListPrefix(HtmlTextWriter output) {
			if (this.NavigationData != null) {
				this.ItemCount = this.NavigationData.Count;
			}

			if (this.ItemCount > 0 && !string.IsNullOrEmpty(this.MetaDataTitle)) {
				var head = new HtmlTag(this.HeadWrapTag.ToString());
				head.MergeAttribute("class", "meta-caption");
				head.InnerHtml = HttpUtility.HtmlEncode(this.MetaDataTitle);
				output.WriteLine(head.RenderTag());
			}

			base.WriteListPrefix(output);
		}

		public virtual List<string> LimitedPropertyList {
			get {
				List<string> lst = new List<string>();
				lst.Add("MetaDataTitle");
				lst.Add("HeadWrapTag");

				return lst.Distinct().ToList();
			}
		}

		protected override void OnPreRender(System.EventArgs e) {
			if (this.PublicParmValues.Any()) {
				string sTmp = "";
				try {
					sTmp = GetParmValue("MetaDataTitle", "");
					if (!string.IsNullOrEmpty(sTmp)) {
						this.MetaDataTitle = sTmp;
					}

					sTmp = GetParmValue("HeadWrapTag", TagType.H2.ToString());
					if (!string.IsNullOrEmpty(sTmp)) {
						this.HeadWrapTag = (TagType)Enum.Parse(typeof(TagType), sTmp, true);
					}
				} catch (Exception ex) {
				}
			}

			base.OnPreRender(e);
		}
	}
}
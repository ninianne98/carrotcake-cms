﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/

namespace Carrotware.Web.UI.Controls {

	class CarrotSortButtonHeaderTemplate : ITemplate {
		private string _cmd { get; set; }
		private string _text { get; set; }
		private string _lnkname { get; set; }
		private int _idx { get; set; }

		public CarrotSortButtonHeaderTemplate(string lnkname, int idx, string text, string cmd) {
			_text = text;
			_cmd = cmd;
			_idx = idx;
			_lnkname = lnkname;
		}

		public CarrotSortButtonHeaderTemplate(string lnkname, string text, string cmd) {
			_text = text;
			_cmd = cmd;
			_idx = -1;
			_lnkname = lnkname;
		}

		public CarrotSortButtonHeaderTemplate(string text, string cmd) {
			_text = text;
			_cmd = cmd;
			_idx = -1;
			_lnkname = "lnkHead";
		}

		public void InstantiateIn(Control container) {
			var x = container.Parent;
			if (!string.IsNullOrEmpty(_cmd)) {

				PlaceHolder lit = new PlaceHolder();
				lit.DataBinding += new EventHandler(litContent_DataBinding);
				container.Controls.Add(lit);

			} else {
				Literal lit = new Literal();
				lit.Text = _text;
				container.Controls.Add(lit);
			}
		}

		private void litContent_DataBinding(object sender, EventArgs e) {
			PlaceHolder ph = (PlaceHolder)sender;

			LinkButton lb = new LinkButton();
			lb.CommandName = _cmd;
			if (_idx >= 0) {
				lb.ID = _lnkname + "_" + _idx.ToString();
			} else {
				string[] IDs = ph.ClientID.Split('_');
				lb.ID = _lnkname + "_" + IDs[IDs.Length - 1];
			}
			lb.Text = _text;

			ph.Controls.Add(lb);
		}

	}

	//=========================================

	public class CarrotAutoItemTemplate : ITemplate {
		private string _format { get; set; }
		private string _field { get; set; }

		public CarrotAutoItemTemplate(string fieldParm, string formatPattern) {
			_format = formatPattern;
			_field = fieldParm;
		}

		public void InstantiateIn(Control container) {

			Literal litContent = new Literal();
			litContent.Text = _field;

			litContent.DataBinding += new EventHandler(litContent_DataBinding);

			container.Controls.Add(litContent);
		}

		private void litContent_DataBinding(object sender, EventArgs e) {
			Literal litContent = (Literal)sender;
			GridViewRow container = (GridViewRow)litContent.NamingContainer;

			object oValue = DataBinder.Eval(container, "DataItem." + _field);

			litContent.Text = String.Format(_format, oValue);
		}

	}



	//=========================================

	public class CarrotBooleanImageItemTemplate : ITemplate {
		private string _imageTrue { get; set; }
		private string _imageFalse { get; set; }
		private string _verbiageTrue { get; set; }
		private string _verbiageFalse { get; set; }
		private string _field { get; set; }
		private string _css { get; set; }

		public CarrotBooleanImageItemTemplate(string fieldParm, string cssStyle) {

			SetImage();

			_field = fieldParm;
			_css = cssStyle;

			SetVerbiage();
		}

		public void SetImage() {
			_imageTrue = BaseWebControl.GetWebResourceUrl(this.GetType(), "Carrotware.Web.UI.Controls.CarrotGridView.accept.png");
			_imageFalse = BaseWebControl.GetWebResourceUrl(this.GetType(), "Carrotware.Web.UI.Controls.CarrotGridView.cancel.png");
		}

		public void SetVerbiage(string imageTextTrue, string imageTextFalse) {
			_verbiageTrue = imageTextTrue;
			_verbiageFalse = imageTextFalse;
		}

		public void SetImage(string imageNameTrue, string imageNameFalse) {
			_imageTrue = imageNameTrue;
			_imageFalse = imageNameFalse;
		}

		public void SetVerbiage() {
			_verbiageTrue = "Active";
			_verbiageFalse = "Inactive";
		}

		public CarrotBooleanImageItemTemplate(string fieldParm, string cssStyle, string imageNameTrue, string imageNameFalse) {
			_imageTrue = imageNameTrue;
			_imageFalse = imageNameFalse;

			_field = fieldParm;
			_css = cssStyle;

			SetVerbiage();
		}

		public CarrotBooleanImageItemTemplate(string fieldParm, string cssStyle, string imageNameTrue, string imageNameFalse, string imageTextTrue, string imageTextFalse) {
			_imageTrue = imageNameTrue;
			_imageFalse = imageNameFalse;

			_field = fieldParm;
			_css = cssStyle;

			_verbiageTrue = imageTextTrue;
			_verbiageFalse = imageTextFalse;
		}

		public void InstantiateIn(Control container) {

			Image imgBool = new Image();
			if (!string.IsNullOrEmpty(_css)) {
				imgBool.CssClass = _css;
			}
			imgBool.AlternateText = _field;
			imgBool.ImageUrl = "noimage.png";

			imgBool.DataBinding += new EventHandler(imgBool_DataBinding);

			container.Controls.Add(imgBool);
		}

		private void imgBool_DataBinding(object sender, EventArgs e) {
			Image imgBool = (Image)sender;
			GridViewRow container = (GridViewRow)imgBool.NamingContainer;

			bool bValue = Convert.ToBoolean(DataBinder.Eval(container, "DataItem." + _field).ToString());

			if (bValue) {
				imgBool.ImageUrl = _imageTrue;
				imgBool.AlternateText = _verbiageTrue;
			} else {
				imgBool.ImageUrl = _imageFalse;
				imgBool.AlternateText = _verbiageFalse;
			}
			imgBool.ToolTip = imgBool.AlternateText;
		}

	}
}
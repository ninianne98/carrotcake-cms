﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using Carrotware.CMS.Interface;


namespace Carrotware.CMS.UI.Plugins.PhotoGallery {
	public partial class PhotoGalleryPrettyPhoto : WidgetParmData, IWidget, IWidgetEditStatus {

		PhotoGalleryDataContext db = new PhotoGalleryDataContext();

		[Description("Display gallery heading")]
		public bool ShowHeading { get; set; }

		[Description("Scale gallery images")]
		public bool ScaleImage { get; set; }

		[Description("Gallery to display")]
		[Widget(WidgetAttribute.FieldMode.DropDownList, "lstGalleryID")]
		public Guid GalleryID { get; set; }


		[Widget(WidgetAttribute.FieldMode.DictionaryList)]
		public Dictionary<string, string> lstGalleryID {
			get {
				if (SiteID == Guid.Empty) {
					SiteID = SiteData.CurrentSiteID;
				}

				Dictionary<string, string> _dict = (from c in db.tblGalleries
													orderby c.GalleryTitle
													where c.SiteID == SiteID
													select c).ToList().ToDictionary(k => k.GalleryID.ToString(), v => v.GalleryTitle);

				return _dict;
			}
		}


		[Description("Gallery image pixel height/width")]
		[Widget(WidgetAttribute.FieldMode.DropDownList, "lstSizes")]
		public int ThumbSize { get; set; }

		[Widget(WidgetAttribute.FieldMode.DictionaryList)]
		public Dictionary<string, string> lstSizes {
			get {

				Dictionary<string, string> _dict = new Dictionary<string, string>();

				_dict.Add("25", "25px");
				_dict.Add("50", "50px");
				_dict.Add("75", "75px");
				_dict.Add("100", "100px");
				_dict.Add("125", "125px");
				_dict.Add("150", "150px");
				_dict.Add("175", "175px");
				_dict.Add("200", "200px");
				_dict.Add("225", "225px");
				_dict.Add("250", "250px");

				return _dict;
			}
		}

		public string GetScale() {
			return ScaleImage.ToString().ToLower();
		}

		public string GetThumbSize() {
			return ThumbSize.ToString().ToLower();
		}

		[Description("Gallery appearance (pretty photo skin)")]
		[Widget(WidgetAttribute.FieldMode.DropDownList, "lstPrettySkins")]
		public string PrettyPhotoSkin { get; set; }


		[Widget(WidgetAttribute.FieldMode.DictionaryList)]
		public Dictionary<string, string> lstPrettySkins {
			get {

				Dictionary<string, string> _dict = new Dictionary<string, string>();

				_dict.Add("default", "default");
				_dict.Add("light_square", "light square");
				_dict.Add("light_rounded", "light rounded");
				_dict.Add("facebook", "facebook");
				_dict.Add("dark_square", "dark square");
				_dict.Add("dark_rounded", "dark rounded");
				return _dict;
			}
		}

		protected void Page_Load(object sender, EventArgs e) {

			if (PublicParmValues.Count > 0) {

				try {
					string sFoundVal = GetParmValue("GalleryID", Guid.Empty.ToString());

					if (!string.IsNullOrEmpty(sFoundVal)) {
						GalleryID = new Guid(sFoundVal);
					}
				} catch (Exception ex) { }


				try {
					string sFoundVal = GetParmValue("ShowHeading", "false");

					if (!string.IsNullOrEmpty(sFoundVal)) {
						ShowHeading = Convert.ToBoolean(sFoundVal);
					}
				} catch (Exception ex) { }


				try {
					string sFoundVal = GetParmValue("ScaleImage", "false");

					if (!string.IsNullOrEmpty(sFoundVal)) {
						ScaleImage = Convert.ToBoolean(sFoundVal);
					}
				} catch (Exception ex) { }

				try {
					string sFoundVal = GetParmValue("ThumbSize", "150");

					if (!string.IsNullOrEmpty(sFoundVal)) {
						ThumbSize = Convert.ToInt32(sFoundVal);
					}
				} catch (Exception ex) { }

				try {
					string sFoundVal = GetParmValue("PrettyPhotoSkin", "light_rounded");

					if (!string.IsNullOrEmpty(sFoundVal)) {
						PrettyPhotoSkin = sFoundVal;
					}
				} catch (Exception ex) { }
			}

			if (string.IsNullOrEmpty(PrettyPhotoSkin)) {
				PrettyPhotoSkin = "light_rounded";
			}

			var gal = (from g in db.tblGalleries
					   where g.GalleryID == GalleryID
							&& g.SiteID == SiteData.CurrentSiteID
					   select g).FirstOrDefault();

			if (gal != null) {

				litGalleryName.Text = gal.GalleryTitle;
				pnlGalleryHead.Visible = ShowHeading;

				rpGallery.DataSource = (from g in db.tblGalleryImages
										join gg in db.tblGalleries on g.GalleryID equals gg.GalleryID
										where g.GalleryID == GalleryID
											&& gg.SiteID == SiteData.CurrentSiteID
										orderby g.ImageOrder ascending
										select g).ToList();

				rpGallery.DataBind();
			}

			if (rpGallery.Items.Count > 0) {
				pnlGallery.Visible = true;
			} else {
				pnlGallery.Visible = false;
			}

			if (!IsBeingEdited) {
				pnlScript.Visible = true;
			} else {
				pnlScript.Visible = false;
			}
		}


		#region IWidget Members

		public Guid PageWidgetID { get; set; }

		public Guid RootContentID { get; set; }

		public Guid SiteID { get; set; }

		public string JSEditFunction {
			get { return ""; }
		}
		public bool EnableEdit {
			get { return true; }
		}

		#endregion

		#region IWidgetEditStatus Members

		public bool IsBeingEdited { get; set; }

		#endregion
	}
}
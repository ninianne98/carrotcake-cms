﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carrotware.CMS.Core;
using Carrotware.CMS.Interface;



namespace Carrotware.CMS.UI.Plugins.PhotoGallery {
	public partial class PhotoGalleryAdminImportWP : AdminModule {

		Guid gTheID = Guid.Empty;
		protected PhotoGalleryDataContext db = new PhotoGalleryDataContext();
		protected FileDataHelper fileHelper = new FileDataHelper();
		protected SiteData siteHelper = new SiteData();
		private PhotoGalleryPrettyPhoto pgpp = new PhotoGalleryPrettyPhoto();

		private ContentPageHelper pageHelper = new ContentPageHelper();
		private CMSConfigHelper cmsHelper = new CMSConfigHelper();
		private WordPressSite wpSite = null;


		protected void Page_Load(object sender, EventArgs e) {

			pnlReview.Visible = false;
			pnlUpload.Visible = false;

			if (!string.IsNullOrEmpty(Request.QueryString["id"])) {
				gTheID = new Guid(Request.QueryString["id"].ToString());
			} else {
				pnlUpload.Visible = true;
			}

			lblWarning.Text = "";
			lblWarning.Attributes["style"] = "color: #000000;";


			if (gTheID != Guid.Empty) {
				wpSite = ContentImportExportUtils.GetSerializedWPExport(gTheID);
			}

			if (!IsPostBack && gTheID != Guid.Empty) {
				pnlReview.Visible = true;
				LoadLists();
			}

		}

		protected void BuildFolderList() {
			List<FileData> lstFolders = new List<FileData>();

			string sRoot = Server.MapPath("~/");

			string[] subdirs;
			try {
				subdirs = Directory.GetDirectories(sRoot);
			} catch {
				subdirs = null;
			}


			if (subdirs != null) {
				foreach (string theDir in subdirs) {
					string w = FileDataHelper.MakeWebFolderPath(theDir);
					lstFolders.Add(new FileData { FileName = w, FolderPath = w, FileDate = DateTime.Now });
				}
			}

			lstFolders.RemoveAll(f => f.FileName.ToLower().StartsWith(SiteData.AdminFolderPath));
			lstFolders.RemoveAll(f => f.FileName.ToLower().StartsWith("/bin/"));
			lstFolders.RemoveAll(f => f.FileName.ToLower().StartsWith("/obj/"));
			lstFolders.RemoveAll(f => f.FileName.ToLower().StartsWith("/app_data/"));

			ddlFolders.DataSource = lstFolders.OrderBy(f => f.FileName);
			ddlFolders.DataBind();
		}




		protected void LoadLists() {

			BuildFolderList();

			ddlSkin.DataSource = pgpp.lstPrettySkins;
			ddlSkin.DataBind();

			ddlSize.DataSource = pgpp.lstSizes;
			ddlSize.DataBind();

			List<WordPressPost> lst = (from p in wpSite.Content
									   join a in wpSite.Content.Where(x => x.PostType == WordPressPost.WPPostType.Attachment) on p.PostID equals a.ParentPostID
									   where p.PostContent.Contains("[gallery")
									   select p).Distinct().ToList();

			lblPages.Text = lst.Count.ToString();

			gvPages.DataSource = lst;
			gvPages.DataBind();

		}


		protected void btnCreate_Click(object sender, EventArgs e) {

			BuildWidgetInstall();

			Response.Redirect(CreateLink("CategoryList"));
		}


		protected void BuildWidgetInstall() {

			pnlReview.Visible = true;

			SiteData site = SiteData.CurrentSite;

			CMSAdminModuleMenu mod = cmsHelper.GetCurrentAdminModuleControl();
			string sDir = mod.ControlFile.Substring(0, mod.ControlFile.LastIndexOf("/"));
			List<CMSPlugin> lstp = cmsHelper.GetPluginsInFolder(sDir);

			CMSPlugin plug = lstp.Where(x => x.FilePath.EndsWith("PhotoGalleryPrettyPhoto.ascx")).FirstOrDefault();
			plug.FilePath = plug.FilePath.Replace("~~/", "~/");


			foreach (GridViewRow row in gvPages.Rows) {
				Guid gRootPage = Guid.Empty;
				Guid gGallery = Guid.Empty;

				CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");

				if (chkSelect.Checked) {
					HiddenField hdnPostID = (HiddenField)row.FindControl("hdnPostID");

					int iPost = int.Parse(hdnPostID.Value);

					List<WordPressPost> lstA = (from a in wpSite.Content
												where a.PostType == WordPressPost.WPPostType.Attachment
												&& a.ParentPostID == iPost
												orderby a.PostDateUTC
												select a).Distinct().ToList();


					lstA.ToList().ForEach(q => q.ImportFileSlug = ddlFolders.SelectedValue + "/" + q.ImportFileSlug);
					lstA.ToList().ForEach(q => q.ImportFileSlug = q.ImportFileSlug.Replace("//", "/").Replace("//", "/"));


					WordPressPost post = (from p in wpSite.Content
										  where p.PostID == iPost
										  select p).FirstOrDefault();

					ContentPage cp = null;

					List<ContentPage> lstCP = pageHelper.FindPageByTitleAndDate(site.SiteID, post.PostTitle, post.PostName, post.PostDateUTC);

					if (lstCP != null && lstCP.Count > 0) {
						cp = lstCP.FirstOrDefault();
					}

					if (cp != null) {
						gRootPage = cp.Root_ContentID;
						if (cp.PageText.Contains("[gallery]")) {
							cp.PageText = cp.PageText.Replace("[gallery]", "");
							cp.SavePageEdit();
						}
					}

					tblGallery gal = (from c in db.tblGalleries
									  where c.SiteID == site.SiteID
									  && c.GalleryTitle.ToLower() == post.PostTitle.ToLower()
									  select c).FirstOrDefault();

					if (gal == null) {

						gal = new tblGallery();
						gal.SiteID = site.SiteID;
						gal.GalleryID = Guid.NewGuid();
						gal.GalleryTitle = post.PostTitle;

						if (gal.GalleryID != gTheID) {
							db.tblGalleries.InsertOnSubmit(gal);
						}

						db.SubmitChanges();
					}

					gGallery = gal.GalleryID;

					int iPos = 0;

					foreach (var img in lstA) {
						bool bAdding = false;

						img.ImportFileSlug = img.ImportFileSlug.Replace("//", "/").Replace("//", "/");

						if (!chkFileGrab.Checked) {
							cmsHelper.GetFile(img.AttachmentURL, img.ImportFileSlug);
						}

						if (!string.IsNullOrEmpty(img.ImportFileSlug)) {

							var theImg = (from g in db.tblGalleryImages
										  where g.GalleryID == gGallery
										  && g.GalleryImage.ToLower() == img.ImportFileSlug.ToLower()
										  orderby g.ImageOrder ascending
										  select g).FirstOrDefault();

							if (theImg == null) {
								bAdding = true;
								theImg = new tblGalleryImage();
								theImg.GalleryImage = img.ImportFileSlug;
								theImg.GalleryImageID = Guid.NewGuid();
								theImg.GalleryID = gGallery;
							}
							theImg.ImageOrder = iPos;

							if (bAdding) {
								db.tblGalleryImages.InsertOnSubmit(theImg);
							}
						}
						iPos++;
					}

					db.SubmitChanges();


					if (gRootPage != Guid.Empty) {

						List<Widget> lstW = (from w in cp.GetWidgetList()
											 where w.ControlPath.ToLower() == plug.FilePath.ToLower()
												&& w.ControlProperties.ToLower().Contains(gGallery.ToString().ToLower())
											 select w).ToList();

						if (lstW.Count < 1) {

							Widget newWidget = new Widget();
							newWidget.ControlProperties = null;
							newWidget.Root_ContentID = gRootPage;
							newWidget.Root_WidgetID = Guid.NewGuid();
							newWidget.WidgetDataID = newWidget.Root_WidgetID;
							newWidget.ControlPath = plug.FilePath;
							newWidget.EditDate = SiteData.CurrentSite.Now;

							newWidget.IsLatestVersion = true;
							newWidget.IsWidgetActive = true;
							newWidget.IsWidgetPendingDelete = false;
							newWidget.WidgetOrder = -1;
							newWidget.PlaceholderName = txtPlaceholderName.Text;

							List<WidgetProps> lstProps = new List<WidgetProps>();
							lstProps.Add(new WidgetProps { KeyName = "ShowHeading", KeyValue = chkShowHeading.Checked.ToString() });
							lstProps.Add(new WidgetProps { KeyName = "ScaleImage", KeyValue = chkScaleImage.Checked.ToString() });
							lstProps.Add(new WidgetProps { KeyName = "ThumbSize", KeyValue = ddlSize.SelectedValue });
							lstProps.Add(new WidgetProps { KeyName = "PrettyPhotoSkin", KeyValue = ddlSkin.SelectedValue });
							lstProps.Add(new WidgetProps { KeyName = "GalleryID", KeyValue = gGallery.ToString() });

							newWidget.SaveDefaultControlProperties(lstProps);

							newWidget.Save();
						}
					}
				}
			}
		}


		protected void btnUpload_Click(object sender, EventArgs e) {
			string sXML = "";
			if (upFile.HasFile) {
				using (StreamReader sr = new StreamReader(upFile.FileContent)) {
					sXML = sr.ReadToEnd();
				}
			}
			string sTest = "";
			if (!string.IsNullOrEmpty(sXML) && sXML.Length > 500) {

				sTest = sXML.Substring(0, 250).ToLower();


				try {

					if (sXML.Contains("<channel>") && sXML.Contains("<rss")) {
						int iChnl = sXML.IndexOf("<channel>");
						sTest = sXML.Substring(0, iChnl).ToLower();
					}

					if (sTest.Contains("<!-- this is a wordpress extended rss file generated by wordpress as an export of your")
						&& sTest.Contains("http://purl.org/rss")
						&& sTest.Contains("http://wordpress.org/export")) {

						WordPressSite wps = ContentImportExportUtils.DeserializeWPExportAll(sXML);
						ContentImportExportUtils.AssignWPExportNewIDs(SiteData.CurrentSite, wps);
						ContentImportExportUtils.SaveSerializedDataExport<WordPressSite>(wps.NewSiteID, wps);

						//Response.Redirect(SiteFilename.WPSiteImportURL + "?importid=" + wps.NewSiteID.ToString());

						Response.Redirect(CreateLink("WPImport", String.Format("id={0}", wps.NewSiteID.ToString())));
					}

					lblWarning.Text = "File did not appear to match an expected format.";
					lblWarning.Attributes["style"] = "color: #990000;";

				} catch (Exception ex) {
					lblWarning.Text = ex.ToString();
					lblWarning.Attributes["style"] = "color: #990000;";
				}

			} else {
				lblWarning.Text = "No file appeared in the upload queue.";
				lblWarning.Attributes["style"] = "color: #990000;";
			}

		}



	}
}
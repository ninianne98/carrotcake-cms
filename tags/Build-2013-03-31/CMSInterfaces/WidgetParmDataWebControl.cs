﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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


namespace Carrotware.CMS.Interface {
	public abstract class WidgetParmDataWebControl : WebControl, IWidget, IWidgetParmData {

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


		#region IWidgetParmData Members

		private Dictionary<string, string> _parms = new Dictionary<string, string>();
		public Dictionary<string, string> PublicParmValues {
			get { return _parms; }
			set { _parms = value; }
		}

		#endregion

		protected string GetParmValue(string sKey) {
			string ret = null;

			if (PublicParmValues.Count > 0) {
				ret = (from c in PublicParmValues
					   where c.Key.ToLower() == sKey.ToLower()
					   select c.Value).FirstOrDefault();
			}

			return ret;
		}

		protected string GetParmValue(string sKey, string sDefault) {
			string ret = null;

			if (PublicParmValues.Count > 0) {
				ret = (from c in PublicParmValues
					   where c.Key.ToLower() == sKey.ToLower()
					   select c.Value).FirstOrDefault();
			}

			ret = ret == null ? sDefault : ret;

			return ret;
		}

		protected string GetParmValueDefaultEmpty(string sKey, string sDefault) {
			string ret = null;
			ret = GetParmValue(sKey, sDefault);

			ret = string.IsNullOrEmpty(ret) ? sDefault : ret;

			return ret;
		}

		protected List<string> GetParmValueList(string sKey) {

			sKey = sKey.EndsWith("|") ? sKey : sKey + "|";
			sKey = sKey.ToLower();

			List<string> ret = new List<string>();

			if (PublicParmValues.Count > 0) {
				ret = (from c in PublicParmValues
					   where c.Key.ToLower().StartsWith(sKey)
					   select c.Value).ToList();
			}

			return ret;
		}

	}
}
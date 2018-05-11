using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace MPSC.DomainDrivenDesign.Apresentacao.WebForms.App.SharedControls
{
	public class MessageBox
	{
		private Page _page;
		public MessageBox (Page page)
		{
			_page = page;
		}

		public void Show(String format, params Object[] args)
		{
			var script = String.Format("alert('{0}');", String.Format(format, args));
			ScriptManager.RegisterClientScriptBlock(_page, _page.GetType(), "alert", script, true);
		}
	}
}
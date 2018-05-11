using System.Web.UI;

namespace MPSC.DomainDrivenDesign.Apresentacao.WebForms.App.SharedControls
{
	public class BasePage : Page
	{
		private MessageBox _messageBox;
		protected MessageBox MessageBox { get { return _messageBox ?? (_messageBox = new MessageBox(this)); } }
	}
}
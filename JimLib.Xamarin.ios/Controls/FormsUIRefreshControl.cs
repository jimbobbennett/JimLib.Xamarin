using System.Windows.Input;
using MonoTouch.UIKit;

namespace JimBobBennett.JimLib.Xamarin.ios.Controls
{
	public class FormsUIRefreshControl : UIRefreshControl
	{
		public FormsUIRefreshControl()
		{
			ValueChanged += (sender, e) => 
			{
				var command = RefreshCommand;
				if(command  == null)
					return;

				command.Execute(null);
			};
		}

		private string _message;

		public string Message 
		{ 
			get { return _message;}
			set 
			{ 
				_message = value;
				if (string.IsNullOrWhiteSpace (_message))
					return;

				AttributedTitle = new MonoTouch.Foundation.NSAttributedString(_message);
			}
		}

		private bool _isRefreshing;

		public bool IsRefreshing
		{
			get { return _isRefreshing;}
			set
			{ 
				_isRefreshing = value; 
				if (_isRefreshing)
					BeginRefreshing();
				else
					EndRefreshing();
			}
		}

        public ICommand RefreshCommand { get; set; }
	}
}


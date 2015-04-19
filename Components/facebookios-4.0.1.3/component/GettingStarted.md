The Facebook SDK for iOS makes it easier and faster to develop Facebook integrated iOS apps.

## Sample

### AppDelegate.cs

```csharp
using Facebook.CoreKit;
//...

// Get your own App ID at developers.facebook.com/apps
const string AppId = "Your-Id-Here";
const string DisplayName = "Your_App_Display_Name";

public override bool FinishedLaunching (UIApplication app, NSDictionary options)
{
	Settings.AppID = FacebookAppId;
	Settings.DefaultDisplayName = DisplayName;
	//...
}

public override bool OpenUrl (UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
{
	// We need to handle URLs by passing them to their own OpenUrl in order to make the SSO authentication works.
	return ApplicationDelegate.SharedInstance.OpenUrl (application, url, sourceApplication, annotation);
}

```

### YourViewController.cs

```csharp
using Facebook.LoginKit;
using Facebook.CoreKit;
//...

// For extensive list of available extended permissions refer to 
// https://developers.facebook.com/docs/reference/api/permissions/
string [] extendedPermissions = new [] { "user_about_me", "read_stream"};
string [] publishPermissions = new [] { "publish_stream" };

LoginButton loginView;
ProfilePictureView pictureView;

public override void ViewDidLoad ()
{
	base.ViewDidLoad ();

	// If you use Native login behavior, you will get all read and publish permisions
	// otherwise, set the Read and Publish permissions you want to get
	loginView = new LoginButton (new CGRect (51, 0, 218, 46)) {
		LoginBehavior = LoginBehavior.Native,
		// ReadPermissions = extendedPermissions,
		// PublishPermissions = publishPermissions
	};

	// Handle actions once the user is logged in
	loginView.Completed += (sender, e) => {
		if (e.Error != null) {
			// Handle if there was an error
		}
		
		// Handle your successful login
	};

	// Handle actions once the user is logged out
	loginView.LoggedOut += (sender, e) => {
		// Handle your logout
	};

	// The user image profile is set automatically once is logged in
	pictureView = new ProfilePictureView (new CGRect (50, 0, 220, 220));

	// Create the label that will hold user's facebook name
	nameLabel = new UILabel (new RectangleF (20, 319, 280, 21)) {
		TextAlignment = UITextAlignment.Center,
		BackgroundColor = UIColor.Clear
	};

	// Add views to main view
	View.AddSubview (loginView);
	View.AddSubview (pictureView);
	View.AddSubview (nameLabel);
}

```

### Controlling the login dialogs

The Facebook SDK automatically selects the optimal login dialog flow based on the account settings and capabilities of a person's device.

If you want to use your System Account of Settings, just change the FB Login's behavior:

```csharp

loginView.LoginBehavior = LoginBehavior.SystemAccount;

```

## Documentation

* SDK Reference: [https://developers.facebook.com/docs/reference/ios/current/](https://developers.facebook.com/docs/reference/ios/current/)

## Contact & Discuss

* Bugs Tracker: [https://developers.facebook.com/bugs](https://developers.facebook.com/bugs)
* StackOverflow: [http://facebook.stackoverflow.com/questions/tagged/facebook-ios-sdk](http://facebook.stackoverflow.com/questions/tagged/facebook-ios-sdk)
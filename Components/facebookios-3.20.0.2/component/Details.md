From Facebook's [What's New in 3.6](https://developers.facebook.com/ios/features/whats-new-ios-sdk-3.6/) article:

The Facebook SDK for iOS makes it easier and faster to develop Facebook
integrated iOS apps. The Facebook SDK 3.6 for iOS adds support for the
Facebook integration in iOS 6:

* **Native Facebook Login**, which is the first step in using Facebook to drive user engagement and app installs.
* **Sharing**: Native sheet for posting to Facebook.
 
Along with support for the iOS 6 integration, our SDK provides advanced functionality that makes it easier to build more immersive social experiences, distribute and promote your app. These features include:

* **Friend Picker** to help apps easily pick friends. Example: use a friend picker to choose friends to tag in an Open Graph action.
* **Places Picker** so apps can easily integrate with Facebook places. Example: use this picker to let users include a place with their posts.
* **Profile Picture control** so your app can easily show the profile picture of a user, their friends, places, or other kinds of Facebook objects.
* **Login controls** for easily building Login and Logout experiences.

Additional SDK features and improvements include:
 
* Improved API support: Features that make it easier to integrate Open Graph into your mobile apps, FQL, and other APIs. It natively supports batched API requests to significantly improve performance for API calls, which translate into faster, better user experiences.
* Mobile install measurement: With our SDK, you can measure clicks and installs for mobile app install ads. Learn more about mobile app install ads.

### AppDelegate.cs

```csharp
using MonoTouch.FacebookConnect;
//...

// Get your own App ID at developers.facebook.com/apps
const string FacebookAppId = "Your-Id-Here";
const string DisplayName = "Your_App_Display_Name";

public override bool FinishedLaunching (UIApplication app, NSDictionary options)
{
	FBSettings.DefaultAppID = FacebookAppId;
	FBSettings.DefaultDisplayName = DisplayName;
	//...
}

public override bool OpenUrl (UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
{
	// We need to handle URLs by passing them to FBSession in order for SSO authentication
	// to work.
	return FBSession.ActiveSession.HandleOpenURL(url);
}

public override void OnActivated (UIApplication application)
{
	// We need to properly handle activation of the application with regards to SSO
	// (e.g., returning from iOS 6.0 authorization dialog or from fast app switching).
	FBSession.ActiveSession.HandleDidBecomeActive();
}

```

### YourViewController.cs

```csharp
using MonoTouch.FacebookConnect;
//...

// For extensive list of available extended permissions refer to 
// https://developers.facebook.com/docs/reference/api/permissions/
private string [] ExtendedPermissions = new [] { "user_about_me", "read_stream"};

FBLoginView loginView;
FBProfilePictureView pictureView;
IFBGraphUser user;
UILabel nameLabel;

public override void ViewDidLoad ()
{
	base.ViewDidLoad ();

	// Create the Facebook LogIn View with the needed Permissions
	// https://developers.facebook.com/ios/login-ui-control/
	loginView = new FBLoginView (ExtendedPermissions) {
		Frame = new RectangleF (85, 20, 151, 43)
	};

	// Hook up to FetchedUserInfo event, so you know when
	// you have the user information available
	loginView.FetchedUserInfo += (sender, e) => {
		user = e.User;
		pictureView.ProfileID = user.GetId ();
		nameLabel.Text = user.GetName ();
	};

	// Clean user Picture and label when Logged Out
	loginView.ShowingLoggedOutUser += (sender, e) => {
		pictureView.ProfileID = null;
		nameLabel.Text = string.Empty;
	};

	// Create view that will display user's profile picture
	// https://developers.facebook.com/ios/profilepicture-ui-control/
	pictureView = new FBProfilePictureView () {
		Frame = new RectangleF (40, 71, 240, 240)
	};

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

The Facebook SDK automatically selects the optimal login dialog flow based on the account settings and capabilities of a person's device. This is the default sequence that the Facebook SDK implements:

- Facebook App Native Login Dialog
- Facebook App Web Login Dialog
- Mobile Safari Login Dialog

If you want to use your System Account of Settings, just change the FB Login's behavior:

```csharp

loginView.LoginBehavior = FBSessionLoginBehavior.UseSystemAccountIfPresent;

```

If your System Account is not setup, the SDK will follow the default sequence.

*Screenshot generated with [PlaceIt](http://placeit.breezi.com/).*

# News: iOS Unified API Support
# drawboard-projects
UWP application to play with the Drawboard Projects API.

It targets Windows 10, version 1903 (10.0 Build 18362) and minimum version it supports is Windows 10 Fall Creators Update (10.0, Build 16299).

## IMPORTANT
For consideration of sensitive information, I have not stored authentication credentials in this repository, which are required for the Unit Test project.

Both the UWP project and Unit Test project depend on an appsettings.json file being present in each (not tracked and added to the gitignore list).

You must:
- Build the projects for this file to be generated from an appsettings-template.json file in each project
  - Controlled by the pre-build event command
- Refresh the solution to load the newly created appsettings.json files
- Edit the appsettings.json file in the Unit Test project for the Services tests to run successfully.
- Add the required Username and PlainTextPassword in the <b>AuthenticationForTesting</b> section and save.
- You can optionally edit the file in the UWP project, but this is only required for prep-populating the login screen with the credentials.

## Design and Implementations
I love to use MVVM architecture, Inversion of Control (IoC) and Dependency Injection (DI).

For quick setup of MVVM archtecture and ViewModel-based navigation, I chose to use MVVMCross. It seemed to have the best support for UWP. Other considerations were the Prism Library (but only supported UWP via the Uno framework) and MVVM Light (didn't seem up-to-date).

Made use of the MvvmDialogs package (https://github.com/FantasticFiasco/mvvm-dialogs) for quick setup of showing dialogs to the user from the View Models.

I kept the screen design relatively simple.

## Limitations
- Tightly coupled with the MVVMCross Framework
- Not using cancellation tokens in REST services (easy to add)
- Very simple implementation of the projects view
- No app state save and restore

## Enhancements
- Number one would be to enhance the main entry/view after logging in, and:
  - Create a Main View or user dashboard, which would contain a projects overview and user information (and maybe some stats, reports, recent activity).
    - Selecting a project would then dive into presenting a menu for viewing documents, drawings, users, issues, etc.
  - Making it more visually appealing
- Show more project information in the Projects view e.g. owner, number of users, documents, drawings, and issues.
- Add theming, styles, colours to App.xaml
- Add option to save login credentials and store them securely
- Add some POST and PUT examples e.g. to edit a project or user information
- Implement REST client methods using a canncellation token.
- Normally make use of an event aggregator service (e.g. MvvmCross Messenger) that enables communications between loosely coupled components in the application
- Present login validation feedback better
- Add more tests
- Add feedback for when there is not internet
- Normally would make use of the App Center SDK for diagnostics, crash detection, and more.

## Considerations
### Logging
Default Logging provided by MVVMCross wasn't logging to the console. I then added Serilog as the log provider, but that didn't seem to make a difference. While very important to get working, this was low priority for me for the purpose of this exercise and I didn't spend time investigating further.

### Binding
Using compile-time binding, {x:Bind }, is faster than the runtime {Binding }, but I'm used to not directly referencing the View Model in the Views. Nothing too wrong with it, but this prevents you linking other view models to a view, which is something I do allow in my current employment projects, making it more configurable and customisable. Creating ViewModel interfaces (e.g. ILoginViewModel) which could be used in the Views might be a good option.

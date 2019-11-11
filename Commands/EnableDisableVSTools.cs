using System;
using System.ComponentModel.Design;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Task = System.Threading.Tasks.Task;

namespace Visual_Studio_Tools_C_Sharp
{
	/// <summary>
	/// Command handler
	/// </summary>
	internal sealed class EnableDisableVSTools
	{
		/// <summary>
		/// Command ID.
		/// </summary>
		public const int CommandId = 0x0100;

		/// <summary>
		/// Command menu group (command set GUID).
		/// </summary>
		public static readonly Guid CommandSet = new Guid("4748639c-7de1-43c1-af36-c237f5393e73");

		/// <summary>
		/// VS Package that provides this command, not null.
		/// </summary>
		private readonly AsyncPackage package;

		/// <summary>
		/// Initializes a new instance of the <see cref="EnableDisableVSTools"/> class.
		/// Adds our command handlers for menu (commands must exist in the command table file)
		/// </summary>
		/// <param name="package">Owner package, not null.</param>
		/// <param name="commandService">Command service to add command to, not null.</param>
		private EnableDisableVSTools(AsyncPackage package, OleMenuCommandService commandService)
		{
			this.package = package ?? throw new ArgumentNullException(nameof(package));
			commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

			CommandID menuCommandID	= new CommandID(CommandSet, CommandId);
			MenuCommand menuItem	= new MenuCommand(this.Execute, menuCommandID);
			menuItem.Checked	= GeneralSettings.Default.EnableVSTools;
			commandService.AddCommand(menuItem);
		}

		/// <summary>
		/// Gets the instance of the command.
		/// </summary>
		public static EnableDisableVSTools Instance
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the service provider from the owner package.
		/// </summary>
		private Microsoft.VisualStudio.Shell.IAsyncServiceProvider ServiceProvider
		{
			get
			{
				return this.package;
			}
		}

		/// <summary>
		/// Initializes the singleton instance of the command.
		/// </summary>
		/// <param name="package">Owner package, not null.</param>
		public static async Task InitializeAsync(AsyncPackage package)
		{
			// Switch to the main thread - the call to AddCommand in EnableDisableVSTools's constructor requires
			// the UI thread.
			await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

			OleMenuCommandService commandService = await package.GetServiceAsync((typeof(IMenuCommandService))) as OleMenuCommandService;
			Instance = new EnableDisableVSTools(package, commandService);
		}

		/// <summary>
		/// This function is the callback used to execute the command when the menu item is clicked.
		/// See the constructor to see how the menu item is associated with this function using
		/// OleMenuCommandService service and MenuCommand class.
		/// </summary>
		/// <param name="sender">Event sender.</param>
		/// <param name="e">Event args.</param>
		private void Execute(object sender, EventArgs e)
		{
			ThreadHelper.ThrowIfNotOnUIThread();
			//string message = string.Format(CultureInfo.CurrentCulture, "Inside {0}.MenuItemCallback()", this.GetType().FullName);
			//string title = "EnableDisableVSTools";

			//// Show a message box to prove we were here
			//VsShellUtilities.ShowMessageBox(
			//	this.package,
			//	message,
			//	title,
			//	OLEMSGICON.OLEMSGICON_INFO,
			//	OLEMSGBUTTON.OLEMSGBUTTON_OK,
			//	OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);

			GeneralSettings.Default.EnableVSTools = !GeneralSettings.Default.EnableVSTools;
			GeneralSettings.Default.Save();
			var command = sender as MenuCommand;
			command.Checked = GeneralSettings.Default.EnableVSTools;
		}
	}
}

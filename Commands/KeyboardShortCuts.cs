using System;
using System.ComponentModel.Design;
using System.Globalization;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Task = System.Threading.Tasks.Task;

namespace Visual_Studio_Tools_C_Sharp
{
	/// <summary>
	/// Command handler.
	/// </summary>
	internal sealed class KeyboardShortCuts
	{
		#region Members

		/// <summary>
		/// Command ID.
		/// </summary>
		public const int CommandId = 256;

		/// <summary>
		/// Command menu group (command set GUID).
		/// </summary>
		public static readonly Guid CommandSet = new Guid("6812F808-9F6B-42DD-B98E-E474BA6CD122");

		/// <summary>
		/// VS Package that provides this command, not null.
		/// </summary>
		private readonly AsyncPackage package;

		#endregion

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="ReverseEquals"/> class.
		/// Adds our command handlers for menu (commands must exist in the command table file)
		/// </summary>
		/// <param name="package">Owner package, not null.</param>
		/// <param name="commandService">Command service to add command to, not null.</param>
		private KeyboardShortCuts(AsyncPackage package, OleMenuCommandService commandService)
		{
			this.package = package ?? throw new ArgumentNullException(nameof(package));
			commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

			var menuCommandID = new CommandID(CommandSet, CommandId);
			var menuItem = new MenuCommand(this.Execute, menuCommandID);
			commandService.AddCommand(menuItem);
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the instance of the command.
		/// </summary>
		public static KeyboardShortCuts Instance
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

		#endregion

		#region Methods

		/// <summary>
		/// Initializes the singleton instance of the command.
		/// </summary>
		/// <param name="package">Owner package, not null.</param>
		public static async Task InitializeAsync(AsyncPackage package)
		{
			// Switch to the main thread - the call to AddCommand in ReverseEquals's constructor requires
			// the UI thread.
			await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

			OleMenuCommandService commandService = await package.GetServiceAsync((typeof(IMenuCommandService))) as OleMenuCommandService;
			Instance = new KeyboardShortCuts(package, commandService);
		}

		/// <summary>
		/// This function is the callback used to execute the command when the menu item is clicked.
		/// See the constructor to see how the menu item is associated with this function using
		/// OleMenuCommandService service and MenuCommand class.
		/// </summary>
		/// <param name="sender">Event sender.</param>
		/// <param name="eventArgs">Event args.</param>
		private void Execute(object sender, EventArgs eventArgs)
		{
			ThreadHelper.ThrowIfNotOnUIThread();

			DTE dte				= Package.GetGlobalService(typeof(DTE)) as DTE;
			Commands commands	= dte.Commands;

			// F7 for build.
			commands.Item("Build.BuildSolution").Bindings								= "Global::F7";

			// You're getting in my way you dirty dogs.
			commands.Item("Edit.IncreaseFilterLevel").Bindings							= "Global::Ctrl+.";
			commands.Item("EditorContextMenus.Navigate.GoToContainingBlock").Bindings	= new object[] { };
					

			// Add some additional short cuts.
			commands.Item("Debug.DisableAllBreakpoints").Bindings						= "Global::Ctrl+E,Ctrl+B";
			commands.Item("Debug.EnableAllBreakpoints").Bindings						= "Global::Ctrl+D,Ctrl+B";

			// Custom.
			commands.Item("VSTools.InsertSquareBraces").Bindings						= "Text Editor::Alt+[";
			commands.Item("VSTools.InsertCurlyBraces").Bindings                         = "Text Editor::Shift+Alt+[";
			commands.Item("VSTools.InsertCStylePointer").Bindings						= "Text Editor::Alt+.";
			commands.Item("VSTools.ReverseEquals").Bindings								= "Text Editor::Alt+=";
		}

		#endregion

	} // End class.
} // End namespace.

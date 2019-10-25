using System;
using System.ComponentModel.Design;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Task = System.Threading.Tasks.Task;

namespace Visual_Studio_Tools_C_Sharp
{
	/// <summary>
	/// Command handler.
	/// </summary>
	internal sealed class CurlyBraces
	{
		#region Members

		/// <summary>
		/// Command ID.
		/// </summary>
		public const int CommandId = 256;

		/// <summary>
		/// Command menu group (command set GUID).
		/// </summary>
		public static readonly Guid CommandSet = new Guid("0fbf5180-4802-4377-9b01-49e286280b29");

		/// <summary>
		/// VS Package that provides this command, not null.
		/// </summary>
		private readonly AsyncPackage package;

		#endregion

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="CurlyBraces"/> class.
		/// Adds our command handlers for menu (commands must exist in the command table file)
		/// </summary>
		/// <param name="package">Owner package, not null.</param>
		/// <param name="commandService">Command service to add command to, not null.</param>
		private CurlyBraces(AsyncPackage package, OleMenuCommandService commandService)
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
		public static CurlyBraces Instance
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
			// Switch to the main thread - the call to AddCommand in CurlyBraces's constructor requires
			// the UI thread.
			await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

			OleMenuCommandService commandService = await package.GetServiceAsync((typeof(IMenuCommandService))) as OleMenuCommandService;
			Instance = new CurlyBraces(package, commandService);
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

			TextDocument textDocument = VSTools.GetTextDocument();

			textDocument.Selection.Text = "{";
			textDocument.Selection.NewLine();
			textDocument.Selection.Backspace();
			textDocument.Selection.Text = "}";
			textDocument.Selection.LineUp(false, 1);
			textDocument.Selection.NewLine();
		}

		#endregion

	} // End class.
} // End namespace.

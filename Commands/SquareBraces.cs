using System;
using System.ComponentModel.Design;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Task = System.Threading.Tasks.Task;

namespace Visual_Studio_Tools_C_Sharp
{
	/// <summary>
	/// Command handler
	/// </summary>
	internal sealed class SquareBraces
	{
		#region Members

		/// <summary>
		/// Command ID.
		/// </summary>
		public const int CommandId = 256;

		/// <summary>
		/// Command menu group (command set GUID).
		/// </summary>
		public static readonly Guid CommandSet = new Guid("0040a892-7a77-40ce-bc13-bb6ec1061764");

		/// <summary>
		/// VS Package that provides this command, not null.
		/// </summary>
		private readonly AsyncPackage package;

		#endregion

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="SquareBraces"/> class.
		/// Adds our command handlers for menu (commands must exist in the command table file)
		/// </summary>
		/// <param name="package">Owner package, not null.</param>
		/// <param name="commandService">Command service to add command to, not null.</param>
		private SquareBraces(AsyncPackage package, OleMenuCommandService commandService)
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
		public static SquareBraces Instance
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
			// Switch to the main thread - the call to AddCommand in SquareBraces's constructor requires
			// the UI thread.
			await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

			OleMenuCommandService commandService = await package.GetServiceAsync((typeof(IMenuCommandService))) as OleMenuCommandService;
			Instance = new SquareBraces(package, commandService);
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

			DTE dte = Package.GetGlobalService(typeof(DTE)) as DTE;
			Document document = dte.ActiveDocument;
			TextDocument textDocument = document.Object() as TextDocument;

			int count = 0;
			textDocument.Selection.CharLeft(true, 1);

			while (textDocument.Selection.Text != " " && textDocument.Selection.Text != ";")
			{
				textDocument.Selection.CharLeft(false, 1);
				textDocument.Selection.CharLeft(true, 1);
				count++;

				//Ensure I don't back up over a semi-colon.Use this as a test if the
				//routine was called at the end or beginning of a line (perhaps by accident).
				//Should immediately test if there is a semi-colon, new-line or tab when the routine
				//is entered, by I don't know how the tab and new-line is represented "\t" did not work.
				if (textDocument.Selection.Text == ";")
				{
					textDocument.Selection.CharRight(false, count);
					textDocument.Selection.Text = "[]";
					break;
				}
			}

			textDocument.Selection.Text = "[";
			textDocument.Selection.CharRight(false, count);
			textDocument.Selection.Text = "]";
		}

		#endregion

	} // End class.
} // End namespace.


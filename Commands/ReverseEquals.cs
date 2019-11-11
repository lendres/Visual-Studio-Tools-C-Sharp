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
	internal sealed class ReverseEquals
	{
		#region Members

		/// <summary>
		/// Command ID.
		/// </summary>
		public const int CommandId = 256;

		/// <summary>
		/// Command menu group (command set GUID).
		/// </summary>
		public static readonly Guid CommandSet = new Guid("879fc60d-7fa0-41f9-a1e3-4fa762c98502");

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
		private ReverseEquals(AsyncPackage package, OleMenuCommandService commandService)
		{
			this.package = package ?? throw new ArgumentNullException(nameof(package));
			commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

			CommandID menuCommandID	= new CommandID(CommandSet, CommandId);
			MenuCommand menuItem	= new MenuCommand(this.Execute, menuCommandID);
			commandService.AddCommand(menuItem);
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the instance of the command.
		/// </summary>
		public static ReverseEquals Instance
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
			Instance = new ReverseEquals(package, commandService);
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

			// Select the current line and copy the text to a string.
			textDocument.Selection.SelectLine();

			// Copy the line from the selection while remove ending spaces, tabs, carriage returns, and carriage return, line feeds.  The
			// selection of the lines seems to grab the "return" so we need to remove it.
			// 9  - Tab
			// 10 - Line feed
			// 13 - Carriage return
			string line				= textDocument.Selection.Text.TrimEnd(System.Text.Encoding.ASCII.GetChars(new byte[] { 9, 10, 13 }));

			// Split the string into the left and right parts at the equal sign.  These will be used as find and replace values.  We only want
			// the left code section and right code section, so we need to strip tabs and the ending semi-colon as well.
			string[] leftandright	= line.Split(new char[] {Convert.ToChar(9), '=', ';' }, StringSplitOptions.RemoveEmptyEntries);

			// Now that we've split them into the two parts, removing the equal sign and trailing semi-color, we remove all leading and trailing
			// spaces.  We only want the two pieces of code on either side of the equal sign.
			leftandright[0] = leftandright[0].Trim();
			leftandright[1] = leftandright[1].Trim();

			// Split the line at the equal sign, removing the equal sign.
			string[] halves = line.Split('=');

			// Replace the strings in the two halves.  This preserves the leading tabs and any spacing between the two sides.
			halves[0] = halves[0].Replace(leftandright[0], leftandright[1]);
			halves[1] = halves[1].Replace(leftandright[1], leftandright[0]);

			// Reassemble the string from the two halves which had the strings swapped.
			// We also never have extraneous blank/white space at the end of a line, so we might as well kill that while we are here.
			textDocument.Selection.Insert(halves[0] + "=" + halves[1].TrimEnd() + "\n");
		}

		#endregion

	} // End class.
} // End namespace.

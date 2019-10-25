namespace Visual_Studio_Tools_C_Sharp
{
    public int Property1 { get; set; }
    public int Property2 { get; set; }
    public int Property3 { get; set; }

    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class SquareBraces
    {

    }

    private void Test()
    {
        for (int i = 0; i < 10; i++)
        {
			double[] testArray = new double[10];
        }

		for (int i = 0; i < 10; i++)
		


		this.Property3      = this.Property1;
        this.Property1      = this.Property2;
        this.Property2      = this.Property3;
    }
}


// ThreadHelper.ThrowIfNotOnUIThread();
// string message = string.Format(CultureInfo.CurrentCulture, "Inside {0}.MenuItemCallback()", this.GetType().FullName);
// string title = "ReverseEquals";

// // Show a message box to prove we were here
// VsShellUtilities.ShowMessageBox(
// 		this.package,
// 		message,
// 		title,
// 		OLEMSGICON.OLEMSGICON_INFO,
// 		OLEMSGBUTTON.OLEMSGBUTTON_OK,
// 		OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
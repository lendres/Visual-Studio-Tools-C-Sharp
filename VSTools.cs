using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;
using Microsoft.VisualStudio.Shell;

namespace Visual_Studio_Tools_C_Sharp
{
	static class VSTools
	{
		#region Construction

		#endregion

		#region Methods

		public static TextDocument GetTextDocument()
		{
			ThreadHelper.ThrowIfNotOnUIThread();

			DTE dte = Package.GetGlobalService(typeof(DTE)) as DTE;
			Document document = dte.ActiveDocument;

			// If a document is not open, we cannot work.
			if (document == null)
			{
				return null;
			}

			return document.Object() as TextDocument;
		}

		#endregion

	} // End class.
} // End namespace.

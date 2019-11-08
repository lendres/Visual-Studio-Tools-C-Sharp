using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace $rootnamespace$
{
	/// <summary>
	/// 
	/// </summary>
	public partial class $safeitemname$ : Form
	{
		#region Members

		#endregion

		#region Construction

		/// <summary>
		/// Default constructor.
		/// </summary>
		public $safeitemname$()
		{
			InitializeComponent();

			PopulateControls();
		}

		#endregion

		#region Properties

		#endregion

		#region Event Handlers

		/// <summary>
		/// Ok button event handler.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">Event arguments.</param>
		private void buttonOK_Click(object sender, EventArgs e)
		{
			// TODO: Validation code goes here.

			PushEntriesToDataStructure();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Initialize the controls with the values from the data structure.
		/// </summary>
		protected void PopulateControls()
		{

		}

		/// <summary>
		/// Save the data back to the data structure.
		/// </summary>
		protected void PushEntriesToDataStructure()
		{

		}

		#endregion

	} // End class.
} // End namespace.
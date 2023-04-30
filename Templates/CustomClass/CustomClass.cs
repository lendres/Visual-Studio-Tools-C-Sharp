using DigitalProduction.XML.Serialization;
using System;
using System.Collections.Generic;
$if$ ($targetframeworkversion$ >= 3.5)using System.Linq;
$endif$using System.Text;
$if$ ($targetframeworkversion$ >= 4.5)using System.Threading.Tasks;
$endif$using System.ComponentModel;
using System.Xml.Serialization;

namespace $rootnamespace$
{
	/// <summary>
	/// 
	/// </summary>
	[XmlRoot("$safeitemrootname$")]
	public class $safeitemrootname$
	{
		#region Enumerations

		#endregion

		#region Delegates

		#endregion

		#region Events

		#endregion

		#region Fields

		#endregion

		#region Construction

		/// <summary>
		/// Default constructor.
		/// </summary>
		public $safeitemrootname$()
		{
		}

		#endregion

		#region Properties

		#endregion

		#region Methods

		#endregion

		#region XML

		/// <summary>
		/// Write this object to a file to the provided path.
		/// </summary>
		/// <param name="path">Path (full path and filename) to write to.</param>
		/// <exception cref="InvalidOperationException">Thrown when the projects path is not valid.</exception>
		public void Serialize(string path)
		{
			if (!DigitalProduction.IO.Path.PathIsWritable(path))
			{
				throw new InvalidOperationException("The file cannot be saved.  A valid path must be specified.");
			}

			Serialization.SerializeObject(this, path);
		}

		/// <summary>
		/// Create an instance from a file.
		/// </summary>
		/// <param name="path">The file to read from.</param>
		public static $safeitemrootname$ Deserialize(string path)
		{
			return Serialization.DeserializeObject<$safeitemrootname$>(path);
		}

		#endregion

	} // End class.
} // End namespace.
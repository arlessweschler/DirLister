﻿namespace Sander.DirLister.Core
{
	/// <summary>
	///     Output format. Multiple formats can be used at the same time
	/// </summary>
	public enum OutputFormat
	{
		/// <summary>
		///     Plain text file
		/// </summary>
		Txt,

		/// <summary>
		///     HTML5
		/// </summary>
		Html,

		/// <summary>
		///     CSV. All fields are quoted, comma delimiter, header exists
		/// </summary>
		Csv,

		/// <summary>
		///     JSON
		/// </summary>
		Json,

		/// <summary>
		///     XML
		/// </summary>
		Xml,

		/// <summary>
		///     Markdown
		/// </summary>
		Md
	}
}

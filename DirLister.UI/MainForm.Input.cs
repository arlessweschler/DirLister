﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sander.DirLister.Core;
using Sander.DirLister.UI.Properties;

namespace Sander.DirLister.UI
{
	public partial class MainForm
	{
		private void AddFolderToHistory(string directory, bool isStartup = false)
		{
			var addToHistory = true;

			foreach (var historyMenuItem in HistoryMenu.Items.OfType<ToolStripMenuItem>())
				if (string.Compare(historyMenuItem.Text, directory, StringComparison.OrdinalIgnoreCase) == 0)
					addToHistory = false;

			if (addToHistory)
			{
				while (HistoryMenu.Items.Count >= Settings.Default.HistoryLength)
					HistoryMenu.Items.RemoveAt(Settings.Default.HistoryLength - 1);

				var menuItem = new ToolStripMenuItem(directory) {Tag = "folder"};
				menuItem.Click += delegate(object sender, EventArgs args)
				{
					if (sender is ToolStripMenuItem item) AddFolderToList(item.Text);
				};
				HistoryMenu.Items.Insert(2, menuItem);
			}

			if (!isStartup) Task.Run(() => Mediator.AddToHistory(directory));
		}


		private void AddFolderToList(string directory)
		{
			var addToList = true;
			foreach (ListViewItem listItem in DirectoryList.Items)
				if (string.Compare(listItem.Text, directory, StringComparison.OrdinalIgnoreCase) == 0)
					addToList = false;

			if (addToList) DirectoryList.Items.Add(new ListViewItem {Text = directory});
		}

		/// <summary>
		/// Remove all items from DirectoryList
		/// </summary>
		private void RemoveAll_Click(object sender, EventArgs e)
		{
			DirectoryList.Items.Clear();
		}

		/// <summary>
		/// Open folder in Explorer
		/// </summary>
		private void DirectoryList_DoubleClick(object sender, EventArgs e)
		{
			Process.Start("explorer.exe", DirectoryList.SelectedItems[0].Text);
		}

		/// <summary>
		/// Clear history menu
		/// </summary>
		private void HistoryClearMenuItem_Click(object sender, EventArgs e)
		{
			var count = HistoryMenu.Items.Count;
			for (var i = 2; i < count; i++) HistoryMenu.Items.RemoveAt(2);
		}

		private void InitializeInput(Configuration configuration, IEnumerable<string> inputFolders)
		{
			FilenameFilter.SelectedText = Settings.Default.SelectedFilter;
			//todo: filters

			if (Settings.Default?.DirectoryHistory != null)
				foreach (var folder in Settings.Default.DirectoryHistory)
					AddFolderToHistory(folder, true);

			if (inputFolders != null)
			{
				foreach (var folder in inputFolders)
				{
					AddFolderToList(folder);
				}
			}

			IncludeHidden.Checked = configuration.IncludeHidden;
			Recursive.Checked = configuration.IncludeSubfolders;

			FirstRunLabel.Visible = Settings.Default.FirstRun;
			StartButton.Enabled = !Settings.Default.FirstRun;
		}
	}
}
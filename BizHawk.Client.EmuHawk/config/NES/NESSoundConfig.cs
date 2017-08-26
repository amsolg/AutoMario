﻿using System;
using System.Windows.Forms;

using BizHawk.Emulation.Cores.Nintendo.NES;
using BizHawk.Emulation.Common;

namespace BizHawk.Client.EmuHawk
{
	public partial class NESSoundConfig : Form, IToolForm
	{
		[RequiredService]
		private NES NES { get; set; }

		private NES.NESSettings _oldSettings;
		private NES.NESSettings _settings;

		public bool AskSaveChanges() { return true; }
		public bool UpdateBefore => false;

		public void UpdateValues()
		{
		}

		public void NewUpdate(ToolFormUpdateType type) { }

		public void FastUpdate()
		{
		}

		public void Restart()
		{
			NESSoundConfig_Load(null, null);
		}

		public NESSoundConfig()
		{
			InitializeComponent();

			// get baseline maxes from a default config object
			var d = new NES.NESSettings();
			trackBar1.Maximum = d.Square1;
			trackBar2.Maximum = d.Square2;
			trackBar3.Maximum = d.Triangle;
			trackBar4.Maximum = d.Noise;
			trackBar5.Maximum = d.DMC;
		}

		private void NESSoundConfig_Load(object sender, EventArgs e)
		{
			_oldSettings = NES.GetSettings();
			_settings = _oldSettings.Clone();

			trackBar1.Value = _settings.Square1;
			trackBar2.Value = _settings.Square2;
			trackBar3.Value = _settings.Triangle;
			trackBar4.Value = _settings.Noise;
			trackBar5.Value = _settings.DMC;
		}

		private void Ok_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void Cancel_Click(object sender, EventArgs e)
		{
			// restore previous value
			NES.PutSettings(_oldSettings);
			Close();
		}

		private void TrackBar1_ValueChanged(object sender, EventArgs e)
		{
			label6.Text = trackBar1.Value.ToString();
			_settings.Square1 = trackBar1.Value;
			NES.PutSettings(_settings);
		}

		private void TrackBar2_ValueChanged(object sender, EventArgs e)
		{
			label7.Text = trackBar2.Value.ToString();
			_settings.Square2 = trackBar2.Value;
			NES.PutSettings(_settings);
		}

		private void TrackBar3_ValueChanged(object sender, EventArgs e)
		{
			label8.Text = trackBar3.Value.ToString();
			_settings.Triangle = trackBar3.Value;
			NES.PutSettings(_settings);
		}

		private void TrackBar4_ValueChanged(object sender, EventArgs e)
		{
			label9.Text = trackBar4.Value.ToString();
			_settings.Noise = trackBar4.Value;
			NES.PutSettings(_settings);
		}

		private void TrackBar5_ValueChanged(object sender, EventArgs e)
		{
			label10.Text = trackBar5.Value.ToString();
			_settings.DMC = trackBar5.Value;
			NES.PutSettings(_settings);
		}
	}
}

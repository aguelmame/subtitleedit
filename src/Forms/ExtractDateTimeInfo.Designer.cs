﻿namespace Nikse.SubtitleEdit.Forms
{
    partial class ExtractDateTimeInfo
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.buttonOpenVideo = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.labelDescription = new System.Windows.Forms.Label();
            this.comboBoxDateTimeFormats = new System.Windows.Forms.ComboBox();
            this.labelWriteFormat = new System.Windows.Forms.Label();
            this.labelVideoFileName = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.labelExample = new System.Windows.Forms.Label();
            this.textBoxExample = new System.Windows.Forms.TextBox();
            this.labelDuration = new System.Windows.Forms.Label();
            this.timeUpDownDuration = new Nikse.SubtitleEdit.Controls.TimeUpDown();
            this.timeUpDownStartTime = new Nikse.SubtitleEdit.Controls.TimeUpDown();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // buttonOpenVideo
            // 
            this.buttonOpenVideo.Location = new System.Drawing.Point(12, 29);
            this.buttonOpenVideo.Name = "buttonOpenVideo";
            this.buttonOpenVideo.Size = new System.Drawing.Size(28, 22);
            this.buttonOpenVideo.TabIndex = 1;
            this.buttonOpenVideo.Text = "...";
            this.buttonOpenVideo.UseVisualStyleBackColor = true;
            this.buttonOpenVideo.Click += new System.EventHandler(this.buttonOpenVideo_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonCancel.Location = new System.Drawing.Point(269, 269);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 21);
            this.buttonCancel.TabIndex = 53;
            this.buttonCancel.Text = "C&ancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonOK.Location = new System.Drawing.Point(96, 269);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(167, 21);
            this.buttonOK.TabIndex = 52;
            this.buttonOK.Text = "&Generate subtitle";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // labelDescription
            // 
            this.labelDescription.AutoSize = true;
            this.labelDescription.Location = new System.Drawing.Point(13, 13);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(226, 13);
            this.labelDescription.TabIndex = 54;
            this.labelDescription.Text = "Choose video file to extract date/time info from";
            // 
            // comboBoxDateTimeFormats
            // 
            this.comboBoxDateTimeFormats.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDateTimeFormats.FormattingEnabled = true;
            this.comboBoxDateTimeFormats.Items.AddRange(new object[] {
            "M/d/yyyy hh:mm:ss ",
            "M/d/yyyy<br />hh:mm:ss",
            "F  (Tuesday, April 10, 2012 9:15:07 PM)",
            "dd MMM yyyy, HH:mm:ss (10 Apr 2012 21:15:07)",
            "Current culture",
            "hh:mm:ss",
            "HH:mm:ss"});
            this.comboBoxDateTimeFormats.Location = new System.Drawing.Point(12, 153);
            this.comboBoxDateTimeFormats.Name = "comboBoxDateTimeFormats";
            this.comboBoxDateTimeFormats.Size = new System.Drawing.Size(332, 21);
            this.comboBoxDateTimeFormats.TabIndex = 55;
            this.comboBoxDateTimeFormats.SelectedIndexChanged += new System.EventHandler(this.comboBoxDateTimeFormats_SelectedIndexChanged);
            // 
            // labelWriteFormat
            // 
            this.labelWriteFormat.AutoSize = true;
            this.labelWriteFormat.Location = new System.Drawing.Point(12, 137);
            this.labelWriteFormat.Name = "labelWriteFormat";
            this.labelWriteFormat.Size = new System.Drawing.Size(143, 13);
            this.labelWriteFormat.TabIndex = 56;
            this.labelWriteFormat.Text = "Date/time format write format";
            // 
            // labelVideoFileName
            // 
            this.labelVideoFileName.AutoSize = true;
            this.labelVideoFileName.Location = new System.Drawing.Point(46, 34);
            this.labelVideoFileName.Name = "labelVideoFileName";
            this.labelVideoFileName.Size = new System.Drawing.Size(100, 13);
            this.labelVideoFileName.TabIndex = 57;
            this.labelVideoFileName.Text = "labelVideoFileName";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "yyyy-MM-dd";
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(12, 87);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(101, 20);
            this.dateTimePicker1.TabIndex = 58;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 59;
            this.label2.Text = "Start from";
            // 
            // labelExample
            // 
            this.labelExample.AutoSize = true;
            this.labelExample.Location = new System.Drawing.Point(12, 188);
            this.labelExample.Name = "labelExample";
            this.labelExample.Size = new System.Drawing.Size(47, 13);
            this.labelExample.TabIndex = 61;
            this.labelExample.Text = "Example";
            // 
            // textBoxExample
            // 
            this.textBoxExample.BackColor = System.Drawing.SystemColors.MenuText;
            this.textBoxExample.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxExample.ForeColor = System.Drawing.Color.White;
            this.textBoxExample.Location = new System.Drawing.Point(12, 204);
            this.textBoxExample.Multiline = true;
            this.textBoxExample.Name = "textBoxExample";
            this.textBoxExample.Size = new System.Drawing.Size(332, 53);
            this.textBoxExample.TabIndex = 62;
            // 
            // labelDuration
            // 
            this.labelDuration.AutoSize = true;
            this.labelDuration.Location = new System.Drawing.Point(249, 70);
            this.labelDuration.Name = "labelDuration";
            this.labelDuration.Size = new System.Drawing.Size(47, 13);
            this.labelDuration.TabIndex = 64;
            this.labelDuration.Text = "Duration";
            // 
            // timeUpDownDuration
            // 
            this.timeUpDownDuration.AutoSize = true;
            this.timeUpDownDuration.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.timeUpDownDuration.Location = new System.Drawing.Point(252, 87);
            this.timeUpDownDuration.Margin = new System.Windows.Forms.Padding(4);
            this.timeUpDownDuration.Name = "timeUpDownDuration";
            this.timeUpDownDuration.Size = new System.Drawing.Size(92, 24);
            this.timeUpDownDuration.TabIndex = 65;
            // 
            // timeUpDownStartTime
            // 
            this.timeUpDownStartTime.AutoSize = true;
            this.timeUpDownStartTime.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.timeUpDownStartTime.Location = new System.Drawing.Point(118, 86);
            this.timeUpDownStartTime.Margin = new System.Windows.Forms.Padding(4);
            this.timeUpDownStartTime.Name = "timeUpDownStartTime";
            this.timeUpDownStartTime.Size = new System.Drawing.Size(92, 24);
            this.timeUpDownStartTime.TabIndex = 63;
            // 
            // ExtractDateTimeInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(356, 302);
            this.Controls.Add(this.timeUpDownDuration);
            this.Controls.Add(this.labelDuration);
            this.Controls.Add(this.timeUpDownStartTime);
            this.Controls.Add(this.textBoxExample);
            this.Controls.Add(this.labelExample);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.labelVideoFileName);
            this.Controls.Add(this.labelWriteFormat);
            this.Controls.Add(this.comboBoxDateTimeFormats);
            this.Controls.Add(this.labelDescription);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonOpenVideo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExtractDateTimeInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Extract date and time information";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button buttonOpenVideo;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.ComboBox comboBoxDateTimeFormats;
        private System.Windows.Forms.Label labelWriteFormat;
        private System.Windows.Forms.Label labelVideoFileName;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelExample;
        private System.Windows.Forms.TextBox textBoxExample;
        private Controls.TimeUpDown timeUpDownStartTime;
        private System.Windows.Forms.Label labelDuration;
        private Controls.TimeUpDown timeUpDownDuration;
    }
}
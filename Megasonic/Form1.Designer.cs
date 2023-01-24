namespace Megasonic
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.soundButton = new System.Windows.Forms.Button();
            this.soundLabel = new System.Windows.Forms.Label();
            this.soundDialog = new System.Windows.Forms.OpenFileDialog();
            this.imageLabel = new System.Windows.Forms.Label();
            this.imageButton = new System.Windows.Forms.Button();
            this.lineLabel = new System.Windows.Forms.Label();
            this.lineButton = new System.Windows.Forms.Button();
            this.imageDialog = new System.Windows.Forms.OpenFileDialog();
            this.lineDialog = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label14 = new System.Windows.Forms.Label();
            this.frameWidthNumeric = new System.Windows.Forms.NumericUpDown();
            this.frameHeightNumeric = new System.Windows.Forms.NumericUpDown();
            this.barWidthNumeric = new System.Windows.Forms.NumericUpDown();
            this.titleTextbox = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.decayExponentNumeric = new System.Windows.Forms.NumericUpDown();
            this.noteRangeEndNumeric = new System.Windows.Forms.NumericUpDown();
            this.decayTimeNumeric = new System.Windows.Forms.NumericUpDown();
            this.sourcePicture = new System.Windows.Forms.PictureBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.noteRangeStartNumeric = new System.Windows.Forms.NumericUpDown();
            this.colorStartNumeric = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.colorLengthNumeric = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.outputPicture = new System.Windows.Forms.PictureBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label13 = new System.Windows.Forms.Label();
            this.windowCombobox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lookaheadNumeric = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.framerateNumeric = new System.Windows.Forms.NumericUpDown();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.soundAnalyzeButton = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.videoRenderButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.videoButton = new System.Windows.Forms.Button();
            this.videoDialog = new System.Windows.Forms.SaveFileDialog();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openProjectDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveProjectDialog = new System.Windows.Forms.SaveFileDialog();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.applyPropertiesButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.frameWidthNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.frameHeightNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barWidthNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.decayExponentNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.noteRangeEndNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.decayTimeNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourcePicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.noteRangeStartNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.colorStartNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.colorLengthNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.outputPicture)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lookaheadNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.framerateNumeric)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // soundButton
            // 
            this.soundButton.Location = new System.Drawing.Point(132, 36);
            this.soundButton.Name = "soundButton";
            this.soundButton.Size = new System.Drawing.Size(588, 36);
            this.soundButton.TabIndex = 3;
            this.soundButton.Text = "<choose file>";
            this.soundButton.UseVisualStyleBackColor = true;
            this.soundButton.Click += new System.EventHandler(this.audioButton_Click);
            // 
            // soundLabel
            // 
            this.soundLabel.Location = new System.Drawing.Point(12, 36);
            this.soundLabel.Name = "soundLabel";
            this.soundLabel.Size = new System.Drawing.Size(108, 36);
            this.soundLabel.TabIndex = 4;
            this.soundLabel.Text = "sound";
            this.soundLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // soundDialog
            // 
            this.soundDialog.Filter = "Audio files|*.wav;*.mp3";
            // 
            // imageLabel
            // 
            this.imageLabel.Location = new System.Drawing.Point(12, 36);
            this.imageLabel.Name = "imageLabel";
            this.imageLabel.Size = new System.Drawing.Size(108, 36);
            this.imageLabel.TabIndex = 7;
            this.imageLabel.Text = "image";
            this.imageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // imageButton
            // 
            this.imageButton.Location = new System.Drawing.Point(132, 36);
            this.imageButton.Name = "imageButton";
            this.imageButton.Size = new System.Drawing.Size(588, 36);
            this.imageButton.TabIndex = 6;
            this.imageButton.Text = "<choose file>";
            this.imageButton.UseVisualStyleBackColor = true;
            this.imageButton.Click += new System.EventHandler(this.imageButton_Click);
            // 
            // lineLabel
            // 
            this.lineLabel.Location = new System.Drawing.Point(12, 84);
            this.lineLabel.Name = "lineLabel";
            this.lineLabel.Size = new System.Drawing.Size(108, 36);
            this.lineLabel.TabIndex = 9;
            this.lineLabel.Text = "line";
            this.lineLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lineButton
            // 
            this.lineButton.Location = new System.Drawing.Point(132, 84);
            this.lineButton.Name = "lineButton";
            this.lineButton.Size = new System.Drawing.Size(588, 36);
            this.lineButton.TabIndex = 8;
            this.lineButton.Text = "<choose file>";
            this.lineButton.UseVisualStyleBackColor = true;
            this.lineButton.Click += new System.EventHandler(this.lineButton_Click);
            // 
            // imageDialog
            // 
            this.imageDialog.Filter = "Image files|*.png;*.jpg";
            // 
            // lineDialog
            // 
            this.lineDialog.Filter = "Vector files|*.svg";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.imageButton);
            this.groupBox1.Controls.Add(this.lineLabel);
            this.groupBox1.Controls.Add(this.imageLabel);
            this.groupBox1.Controls.Add(this.lineButton);
            this.groupBox1.Controls.Add(this.frameWidthNumeric);
            this.groupBox1.Controls.Add(this.frameHeightNumeric);
            this.groupBox1.Location = new System.Drawing.Point(12, 48);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(732, 180);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Video Source Files";
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(12, 132);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(228, 36);
            this.label14.TabIndex = 21;
            this.label14.Text = "frame width, height";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // frameWidthNumeric
            // 
            this.frameWidthNumeric.Location = new System.Drawing.Point(252, 132);
            this.frameWidthNumeric.Maximum = new decimal(new int[] {
            3840,
            0,
            0,
            0});
            this.frameWidthNumeric.Minimum = new decimal(new int[] {
            240,
            0,
            0,
            0});
            this.frameWidthNumeric.Name = "frameWidthNumeric";
            this.frameWidthNumeric.Size = new System.Drawing.Size(108, 31);
            this.frameWidthNumeric.TabIndex = 19;
            this.frameWidthNumeric.Value = new decimal(new int[] {
            1280,
            0,
            0,
            0});
            // 
            // frameHeightNumeric
            // 
            this.frameHeightNumeric.Location = new System.Drawing.Point(372, 132);
            this.frameHeightNumeric.Maximum = new decimal(new int[] {
            3840,
            0,
            0,
            0});
            this.frameHeightNumeric.Minimum = new decimal(new int[] {
            240,
            0,
            0,
            0});
            this.frameHeightNumeric.Name = "frameHeightNumeric";
            this.frameHeightNumeric.Size = new System.Drawing.Size(108, 31);
            this.frameHeightNumeric.TabIndex = 20;
            this.frameHeightNumeric.Value = new decimal(new int[] {
            720,
            0,
            0,
            0});
            // 
            // barWidthNumeric
            // 
            this.barWidthNumeric.Location = new System.Drawing.Point(252, 516);
            this.barWidthNumeric.Maximum = new decimal(new int[] {
            3840,
            0,
            0,
            0});
            this.barWidthNumeric.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.barWidthNumeric.Name = "barWidthNumeric";
            this.barWidthNumeric.Size = new System.Drawing.Size(228, 31);
            this.barWidthNumeric.TabIndex = 26;
            this.barWidthNumeric.Value = new decimal(new int[] {
            128,
            0,
            0,
            0});
            // 
            // titleTextbox
            // 
            this.titleTextbox.Location = new System.Drawing.Point(252, 36);
            this.titleTextbox.Name = "titleTextbox";
            this.titleTextbox.Size = new System.Drawing.Size(228, 31);
            this.titleTextbox.TabIndex = 25;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(12, 84);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(108, 36);
            this.label11.TabIndex = 10;
            this.label11.Text = "preview 1";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(12, 900);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(228, 36);
            this.label8.TabIndex = 14;
            this.label8.Text = "decay exponent, time (fr)";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // decayExponentNumeric
            // 
            this.decayExponentNumeric.Location = new System.Drawing.Point(252, 900);
            this.decayExponentNumeric.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.decayExponentNumeric.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.decayExponentNumeric.Name = "decayExponentNumeric";
            this.decayExponentNumeric.Size = new System.Drawing.Size(108, 31);
            this.decayExponentNumeric.TabIndex = 21;
            this.decayExponentNumeric.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // noteRangeEndNumeric
            // 
            this.noteRangeEndNumeric.Location = new System.Drawing.Point(372, 420);
            this.noteRangeEndNumeric.Maximum = new decimal(new int[] {
            108,
            0,
            0,
            0});
            this.noteRangeEndNumeric.Minimum = new decimal(new int[] {
            108,
            0,
            0,
            -2147483648});
            this.noteRangeEndNumeric.Name = "noteRangeEndNumeric";
            this.noteRangeEndNumeric.Size = new System.Drawing.Size(108, 31);
            this.noteRangeEndNumeric.TabIndex = 24;
            this.noteRangeEndNumeric.Value = new decimal(new int[] {
            42,
            0,
            0,
            0});
            // 
            // decayTimeNumeric
            // 
            this.decayTimeNumeric.Location = new System.Drawing.Point(372, 900);
            this.decayTimeNumeric.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.decayTimeNumeric.Name = "decayTimeNumeric";
            this.decayTimeNumeric.Size = new System.Drawing.Size(108, 31);
            this.decayTimeNumeric.TabIndex = 22;
            this.decayTimeNumeric.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // sourcePicture
            // 
            this.sourcePicture.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.sourcePicture.Location = new System.Drawing.Point(132, 84);
            this.sourcePicture.Name = "sourcePicture";
            this.sourcePicture.Size = new System.Drawing.Size(588, 324);
            this.sourcePicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.sourcePicture.TabIndex = 0;
            this.sourcePicture.TabStop = false;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(12, 468);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(228, 36);
            this.label6.TabIndex = 12;
            this.label6.Text = "color start, length";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(12, 516);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(228, 36);
            this.label7.TabIndex = 13;
            this.label7.Text = "bar relative width (1/x)";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // noteRangeStartNumeric
            // 
            this.noteRangeStartNumeric.Location = new System.Drawing.Point(252, 420);
            this.noteRangeStartNumeric.Maximum = new decimal(new int[] {
            108,
            0,
            0,
            0});
            this.noteRangeStartNumeric.Minimum = new decimal(new int[] {
            108,
            0,
            0,
            -2147483648});
            this.noteRangeStartNumeric.Name = "noteRangeStartNumeric";
            this.noteRangeStartNumeric.Size = new System.Drawing.Size(108, 31);
            this.noteRangeStartNumeric.TabIndex = 23;
            this.noteRangeStartNumeric.Value = new decimal(new int[] {
            30,
            0,
            0,
            -2147483648});
            // 
            // colorStartNumeric
            // 
            this.colorStartNumeric.Location = new System.Drawing.Point(252, 468);
            this.colorStartNumeric.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.colorStartNumeric.Name = "colorStartNumeric";
            this.colorStartNumeric.Size = new System.Drawing.Size(108, 31);
            this.colorStartNumeric.TabIndex = 17;
            this.colorStartNumeric.Value = new decimal(new int[] {
            160,
            0,
            0,
            0});
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(12, 420);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(228, 36);
            this.label9.TabIndex = 15;
            this.label9.Text = "note range";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // colorLengthNumeric
            // 
            this.colorLengthNumeric.Location = new System.Drawing.Point(372, 468);
            this.colorLengthNumeric.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.colorLengthNumeric.Name = "colorLengthNumeric";
            this.colorLengthNumeric.Size = new System.Drawing.Size(108, 31);
            this.colorLengthNumeric.TabIndex = 18;
            this.colorLengthNumeric.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(12, 36);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(228, 36);
            this.label10.TabIndex = 16;
            this.label10.Text = "title";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(12, 564);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(108, 36);
            this.label12.TabIndex = 12;
            this.label12.Text = "preview 2";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // outputPicture
            // 
            this.outputPicture.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.outputPicture.Location = new System.Drawing.Point(132, 564);
            this.outputPicture.Name = "outputPicture";
            this.outputPicture.Size = new System.Drawing.Size(588, 324);
            this.outputPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.outputPicture.TabIndex = 11;
            this.outputPicture.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.windowCombobox);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.lookaheadNumeric);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.framerateNumeric);
            this.groupBox2.Controls.Add(this.progressBar1);
            this.groupBox2.Controls.Add(this.soundAnalyzeButton);
            this.groupBox2.Controls.Add(this.soundLabel);
            this.groupBox2.Controls.Add(this.soundButton);
            this.groupBox2.Location = new System.Drawing.Point(12, 240);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(732, 324);
            this.groupBox2.TabIndex = 25;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Sound Source File";
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(12, 180);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(228, 36);
            this.label13.TabIndex = 23;
            this.label13.Text = "window (for FFT)";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // windowCombobox
            // 
            this.windowCombobox.FormattingEnabled = true;
            this.windowCombobox.Location = new System.Drawing.Point(252, 180);
            this.windowCombobox.Name = "windowCombobox";
            this.windowCombobox.Size = new System.Drawing.Size(228, 33);
            this.windowCombobox.TabIndex = 22;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 132);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(228, 36);
            this.label2.TabIndex = 8;
            this.label2.Text = "lookahead (fr)";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lookaheadNumeric
            // 
            this.lookaheadNumeric.Location = new System.Drawing.Point(252, 132);
            this.lookaheadNumeric.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.lookaheadNumeric.Name = "lookaheadNumeric";
            this.lookaheadNumeric.Size = new System.Drawing.Size(228, 31);
            this.lookaheadNumeric.TabIndex = 21;
            this.lookaheadNumeric.UseWaitCursor = true;
            this.lookaheadNumeric.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(228, 36);
            this.label1.TabIndex = 7;
            this.label1.Text = "framerate";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // framerateNumeric
            // 
            this.framerateNumeric.Location = new System.Drawing.Point(252, 84);
            this.framerateNumeric.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.framerateNumeric.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.framerateNumeric.Name = "framerateNumeric";
            this.framerateNumeric.Size = new System.Drawing.Size(228, 31);
            this.framerateNumeric.TabIndex = 20;
            this.framerateNumeric.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 276);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(708, 34);
            this.progressBar1.TabIndex = 6;
            // 
            // soundAnalyzeButton
            // 
            this.soundAnalyzeButton.Enabled = false;
            this.soundAnalyzeButton.Location = new System.Drawing.Point(12, 228);
            this.soundAnalyzeButton.Name = "soundAnalyzeButton";
            this.soundAnalyzeButton.Size = new System.Drawing.Size(708, 36);
            this.soundAnalyzeButton.TabIndex = 5;
            this.soundAnalyzeButton.Text = "Analyze";
            this.soundAnalyzeButton.UseVisualStyleBackColor = true;
            this.soundAnalyzeButton.Click += new System.EventHandler(this.soundAnalyze_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.progressBar2);
            this.groupBox3.Controls.Add(this.videoRenderButton);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.videoButton);
            this.groupBox3.Enabled = false;
            this.groupBox3.Location = new System.Drawing.Point(12, 576);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(732, 180);
            this.groupBox3.TabIndex = 26;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Video Output File";
            // 
            // progressBar2
            // 
            this.progressBar2.Location = new System.Drawing.Point(12, 132);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(708, 34);
            this.progressBar2.TabIndex = 11;
            // 
            // videoRenderButton
            // 
            this.videoRenderButton.Enabled = false;
            this.videoRenderButton.Location = new System.Drawing.Point(12, 84);
            this.videoRenderButton.Name = "videoRenderButton";
            this.videoRenderButton.Size = new System.Drawing.Size(708, 36);
            this.videoRenderButton.TabIndex = 10;
            this.videoRenderButton.Text = "Render";
            this.videoRenderButton.UseVisualStyleBackColor = true;
            this.videoRenderButton.Click += new System.EventHandler(this.videoRenderButton_Click);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(12, 36);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(108, 36);
            this.label5.TabIndex = 11;
            this.label5.Text = "video";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // videoButton
            // 
            this.videoButton.Location = new System.Drawing.Point(132, 36);
            this.videoButton.Name = "videoButton";
            this.videoButton.Size = new System.Drawing.Size(588, 36);
            this.videoButton.TabIndex = 10;
            this.videoButton.Text = "<choose file>";
            this.videoButton.UseVisualStyleBackColor = true;
            this.videoButton.Click += new System.EventHandler(this.videoButton_Click);
            // 
            // videoDialog
            // 
            this.videoDialog.FileName = "untitled";
            this.videoDialog.Filter = "Video files|*.mp4";
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1501, 33);
            this.menuStrip1.TabIndex = 28;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(54, 29);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(158, 34);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(158, 34);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // openProjectDialog
            // 
            this.openProjectDialog.FileName = "<choose file>";
            this.openProjectDialog.Filter = "MegaSonic Project Files|*.meso";
            // 
            // saveProjectDialog
            // 
            this.saveProjectDialog.FileName = "untitled";
            this.saveProjectDialog.Filter = "MegaSonic Project Files|*.meso";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.applyPropertiesButton);
            this.groupBox4.Controls.Add(this.barWidthNumeric);
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Controls.Add(this.outputPicture);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.titleTextbox);
            this.groupBox4.Controls.Add(this.label12);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.colorLengthNumeric);
            this.groupBox4.Controls.Add(this.decayExponentNumeric);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.noteRangeEndNumeric);
            this.groupBox4.Controls.Add(this.colorStartNumeric);
            this.groupBox4.Controls.Add(this.decayTimeNumeric);
            this.groupBox4.Controls.Add(this.noteRangeStartNumeric);
            this.groupBox4.Controls.Add(this.sourcePicture);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Enabled = false;
            this.groupBox4.Location = new System.Drawing.Point(756, 48);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(732, 996);
            this.groupBox4.TabIndex = 29;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Parameters";
            // 
            // applyPropertiesButton
            // 
            this.applyPropertiesButton.Enabled = false;
            this.applyPropertiesButton.Location = new System.Drawing.Point(12, 948);
            this.applyPropertiesButton.Name = "applyPropertiesButton";
            this.applyPropertiesButton.Size = new System.Drawing.Size(708, 36);
            this.applyPropertiesButton.TabIndex = 12;
            this.applyPropertiesButton.Text = "Apply Changes";
            this.applyPropertiesButton.UseVisualStyleBackColor = true;
            this.applyPropertiesButton.Click += new System.EventHandler(this.applyPropertiesButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1501, 1056);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.frameWidthNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.frameHeightNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barWidthNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.decayExponentNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.noteRangeEndNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.decayTimeNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourcePicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.noteRangeStartNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.colorStartNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.colorLengthNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.outputPicture)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lookaheadNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.framerateNumeric)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Button soundButton;
        private Label soundLabel;
        private OpenFileDialog soundDialog;
        private Label imageLabel;
        private Button imageButton;
        private Label lineLabel;
        private Button lineButton;
        private OpenFileDialog imageDialog;
        private OpenFileDialog lineDialog;
        private GroupBox groupBox1;
        private PictureBox sourcePicture;
        private GroupBox groupBox2;
        private Label label2;
        private Label label1;
        private ProgressBar progressBar1;
        private Button soundAnalyzeButton;
        private GroupBox groupBox3;
        private Label label10;
        private Label label9;
        private Label label8;
        private Label label7;
        private Label label6;
        private Label label5;
        private Button videoButton;
        private ProgressBar progressBar2;
        private Button videoRenderButton;
        private Label label11;
        private Label label12;
        private PictureBox outputPicture;
        private SaveFileDialog videoDialog;
        private NumericUpDown noteRangeEndNumeric;
        private NumericUpDown noteRangeStartNumeric;
        private NumericUpDown decayTimeNumeric;
        private NumericUpDown decayExponentNumeric;
        private NumericUpDown frameHeightNumeric;
        private NumericUpDown frameWidthNumeric;
        private NumericUpDown colorLengthNumeric;
        private NumericUpDown colorStartNumeric;
        private NumericUpDown barWidthNumeric;
        private TextBox titleTextbox;
        private ComboBox windowCombobox;
        private NumericUpDown lookaheadNumeric;
        private NumericUpDown framerateNumeric;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private OpenFileDialog openProjectDialog;
        private SaveFileDialog saveProjectDialog;
        private Label label13;
        private Label label14;
        private GroupBox groupBox4;
        private Button applyPropertiesButton;
    }
}
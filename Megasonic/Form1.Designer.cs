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
            this.label11 = new System.Windows.Forms.Label();
            this.sourcePicture = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.frameWidthNumeric = new System.Windows.Forms.NumericUpDown();
            this.frameHeightNumeric = new System.Windows.Forms.NumericUpDown();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDown11 = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown10 = new System.Windows.Forms.NumericUpDown();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.soundAnalyzeButton = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.numericUpDown9 = new System.Windows.Forms.NumericUpDown();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.numericUpDown7 = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.numericUpDown5 = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.numericUpDown8 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown6 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.outputPicture = new System.Windows.Forms.PictureBox();
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.label10 = new System.Windows.Forms.Label();
            this.videoRenderButton = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.videoButton = new System.Windows.Forms.Button();
            this.videoDialog = new System.Windows.Forms.SaveFileDialog();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sourcePicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.frameWidthNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.frameHeightNumeric)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown10)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.outputPicture)).BeginInit();
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
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.sourcePicture);
            this.groupBox1.Controls.Add(this.imageButton);
            this.groupBox1.Controls.Add(this.lineLabel);
            this.groupBox1.Controls.Add(this.imageLabel);
            this.groupBox1.Controls.Add(this.lineButton);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.frameWidthNumeric);
            this.groupBox1.Controls.Add(this.frameHeightNumeric);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(732, 528);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Video Source Files";
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(12, 180);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(108, 36);
            this.label11.TabIndex = 10;
            this.label11.Text = "preview";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // sourcePicture
            // 
            this.sourcePicture.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.sourcePicture.Location = new System.Drawing.Point(132, 180);
            this.sourcePicture.Name = "sourcePicture";
            this.sourcePicture.Size = new System.Drawing.Size(588, 336);
            this.sourcePicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.sourcePicture.TabIndex = 0;
            this.sourcePicture.TabStop = false;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(12, 132);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(228, 36);
            this.label4.TabIndex = 10;
            this.label4.Text = "frame width, height";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // frameWidthNumeric
            // 
            this.frameWidthNumeric.Location = new System.Drawing.Point(252, 132);
            this.frameWidthNumeric.Maximum = new decimal(new int[] {
            3940,
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
            this.frameWidthNumeric.ValueChanged += new System.EventHandler(this.frameWidthNumeric_ValueChanged);
            // 
            // frameHeightNumeric
            // 
            this.frameHeightNumeric.Location = new System.Drawing.Point(372, 132);
            this.frameHeightNumeric.Maximum = new decimal(new int[] {
            3940,
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
            this.frameHeightNumeric.ValueChanged += new System.EventHandler(this.frameHeightNumeric_ValueChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.comboBox1);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.numericUpDown11);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.numericUpDown10);
            this.groupBox2.Controls.Add(this.progressBar1);
            this.groupBox2.Controls.Add(this.soundAnalyzeButton);
            this.groupBox2.Controls.Add(this.soundLabel);
            this.groupBox2.Controls.Add(this.soundButton);
            this.groupBox2.Location = new System.Drawing.Point(12, 552);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(732, 324);
            this.groupBox2.TabIndex = 25;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Sound Source File";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(252, 180);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(228, 33);
            this.comboBox1.TabIndex = 22;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 180);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(228, 36);
            this.label3.TabIndex = 9;
            this.label3.Text = "window";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 132);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(228, 36);
            this.label2.TabIndex = 8;
            this.label2.Text = "lookahead";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numericUpDown11
            // 
            this.numericUpDown11.Location = new System.Drawing.Point(252, 132);
            this.numericUpDown11.Name = "numericUpDown11";
            this.numericUpDown11.Size = new System.Drawing.Size(228, 31);
            this.numericUpDown11.TabIndex = 21;
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
            // numericUpDown10
            // 
            this.numericUpDown10.Location = new System.Drawing.Point(252, 84);
            this.numericUpDown10.Name = "numericUpDown10";
            this.numericUpDown10.Size = new System.Drawing.Size(228, 31);
            this.numericUpDown10.TabIndex = 20;
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
            this.groupBox3.Controls.Add(this.numericUpDown9);
            this.groupBox3.Controls.Add(this.textBox1);
            this.groupBox3.Controls.Add(this.numericUpDown7);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.numericUpDown5);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.numericUpDown8);
            this.groupBox3.Controls.Add(this.numericUpDown1);
            this.groupBox3.Controls.Add(this.numericUpDown6);
            this.groupBox3.Controls.Add(this.numericUpDown2);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.outputPicture);
            this.groupBox3.Controls.Add(this.progressBar2);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.videoRenderButton);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.videoButton);
            this.groupBox3.Enabled = false;
            this.groupBox3.Location = new System.Drawing.Point(756, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(732, 768);
            this.groupBox3.TabIndex = 26;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Video Output File";
            // 
            // numericUpDown9
            // 
            this.numericUpDown9.Location = new System.Drawing.Point(252, 228);
            this.numericUpDown9.Name = "numericUpDown9";
            this.numericUpDown9.Size = new System.Drawing.Size(228, 31);
            this.numericUpDown9.TabIndex = 26;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(252, 276);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(228, 31);
            this.textBox1.TabIndex = 25;
            // 
            // numericUpDown7
            // 
            this.numericUpDown7.Location = new System.Drawing.Point(372, 84);
            this.numericUpDown7.Name = "numericUpDown7";
            this.numericUpDown7.Size = new System.Drawing.Size(108, 31);
            this.numericUpDown7.TabIndex = 24;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(12, 132);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(228, 36);
            this.label8.TabIndex = 14;
            this.label8.Text = "decay exponent, time";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numericUpDown5
            // 
            this.numericUpDown5.Location = new System.Drawing.Point(372, 132);
            this.numericUpDown5.Name = "numericUpDown5";
            this.numericUpDown5.Size = new System.Drawing.Size(108, 31);
            this.numericUpDown5.TabIndex = 22;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(12, 180);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(228, 36);
            this.label6.TabIndex = 12;
            this.label6.Text = "color start, length";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(12, 228);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(228, 36);
            this.label7.TabIndex = 13;
            this.label7.Text = "bar relative width (1/x)";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numericUpDown8
            // 
            this.numericUpDown8.Location = new System.Drawing.Point(252, 84);
            this.numericUpDown8.Name = "numericUpDown8";
            this.numericUpDown8.Size = new System.Drawing.Size(108, 31);
            this.numericUpDown8.TabIndex = 23;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(252, 180);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(108, 31);
            this.numericUpDown1.TabIndex = 17;
            // 
            // numericUpDown6
            // 
            this.numericUpDown6.Location = new System.Drawing.Point(252, 132);
            this.numericUpDown6.Name = "numericUpDown6";
            this.numericUpDown6.Size = new System.Drawing.Size(108, 31);
            this.numericUpDown6.TabIndex = 21;
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Location = new System.Drawing.Point(372, 180);
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(108, 31);
            this.numericUpDown2.TabIndex = 18;
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(12, 324);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(108, 36);
            this.label12.TabIndex = 12;
            this.label12.Text = "preview";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // outputPicture
            // 
            this.outputPicture.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.outputPicture.Location = new System.Drawing.Point(132, 324);
            this.outputPicture.Name = "outputPicture";
            this.outputPicture.Size = new System.Drawing.Size(588, 336);
            this.outputPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.outputPicture.TabIndex = 11;
            this.outputPicture.TabStop = false;
            // 
            // progressBar2
            // 
            this.progressBar2.Location = new System.Drawing.Point(12, 720);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(708, 34);
            this.progressBar2.TabIndex = 11;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(12, 276);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(228, 36);
            this.label10.TabIndex = 16;
            this.label10.Text = "title";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // videoRenderButton
            // 
            this.videoRenderButton.Enabled = false;
            this.videoRenderButton.Location = new System.Drawing.Point(12, 672);
            this.videoRenderButton.Name = "videoRenderButton";
            this.videoRenderButton.Size = new System.Drawing.Size(708, 36);
            this.videoRenderButton.TabIndex = 10;
            this.videoRenderButton.Text = "Render";
            this.videoRenderButton.UseVisualStyleBackColor = true;
            this.videoRenderButton.Click += new System.EventHandler(this.videoRenderButton_Click);
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(12, 84);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(228, 36);
            this.label9.TabIndex = 15;
            this.label9.Text = "note borders";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2178, 1367);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sourcePicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.frameWidthNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.frameHeightNumeric)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown10)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.outputPicture)).EndInit();
            this.ResumeLayout(false);

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
        private Label label3;
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
        private Label label4;
        private ProgressBar progressBar2;
        private Button videoRenderButton;
        private Label label11;
        private Label label12;
        private PictureBox outputPicture;
        private SaveFileDialog videoDialog;
        private NumericUpDown numericUpDown7;
        private NumericUpDown numericUpDown8;
        private NumericUpDown numericUpDown5;
        private NumericUpDown numericUpDown6;
        private NumericUpDown frameHeightNumeric;
        private NumericUpDown frameWidthNumeric;
        private NumericUpDown numericUpDown2;
        private NumericUpDown numericUpDown1;
        private NumericUpDown numericUpDown9;
        private TextBox textBox1;
        private ComboBox comboBox1;
        private NumericUpDown numericUpDown11;
        private NumericUpDown numericUpDown10;
    }
}
namespace ZoomPanControl
{
    partial class Form1
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
            this.zoomControl1 = new ZoomPanControl.ZoomControl();
            this.SuspendLayout();
            // 
            // zoomControl1
            // 
            this.zoomControl1.Location = new System.Drawing.Point(12, 12);
            this.zoomControl1.MaxZoom = 20F;
            this.zoomControl1.MinZoom = 0.1F;
            this.zoomControl1.Name = "zoomControl1";
            this.zoomControl1.Size = new System.Drawing.Size(500, 500);
            this.zoomControl1.TabIndex = 0;
            this.zoomControl1.ZoomScale = 1F;
            this.zoomControl1.ZoomSensitivity = 0.2F;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(518, 518);
            this.Controls.Add(this.zoomControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private ZoomControl zoomControl1;
    }
}


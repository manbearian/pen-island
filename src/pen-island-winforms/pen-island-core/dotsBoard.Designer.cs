namespace PenIsland
{
    partial class DotsBoard
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // DotsBoard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "DotsBoard";
            this.Size = new System.Drawing.Size(305, 304);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.DotsBoard_Paint);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.DotsBoard_MouseClick);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.DotsBoard_MouseDoubleClick);
            this.Resize += new System.EventHandler(this.DotsBoard_Resize);
            this.ResumeLayout(false);

        }

        #endregion
    }
}

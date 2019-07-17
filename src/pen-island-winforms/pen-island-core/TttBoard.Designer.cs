namespace PenIsland
{
    partial class TttBoard
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
            // TttBoard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.DoubleBuffered = true;
            this.Name = "TttBoard";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.TTTBoard_Paint);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.TttBoard_MouseClick);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.TttBoard_MouseDoubleClick);
            this.ResumeLayout(false);

        }

        #endregion
    }
}

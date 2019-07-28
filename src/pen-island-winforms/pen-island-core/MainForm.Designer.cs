namespace PenIsland
{
    partial class MainForm
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.gameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newGameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newTicTacToeGameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newMatchLineGameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playerColorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dotsBoard = new PenIsland.DotsBoard();
            this.tttBoard = new PenIsland.TttBoard();
            this.c4Board = new PenIsland.C4Board();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gameToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(464, 33);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // gameToolStripMenuItem
            // 
            this.gameToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newGameToolStripMenuItem,
            this.newTicTacToeGameToolStripMenuItem,
            this.newMatchLineGameToolStripMenuItem,
            this.playerColorsToolStripMenuItem});
            this.gameToolStripMenuItem.Name = "gameToolStripMenuItem";
            this.gameToolStripMenuItem.Size = new System.Drawing.Size(74, 29);
            this.gameToolStripMenuItem.Text = "Game";
            // 
            // newGameToolStripMenuItem
            // 
            this.newGameToolStripMenuItem.Name = "newGameToolStripMenuItem";
            this.newGameToolStripMenuItem.Size = new System.Drawing.Size(303, 34);
            this.newGameToolStripMenuItem.Text = "New Dots Game...";
            this.newGameToolStripMenuItem.Click += new System.EventHandler(this.NewDotsGameToolStripMenuItem_Click);
            // 
            // newTicTacToeGameToolStripMenuItem
            // 
            this.newTicTacToeGameToolStripMenuItem.Name = "newTicTacToeGameToolStripMenuItem";
            this.newTicTacToeGameToolStripMenuItem.Size = new System.Drawing.Size(303, 34);
            this.newTicTacToeGameToolStripMenuItem.Text = "New Tic-Tac-Toe Game...";
            this.newTicTacToeGameToolStripMenuItem.Click += new System.EventHandler(this.NewTicTacToeGameToolStripMenuItem_Click);
            // 
            // newMatchLineGameToolStripMenuItem
            // 
            this.newMatchLineGameToolStripMenuItem.Name = "newMatchLineGameToolStripMenuItem";
            this.newMatchLineGameToolStripMenuItem.Size = new System.Drawing.Size(303, 34);
            this.newMatchLineGameToolStripMenuItem.Text = "New Match Line Game...";
            this.newMatchLineGameToolStripMenuItem.Click += new System.EventHandler(this.NewMatchLineGameToolStripMenuItem_Click);
            // 
            // playerColorsToolStripMenuItem
            // 
            this.playerColorsToolStripMenuItem.Name = "playerColorsToolStripMenuItem";
            this.playerColorsToolStripMenuItem.Size = new System.Drawing.Size(303, 34);
            this.playerColorsToolStripMenuItem.Text = "Player Settings...";
            this.playerColorsToolStripMenuItem.Click += new System.EventHandler(this.PlayerColorsToolStripMenuItem_Click);
            // 
            // dotsBoard
            // 
            this.dotsBoard.Location = new System.Drawing.Point(0, 40);
            this.dotsBoard.Name = "dotsBoard";
            this.dotsBoard.Size = new System.Drawing.Size(253, 274);
            this.dotsBoard.TabIndex = 1;
            this.dotsBoard.Visible = false;
            // 
            // tttBoard
            // 
            this.tttBoard.Location = new System.Drawing.Point(0, 40);
            this.tttBoard.Name = "tttBoard";
            this.tttBoard.Size = new System.Drawing.Size(284, 304);
            this.tttBoard.TabIndex = 2;
            this.tttBoard.Visible = false;
            // 
            // c4Board
            // 
            this.c4Board.Location = new System.Drawing.Point(0, 40);
            this.c4Board.Name = "c4Board";
            this.c4Board.Size = new System.Drawing.Size(242, 269);
            this.c4Board.TabIndex = 3;
            this.c4Board.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 511);
            this.Controls.Add(this.c4Board);
            this.Controls.Add(this.tttBoard);
            this.Controls.Add(this.dotsBoard);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Pen Island";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.MainForm_Paint);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem gameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newGameToolStripMenuItem;
        private DotsBoard dotsBoard;
        private System.Windows.Forms.ToolStripMenuItem playerColorsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newTicTacToeGameToolStripMenuItem;
        private TttBoard tttBoard;
        private System.Windows.Forms.ToolStripMenuItem newMatchLineGameToolStripMenuItem;
        private C4Board c4Board;
    }
}


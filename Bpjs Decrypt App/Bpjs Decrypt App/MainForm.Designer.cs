namespace Bpjs_Decrypt_App
{
    partial class MainForm
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
            components = new System.ComponentModel.Container();
            contextMenuStrip1 = new ContextMenuStrip(components);
            menuStrip1 = new MenuStrip();
            serviceToolStripMenuItem = new ToolStripMenuItem();
            pCareToolStripMenuItem = new ToolStripMenuItem();
            vClaimToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize = new Size(20, 20);
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(61, 4);
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { serviceToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(7, 2, 0, 2);
            menuStrip1.Size = new Size(900, 24);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // serviceToolStripMenuItem
            // 
            serviceToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { pCareToolStripMenuItem, vClaimToolStripMenuItem });
            serviceToolStripMenuItem.Name = "serviceToolStripMenuItem";
            serviceToolStripMenuItem.Size = new Size(61, 20);
            serviceToolStripMenuItem.Text = "Services";
            // 
            // pCareToolStripMenuItem
            // 
            pCareToolStripMenuItem.Name = "pCareToolStripMenuItem";
            pCareToolStripMenuItem.Size = new Size(112, 22);
            pCareToolStripMenuItem.Text = "PCare";
            pCareToolStripMenuItem.Click += pCareToolStripMenuItem_Click;
            // 
            // vClaimToolStripMenuItem
            // 
            vClaimToolStripMenuItem.Name = "vClaimToolStripMenuItem";
            vClaimToolStripMenuItem.Size = new Size(112, 22);
            vClaimToolStripMenuItem.Text = "VClaim";
            vClaimToolStripMenuItem.Click += vClaimToolStripMenuItem_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 14F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(900, 405);
            Controls.Add(menuStrip1);
            Font = new Font("Verdana", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            IsMdiContainer = true;
            MainMenuStrip = menuStrip1;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Main";
            WindowState = FormWindowState.Maximized;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ContextMenuStrip contextMenuStrip1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem serviceToolStripMenuItem;
        private ToolStripMenuItem pCareToolStripMenuItem;
        private ToolStripMenuItem vClaimToolStripMenuItem;
    }
}

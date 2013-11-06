namespace EdwardsOutline
{
   partial class EdwardsOutlineForm
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
         this.components = new System.ComponentModel.Container();
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EdwardsOutlineForm));
         this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
         this.splitContainer1 = new System.Windows.Forms.SplitContainer();
         this.m_tree = new System.Windows.Forms.TreeView();
         this.m_nodeContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
         this.m_cutContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.m_content = new System.Windows.Forms.RichTextBox();
         this.toolStrip1 = new System.Windows.Forms.ToolStrip();
         this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
         this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
         this.toolStripContainer1.ContentPanel.SuspendLayout();
         this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
         this.toolStripContainer1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
         this.splitContainer1.Panel1.SuspendLayout();
         this.splitContainer1.Panel2.SuspendLayout();
         this.splitContainer1.SuspendLayout();
         this.m_nodeContextMenu.SuspendLayout();
         this.toolStrip1.SuspendLayout();
         this.SuspendLayout();
         // 
         // toolStripContainer1
         // 
         // 
         // toolStripContainer1.ContentPanel
         // 
         this.toolStripContainer1.ContentPanel.Controls.Add(this.splitContainer1);
         this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(589, 398);
         this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
         this.toolStripContainer1.Name = "toolStripContainer1";
         this.toolStripContainer1.Size = new System.Drawing.Size(589, 423);
         this.toolStripContainer1.TabIndex = 0;
         this.toolStripContainer1.Text = "toolStripContainer1";
         // 
         // toolStripContainer1.TopToolStripPanel
         // 
         this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
         // 
         // splitContainer1
         // 
         this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.splitContainer1.Location = new System.Drawing.Point(0, 0);
         this.splitContainer1.Name = "splitContainer1";
         // 
         // splitContainer1.Panel1
         // 
         this.splitContainer1.Panel1.Controls.Add(this.m_tree);
         // 
         // splitContainer1.Panel2
         // 
         this.splitContainer1.Panel2.Controls.Add(this.m_content);
         this.splitContainer1.Size = new System.Drawing.Size(589, 398);
         this.splitContainer1.SplitterDistance = 196;
         this.splitContainer1.TabIndex = 0;
         // 
         // m_tree
         // 
         this.m_tree.Dock = System.Windows.Forms.DockStyle.Fill;
         this.m_tree.FullRowSelect = true;
         this.m_tree.HideSelection = false;
         this.m_tree.Location = new System.Drawing.Point(0, 0);
         this.m_tree.Name = "m_tree";
         this.m_tree.Size = new System.Drawing.Size(196, 398);
         this.m_tree.TabIndex = 0;
         this.m_tree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.m_tree_AfterSelect);
         this.m_tree.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.m_tree_NodeMouseClick);
         // 
         // m_nodeContextMenu
         // 
         this.m_nodeContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_cutContextMenuItem});
         this.m_nodeContextMenu.Name = "m_treeContextMenu";
         this.m_nodeContextMenu.Size = new System.Drawing.Size(153, 48);
         // 
         // m_cutContextMenuItem
         // 
         this.m_cutContextMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("m_cutContextMenuItem.Image")));
         this.m_cutContextMenuItem.Name = "m_cutContextMenuItem";
         this.m_cutContextMenuItem.Size = new System.Drawing.Size(152, 22);
         this.m_cutContextMenuItem.Text = "Cut";
         this.m_cutContextMenuItem.Click += new System.EventHandler(this.m_cutContextMenuItem_Click);
         // 
         // m_content
         // 
         this.m_content.Dock = System.Windows.Forms.DockStyle.Fill;
         this.m_content.Location = new System.Drawing.Point(0, 0);
         this.m_content.Name = "m_content";
         this.m_content.Size = new System.Drawing.Size(389, 398);
         this.m_content.TabIndex = 0;
         this.m_content.Text = "";
         // 
         // toolStrip1
         // 
         this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
         this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2});
         this.toolStrip1.Location = new System.Drawing.Point(3, 0);
         this.toolStrip1.Name = "toolStrip1";
         this.toolStrip1.Size = new System.Drawing.Size(114, 25);
         this.toolStrip1.TabIndex = 0;
         // 
         // toolStripButton1
         // 
         this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
         this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
         this.toolStripButton1.Name = "toolStripButton1";
         this.toolStripButton1.Size = new System.Drawing.Size(53, 22);
         this.toolStripButton1.Text = "Open";
         this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
         // 
         // toolStripButton2
         // 
         this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
         this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
         this.toolStripButton2.Name = "toolStripButton2";
         this.toolStripButton2.Size = new System.Drawing.Size(51, 22);
         this.toolStripButton2.Text = "Save";
         this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
         // 
         // EdwardsOutlineForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(589, 423);
         this.Controls.Add(this.toolStripContainer1);
         this.Name = "EdwardsOutlineForm";
         this.Text = "Edwards Outline";
         this.toolStripContainer1.ContentPanel.ResumeLayout(false);
         this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
         this.toolStripContainer1.TopToolStripPanel.PerformLayout();
         this.toolStripContainer1.ResumeLayout(false);
         this.toolStripContainer1.PerformLayout();
         this.splitContainer1.Panel1.ResumeLayout(false);
         this.splitContainer1.Panel2.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
         this.splitContainer1.ResumeLayout(false);
         this.m_nodeContextMenu.ResumeLayout(false);
         this.toolStrip1.ResumeLayout(false);
         this.toolStrip1.PerformLayout();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.ToolStripContainer toolStripContainer1;
      private System.Windows.Forms.SplitContainer splitContainer1;
      private System.Windows.Forms.TreeView m_tree;
      private System.Windows.Forms.RichTextBox m_content;
      private System.Windows.Forms.ToolStrip toolStrip1;
      private System.Windows.Forms.ToolStripButton toolStripButton1;
      private System.Windows.Forms.ContextMenuStrip m_nodeContextMenu;
      private System.Windows.Forms.ToolStripMenuItem m_cutContextMenuItem;
      private System.Windows.Forms.ToolStripButton toolStripButton2;
   }
}


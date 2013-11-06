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
         this.m_toolStripContainer = new System.Windows.Forms.ToolStripContainer();
         this.m_splitContainer = new System.Windows.Forms.SplitContainer();
         this.m_tree = new System.Windows.Forms.TreeView();
         this.m_content = new System.Windows.Forms.RichTextBox();
         this.m_toolStrip = new System.Windows.Forms.ToolStrip();
         this.m_openButton = new System.Windows.Forms.ToolStripButton();
         this.m_saveButton = new System.Windows.Forms.ToolStripButton();
         this.m_nodeContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
         this.m_cutContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.m_nodeImageList = new System.Windows.Forms.ImageList(this.components);
         this.m_toolStripContainer.ContentPanel.SuspendLayout();
         this.m_toolStripContainer.TopToolStripPanel.SuspendLayout();
         this.m_toolStripContainer.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.m_splitContainer)).BeginInit();
         this.m_splitContainer.Panel1.SuspendLayout();
         this.m_splitContainer.Panel2.SuspendLayout();
         this.m_splitContainer.SuspendLayout();
         this.m_toolStrip.SuspendLayout();
         this.m_nodeContextMenu.SuspendLayout();
         this.SuspendLayout();
         // 
         // m_toolStripContainer
         // 
         // 
         // m_toolStripContainer.ContentPanel
         // 
         this.m_toolStripContainer.ContentPanel.Controls.Add(this.m_splitContainer);
         this.m_toolStripContainer.ContentPanel.Size = new System.Drawing.Size(589, 398);
         this.m_toolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
         this.m_toolStripContainer.Location = new System.Drawing.Point(0, 0);
         this.m_toolStripContainer.Name = "m_toolStripContainer";
         this.m_toolStripContainer.Size = new System.Drawing.Size(589, 423);
         this.m_toolStripContainer.TabIndex = 0;
         this.m_toolStripContainer.Text = "toolStripContainer1";
         // 
         // m_toolStripContainer.TopToolStripPanel
         // 
         this.m_toolStripContainer.TopToolStripPanel.Controls.Add(this.m_toolStrip);
         // 
         // m_splitContainer
         // 
         this.m_splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
         this.m_splitContainer.Location = new System.Drawing.Point(0, 0);
         this.m_splitContainer.Name = "m_splitContainer";
         // 
         // m_splitContainer.Panel1
         // 
         this.m_splitContainer.Panel1.Controls.Add(this.m_tree);
         // 
         // m_splitContainer.Panel2
         // 
         this.m_splitContainer.Panel2.Controls.Add(this.m_content);
         this.m_splitContainer.Size = new System.Drawing.Size(589, 398);
         this.m_splitContainer.SplitterDistance = 196;
         this.m_splitContainer.TabIndex = 0;
         // 
         // m_tree
         // 
         this.m_tree.Dock = System.Windows.Forms.DockStyle.Fill;
         this.m_tree.FullRowSelect = true;
         this.m_tree.HideSelection = false;
         this.m_tree.ImageIndex = 0;
         this.m_tree.ImageList = this.m_nodeImageList;
         this.m_tree.LabelEdit = true;
         this.m_tree.Location = new System.Drawing.Point(0, 0);
         this.m_tree.Name = "m_tree";
         this.m_tree.SelectedImageIndex = 0;
         this.m_tree.Size = new System.Drawing.Size(196, 398);
         this.m_tree.TabIndex = 0;
         this.m_tree.BeforeLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.m_tree_BeforeLabelEdit);
         this.m_tree.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.m_tree_AfterLabelEdit);
         this.m_tree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.m_tree_AfterSelect);
         this.m_tree.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.m_tree_NodeMouseClick);
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
         // m_toolStrip
         // 
         this.m_toolStrip.Dock = System.Windows.Forms.DockStyle.None;
         this.m_toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_openButton,
            this.m_saveButton});
         this.m_toolStrip.Location = new System.Drawing.Point(3, 0);
         this.m_toolStrip.Name = "m_toolStrip";
         this.m_toolStrip.Size = new System.Drawing.Size(114, 25);
         this.m_toolStrip.TabIndex = 0;
         // 
         // m_openButton
         // 
         this.m_openButton.Image = ((System.Drawing.Image)(resources.GetObject("m_openButton.Image")));
         this.m_openButton.ImageTransparentColor = System.Drawing.Color.Magenta;
         this.m_openButton.Name = "m_openButton";
         this.m_openButton.Size = new System.Drawing.Size(53, 22);
         this.m_openButton.Text = "Open";
         this.m_openButton.Click += new System.EventHandler(this.toolStripButton1_Click);
         // 
         // m_saveButton
         // 
         this.m_saveButton.Image = ((System.Drawing.Image)(resources.GetObject("m_saveButton.Image")));
         this.m_saveButton.ImageTransparentColor = System.Drawing.Color.Magenta;
         this.m_saveButton.Name = "m_saveButton";
         this.m_saveButton.Size = new System.Drawing.Size(51, 22);
         this.m_saveButton.Text = "Save";
         this.m_saveButton.Click += new System.EventHandler(this.toolStripButton2_Click);
         // 
         // m_nodeContextMenu
         // 
         this.m_nodeContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_cutContextMenuItem});
         this.m_nodeContextMenu.Name = "m_treeContextMenu";
         this.m_nodeContextMenu.Size = new System.Drawing.Size(92, 26);
         // 
         // m_cutContextMenuItem
         // 
         this.m_cutContextMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("m_cutContextMenuItem.Image")));
         this.m_cutContextMenuItem.Name = "m_cutContextMenuItem";
         this.m_cutContextMenuItem.Size = new System.Drawing.Size(91, 22);
         this.m_cutContextMenuItem.Text = "Cut";
         this.m_cutContextMenuItem.Click += new System.EventHandler(this.m_cutContextMenuItem_Click);
         // 
         // m_nodeImageList
         // 
         this.m_nodeImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_nodeImageList.ImageStream")));
         this.m_nodeImageList.TransparentColor = System.Drawing.Color.Transparent;
         this.m_nodeImageList.Images.SetKeyName(0, "0");
         this.m_nodeImageList.Images.SetKeyName(1, "1");
         this.m_nodeImageList.Images.SetKeyName(2, "2");
         this.m_nodeImageList.Images.SetKeyName(3, "3");
         this.m_nodeImageList.Images.SetKeyName(4, "4");
         this.m_nodeImageList.Images.SetKeyName(5, "5");
         this.m_nodeImageList.Images.SetKeyName(6, "6");
         this.m_nodeImageList.Images.SetKeyName(7, "7");
         this.m_nodeImageList.Images.SetKeyName(8, "8");
         this.m_nodeImageList.Images.SetKeyName(9, "9");
         // 
         // EdwardsOutlineForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(589, 423);
         this.Controls.Add(this.m_toolStripContainer);
         this.Name = "EdwardsOutlineForm";
         this.Text = "Edwards Outline";
         this.m_toolStripContainer.ContentPanel.ResumeLayout(false);
         this.m_toolStripContainer.TopToolStripPanel.ResumeLayout(false);
         this.m_toolStripContainer.TopToolStripPanel.PerformLayout();
         this.m_toolStripContainer.ResumeLayout(false);
         this.m_toolStripContainer.PerformLayout();
         this.m_splitContainer.Panel1.ResumeLayout(false);
         this.m_splitContainer.Panel2.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.m_splitContainer)).EndInit();
         this.m_splitContainer.ResumeLayout(false);
         this.m_toolStrip.ResumeLayout(false);
         this.m_toolStrip.PerformLayout();
         this.m_nodeContextMenu.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.ToolStripContainer m_toolStripContainer;
      private System.Windows.Forms.SplitContainer m_splitContainer;
      private System.Windows.Forms.TreeView m_tree;
      private System.Windows.Forms.RichTextBox m_content;
      private System.Windows.Forms.ToolStrip m_toolStrip;
      private System.Windows.Forms.ToolStripButton m_openButton;
      private System.Windows.Forms.ContextMenuStrip m_nodeContextMenu;
      private System.Windows.Forms.ToolStripMenuItem m_cutContextMenuItem;
      private System.Windows.Forms.ToolStripButton m_saveButton;
      private System.Windows.Forms.ImageList m_nodeImageList;
   }
}


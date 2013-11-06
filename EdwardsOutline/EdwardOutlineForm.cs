using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using LibEdward;

namespace EdwardsOutline
{
   public partial class EdwardsOutlineForm : Form
   {
      public EdwardsOutlineForm()
      {
         InitializeComponent();
      }

      private void toolStripButton1_Click(object sender, EventArgs e)
      {
         using (OpenFileDialog dialog = new OpenFileDialog())
         {
            dialog.Filter = "Word .docx files|*.docx|All files|*.*";
            if (dialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
               Edward.CloseAll(false);
               OutlineItem outline = Edward.LoadAndOutline(dialog.FileName);
               UpdateTree(outline);
            }
         }
      }

      protected override void OnClosing(CancelEventArgs e)
      {
         base.OnClosing(e);
         Edward.StopWord();
      }
      
      private void UpdateTree(OutlineItem outline)
      {
         m_tree.BeginUpdate();
         m_tree.Nodes.Clear();
         Populate(outline, m_tree.Nodes);
         m_tree.EndUpdate();
      }

      private void Populate(OutlineItem outline, TreeNodeCollection nodes)
      {
         TreeNode node = nodes.Add(outline.Title, outline.Title, outline.Level,outline.Level);
         node.Tag = outline;
         foreach (OutlineItem child in outline.Children)
         {
            Populate(child, node.Nodes);
         }
      }

      private void m_tree_AfterSelect(object sender, TreeViewEventArgs e)
      {
         if (e.Node != null)
         {
            OutlineItem item = e.Node.Tag as OutlineItem;
            if (item != null)
            {
               m_content.Text = item.TextContent;
            }
         }
      }

      private void m_cutContextMenuItem_Click(object sender, EventArgs e)
      {
         if (m_tree.SelectedNode != null)
         {
            OutlineItem item = m_tree.SelectedNode.Tag as OutlineItem;
            if (item != null)
            {
               item.Cut();
               m_tree.SelectedNode.Remove();
            }
         }
      }

      private void toolStripButton2_Click(object sender, EventArgs e)
      {
         Edward.SaveAll();
      }

      private void m_tree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
      {
         if (e.Node != null && e.Button == System.Windows.Forms.MouseButtons.Right)
         {
            m_tree.SelectedNode = e.Node;
            m_nodeContextMenu.Show(m_tree, e.Location);
         }
      }

      private void m_tree_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
      {
         if ((e.Node.Tag as OutlineItem).Level == 0)
         {
            e.CancelEdit = true;
         }
      }

      private void m_tree_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
      {
         (e.Node.Tag as OutlineItem).Title = e.Label;
      }
   }
}

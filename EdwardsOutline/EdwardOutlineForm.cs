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
      private HashSet<string> m_expanded;
      
      public EdwardsOutlineForm()
      {
         InitializeComponent();

         m_expanded = new HashSet<string>();
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
         this.Enabled = false;
         
         m_tree.BeginUpdate();
         m_tree.Nodes.Clear();
         Populate(outline, m_tree.Nodes);
         m_tree.EndUpdate();
         
         this.Enabled = true;
      }

      private void Populate(OutlineItem outline, TreeNodeCollection nodes)
      {
         TreeNode node = nodes.Add(outline.Path(" / "), outline.Title, outline.Level,outline.Level);
         node.Tag = outline;
         foreach (OutlineItem child in outline.Children)
         {
            Populate(child, node.Nodes);
         }
         if (m_expanded.Contains(node.Name))
         {
            node.Expand();
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

      private void m_saveButton_Click(object sender, EventArgs e)
      {
         Edward.SaveAll();
      }

      private void m_tree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
      {
         if (e.Node != null && e.Button == System.Windows.Forms.MouseButtons.Right)
         {
            m_tree.SelectedNode = e.Node;
            OutlineItem item = e.Node.Tag as OutlineItem;
            m_cutContextMenuItem.Enabled = item.Level != 0;
            m_demoteContextMenuItem.Enabled = item.Level > 0 && item.Level < 10;
            m_promoteContextMenuItem.Enabled = item.Level > 1;
            m_demoteRecursivelyContextMenuItem.Enabled = item.Level > 0 && item.Level < 10;
            m_promoteRecursivelyContextMenuItem.Enabled = item.Level > 1;
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

      private void m_demoteContextMenuItem_Click(object sender, EventArgs e)
      {
         if (m_tree.SelectedNode != null)
         {
            OutlineItem item = m_tree.SelectedNode.Tag as OutlineItem;
            if (item != null)
            {
               item.Demote();
               UpdateTree(Edward.Refresh(item));
            }
         }
      }

      private void m_promoteContextMenuItem_Click(object sender, EventArgs e)
      {
         if (m_tree.SelectedNode != null)
         {
            OutlineItem item = m_tree.SelectedNode.Tag as OutlineItem;
            if (item != null)
            {
               item.Promote();
               UpdateTree(Edward.Refresh(item));
            }
         }
      }

      private void m_tree_AfterExpand(object sender, TreeViewEventArgs e)
      {
         m_expanded.Add(e.Node.Name);
      }

      private void m_tree_AfterCollapse(object sender, TreeViewEventArgs e)
      {
         m_expanded.Remove(e.Node.Name);
      }

      private void m_demoteRecursivelyContextMenuItem_Click(object sender, EventArgs e)
      {
         if (m_tree.SelectedNode != null)
         {
            OutlineItem item = m_tree.SelectedNode.Tag as OutlineItem;
            if (item != null)
            {
               DemoteRecursively(item);
               UpdateTree(Edward.Refresh(item));
            }
         }
      }

      private void DemoteRecursively(OutlineItem item)
      {
         item.Demote();
         foreach (OutlineItem child in item.Children)
         {
            DemoteRecursively(child);
         }
      }

      private void m_promoteRecursivelyContextMenuItem_Click(object sender, EventArgs e)
      {
         if (m_tree.SelectedNode != null)
         {
            OutlineItem item = m_tree.SelectedNode.Tag as OutlineItem;
            if (item != null)
            {
               PromoteRecursively(item);
               UpdateTree(Edward.Refresh(item));
            }
         }
      }

      private void PromoteRecursively(OutlineItem item)
      {
         item.Promote();
         foreach (OutlineItem child in item.Children)
         {
            PromoteRecursively(child);
         }
      }
   }
}

﻿using System;
using System.Windows.Forms;
using GraphLibrary.Graph;
using GraphLibrary.RoleChanger;
using GraphLibrary.Vertex;
using WinFormsVersion.Vertex;

namespace WinFormsVersion.RoleChanger
{
    public class WinFormsVertexRoleChanger : AbstractVertexRoleChanger
    {
        public WinFormsVertexRoleChanger(AbstractGraph graph) : base(graph)
        {

        }

        public override void ChangeTopText(object sender, EventArgs e)
        {
            if ((e as MouseEventArgs).Button == MouseButtons.Middle)
            {
                if ((sender as WinFormsVertex).IsObstacle)
                    return;
                int current = int.Parse((sender as WinFormsVertex).Text);
                current++;
                if (current > 9)
                    current = 1;
                (sender as WinFormsVertex).Text = current.ToString();
            }
        }

        public override void SetStartPoint(object sender, EventArgs e)
        {
            if ((e as MouseEventArgs).Button == MouseButtons.Left)
            {
                base.SetStartPoint(sender, e);
                if ((sender as IVertex).IsObstacle)
                    return;
                foreach (var butt in graph)
                {
                    (butt as WinFormsVertex).MouseClick -= SetStartPoint;
                    (butt as WinFormsVertex).MouseClick += SetDestinationPoint;
                }

            }
        }

        public override void SetDestinationPoint(object sender, EventArgs e)
        {
            if ((e as MouseEventArgs).Button == MouseButtons.Left)
            {
                base.SetDestinationPoint(sender, e);
                if ((sender as IVertex).IsObstacle)
                    return;
                foreach (var butt in graph) 
                    (butt as WinFormsVertex).MouseClick -= SetDestinationPoint;

            }
        }

        public override void ReversePolarity(object sender, EventArgs e)
        {
            if ((e as MouseEventArgs).Button == MouseButtons.Right)
                base.ReversePolarity(sender, e);            
        }
    }
}
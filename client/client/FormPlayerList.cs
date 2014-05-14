using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace client
{
    public partial class FormPlayerList : Form
    {
        public FormPlayerList(List<string>nick, List<Color> color, List<int> li)
        {
            InitializeComponent();
            this.dataGridView1.RowCount = nick.Count+1;
            dataGridView1[0, 0].Value = "Список игроков";
            dataGridView1[1, 0].Value = "Взорваных пуканов";
            for (int i = 0; i < nick.Count; i++)
            {
                dataGridView1[0, i + 1].Value = nick[i];
                dataGridView1[0, i + 1].Style.BackColor = color[i];
                dataGridView1[0, i + 1].Style.SelectionBackColor = color[i];
                dataGridView1[1, i + 1].Value = li[i];
                dataGridView1[1, i + 1].Style.BackColor = color[i];
                dataGridView1[1, i + 1].Style.SelectionBackColor = color[i];
            }
        }

        private void FormPlayerList_KeyUp(object sender, KeyEventArgs e)
        {
            this.Close();
        }
    }
}

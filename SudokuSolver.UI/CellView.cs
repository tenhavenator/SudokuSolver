using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Text.RegularExpressions;


namespace SudokuSolver.UI
{
    internal class CellView : TextBox, ICellView
    {
        private const int TEXTBOX_SIZE_PX = 43;

        public CellView()
        {
            BorderStyle = BorderStyle.None;
            Cursor = Cursors.IBeam;
            Font = new Font("Arial", 27.75F);
            Margin = new Padding(0);
            MaxLength = 1;
            Multiline = true;
            Size = new Size(TEXTBOX_SIZE_PX, TEXTBOX_SIZE_PX);
            TextAlign = HorizontalAlignment.Center;
            Anchor =  AnchorStyles.None;
            Text = "";
            TextChanged += new EventHandler(OnTextBoxChanged);
        }

        public char Value
        {
            get
            {
                return Text.Length == 0 ? '\0' : Text[0];
            }

            set
            {
                Text = value.ToString();
            }
        }

        public bool Enabled
        {
            set
            {
                ReadOnly = !value;
                Cursor = value ? Cursors.IBeam : Cursors.Arrow;
            }
        }

        public Color Background 
        {
            set
            {
                BackColor = value;
            }
        }

        private void OnTextBoxChanged(object _pSender, EventArgs _pArgs)
        {
            if (!Regex.IsMatch(Text, "[1-9]|^$"))
            {
                Text = "";
            }
        }
    }
}

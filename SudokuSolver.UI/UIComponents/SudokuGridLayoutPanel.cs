using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SudokuSolver
{
    /// <summary>
    /// This class contains a custom UI control that represents a sudoku grid
    /// </summary>
    public class SudokuGridLayoutPanel : TableLayoutPanel
    {
        private const int SUDOKU_SIZE = 81;
        private const int TABLE_SIZE_AREA = 9;
        private const int TABLE_SIZE_SIDE = 3;
        private const int GRID_SIZE_PX = 400;
        private const int BOX_SIZE_PX = 132;
        private const int TEXTBOX_SIZE_PX = 43;
        private const float ROW_COL_WEIGHT = 33.33333F;

        private TextBox[] mTextBoxes;

        public TextBox[] TextBoxes
        { 
            get
            {
                return mTextBoxes;
            }
        }

        public void ForEachTextBox(Action<TextBox> pAction)
        {
            for (int i = 0; i < SUDOKU_SIZE; i++)
            {
                pAction(mTextBoxes[i]);
            }
        }

        public SudokuGridLayoutPanel()
        {
            this.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            this.Size = new Size(GRID_SIZE_PX, GRID_SIZE_PX);
            this.ColumnCount = TABLE_SIZE_SIDE;
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, ROW_COL_WEIGHT));
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, ROW_COL_WEIGHT));
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, ROW_COL_WEIGHT));
            this.RowCount = TABLE_SIZE_SIDE;
            this.RowStyles.Add(new RowStyle(SizeType.Percent, ROW_COL_WEIGHT));
            this.RowStyles.Add(new RowStyle(SizeType.Percent, ROW_COL_WEIGHT));
            this.RowStyles.Add(new RowStyle(SizeType.Percent, ROW_COL_WEIGHT));
            
            // Configure the boxes
            mTextBoxes = new TextBox[SUDOKU_SIZE];
            for (int boxIndex = 0; boxIndex < TABLE_SIZE_AREA; boxIndex++)
            {
                TableLayoutPanel boxPanel = new TableLayoutPanel();

                boxPanel.Anchor = AnchorStyles.None;
                boxPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
                boxPanel.ColumnCount = TABLE_SIZE_SIDE;
                boxPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, ROW_COL_WEIGHT));
                boxPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, ROW_COL_WEIGHT));
                boxPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, ROW_COL_WEIGHT));
                boxPanel.Margin = new Padding(0);
                boxPanel.RowCount = TABLE_SIZE_SIDE;
                boxPanel.RowStyles.Add(new RowStyle(SizeType.Percent, ROW_COL_WEIGHT));
                boxPanel.RowStyles.Add(new RowStyle(SizeType.Percent, ROW_COL_WEIGHT));
                boxPanel.RowStyles.Add(new RowStyle(SizeType.Percent, ROW_COL_WEIGHT));
                boxPanel.Size = new Size(BOX_SIZE_PX, BOX_SIZE_PX);

                // Configure the cells
                for (int cellIndex = 0; cellIndex < TABLE_SIZE_AREA; cellIndex++)
                {
                    TextBox textBox = new TextBox()
                    { 

                        BorderStyle = BorderStyle.None,
                        Cursor = Cursors.IBeam,
                        Font = new Font("Arial", 27.75F),
                        Margin = new Padding(0),
                        MaxLength = 1,
                        Multiline = true,
                        Size = new Size(TEXTBOX_SIZE_PX, TEXTBOX_SIZE_PX),
                        TextAlign = HorizontalAlignment.Center,
                        Anchor =  AnchorStyles.None,
                        Text = "",
                    };

                    // Check and see if I can calculate this less stupidly
                    int index = (cellIndex / TABLE_SIZE_SIDE) * TABLE_SIZE_AREA
                        + (boxIndex / TABLE_SIZE_SIDE) * TABLE_SIZE_SIDE * TABLE_SIZE_AREA 
                        + (cellIndex % TABLE_SIZE_SIDE)
                        + (boxIndex % TABLE_SIZE_SIDE) * TABLE_SIZE_SIDE;

                    textBox.TextChanged += (_pSender, _pArgs) => OnTextBoxChanged(textBox, index);

                    mTextBoxes[index] = textBox;
                    boxPanel.Controls.Add(textBox);
                }

                this.Controls.Add(boxPanel);
            }
        }

        private void OnTextBoxChanged(TextBox pTextBox, int pIndex)
        {
            if (!Regex.IsMatch(pTextBox.Text, "[1-9]|^$"))
            {
                pTextBox.Text = "";
            }
        }
    }
}

﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace SudokuSolver.UI
{
    public class GridView : TableLayoutPanel, IGridView
    {
        private const int SUDOKU_SIZE = 81;
        private const int TABLE_SIZE_AREA = 9;
        private const int TABLE_SIZE_SIDE = 3;
        private const int GRID_SIZE_PX = 400;
        private const int BOX_SIZE_PX = 132;
        private const float ROW_COL_WEIGHT = 33.33333F;

        private CellView[] mCells;

        public ICellView this[int pIndex]
        { 
            get
            {
                return mCells[pIndex];
            }
        }

        public void ForEachCell(Action<ICellView> pAction)
        {
            foreach (var cell in mCells)
            {
                pAction(cell);
            }
        }

        public bool LoadingControlsEnabled
        {
            set 
            {
                ForEachCell(c =>
                {
                    c.Enabled = !value;
                });
            }
        }

        public GridView()
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
            mCells = new CellView[SUDOKU_SIZE];
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
                    CellView cell = new CellView();

                    // Check and see if I can calculate this less stupidly
                    int index = (cellIndex / TABLE_SIZE_SIDE) * TABLE_SIZE_AREA
                        + (boxIndex / TABLE_SIZE_SIDE) * TABLE_SIZE_SIDE * TABLE_SIZE_AREA 
                        + (cellIndex % TABLE_SIZE_SIDE)
                        + (boxIndex % TABLE_SIZE_SIDE) * TABLE_SIZE_SIDE;

                    mCells[index] = cell;
                    boxPanel.Controls.Add(cell);
                }

                this.Controls.Add(boxPanel);
            }
        }
    }
}

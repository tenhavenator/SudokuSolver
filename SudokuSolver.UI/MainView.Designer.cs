﻿/// <summary>
/// This class was generated by Visual studio and contains the attributes for each of the UI objects
/// </summary>

namespace SudokuSolver.UI
{
    partial class MainView
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
            this.mLabelTitle = new System.Windows.Forms.Label();
            this.mLabelBottom = new System.Windows.Forms.Label();
            this.mPanelSudokuMain = new System.Windows.Forms.Panel();
            this.mPanelSudokuControls = new System.Windows.Forms.Panel();
            this.mButtonUnsolve = new System.Windows.Forms.Button();
            this.mLabelSolveDetails = new System.Windows.Forms.Label();
            this.mButtonBack = new System.Windows.Forms.Button();
            this.mButtonNext = new System.Windows.Forms.Button();
            this.mButtonClear = new System.Windows.Forms.Button();
            this.mButtonSolve = new System.Windows.Forms.Button();
            this.mButtonFinished = new System.Windows.Forms.Button();
            this.mLabelFinished = new System.Windows.Forms.Label();
            this.mPanelSudokuMain.SuspendLayout();
            this.mPanelSudokuControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // mLabelTitle
            // 
            this.mLabelTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mLabelTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.mLabelTitle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.mLabelTitle.Font = new System.Drawing.Font("Comic Sans MS", 20.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mLabelTitle.Location = new System.Drawing.Point(0, 0);
            this.mLabelTitle.Margin = new System.Windows.Forms.Padding(0);
            this.mLabelTitle.Name = "mLabelTitle";
            this.mLabelTitle.Size = new System.Drawing.Size(687, 59);
            this.mLabelTitle.TabIndex = 1;
            this.mLabelTitle.Text = "Sudoku Solver";
            this.mLabelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // mLabelBottom
            // 
            this.mLabelBottom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mLabelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.mLabelBottom.Location = new System.Drawing.Point(0, 558);
            this.mLabelBottom.Margin = new System.Windows.Forms.Padding(0);
            this.mLabelBottom.Name = "mLabelBottom";
            this.mLabelBottom.Size = new System.Drawing.Size(687, 23);
            this.mLabelBottom.TabIndex = 2;
            this.mLabelBottom.Text = "Sudoku Solver. Created by Jeff ten Have. 2015.";
            this.mLabelBottom.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // mPanelSudokuMain
            // 
            this.mPanelSudokuMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mPanelSudokuMain.Controls.Add(this.mPanelSudokuControls);
            this.mPanelSudokuMain.Location = new System.Drawing.Point(0, 59);
            this.mPanelSudokuMain.Margin = new System.Windows.Forms.Padding(0);
            this.mPanelSudokuMain.Name = "mPanelSudokuMain";
            this.mPanelSudokuMain.Size = new System.Drawing.Size(687, 499);
            this.mPanelSudokuMain.TabIndex = 4;
            // 
            // mPanelSudokuControls
            // 
            this.mPanelSudokuControls.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mPanelSudokuControls.Controls.Add(this.mButtonUnsolve);
            this.mPanelSudokuControls.Controls.Add(this.mLabelSolveDetails);
            this.mPanelSudokuControls.Controls.Add(this.mButtonBack);
            this.mPanelSudokuControls.Controls.Add(this.mButtonNext);
            this.mPanelSudokuControls.Controls.Add(this.mButtonClear);
            this.mPanelSudokuControls.Controls.Add(this.mButtonSolve);
            this.mPanelSudokuControls.Controls.Add(this.mButtonFinished);
            this.mPanelSudokuControls.Controls.Add(this.mLabelFinished);
            this.mPanelSudokuControls.Dock = System.Windows.Forms.DockStyle.Right;
            this.mPanelSudokuControls.Location = new System.Drawing.Point(487, 0);
            this.mPanelSudokuControls.Margin = new System.Windows.Forms.Padding(0);
            this.mPanelSudokuControls.Name = "mPanelSudokuControls";
            this.mPanelSudokuControls.Size = new System.Drawing.Size(200, 499);
            this.mPanelSudokuControls.TabIndex = 5;
            // 
            // mButtonUnsolve
            // 
            this.mButtonUnsolve.Location = new System.Drawing.Point(84, 52);
            this.mButtonUnsolve.Name = "mButtonUnsolve";
            this.mButtonUnsolve.Size = new System.Drawing.Size(78, 23);
            this.mButtonUnsolve.TabIndex = 9;
            this.mButtonUnsolve.Text = "Unsolve";
            this.mButtonUnsolve.UseVisualStyleBackColor = true;
            this.mButtonUnsolve.Visible = false;
            // 
            // mLabelSolveDetails
            // 
            this.mLabelSolveDetails.Enabled = false;
            this.mLabelSolveDetails.Location = new System.Drawing.Point(3, 145);
            this.mLabelSolveDetails.Name = "mLabelSolveDetails";
            this.mLabelSolveDetails.Size = new System.Drawing.Size(159, 45);
            this.mLabelSolveDetails.TabIndex = 8;
            this.mLabelSolveDetails.Text = "Click on a solved square to see the details of how that value was found.";
            this.mLabelSolveDetails.Visible = false;
            // 
            // mButtonBack
            // 
            this.mButtonBack.Location = new System.Drawing.Point(3, 81);
            this.mButtonBack.Name = "mButtonBack";
            this.mButtonBack.Size = new System.Drawing.Size(78, 23);
            this.mButtonBack.TabIndex = 7;
            this.mButtonBack.Text = "< Back";
            this.mButtonBack.UseVisualStyleBackColor = true;
            this.mButtonBack.Visible = false;
            // 
            // mButtonNext
            // 
            this.mButtonNext.Location = new System.Drawing.Point(84, 81);
            this.mButtonNext.Name = "mButtonNext";
            this.mButtonNext.Size = new System.Drawing.Size(78, 23);
            this.mButtonNext.TabIndex = 6;
            this.mButtonNext.Text = "Show Next >";
            this.mButtonNext.UseVisualStyleBackColor = true;
            this.mButtonNext.Visible = false;
            // 
            // mButtonClear
            // 
            this.mButtonClear.Location = new System.Drawing.Point(3, 3);
            this.mButtonClear.Name = "mButtonClear";
            this.mButtonClear.Size = new System.Drawing.Size(75, 23);
            this.mButtonClear.TabIndex = 5;
            this.mButtonClear.Text = "Clear";
            this.mButtonClear.UseVisualStyleBackColor = true;
            // 
            // mButtonSolve
            // 
            this.mButtonSolve.Location = new System.Drawing.Point(3, 52);
            this.mButtonSolve.Name = "mButtonSolve";
            this.mButtonSolve.Size = new System.Drawing.Size(78, 23);
            this.mButtonSolve.TabIndex = 4;
            this.mButtonSolve.Text = "Solve";
            this.mButtonSolve.UseVisualStyleBackColor = true;
            // 
            // mButtonFinished
            // 
            this.mButtonFinished.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.mButtonFinished.Location = new System.Drawing.Point(3, 471);
            this.mButtonFinished.Name = "mButtonFinished";
            this.mButtonFinished.Size = new System.Drawing.Size(75, 23);
            this.mButtonFinished.TabIndex = 2;
            this.mButtonFinished.Text = "Finished";
            this.mButtonFinished.UseVisualStyleBackColor = true;
            // 
            // mLabelFinished
            // 
            this.mLabelFinished.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.mLabelFinished.Location = new System.Drawing.Point(3, 433);
            this.mLabelFinished.Name = "mLabelFinished";
            this.mLabelFinished.Size = new System.Drawing.Size(159, 35);
            this.mLabelFinished.TabIndex = 3;
            this.mLabelFinished.Text = "Enter your custom sudoku and press \"Finished\"\r\n";
            // 
            // MainView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(687, 581);
            this.Controls.Add(this.mLabelBottom);
            this.Controls.Add(this.mLabelTitle);
            this.Controls.Add(this.mPanelSudokuMain);
            this.MinimumSize = new System.Drawing.Size(703, 620);
            this.Name = "MainView";
            this.Text = "Sudoku Solver";
            this.mPanelSudokuMain.ResumeLayout(false);
            this.mPanelSudokuControls.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label mLabelTitle;
        private System.Windows.Forms.Label mLabelBottom;
        private System.Windows.Forms.Panel mPanelSudokuMain;
        private System.Windows.Forms.Panel mPanelSudokuControls;
        private System.Windows.Forms.Button mButtonFinished;
        private System.Windows.Forms.Label mLabelFinished;
        private System.Windows.Forms.Button mButtonSolve;

        private System.Windows.Forms.Label mLabelSolveDetails;
        private System.Windows.Forms.Button mButtonBack;
        private System.Windows.Forms.Button mButtonNext;
        private System.Windows.Forms.Button mButtonClear;
        private System.Windows.Forms.Button mButtonUnsolve;
    }
}
namespace Testing
{
    partial class FrmTest
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.OFD = new System.Windows.Forms.OpenFileDialog();
            this.SFD = new System.Windows.Forms.SaveFileDialog();
            this.GBCargarDatos = new System.Windows.Forms.GroupBox();
            this.CmBGuardar = new System.Windows.Forms.Button();
            this.CmBCargar = new System.Windows.Forms.Button();
            this.TxtRuta = new System.Windows.Forms.TextBox();
            this.CmBTriangularPuntos = new System.Windows.Forms.Button();
            this.DGV = new System.Windows.Forms.DataGridView();
            this.GBCargarDatos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(518, 93);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(111, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Triangular polígono";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // OFD
            // 
            this.OFD.DefaultExt = "xml";
            this.OFD.Filter = "Ficheros de Texto (*.xml, *.asc, *.txt)|*.xml;*.asc;*.txt";
            // 
            // SFD
            // 
            this.SFD.DefaultExt = "xml";
            this.SFD.Filter = "Ficheros de Texto (*.xml, *.asc, *.txt)|*.xml;*.asc;*.txt";
            // 
            // GBCargarDatos
            // 
            this.GBCargarDatos.Controls.Add(this.CmBTriangularPuntos);
            this.GBCargarDatos.Controls.Add(this.DGV);
            this.GBCargarDatos.Controls.Add(this.CmBGuardar);
            this.GBCargarDatos.Controls.Add(this.CmBCargar);
            this.GBCargarDatos.Controls.Add(this.TxtRuta);
            this.GBCargarDatos.Location = new System.Drawing.Point(12, 12);
            this.GBCargarDatos.Name = "GBCargarDatos";
            this.GBCargarDatos.Size = new System.Drawing.Size(347, 388);
            this.GBCargarDatos.TabIndex = 6;
            this.GBCargarDatos.TabStop = false;
            this.GBCargarDatos.Text = "Vértices:";
            // 
            // CmBGuardar
            // 
            this.CmBGuardar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CmBGuardar.Location = new System.Drawing.Point(266, 47);
            this.CmBGuardar.Name = "CmBGuardar";
            this.CmBGuardar.Size = new System.Drawing.Size(75, 23);
            this.CmBGuardar.TabIndex = 9;
            this.CmBGuardar.Text = "Guardar";
            this.CmBGuardar.UseVisualStyleBackColor = true;
            this.CmBGuardar.Click += new System.EventHandler(this.CmBGuardar_Click);
            // 
            // CmBCargar
            // 
            this.CmBCargar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CmBCargar.Location = new System.Drawing.Point(185, 47);
            this.CmBCargar.Name = "CmBCargar";
            this.CmBCargar.Size = new System.Drawing.Size(75, 23);
            this.CmBCargar.TabIndex = 8;
            this.CmBCargar.Text = "Cargar";
            this.CmBCargar.UseVisualStyleBackColor = true;
            this.CmBCargar.Click += new System.EventHandler(this.CmBCargar_Click);
            // 
            // TxtRuta
            // 
            this.TxtRuta.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtRuta.Location = new System.Drawing.Point(6, 21);
            this.TxtRuta.Name = "TxtRuta";
            this.TxtRuta.Size = new System.Drawing.Size(335, 20);
            this.TxtRuta.TabIndex = 6;
            // 
            // CmBTriangularPuntos
            // 
            this.CmBTriangularPuntos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CmBTriangularPuntos.Location = new System.Drawing.Point(236, 359);
            this.CmBTriangularPuntos.Name = "CmBTriangularPuntos";
            this.CmBTriangularPuntos.Size = new System.Drawing.Size(105, 23);
            this.CmBTriangularPuntos.TabIndex = 7;
            this.CmBTriangularPuntos.Text = "Triangular puntos";
            this.CmBTriangularPuntos.UseVisualStyleBackColor = true;
            this.CmBTriangularPuntos.Click += new System.EventHandler(this.CmBTriangularPuntos_Click);
            // 
            // DGV
            // 
            this.DGV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV.Location = new System.Drawing.Point(6, 76);
            this.DGV.Name = "DGV";
            this.DGV.Size = new System.Drawing.Size(335, 277);
            this.DGV.TabIndex = 10;
            // 
            // FrmTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 412);
            this.Controls.Add(this.GBCargarDatos);
            this.Controls.Add(this.button1);
            this.Name = "FrmTest";
            this.Text = "Form1";
            this.GBCargarDatos.ResumeLayout(false);
            this.GBCargarDatos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.OpenFileDialog OFD;
        private System.Windows.Forms.SaveFileDialog SFD;
        private System.Windows.Forms.GroupBox GBCargarDatos;
        private System.Windows.Forms.Button CmBGuardar;
        private System.Windows.Forms.Button CmBCargar;
        private System.Windows.Forms.TextBox TxtRuta;
        private System.Windows.Forms.Button CmBTriangularPuntos;
        private System.Windows.Forms.DataGridView DGV;
    }
}


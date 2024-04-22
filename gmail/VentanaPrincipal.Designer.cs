namespace gmail
{
    partial class VentanaPrincipal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VentanaPrincipal));
            this.panelFormularios = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.BtnListado = new FontAwesome.Sharp.IconButton();
            this.BtnUsuarios = new FontAwesome.Sharp.IconButton();
            this.BtnSalir = new FontAwesome.Sharp.IconButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.BtnVolver = new System.Windows.Forms.PictureBox();
            this.BtnGmail = new FontAwesome.Sharp.IconButton();
            this.BtnConfig = new FontAwesome.Sharp.IconButton();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BtnVolver)).BeginInit();
            this.SuspendLayout();
            // 
            // panelFormularios
            // 
            this.panelFormularios.AccessibleName = "panelFormularios";
            this.panelFormularios.AutoSize = true;
            this.panelFormularios.BackColor = System.Drawing.Color.White;
            this.panelFormularios.Location = new System.Drawing.Point(197, 1);
            this.panelFormularios.Name = "panelFormularios";
            this.panelFormularios.Size = new System.Drawing.Size(677, 464);
            this.panelFormularios.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.AllowDrop = true;
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(120)))), ((int)(((byte)(188)))));
            this.panel1.Controls.Add(this.BtnListado);
            this.panel1.Controls.Add(this.BtnUsuarios);
            this.panel1.Controls.Add(this.BtnSalir);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.BtnGmail);
            this.panel1.Controls.Add(this.BtnConfig);
            this.panel1.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.Location = new System.Drawing.Point(-3, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 461);
            this.panel1.TabIndex = 2;
            // 
            // BtnListado
            // 
            this.BtnListado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(120)))), ((int)(((byte)(188)))));
            this.BtnListado.FlatAppearance.BorderSize = 0;
            this.BtnListado.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnListado.ForeColor = System.Drawing.Color.White;
            this.BtnListado.IconChar = FontAwesome.Sharp.IconChar.None;
            this.BtnListado.IconColor = System.Drawing.Color.Black;
            this.BtnListado.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.BtnListado.Location = new System.Drawing.Point(3, 232);
            this.BtnListado.Name = "BtnListado";
            this.BtnListado.Size = new System.Drawing.Size(200, 42);
            this.BtnListado.TabIndex = 5;
            this.BtnListado.Text = "Listado de usuarios";
            this.BtnListado.UseVisualStyleBackColor = false;
            this.BtnListado.Click += new System.EventHandler(this.BtnListado_Click);
            // 
            // BtnUsuarios
            // 
            this.BtnUsuarios.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(120)))), ((int)(((byte)(188)))));
            this.BtnUsuarios.FlatAppearance.BorderSize = 0;
            this.BtnUsuarios.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnUsuarios.ForeColor = System.Drawing.Color.White;
            this.BtnUsuarios.IconChar = FontAwesome.Sharp.IconChar.None;
            this.BtnUsuarios.IconColor = System.Drawing.Color.Black;
            this.BtnUsuarios.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.BtnUsuarios.Location = new System.Drawing.Point(3, 193);
            this.BtnUsuarios.Name = "BtnUsuarios";
            this.BtnUsuarios.Size = new System.Drawing.Size(203, 42);
            this.BtnUsuarios.TabIndex = 4;
            this.BtnUsuarios.Text = "Usuarios";
            this.BtnUsuarios.UseVisualStyleBackColor = false;
            this.BtnUsuarios.Click += new System.EventHandler(this.BtnUsuarios_Click);
            // 
            // BtnSalir
            // 
            this.BtnSalir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(120)))), ((int)(((byte)(188)))));
            this.BtnSalir.FlatAppearance.BorderSize = 0;
            this.BtnSalir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnSalir.ForeColor = System.Drawing.Color.White;
            this.BtnSalir.IconChar = FontAwesome.Sharp.IconChar.PowerOff;
            this.BtnSalir.IconColor = System.Drawing.Color.White;
            this.BtnSalir.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.BtnSalir.IconSize = 35;
            this.BtnSalir.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.BtnSalir.Location = new System.Drawing.Point(3, 421);
            this.BtnSalir.Name = "BtnSalir";
            this.BtnSalir.Size = new System.Drawing.Size(203, 40);
            this.BtnSalir.TabIndex = 3;
            this.BtnSalir.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.BtnSalir.UseVisualStyleBackColor = false;
            this.BtnSalir.Click += new System.EventHandler(this.BtnSalir_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.BtnVolver);
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 100);
            this.panel2.TabIndex = 2;
            // 
            // BtnVolver
            // 
            this.BtnVolver.Image = ((System.Drawing.Image)(resources.GetObject("BtnVolver.Image")));
            this.BtnVolver.Location = new System.Drawing.Point(15, 23);
            this.BtnVolver.Name = "BtnVolver";
            this.BtnVolver.Size = new System.Drawing.Size(168, 74);
            this.BtnVolver.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.BtnVolver.TabIndex = 0;
            this.BtnVolver.TabStop = false;
            this.BtnVolver.Click += new System.EventHandler(this.BtnVolver_Click);
            // 
            // BtnGmail
            // 
            this.BtnGmail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(120)))), ((int)(((byte)(188)))));
            this.BtnGmail.FlatAppearance.BorderSize = 0;
            this.BtnGmail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnGmail.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnGmail.ForeColor = System.Drawing.Color.White;
            this.BtnGmail.IconChar = FontAwesome.Sharp.IconChar.None;
            this.BtnGmail.IconColor = System.Drawing.Color.Black;
            this.BtnGmail.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.BtnGmail.Location = new System.Drawing.Point(3, 106);
            this.BtnGmail.Name = "BtnGmail";
            this.BtnGmail.Size = new System.Drawing.Size(200, 43);
            this.BtnGmail.TabIndex = 0;
            this.BtnGmail.Text = "Correos electrónicos";
            this.BtnGmail.UseVisualStyleBackColor = false;
            this.BtnGmail.Click += new System.EventHandler(this.BtnGmail_Click);
            // 
            // BtnConfig
            // 
            this.BtnConfig.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(120)))), ((int)(((byte)(188)))));
            this.BtnConfig.FlatAppearance.BorderSize = 0;
            this.BtnConfig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnConfig.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnConfig.ForeColor = System.Drawing.Color.White;
            this.BtnConfig.IconChar = FontAwesome.Sharp.IconChar.None;
            this.BtnConfig.IconColor = System.Drawing.Color.Black;
            this.BtnConfig.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.BtnConfig.Location = new System.Drawing.Point(3, 146);
            this.BtnConfig.Name = "BtnConfig";
            this.BtnConfig.Size = new System.Drawing.Size(200, 51);
            this.BtnConfig.TabIndex = 0;
            this.BtnConfig.Text = "Configurar credenciales";
            this.BtnConfig.UseVisualStyleBackColor = false;
            this.BtnConfig.Click += new System.EventHandler(this.BtnConfig_Click_1);
            // 
            // VentanaPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(870, 461);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panelFormularios);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "VentanaPrincipal";
            this.Text = "Servidor de correos electrónicos";
            this.Load += new System.EventHandler(this.VentanaPrincipal_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.BtnVolver)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel panelFormularios;
        private System.Windows.Forms.Panel panel1;
        private FontAwesome.Sharp.IconButton BtnConfig;
        private FontAwesome.Sharp.IconButton BtnGmail;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox BtnVolver;
        private FontAwesome.Sharp.IconButton BtnSalir;
        private FontAwesome.Sharp.IconButton BtnUsuarios;
        private FontAwesome.Sharp.IconButton BtnListado;
    }
}


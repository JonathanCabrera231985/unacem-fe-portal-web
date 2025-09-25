
namespace EncriptadoMasivo
{
    partial class Form1
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
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.btn_nocrypt = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(30, 24);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(119, 58);
            this.button1.TabIndex = 0;
            this.button1.Text = "Empleados";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btn_empleados);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(30, 88);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(119, 58);
            this.button2.TabIndex = 1;
            this.button2.Text = "Clientes";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.btn_clientes);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(30, 152);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(119, 58);
            this.button3.TabIndex = 2;
            this.button3.Text = "Certificados";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // btn_nocrypt
            // 
            this.btn_nocrypt.Location = new System.Drawing.Point(30, 216);
            this.btn_nocrypt.Name = "btn_nocrypt";
            this.btn_nocrypt.Size = new System.Drawing.Size(119, 58);
            this.btn_nocrypt.TabIndex = 3;
            this.btn_nocrypt.Text = "No Crypt";
            this.btn_nocrypt.UseVisualStyleBackColor = true;
            this.btn_nocrypt.Click += new System.EventHandler(this.btn_nocrypt_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(178, 297);
            this.Controls.Add(this.btn_nocrypt);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button btn_nocrypt;
    }
}


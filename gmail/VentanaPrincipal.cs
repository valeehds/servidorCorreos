using enviarCorreo;
using System;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;

namespace gmail
{
    public partial class VentanaPrincipal : Form
    {
        public VentanaPrincipal()
        {
            InitializeComponent();
            CenterToScreen();
            MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void formularios(Form form)
        {
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            panelFormularios.Controls.Clear(); // Limpia cualquier formulario anterior
            panelFormularios.Controls.Add(form);
            form.Show();
        }

        private void BtnGmail_Click(object sender, EventArgs e)
        {
            Form1 gmail = new Form1();
            formularios(gmail);
        }

        private void BtnConfig_Click_1(object sender, EventArgs e)
        {
            config config = new config();
            formularios(config);
        }

        private void BtnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnVolver_Click(object sender, EventArgs e)
        {
            panelFormularios.Controls.Clear();
            volver volver = new volver();

            formularios(volver);
            
        }

        private void VentanaPrincipal_Load(object sender, EventArgs e)
        {
            panelFormularios.Controls.Clear();
            volver volver = new volver();

            formularios(volver);
        }

        private void BtnUsuarios_Click(object sender, EventArgs e)
        {
            panelFormularios.Controls.Clear();
            usuarios usuarios = new usuarios();

            formularios(usuarios);

        }

        private void BtnListado_Click(object sender, EventArgs e)
        {
            panelFormularios.Controls.Clear();
            listado listado = new listado(); 

            formularios(listado);
        }
    }
}

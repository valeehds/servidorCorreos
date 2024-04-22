using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gmail
{
    public partial class volver : Form
    {
        private string credencialesFilePath = @"C:\Users\ana.hernandez\OneDrive - Sistemas GyG S.A\Escritorio\credenciales.txt";

        public volver()
        {
            InitializeComponent();
            MostrarCredenciales();
        }

        private string[] ObtenerCredenciales()
        {
            try
            {
                if (!System.IO.File.Exists(credencialesFilePath))
                {
                    MessageBox.Show("No se encontró el archivo de credenciales.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

                return System.IO.File.ReadAllLines(credencialesFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener las credenciales SMTP: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private void MostrarCredenciales()
        {
            // Obtener credenciales guardadas desde el formulario de configuración
            string[] credenciales = ObtenerCredenciales();
            if (credenciales == null || credenciales.Length < 4)
            {
                lblCredenciales.Text = "No se pudieron cargar las credenciales SMTP.";
                return;
            }

            string smtpHost = credenciales[0];
            int smtpPort;
            if (!int.TryParse(credenciales[1], out smtpPort))
            {
                lblCredenciales.Text = "El puerto SMTP no es válido.";
                return;
            }
            string usuarioSmtp = credenciales[2];
            string contraseñaSmtp = credenciales[3];

            // Mostrar las credenciales en el Label
            lblCredenciales.Text = $"Servidor SMTP: {smtpHost}\nPuerto SMTP: {smtpPort}\nUsuario SMTP: {usuarioSmtp}";
        }

        private void volver_Load(object sender, EventArgs e)
        {

        }
    }
}

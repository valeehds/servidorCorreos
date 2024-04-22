using System;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;

namespace enviarCorreo
{
    public partial class Form1 : Form
    {
        public string credencialesFilePath = @"C:\Users\ana.hernandez\OneDrive - Sistemas GyG S.A\Escritorio\credenciales.txt";

        public Form1()
        {
            InitializeComponent();
            CenterToScreen();
            MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
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

        private bool ValidarCorreoElectronico(string correo)
        {
            // Expresión regular para validar el formato del correo electrónico
            string patron = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            return System.Text.RegularExpressions.Regex.IsMatch(correo, patron);
        }

        private void BtnEnviar_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cuenta.Text) || string.IsNullOrWhiteSpace(asunto.Text) || string.IsNullOrWhiteSpace(cuerpo.Text))
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!ValidarCorreoElectronico(cuenta.Text))
            {
                MessageBox.Show("Por favor, ingrese un correo electrónico válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {

                string[] credenciales = ObtenerCredenciales();
                if (credenciales == null || credenciales.Length < 4)
                {
                    MessageBox.Show("No se pudieron cargar las credenciales SMTP.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string smtpHost = credenciales[0];
                int smtpPort;
                if (!int.TryParse(credenciales[1], out smtpPort))
                {
                    MessageBox.Show("El puerto SMTP no es válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string usuarioSmtp = credenciales[2];
                string contraseñaSmtp = credenciales[3];

                // Crear cliente SMTP con las credenciales
                SmtpClient smtp = new SmtpClient(smtpHost, smtpPort);
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(usuarioSmtp, contraseñaSmtp);
                smtp.EnableSsl = true;

                // Crear el mensaje
                MailMessage mensaje = new MailMessage(usuarioSmtp, cuenta.Text, asunto.Text, cuerpo.Text);

                // Enviar el mensaje
                smtp.Send(mensaje);

                // Limpiar campos después del envío
                cuenta.Text = "";
                asunto.Text = "";
                cuerpo.Text = "";

                MessageBox.Show("Correo enviado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Se produjo un error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

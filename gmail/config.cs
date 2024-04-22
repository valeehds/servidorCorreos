using System;
using System.IO;
using System.Windows.Forms;

namespace gmail
{
    public partial class config : Form
    {
        // Puerto y host estáticos
        private const string SmtpHost = "smtp.gmail.com";
        private const int SmtpPort = 587;

        public config()
        {
            InitializeComponent();

            // Mostrar el host y el puerto predeterminados en los campos correspondientes del formulario
            host.Text = SmtpHost;
            puerto.Text = SmtpPort.ToString();
        }

        public void GuardarConfiguracionSMTPEnArchivo(string smtpHost, string smtpPort, string usuarioSmtp, string contraseñaSmtp)
        {
            try
            {
                // Especifica la ruta completa y el nombre de archivo para guardar la configuración SMTP
                string rutaArchivo = @"C:\Users\ana.hernandez\OneDrive - Sistemas GyG S.A\Escritorio\credenciales.txt";

                // Crear o sobrescribir el archivo de texto con los nuevos datos
                using (StreamWriter writer = new StreamWriter(rutaArchivo))
                {
                    // Guardar las credenciales en el formato adecuado
                    writer.WriteLine(smtpHost);
                    writer.WriteLine(smtpPort);
                    writer.WriteLine(usuarioSmtp);
                    writer.WriteLine(contraseñaSmtp);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar la configuración SMTP: {ex.Message}");
            }
        }

        public void BtnEnviar_Click_1(object sender, EventArgs e)
        {
            // Verificar si todos los campos están completos
            if (string.IsNullOrWhiteSpace(host.Text) || string.IsNullOrWhiteSpace(puerto.Text) ||
                string.IsNullOrWhiteSpace(usuario.Text) || string.IsNullOrWhiteSpace(contrasena.Text))
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Obtener los valores de configuración SMTP del formulario
            string smtpHost = host.Text;
            string smtpPort = puerto.Text;
            string usuarioSmtp = usuario.Text;
            string contraseñaSmtp = contrasena.Text;

            // Guardar los datos en un archivo de texto
            GuardarConfiguracionSMTPEnArchivo(smtpHost, smtpPort, usuarioSmtp, contraseñaSmtp);

            // Mostrar un mensaje de confirmación
            MessageBox.Show("Configuración SMTP guardada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void config_Load(object sender, EventArgs e)
        {

        }
    }
}

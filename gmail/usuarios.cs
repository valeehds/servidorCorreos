using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace gmail
{
    public partial class usuarios : Form
    {
        private const string rutaArchivo = @"C:\Users\ana.hernandez\OneDrive - Sistemas GyG S.A\Escritorio\datosUsuario.json";
        private List<Usuario> listaUsuarios;

        public usuarios()
        {
            InitializeComponent();
            listaUsuarios = new List<Usuario>();
        }

        private void GuardarUsuarios()
        {
            try
            {
                List<Usuario> usuariosExistentes = new List<Usuario>();

                // Intenta leer los usuarios existentes desde el archivo
                if (File.Exists(rutaArchivo))
                {
                    string json = File.ReadAllText(rutaArchivo);
                    usuariosExistentes = JsonConvert.DeserializeObject<List<Usuario>>(json);
                }

                // Agrega los nuevos usuarios a la lista existente
                foreach (var usuario in listaUsuarios)
                {
                    if (!usuariosExistentes.Any(u => u.CorreoUsuario == usuario.CorreoUsuario))
                    {
                        usuariosExistentes.Add(usuario);
                    }
                }

                // Serializa la lista combinada y guárdala en el archivo
                string jsonFinal = JsonConvert.SerializeObject(usuariosExistentes);
                File.WriteAllText(rutaArchivo, jsonFinal);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar los usuarios: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                string nombre = nombreUsuario.Text.Trim();
                string correo = correoUsuario.Text.Trim();

                if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(correo))
                {
                    MessageBox.Show("Por favor, complete todos los campos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!EsCorreoValido(correo))
                {
                    MessageBox.Show("Por favor, ingrese un correo electrónico válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    nombreUsuario.Text = "";
                    correoUsuario.Text = "";
                    return;
                }

                if (listaUsuarios.Any(u => u.CorreoUsuario == correo))
                {
                    MessageBox.Show("El correo electrónico ya está registrado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    nombreUsuario.Text = "";
                    correoUsuario.Text = "";
                    return;
                }

                Usuario nuevoUsuario = new Usuario { NombreUsuario = nombre, CorreoUsuario = correo };
                listaUsuarios.Add(nuevoUsuario);

                nombreUsuario.Text = "";
                correoUsuario.Text = "";

                GuardarUsuarios();

                MessageBox.Show("Usuario agregado correctamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar el usuario: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool EsCorreoValido(string correo)
        {
            string patron = @"^[a-zA-Z0-9._%+-]+@(outlook\.com|gmail\.com|yahoo\.com|hotmail\.com)$";
            Regex regex = new Regex(patron);
            return regex.IsMatch(correo);
        }

        private void BtnListado_Click(object sender, EventArgs e)
        {

        }
    }

    public class Usuario
    {
        public string NombreUsuario { get; set; }
        public string CorreoUsuario { get; set; }
    }
}

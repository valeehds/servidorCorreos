using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gmail
{
    public partial class listado : Form
    {
        private const string rutaArchivo = @"C:\Users\ana.hernandez\OneDrive - Sistemas GyG S.A\Escritorio\datosUsuario.json";
        public listado()
        {
            InitializeComponent();
            CenterToScreen();
            MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            CargarDatosDesdeJson();
            dataGridView1.KeyDown += dataGridView1_KeyDown;
        }


        private void CargarDatosDesdeJson()
        {
            if (!File.Exists(rutaArchivo))
            {
                MessageBox.Show("No se encontró el archivo JSON", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                string json = File.ReadAllText(rutaArchivo);
                List<Datos> datos = JsonConvert.DeserializeObject<List<Datos>>(json);

                dataGridView1.AutoGenerateColumns = false;

                datos = datos.OrderBy(d => d.nombreUsuario).ToList(); //Para ordenar ASC

                dataGridView1.Columns["nombre"].DataPropertyName = "nombreUsuario";
                dataGridView1.Columns["correo"].DataPropertyName = "correoUsuario";

                dataGridView1.DataSource = datos;

                dataGridView1.ClearSelection();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos desde el archivo JSON: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GuardarRegistro()
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                return;
            }

            DataGridViewRow row = dataGridView1.SelectedRows[0];

            if (row == null)
            {
                MessageBox.Show("No hay fila seleccionada", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string nombre = row.Cells["nombre"].Value?.ToString();
            if (string.IsNullOrEmpty(nombre))
            {
                MessageBox.Show("El nombre no puede ser vacío", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string correo = row.Cells["correo"].Value?.ToString();
            if (string.IsNullOrEmpty(correo))
            {
                MessageBox.Show("El correo no puede ser vacío", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!EsCorreoValido(correo))
            {
                MessageBox.Show("El correo ingresado no es válido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CargarDatosDesdeJson();
                return;
            }


            Datos dato = new Datos
            {
                nombreUsuario = nombre,
                correoUsuario = correo
            };

            try
            {
                List<Datos> listaDatos = JsonConvert.DeserializeObject<List<Datos>>(File.ReadAllText(rutaArchivo));

                foreach (var registro in listaDatos)
                {
                    if (registro.correoUsuario == correo)
                    {
                        registro.nombreUsuario = nombre;
                    }
                    if (registro.nombreUsuario == nombre)
                    {
                        registro.correoUsuario = correo;
                    }
                }

                string json = JsonConvert.SerializeObject(listaDatos, Formatting.Indented);
                File.WriteAllText(rutaArchivo, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar los cambios en el archivo JSON: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Registro editado y guardado correctamente", "Guardado", MessageBoxButtons.OK, MessageBoxIcon.Information);

            CargarDatosDesdeJson();
        }



        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GuardarRegistro();
            }
        }

        private void eliminarDatos()
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Debe seleccionar filas para continuar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                List<Datos> listaDatos = JsonConvert.DeserializeObject<List<Datos>>(File.ReadAllText(rutaArchivo));

                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    string correo = row.Cells["correo"].Value?.ToString();
                    if (!string.IsNullOrEmpty(correo))
                    {
                        Datos datoEliminar = listaDatos.FirstOrDefault(u => u.correoUsuario == correo);
                        if (datoEliminar != null)
                        {
                            listaDatos.Remove(datoEliminar);
                        }
                    }
                }

                string json = JsonConvert.SerializeObject(listaDatos, Formatting.Indented);
                File.WriteAllText(rutaArchivo, json);

                MessageBox.Show("Filas eliminadas correctamente", "Eliminación", MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarDatosDesdeJson();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar filas: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (MostrarDialogoConfirmacion())
            {
                eliminarDatos();
            }
        }

        private bool MostrarDialogoConfirmacion()
        {
            int filas = dataGridView1.SelectedRows.Count;

            string mensaje = ("¿Está seguro de querer eliminar " + filas + " correos?");

            string mensaje2 = ("¿Está seguro de querer eliminar este correo?");
           
            if (filas == 1) {
                var resultado2 = MessageBox.Show(mensaje2, "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                return resultado2 == DialogResult.Yes;
            }

            var resultado = MessageBox.Show(mensaje, "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return resultado == DialogResult.Yes;
        }

        private bool EsCorreoValido(string correo)
        {
            string patron = @"^[a-zA-Z0-9._%+-]+@(outlook\.com|gmail\.com|yahoo\.com|hotmail\.com)$";
            Regex regex = new Regex(patron);
            return regex.IsMatch(correo);
        }


        //MODELO
        public class Datos
        {
            public string nombreUsuario { get; set; }
            public string correoUsuario { get; set; }
        }

        private void busqueda_TextChanged(object sender, EventArgs e)
        {
            string terminoBusqueda = busqueda.Text.Trim().ToLower();

            List<Datos> datos = (List<Datos>)dataGridView1.DataSource;

            if (string.IsNullOrEmpty(terminoBusqueda))
            {
                CargarDatosDesdeJson();
            }
            else
            {
                List<Datos> datosFiltrados = datos.Where(d => d.nombreUsuario.ToLower().Contains(terminoBusqueda) || d.correoUsuario.ToLower().Contains(terminoBusqueda)).ToList();
                dataGridView1.DataSource = datosFiltrados;
            }
            terminoBusqueda = "";
        }

        private string ConstruirCuerpoCorreo(string nombre)
        {
            string cuerpoCorreo = @"<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd""> <html dir=""ltr"" xmlns=""http://www.w3.org/1999/xhtml"" xmlns:o=""urn:schemas-microsoft-com:office:office"" lang=""und""> <head> <meta charset=""UTF-8""> <meta content=""width=device-width, initial-scale=1"" name=""viewport""> <meta name=""x-apple-disable-message-reformatting""> <meta http-equiv=""X-UA-Compatible"" content=""IE=edge""> <meta content=""telephone=no"" name=""format-detection""> <title>New Template</title><!--[if (mso 16)]> <style type=""text/css""> a {text-decoration: none;} </style> <![endif]--><!--[if gte mso 9]><style>sup { font-size: 100% !important; }</style><![endif]--><!--[if gte mso 9]> <xml> <o:OfficeDocumentSettings> <o:AllowPNG></o:AllowPNG> <o:PixelsPerInch>96</o:PixelsPerInch> </o:OfficeDocumentSettings> </xml> <![endif]--><!--<![endif]--><!--[if !mso]><!-- --> <link rel=""stylesheet"" href=""https://fonts.googleapis.com/css?family=Lato:400,400i,700,700i""> <link rel=""stylesheet"" href=""https://fonts.googleapis.com/css?family=Roboto:400,400i,700,700i""> <style type=""text/css""> .rollover:hover .rollover-first { max-height:0px!important; display:none!important; } .rollover:hover .rollover-second { max-height:none!important; display:block!important; } .rollover span { font-size:0px; } u + .body img ~ div div { display:none; } #outlook a { padding:0; } span.MsoHyperlink, span.MsoHyperlinkFollowed { color:inherit; mso-style-priority:99; } a.es-button { mso-style-priority:100!important; text-decoration:none!important; } a[x-apple-data-detectors] { color:inherit!important; text-decoration:none!important; font-size:inherit!important; font-family:inherit!important; font-weight:inherit!important; line-height:inherit!important; } .es-desk-hidden { display:none; float:left; overflow:hidden; width:0; max-height:0; line-height:0; mso-hide:all; } .es-button-border:hover > a.es-button { color:#ffffff!important; } @media only screen and (max-width:600px) {.es-m-p0r { padding-right:0px!important } .es-m-p20b { padding-bottom:20px!important } *[class=""gmail-fix""] { display:none!important } p, a { line-height:150%!important } h1, h1 a { line-height:120%!important } h2, h2 a { line-height:120%!important } h3, h3 a { line-height:120%!important } h4, h4 a { line-height:120%!important } h5, h5 a { line-height:120%!important } h6, h6 a { line-height:120%!important } h1 { font-size:36px!important; text-align:left } h2 { font-size:26px!important; text-align:left } h3 { font-size:20px!important; text-align:left } h4 { font-size:24px!important; text-align:left } h5 { font-size:20px!important; text-align:left } h6 { font-size:16px!important; text-align:left } .es-header-body h1 a, .es-content-body h1 a, .es-footer-body h1 a { font-size:36px!important } .es-header-body h2 a, .es-content-body h2 a, .es-footer-body h2 a { font-size:26px!important } .es-header-body h3 a, .es-content-body h3 a, .es-footer-body h3 a { font-size:20px!important } .es-header-body h4 a, .es-content-body h4 a, .es-footer-body h4 a { font-size:24px!important } .es-header-body h5 a, .es-content-body h5 a, .es-footer-body h5 a { font-size:20px!important } .es-header-body h6 a, .es-content-body h6 a, .es-footer-body h6 a { font-size:16px!important } .es-menu td a { font-size:12px!important } .es-header-body p, .es-header-body a { font-size:14px!important } .es-content-body p, .es-content-body a { font-size:14px!important } .es-footer-body p, .es-footer-body a { font-size:14px!important } .es-infoblock p, .es-infoblock a { font-size:12px!important } .es-m-txt-c, .es-m-txt-c h1, .es-m-txt-c h2, .es-m-txt-c h3, .es-m-txt-c h4, .es-m-txt-c h5, .es-m-txt-c h6 { text-align:center!important } .es-m-txt-r, .es-m-txt-r h1, .es-m-txt-r h2, .es-m-txt-r h3, .es-m-txt-r h4, .es-m-txt-r h5, .es-m-txt-r h6 { text-align:right!important } .es-m-txt-j, .es-m-txt-j h1, .es-m-txt-j h2, .es-m-txt-j h3, .es-m-txt-j h4, .es-m-txt-j h5, .es-m-txt-j h6 { text-align:justify!important } .es-m-txt-l, .es-m-txt-l h1, .es-m-txt-l h2, .es-m-txt-l h3, .es-m-txt-l h4, .es-m-txt-l h5, .es-m-txt-l h6 { text-align:left!important } .es-m-txt-r img, .es-m-txt-c img, .es-m-txt-l img { display:inline!important } .es-m-txt-r .rollover:hover .rollover-second, .es-m-txt-c .rollover:hover .rollover-second, .es-m-txt-l .rollover:hover .rollover-second { display:inline!important } .es-m-txt-r .rollover span, .es-m-txt-c .rollover span, .es-m-txt-l .rollover span { line-height:0!important; font-size:0!important } .es-spacer { display:inline-table } a.es-button, button.es-button { font-size:20px!important; line-height:120%!important } a.es-button, button.es-button, .es-button-border { display:inline-block!important } .es-m-fw, .es-m-fw.es-fw, .es-m-fw .es-button { display:block!important } .es-m-il, .es-m-il .es-button, .es-social, .es-social td, .es-menu { display:inline-block!important } .es-adaptive table, .es-left, .es-right { width:100%!important } .es-content table, .es-header table, .es-footer table, .es-content, .es-footer, .es-header { width:100%!important; max-width:600px!important } .adapt-img { width:100%!important; height:auto!important } .es-mobile-hidden, .es-hidden { display:none!important } .es-desk-hidden { width:auto!important; overflow:visible!important; float:none!important; max-height:inherit!important; line-height:inherit!important } tr.es-desk-hidden { display:table-row!important } table.es-desk-hidden { display:table!important } td.es-desk-menu-hidden { display:table-cell!important } .es-menu td { width:1%!important } table.es-table-not-adapt, .esd-block-html table { width:auto!important } .es-social td { padding-bottom:10px } .h-auto { height:auto!important } } @media screen and (max-width:384px) {.mail-message-content { width:414px!important } } </style> </head> <body class=""body"" style=""width:100%;height:100%;padding:0;Margin:0""> b <!--[if mso]> <style type=""text/css""> ul { margin: 0 !important; } ol { margin: 0 !important; } li { margin-left: 47px !important; } </style><![endif] --> <div dir=""ltr"" class=""es-wrapper-color"" lang=""und"" style=""background-color:#FAFAFA""><!--[if gte mso 9]> <v:background xmlns:v=""urn:schemas-microsoft-com:vml"" fill=""t""> <v:fill type=""tile"" color=""#fafafa""></v:fill> </v:background> <![endif]--> <table class=""es-wrapper"" width=""100%"" cellspacing=""0"" cellpadding=""0"" role=""none"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;padding:0;Margin:0;width:100%;height:100%;background-repeat:repeat;background-position:center top;background-color:#FAFAFA""> <tr> <td valign=""top"" style=""padding:0;Margin:0""> <table cellpadding=""0"" cellspacing=""0"" class=""es-content"" align=""center"" role=""none"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;width:100%;table-layout:fixed !important""> <tr> <td class=""es-info-area"" align=""center"" style=""padding:0;Margin:0""> <table class=""es-content-body"" align=""center"" cellpadding=""0"" cellspacing=""0"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;background-color:transparent;width:600px"" bgcolor=""#FFFFFF"" role=""none""> <tr> <td align=""left"" style=""padding:20px;Margin:0""> <table cellpadding=""0"" cellspacing=""0"" width=""100%"" role=""none"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px""> <tr> <td align=""center"" valign=""top"" style=""padding:0;Margin:0;width:560px""> <table cellpadding=""0"" cellspacing=""0"" width=""100%"" role=""presentation"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px""> <tr> <td align=""center"" class=""es-infoblock"" style=""padding:0;Margin:0""><p style=""Margin:0;mso-line-height-rule:exactly;font-family:arial, 'helvetica neue', helvetica, sans-serif;line-height:18px;letter-spacing:0;color:#CCCCCC;font-size:12px""><a target=""_blank"" href="""" style=""mso-line-height-rule:exactly;text-decoration:underline;color:#CCCCCC;font-size:12px"">View online version</a></p></td> </tr> </table></td> </tr> </table></td> </tr> </table></td> </tr> </table> <table cellpadding=""0"" cellspacing=""0"" class=""es-header"" align=""center"" role=""none"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;width:100%;table-layout:fixed !important;background-color:transparent;background-repeat:repeat;background-position:center top""> <tr> <td align=""center"" style=""padding:0;Margin:0""> <table bgcolor=""#ffffff"" class=""es-header-body"" align=""center"" cellpadding=""0"" cellspacing=""0"" role=""none"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;background-color:transparent;width:600px""> <tr> <td align=""left"" style=""Margin:0;padding-top:10px;padding-right:20px;padding-bottom:10px;padding-left:20px""> <table cellpadding=""0"" cellspacing=""0"" width=""100%"" role=""none"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px""> <tr> <td class=""es-m-p0r"" valign=""top"" align=""center"" style=""padding:0;Margin:0;width:560px""> <table cellpadding=""0"" cellspacing=""0"" width=""100%"" role=""none"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px""> <tr> <td align=""center"" style=""padding:0;Margin:0;display:none""></td> </tr> </table></td> </tr> </table></td> </tr> </table></td> </tr> </table> <table cellpadding=""0"" cellspacing=""0"" class=""es-content"" align=""center"" role=""none"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;width:100%;table-layout:fixed !important""> <tr> <td align=""center"" style=""padding:0;Margin:0""> <table bgcolor=""#ffffff"" class=""es-content-body"" align=""center"" cellpadding=""0"" cellspacing=""0"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;background-color:#FFFFFF;width:600px;border-width:1px;border-color:#3d85c6;border-style:solid"" role=""none""> <tr> <td align=""left"" style=""Margin:0;padding-right:20px;padding-left:20px;padding-top:15px;padding-bottom:30px""> <table cellpadding=""0"" cellspacing=""0"" width=""100%"" role=""none"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px""> <tr> <td align=""center"" valign=""top"" style=""padding:0;Margin:0;width:558px""> <table cellpadding=""0"" cellspacing=""0"" width=""100%"" role=""presentation"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px""> <tr> <td align=""center"" style=""padding:0;Margin:0;padding-top:10px;padding-bottom:10px;font-size:0px""><img src=""https://fhfaird.stripocdn.email/content/guids/CABINET_9ca3362f6e3ce2c3ea60da8fe4f8a850/images/78491618321704551.png"" alt="""" style=""display:block;font-size:14px;border:0;outline:none;text-decoration:none"" width=""150""></td> </tr> </table></td> </tr> </table></td> </tr> <tr> <td align=""left"" style=""Margin:0;padding-right:20px;padding-bottom:10px;padding-left:20px;padding-top:20px""> <table cellpadding=""0"" cellspacing=""0"" width=""100%"" role=""none"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px""> <tr> <td align=""center"" valign=""top"" style=""padding:0;Margin:0;width:558px""> <table cellpadding=""0"" cellspacing=""0"" width=""100%"" role=""presentation"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px""> <tr> <td align=""center"" class=""es-m-txt-l"" style=""padding:0;Margin:0;padding-bottom:10px""><h1 style=""Margin:0;font-family:lato, 'helvetica neue', helvetica, arial, sans-serif;mso-line-height-rule:exactly;letter-spacing:0;font-size:46px;font-style:normal;font-weight:bold;line-height:55px;color:#3d85c6"">Bienvenid@</h1></td> </tr> </table></td> </tr> </table></td> </tr> <tr> <td align=""left"" style=""padding:0;Margin:0;padding-right:20px;padding-left:20px;padding-bottom:20px""><!--[if mso]><table style=""width:560px"" cellpadding=""0"" cellspacing=""0""><tr><td style=""width:90px"" valign=""top""><![endif]--> <table cellpadding=""0"" cellspacing=""0"" class=""es-left"" align=""left"" role=""none"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;float:left""> <tr> <td class=""es-m-p0r es-m-p20b"" align=""center"" style=""padding:0;Margin:0;width:89px""> <table cellpadding=""0"" cellspacing=""0"" width=""100%"" role=""presentation"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px""> <tr> <td align=""center"" style=""padding:0;Margin:0;font-size:0""><img src=""https://fhfaird.stripocdn.email/content/guids/CABINET_34f3f66ff37a88b49abcac8707e36da9c3bb6b7e140059ed4b6a3ba00eb8fdbb/images/email_blue_23344.png"" alt="""" width=""89"" class=""adapt-img"" style=""display:block;font-size:14px;border:0;outline:none;text-decoration:none""></td> </tr> </table></td> </tr> </table><!--[if mso]></td><td style=""width:30px""></td><td style=""width:440px"" valign=""top""><![endif]--> <table cellpadding=""0"" cellspacing=""0"" class=""es-right"" align=""right"" role=""none"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;float:right""> <tr> <td align=""center"" style=""padding:0;Margin:0;width:439px""> <table cellpadding=""0"" cellspacing=""0"" width=""100%"" role=""presentation"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px""> <tr> <td align=""left"" style=""padding:0;Margin:0""><p align=""justify"" style=""Margin:0;mso-line-height-rule:exactly;font-family:roboto, 'helvetica neue', helvetica, arial, sans-serif;line-height:21px;letter-spacing:0;color:#333333;font-size:14px"">Es un placer darte la bienvenida al nuevo servidor de correos electrónicos que hemos implementado. Como parte de nuestro compromiso con la eficiencia y la seguridad de las comunicaciones, hemos migrado a esta plataforma con el objetivo de ofrecerte una experiencia de correo electrónico más robusta y confiable.</p></td> </tr> </table></td> </tr> </table><!--[if mso]></td></tr></table><![endif]--></td> </tr> <tr> <td align=""left"" style=""padding:20px;Margin:0""><!--[if mso]><table dir=""ltr"" cellpadding=""0""><table dir=""rtl"" style=""width:560px"" cellpadding=""0"" cellspacing=""0""><tr><td dir=""ltr"" style=""width:440px"" valign=""top""><![endif]--> <table cellpadding=""0"" cellspacing=""0"" class=""es-right"" align=""right"" role=""none"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;float:right""> <tr> <td class=""es-m-p20b"" align=""center"" style=""padding:0;Margin:0;width:89px""> <table cellpadding=""0"" cellspacing=""0"" width=""100%"" role=""presentation"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px""> <tr> <td align=""center"" style=""padding:0;Margin:0;font-size:0""><img src=""https://fhfaird.stripocdn.email/content/guids/CABINET_34f3f66ff37a88b49abcac8707e36da9c3bb6b7e140059ed4b6a3ba00eb8fdbb/images/buscar.png"" alt="""" width=""89"" class=""adapt-img"" style=""display:block;font-size:14px;border:0;outline:none;text-decoration:none""></td> </tr> </table></td> </tr> </table><!--[if mso]></td><td dir=""ltr"" style=""width:30px""></td><td dir=""ltr"" style=""width:90px"" valign=""top""><![endif]--> <table cellpadding=""0"" cellspacing=""0"" class=""es-left"" align=""left"" role=""none"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;float:left""> <tr> <td class=""es-m-p0r"" align=""center"" style=""padding:0;Margin:0;width:439px""> <table cellpadding=""0"" cellspacing=""0"" width=""100%"" role=""presentation"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px""> <tr> <td align=""left"" style=""padding:0;Margin:0""><p align=""justify"" style=""Margin:0;mso-line-height-rule:exactly;font-family:arial, 'helvetica neue', helvetica, sans-serif;line-height:21px;letter-spacing:0;color:#333333;font-size:14px"">Si en algún momento encuentras algún inconveniente o necesitas asistencia técnica, nuestro equipo de soporte técnico está disponible para ayudarte. No dudes en contactarnos ante cualquier consulta o sugerencia que puedas tener.</p></td> </tr> </table></td> </tr> </table><!--[if mso]></td></tr></table></table><![endif]--></td> </tr> <tr> <td align=""left"" style=""padding:20px;Margin:0""><!--[if mso]><table dir=""ltr"" cellpadding=""0""><table dir=""rtl"" style=""width:560px"" cellpadding=""0"" cellspacing=""0""><tr><td dir=""ltr"" style=""width:440px"" valign=""top""><![endif]--> <table cellpadding=""0"" cellspacing=""0"" class=""es-right"" align=""right"" role=""none"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;float:right""> <tr> <td class=""es-m-p20b"" align=""center"" style=""padding:0;Margin:0;width:89px""> <table cellpadding=""0"" cellspacing=""0"" width=""100%"" role=""presentation"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px""> <tr> <td align=""center"" style=""padding:0;Margin:0;font-size:0""><img src=""https://fhfaird.stripocdn.email/content/guids/CABINET_34f3f66ff37a88b49abcac8707e36da9c3bb6b7e140059ed4b6a3ba00eb8fdbb/images/things_34164.png"" alt="""" width=""89"" class=""adapt-img"" style=""display:block;font-size:14px;border:0;outline:none;text-decoration:none""></td> </tr> </table></td> </tr> </table><!--[if mso]></td><td dir=""ltr"" style=""width:30px""></td><td dir=""ltr"" style=""width:90px"" valign=""top""><![endif]--> <table cellpadding=""0"" cellspacing=""0"" class=""es-left"" align=""left"" role=""none"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;float:left""> <tr> <td class=""es-m-p0r"" align=""center"" style=""padding:0;Margin:0;width:439px""> <table cellpadding=""0"" cellspacing=""0"" width=""100%"" role=""presentation"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px""> <tr> <td align=""left"" style=""padding:0;Margin:0""><p align=""justify"" style=""Margin:0;mso-line-height-rule:exactly;font-family:arial, 'helvetica neue', helvetica, sans-serif;line-height:21px;letter-spacing:0;color:#333333;font-size:14px"">Una vez más, te damos la bienvenida al nuevo servidor de correos electrónicos y esperamos que esta plataforma contribuya positivamente a tu productividad y eficiencia en tus comunicaciones.</p></td> </tr> </table></td> </tr> </table><!--[if mso]></td></tr></table></table><![endif]--></td> </tr> <tr> <td align=""left"" style=""Margin:0;padding-right:20px;padding-bottom:10px;padding-left:20px;padding-top:20px""> <table cellpadding=""0"" cellspacing=""0"" width=""100%"" role=""none"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px""> <tr> <td align=""center"" valign=""top"" style=""padding:0;Margin:0;width:558px""> <table cellpadding=""0"" cellspacing=""0"" width=""100%"" role=""presentation"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px""> <tr> <td align=""center"" class=""es-m-txt-l"" style=""padding:0;Margin:0""><h2 style=""Margin:0;font-family:arial, 'helvetica neue', helvetica, sans-serif;mso-line-height-rule:exactly;letter-spacing:0;font-size:26px;font-style:normal;font-weight:bold;line-height:31px;color:#3d85c6"">SERIE DE CARACTERÍSTICAS</h2></td> </tr> </table></td> </tr> </table></td> </tr> <tr> <td class=""esdev-adapt-off"" align=""left"" style=""Margin:0;padding-right:20px;padding-bottom:10px;padding-left:20px;padding-top:15px""> <table cellpadding=""0"" cellspacing=""0"" class=""esdev-mso-table"" role=""none"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;width:560px""> <tr> <td class=""esdev-mso-td"" valign=""top"" style=""padding:0;Margin:0""> <table cellpadding=""0"" cellspacing=""0"" class=""es-left"" align=""left"" role=""none"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;float:left""> <tr> <td align=""left"" style=""padding:0;Margin:0;width:49px""> <table cellpadding=""0"" cellspacing=""0"" width=""100%"" role=""presentation"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px""> <tr> <td align=""center"" style=""padding:0;Margin:0;font-size:0px""><img class=""adapt-img"" src=""https://fhfaird.stripocdn.email/content/guids/CABINET_3d0df4c18b0cea2cd3d10f772261e0b3/images/2851617878322771.png"" alt="""" style=""display:block;font-size:14px;border:0;outline:none;text-decoration:none"" width=""25""></td> </tr> </table></td> </tr> </table></td> <td style=""padding:0;Margin:0;width:20px""></td> <td class=""esdev-mso-td"" valign=""top"" style=""padding:0;Margin:0""> <table cellpadding=""0"" cellspacing=""0"" class=""es-right"" align=""right"" role=""none"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;float:right""> <tr> <td align=""left"" style=""padding:0;Margin:0;width:489px""> <table cellpadding=""0"" cellspacing=""0"" width=""100%"" role=""presentation"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px""> <tr> <td align=""left"" style=""padding:0;Margin:0;padding-top:5px""><p style=""Margin:0;mso-line-height-rule:exactly;font-family:arial, 'helvetica neue', helvetica, sans-serif;line-height:21px;letter-spacing:0;color:#3d85c6;font-size:14px"">Puedes configurar credenciales SMTP.</p></td> </tr> </table></td> </tr> </table></td> </tr> </table></td> </tr> <tr> <td class=""esdev-adapt-off"" align=""left"" style=""Margin:0;padding-top:10px;padding-right:20px;padding-bottom:10px;padding-left:20px""> <table cellpadding=""0"" cellspacing=""0"" class=""esdev-mso-table"" role=""none"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;width:560px""> <tr> <td class=""esdev-mso-td"" valign=""top"" style=""padding:0;Margin:0""> <table cellpadding=""0"" cellspacing=""0"" class=""es-left"" align=""left"" role=""none"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;float:left""> <tr> <td align=""left"" style=""padding:0;Margin:0;width:49px""> <table cellpadding=""0"" cellspacing=""0"" width=""100%"" role=""presentation"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px""> <tr> <td align=""center"" style=""padding:0;Margin:0;font-size:0px""><img class=""adapt-img"" src=""https://fhfaird.stripocdn.email/content/guids/CABINET_3d0df4c18b0cea2cd3d10f772261e0b3/images/2851617878322771.png"" alt="""" style=""display:block;font-size:14px;border:0;outline:none;text-decoration:none"" width=""25""></td> </tr> </table></td> </tr> </table></td> <td style=""padding:0;Margin:0;width:20px""></td> <td class=""esdev-mso-td"" valign=""top"" style=""padding:0;Margin:0""> <table cellpadding=""0"" cellspacing=""0"" class=""es-right"" align=""right"" role=""none"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;float:right""> <tr> <td align=""left"" style=""padding:0;Margin:0;width:489px""> <table cellpadding=""0"" cellspacing=""0"" width=""100%"" role=""presentation"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px""> <tr> <td align=""left"" style=""padding:0;Margin:0;padding-top:5px""><p style=""Margin:0;mso-line-height-rule:exactly;font-family:arial, 'helvetica neue', helvetica, sans-serif;line-height:21px;letter-spacing:0;color:#3d85c6;font-size:14px"">Puedes administrar usuarios y realizar operaciones CRUD.</p></td> </tr> </table></td> </tr> </table></td> </tr> </table></td> </tr> <tr> <td class=""esdev-adapt-off"" align=""left"" style=""Margin:0;padding-top:10px;padding-right:20px;padding-bottom:10px;padding-left:20px""> <table cellpadding=""0"" cellspacing=""0"" class=""esdev-mso-table"" role=""none"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;width:560px""> <tr> <td class=""esdev-mso-td"" valign=""top"" style=""padding:0;Margin:0""> <table cellpadding=""0"" cellspacing=""0"" class=""es-left"" align=""left"" role=""none"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;float:left""> <tr> <td align=""left"" style=""padding:0;Margin:0;width:49px""> <table cellpadding=""0"" cellspacing=""0"" width=""100%"" role=""presentation"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px""> <tr> <td align=""center"" style=""padding:0;Margin:0;font-size:0px""><img class=""adapt-img"" src=""https://fhfaird.stripocdn.email/content/guids/CABINET_3d0df4c18b0cea2cd3d10f772261e0b3/images/2851617878322771.png"" alt="""" style=""display:block;font-size:14px;border:0;outline:none;text-decoration:none"" width=""25""></td> </tr> </table></td> </tr> </table></td> <td style=""padding:0;Margin:0;width:20px""></td> <td class=""esdev-mso-td"" valign=""top"" style=""padding:0;Margin:0""> <table cellpadding=""0"" cellspacing=""0"" class=""es-right"" align=""right"" role=""none"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;float:right""> <tr> <td align=""left"" style=""padding:0;Margin:0;width:489px""> <table cellpadding=""0"" cellspacing=""0"" width=""100%"" role=""presentation"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px""> <tr> <td align=""left"" style=""padding:0;Margin:0""><p style=""Margin:0;mso-line-height-rule:exactly;font-family:arial, 'helvetica neue', helvetica, sans-serif;line-height:21px;letter-spacing:0;color:#3d85c6;font-size:14px"">Envío de correos electrónicos de manera fácil y eficiente.</p></td> </tr> </table></td> </tr> </table></td> </tr> </table></td> </tr> <tr> <td class=""esdev-adapt-off"" align=""left"" style=""Margin:0;padding-top:10px;padding-right:20px;padding-bottom:10px;padding-left:20px""> <table cellpadding=""0"" cellspacing=""0"" class=""esdev-mso-table"" role=""none"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;width:560px""> <tr> <td class=""esdev-mso-td"" valign=""top"" style=""padding:0;Margin:0""> <table cellpadding=""0"" cellspacing=""0"" class=""es-left"" align=""left"" role=""none"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;float:left""> <tr> <td align=""left"" style=""padding:0;Margin:0;width:49px""> <table cellpadding=""0"" cellspacing=""0"" width=""100%"" role=""none"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px""> <tr> <td align=""center"" style=""padding:0;Margin:0;display:none""></td> </tr> </table></td> </tr> </table></td> <td style=""padding:0;Margin:0;width:20px""></td> <td class=""esdev-mso-td"" valign=""top"" style=""padding:0;Margin:0""> <table cellpadding=""0"" cellspacing=""0"" class=""es-right"" align=""right"" role=""none"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;float:right""> <tr> <td align=""left"" style=""padding:0;Margin:0;width:489px""> <table cellpadding=""0"" cellspacing=""0"" width=""100%"" role=""none"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px""> <tr> <td align=""center"" style=""padding:0;Margin:0;display:none""></td> </tr> </table></td> </tr> </table></td> </tr> </table></td> </tr> </table></td> </tr> </table> <table cellpadding=""0"" cellspacing=""0"" class=""es-footer"" align=""center"" role=""none"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;width:100%;table-layout:fixed !important;background-color:transparent;background-repeat:repeat;background-position:center top""> <tr> <td align=""center"" style=""padding:0;Margin:0""> <table class=""es-footer-body"" align=""center"" cellpadding=""0"" cellspacing=""0"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;background-color:transparent;width:600px"" role=""none""> <tr> <td align=""left"" style=""Margin:0;padding-right:20px;padding-left:20px;padding-top:20px;padding-bottom:20px""> <table cellpadding=""0"" cellspacing=""0"" width=""100%"" role=""none"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px""> <tr> <td align=""left"" style=""padding:0;Margin:0;width:560px""> <table cellpadding=""0"" cellspacing=""0"" width=""100%"" role=""presentation"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px""> <tr> <td align=""center"" style=""padding:0;Margin:0;padding-top:15px;padding-bottom:15px;font-size:0""> <table cellpadding=""0"" cellspacing=""0"" class=""es-table-not-adapt es-social"" role=""presentation"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px""> <tr> <td align=""center"" valign=""top"" class=""es-m-p0r"" style=""padding:0;Margin:0;padding-right:40px""><a target=""_blank"" href=""https://www.facebook.com/profile.php?id=100008196004603&mibextid=ibOpuV"" style=""mso-line-height-rule:exactly;text-decoration:underline;color:#333333;font-size:12px""><img title=""Facebook"" src=""https://fhfaird.stripocdn.email/content/assets/img/social-icons/logo-black/facebook-logo-black.png"" alt=""Fb"" height=""35"" width=""35"" style=""display:block;font-size:14px;border:0;outline:none;text-decoration:none""></a></td> <td align=""center"" valign=""top"" class=""es-m-p0r"" style=""padding:0;Margin:0;padding-right:40px""><a target=""_blank"" href=""https://x.com/valeehds"" style=""mso-line-height-rule:exactly;text-decoration:underline;color:#333333;font-size:12px""><img title=""X.com"" src=""https://fhfaird.stripocdn.email/content/assets/img/social-icons/logo-black/x-logo-black.png"" alt=""X"" height=""35"" width=""35"" style=""display:block;font-size:14px;border:0;outline:none;text-decoration:none""></a></td> <td align=""center"" valign=""top"" style=""padding:0;Margin:0""><a target=""_blank"" href=""https://www.instagram.com/valeehds"" style=""mso-line-height-rule:exactly;text-decoration:underline;color:#333333;font-size:12px""><img title=""Instagram"" src=""https://fhfaird.stripocdn.email/content/assets/img/social-icons/logo-black/instagram-logo-black.png"" alt=""Inst"" height=""35"" width=""35"" style=""display:block;font-size:14px;border:0;outline:none;text-decoration:none""></a></td> </tr> </table></td> </tr> <tr> <td align=""center"" style=""padding:0;Margin:0;padding-bottom:35px""><p style=""Margin:0;mso-line-height-rule:exactly;font-family:arial, 'helvetica neue', helvetica, sans-serif;line-height:18px;letter-spacing:0;color:#333333;font-size:12px"">Servidor de correos electrónicos © 2024 &nbsp;All Rights Reserved.</p><p style=""Margin:0;mso-line-height-rule:exactly;font-family:arial, 'helvetica neue', helvetica, sans-serif;line-height:18px;letter-spacing:0;color:#333333;font-size:12px"">​</p></td> </tr> </table></td> </tr> </table></td> </tr> </table></td> </tr> </table> <table cellpadding=""0"" cellspacing=""0"" class=""es-content"" align=""center"" role=""none"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;width:100%;table-layout:fixed !important""> <tr> <td class=""es-info-area"" align=""center"" style=""padding:0;Margin:0""> <table class=""es-content-body"" align=""center"" cellpadding=""0"" cellspacing=""0"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;background-color:transparent;width:600px"" bgcolor=""#FFFFFF"" role=""none""> <tr> <td align=""left"" style=""padding:20px;Margin:0""> <table cellpadding=""0"" cellspacing=""0"" width=""100%"" role=""none"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px""> <tr> <td align=""center"" valign=""top"" style=""padding:0;Margin:0;width:560px""> <table cellpadding=""0"" cellspacing=""0"" width=""100%"" role=""presentation"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px""> <tr> <td align=""center"" class=""es-infoblock"" style=""padding:0;Margin:0""><p style=""Margin:0;mso-line-height-rule:exactly;font-family:arial, 'helvetica neue', helvetica, sans-serif;line-height:18px;letter-spacing:0;color:#CCCCCC;font-size:12px""><a target=""_blank"" href="""" style=""mso-line-height-rule:exactly;text-decoration:underline;color:#CCCCCC;font-size:12px""></a>No longer want to receive these emails?&nbsp;<a href="""" target=""_blank"" style=""mso-line-height-rule:exactly;text-decoration:underline;color:#CCCCCC;font-size:12px"">Unsubscribe</a>.<a target=""_blank"" href="""" style=""mso-line-height-rule:exactly;text-decoration:underline;color:#CCCCCC;font-size:12px""></a></p></td> </tr> </table></td> </tr> </table></td> </tr> </table></td> </tr> </table></td> </tr> </table> </div> <link rel=""stylesheet"" href=""https://fonts.googleapis.com/css?family=Lato:400,400i,700,700i""> <link rel=""stylesheet"" href=""https://fonts.googleapis.com/css?family=Merriweather+Sans:400,400i,700,700i""><!--[if !mso]><!-- --> <link rel=""stylesheet"" href=""https://fonts.googleapis.com/css?family=Lato:400,400i,700,700i""> <link rel=""stylesheet"" href=""https://fonts.googleapis.com/css?family=Roboto:400,400i,700,700i""> </body> </html>";

            return cuerpoCorreo;
        }

        private void enviarCorreo()
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Debe seleccionar filas para continuar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // Leer las credenciales SMTP del archivo
                string[] credenciales = File.ReadAllLines(@"C:\Users\ana.hernandez\OneDrive - Sistemas GyG S.A\Escritorio\credenciales.txt");
                string smtpHost = credenciales[0];
                int smtpPort = int.Parse(credenciales[1]);
                string usuarioSmtp = credenciales[2];
                string contraseñaSmtp = credenciales[3];

                // Configuración del cliente SMTP
                SmtpClient smtpClient = new SmtpClient(smtpHost);
                smtpClient.Port = smtpPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(usuarioSmtp, contraseñaSmtp);
                smtpClient.EnableSsl = true;

                string nombreCompleto = dataGridView1.SelectedRows[0].Cells["nombre"].Value?.ToString();

                string nombre = ObtenerPrimeraPalabra(nombreCompleto);

                string cuerpoCorreo = ConstruirCuerpoCorreo(nombre);

                // Crear el mensaje
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(usuarioSmtp);
                mailMessage.Subject = "Sea bienvenido a nuestro aplicativo"+nombre;
                mailMessage.Body = cuerpoCorreo;
                mailMessage.IsBodyHtml = true;

                // Obtener los destinatarios de las filas seleccionadas
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    string correoDestinatario = row.Cells["correo"].Value?.ToString();
                    if (!string.IsNullOrEmpty(correoDestinatario))
                    {
                        mailMessage.To.Add(correoDestinatario);
                    }
                }

                // Enviar el correo
                smtpClient.Send(mailMessage);

                MessageBox.Show("Correo enviado correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarDatosDesdeJson();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al enviar el correo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string ObtenerPrimeraPalabra(string texto)
        {
            string[] palabras = texto.Split(' ');
            if (palabras.Length > 0)
            {
                return palabras[0];
            }
            else
            {
                return string.Empty;
            }
        }

        private void BtnEnviarCorreo_Click(object sender, EventArgs e)
        {
            enviarCorreo();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.OleDb; //Libreria OleDB
using System.Data; //System Data

namespace VentaJuegosDB
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OleDbConnection con;
        DataTable dt;
        public MainWindow()
        {
            InitializeComponent();
            //conectamos la base de datos
            con = new OleDbConnection();
            con.ConnectionString = "Provider=Microsoft.Jet.Oledb.4.0; Data Source=" + AppDomain.CurrentDomain.BaseDirectory+ "\\VentaJuegosDB.mdb";
            MostrarDatos();
        }
        //Registros de la tabla

            private void MostrarDatos()
        {
            OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd.Connection = con;
            cmd.CommandText = "Select * from Juegos";
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);
            gvDatos.ItemsSource = dt.AsDataView();

            if (dt.Rows.Count>0)
            {
                lbContenido.Visibility = System.Windows.Visibility.Hidden;
                gvDatos.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                lbContenido.Visibility = System.Windows.Visibility.Visible;
                gvDatos.Visibility = System.Windows.Visibility.Hidden;
            }
        }
        //Para limpieza de los campos 

            private void LimpiartTodo()
        {
            txtId.Text = "";
            txtnombreVideojuego.Text = "";
            txtDireccion.Text = "";
            cbDesarrolladores.SelectedIndex = 0;
            txtprecio.Text = "";
            txtTelefono.Text = "";
            btnNuevo.Content = "Nuevo";
            txtId.IsEnabled = true;
        }

        private void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd.Connection = con;

            if (txtId.Text != "")
            {
                if (txtId.IsEnabled == true)
                {
                    if (cbDesarrolladores.Text != "Selecciona el desarrollador ")
                    {
                        cmd.CommandText = "insert into Juegos(Id,Nombre,Precio,Telefono,Direccion,Desarrolladores)" + "Values("
                            + txtnombreVideojuego.Text + ",'" + txtId.Text + "','" + cbDesarrolladores.Text + "','" + txtprecio.Text + "'," + txtTelefono.Text + ",'"+ txtDireccion.Text + "')";
                        cmd.ExecuteNonQuery();
                        MostrarDatos();
                        MessageBox.Show("Videojuego agregado a la tienda...");
                        LimpiartTodo();
                    }
                    else
                    {
                        MessageBox.Show("Seleccione el Desarrollador....");
                    }
                }
                else
                {
                    cmd.CommandText = "update Juegos set Nombre='" + txtnombreVideojuego.Text
                        + "',Desarrollador='" + cbDesarrolladores.Text + "',Telfono='" + txtTelefono.Text
                        + "',Direccion='" + txtDireccion.Text + "',Precio='" + txtprecio +  "'where id=" + txtId.Text;
                    cmd.ExecuteNonQuery();
                    MostrarDatos();
                    MessageBox.Show("Datos del Juego Actualizados...");
                    LimpiartTodo();
                }
            }
            else
            {
                MessageBox.Show("Agregue el Id del Videojuego porfavor...");
            }
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (gvDatos.SelectedItems.Count>0)
            {
                DataRowView row = (DataRowView)gvDatos.SelectedItems[0];
                txtId.Text = row["id"].ToString();
                txtnombreVideojuego.Text = row["Nombre del Videojuego"].ToString();
                cbDesarrolladores.Text = row["Desarrollador"].ToString();
                txtprecio.Text = row["Precio"].ToString();
                txtTelefono.Text = row["Telefono"].ToString();
                txtDireccion.Text = row["Direccion"].ToString();
                txtId.IsEnabled = false;
                btnNuevo.Content = " Actualizar ";
            }
            else
            {
                MessageBox.Show("Porfavor Seleccion el Videojuego a editar...");
            }
        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (gvDatos.SelectedItems.Count > 0 )
            {
                DataRowView row = (DataRowView)gvDatos.SelectedItems[0];
                OleDbCommand cmd = new OleDbCommand();
                if (con.State != ConnectionState.Open)
                    con.Open();
                cmd.Connection = con;
                cmd.CommandText = "delete from Juegos where Id=" + row["Id"].ToString();
                cmd.ExecuteNonQuery();
                MostrarDatos();
                MessageBox.Show("Juego eliminado de la base de datos correctamente...");
                LimpiartTodo();
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            LimpiartTodo();
        }

        private void BtnSalir_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}

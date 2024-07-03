using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Data.SqlClient;
using System.Data;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;



namespace WPF_EstacionBombeo.Paginas
{
	/// <summary>
	/// Lógica de interacción para Alarmasx.xaml
	/// </summary>
	public partial class Alarmas : Page
    {
		string conectionString = "server=localhost ; database = Estacion_Bombeo ; integrated security = true";

		public Alarmas()
        {
            InitializeComponent();
            //conexion a la base de datos
            //string miConexion = ConfigurationManager.ConnectionStrings["Estacion_Bombeo.Properties.Settings.Estacion_BombeoConnectionString"].ConnectionString;
           
            //Le recordamos a nuestra app que vamos a realizar consultas de tipo sql con la base de datos que indicamos arriba 

         //  miConexionSql = new SqlConnection(miConexion); //instanciamos el objeto creado abajo miConexionSQL

        }

        private void btnInicio_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Inicio()); //abrir pagina
        }

        private void btnAlarmas_Click(object sender, RoutedEventArgs e)
        
            {
			this.NavigationService.Refresh();
        }
        

        //
        //void navigateRefreshButton_Click(object sender, RoutedEventArgs e) { this.Refresh(); }
        //

        private void Page_Loaded(object sender, RoutedEventArgs e)
		{
            CargarDatos();
            
        }

		private void CargarDatos()
		{
			SqlConnection conexion = new SqlConnection(conectionString); //para que sepamos a que base de datos nos vamos a conectar
			try
			{
                lbl_conectado.Visibility = Visibility.Visible;
                lbl_desconectado.Visibility = Visibility.Hidden;

                conexion.Open();
				string consulta = "sp_AlarmasTOP15";
				SqlCommand comando = new SqlCommand(consulta, conexion);//consulta lo de arriba utilizando conexion
				SqlDataAdapter da = new SqlDataAdapter(comando); // intermediario, se encarga de traer la info
				DataTable tabla = new DataTable();
				da.Fill(tabla); //poner la info en la tabla
				dgvAlarmas.ItemsSource = tabla.DefaultView; //cargamos datagrid
			}
			catch (Exception ex) { MessageBox.Show(ex.Message); }
			finally { conexion.Close(); }
		}

		private void btnRefresh_Click(object sender, RoutedEventArgs e)
		{
            CargarDatos();
            
        }
	}
}

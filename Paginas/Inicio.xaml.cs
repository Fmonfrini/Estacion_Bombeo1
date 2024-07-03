using System;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using WPF_EstacionBombeo.Ventanas;
using System.Windows.Threading;
using System.Windows.Media;
using System.Collections.Generic;
using System.Linq;

namespace WPF_EstacionBombeo.Paginas
{
	/// <summary>
	/// Lógica de interacción para Inicio.xaml
	/// </summary>
	public partial class Inicio : Page
    {

		DispatcherTimer dispatcherTimer; // libreria o reloj para la funcion de actualizar automatico

		public Inicio()
        {
            InitializeComponent();
			dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = new TimeSpan(0,0, 5); // configuramos Cada 0 horas, 0 minutos, 5 segundos, hará la carga de datos de nuevo para ver si hay actualización.
			dispatcherTimer.Tick += DispatcherTimer_Tick; //cada vez que llega al tiempo al segundo 5
            dispatcherTimer.Start(); // Se inicia el reloj
		}

		private void DispatcherTimer_Tick(object sender, EventArgs e) //esto es lo que hace cada cierto tiempo(5s)
		{
			CargarDatos(true);
			CargarHistorico();
		}

		string conectionString = "server=localhost ; database = Estacion_Bombeo ; integrated security = true"; //conexion
		// Generamos las variables para luego calcular si estan fuera de rango
		
		private float Caudal_Max;
		public float Presion_Min; 
		public float Presion_Max; 

        private int HoraFunc_Min;
        private int HoraFunc_Max;

		public float HoraFunc;
		private float TempBomba_Min;
		public float TempBomba_Max;
		public float TempAmb_Min;
		public float TempAmb_Max;
		public float Caudal_Min;

		private void btnInicio_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Inicio()); //abrir pagina
        }


        private void btnAlarmas_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Alarmas());//abrir pagina
        }

        private void TI_EBR101_Click(object sender, RoutedEventArgs e) //boton Temp° Bomba
        {
            TI_EBR101 VentanaTIEBR101 = new TI_EBR101("TempBomba"); //  - Creo el objeto ventana a partir de la plantilla
            VentanaTIEBR101.ShowDialog(); // muestro el objeto
			CargarDatos();
			CargarHistorico();
		}

        private void btnTIEBR102_Click(object sender, RoutedEventArgs e)
        {
            TI_EBR101 VentanaTIEBR101 = new TI_EBR101("TempAmb"); //  nombre de ventana - Creo el objeto ventana
            VentanaTIEBR101.ShowDialog(); // muestro el objeto
			CargarDatos();
			CargarHistorico();
		}

        private void btnFIEBR101_Click(object sender, RoutedEventArgs e)
        {
            TI_EBR101 VentanaTIEBR101 = new TI_EBR101("Caudal"); // 
            VentanaTIEBR101.ShowDialog(); // muestro el objeto
			CargarDatos();
			CargarHistorico();
		}

        private void btnPIEBR101_Click(object sender, RoutedEventArgs e)
        {
            TI_EBR101 VentanaTIEBR101 = new TI_EBR101("Presion"); // TI_EBR101 nombre de ventana - Creo el objeto ventana
            VentanaTIEBR101.ShowDialog(); // muestro el objeto
			CargarDatos();
			CargarHistorico();
		}

        private void btnKICKEBR101_Click(object sender, RoutedEventArgs e)
        {
            TI_EBR101 VentanaTIEBR101 = new TI_EBR101("HoraFunc"); // 
            VentanaTIEBR101.ShowDialog(); // muestro el objeto
			CargarDatos();
			CargarHistorico();
		}


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            CargarDatos(true);// Acá generar alarma
			CargarHistorico();
		}

        //private void CargarDatos(bool insertarAlarma = false) //se envia parametro que por default esta en falso
        //por default insertar alarma no lo va a insertar nunca
        private void CargarDatos(bool insertarAlarma = false) //se envia parametro que por default esta en falso
        //por default insertar alarma no lo va a insertar nunca
		{

			lbl_ebr101.Background = Brushes.Transparent;  // configuramos
			lbl_ti_ebr102.Background = Brushes.Transparent;
			lbl_pi_ebr101.Background = Brushes.Transparent;
			lbl_fi_ebr101.Background = Brushes.Transparent;
			lbl_kick_ebr101.Background = Brushes.Transparent;

			SqlConnection conexion = new SqlConnection(conectionString); //para que sepamos a que base de datos nos vamos a conectar
            try
            {
                lbl_conectado.Visibility = Visibility.Visible;
                lbl_desconectado.Visibility = Visibility.Hidden;

                conexion.Open();
                string consulta = "sp_VariablesValorActual";
                SqlCommand comando = new SqlCommand(consulta, conexion);//consulta lo de arriba utilizando conexion
                SqlDataAdapter da = new SqlDataAdapter(comando); // intermediario, se encarga de traer la info
                DataTable tabla = new DataTable();
                da.Fill(tabla); //poner la info en la tabla

				//le decimos que tenemos que convertir a float
				float TempBomba_Actual = float.Parse(tabla.Rows[0]["TempBomba_Actual"].ToString());
				float TempAmb_Actual = float.Parse(tabla.Rows[0]["TempAmb_Actual"].ToString());
				float PresionConducto_Actual = float.Parse(tabla.Rows[0]["PresionConducto_Actual"].ToString());
				float Caudal_Actual = float.Parse(tabla.Rows[0]["Caudal_Actual"].ToString());
				float HoraFunc_Actual  = float.Parse(tabla.Rows[0]["HoraFunc_Actual"].ToString());

				//llenamos el lbl
				lbl_ebr101.Content = TempBomba_Actual;
                lbl_ti_ebr102.Content = TempAmb_Actual;
                lbl_pi_ebr101.Content = PresionConducto_Actual;
                lbl_fi_ebr101.Content = Caudal_Actual;
                lbl_kick_ebr101.Content = HoraFunc_Actual;

				//generamos consulta para obtener configuraciones
				//Traemos los valores actuales de configuraciones

				consulta = "sp_ValorCfg";
				comando = new SqlCommand(consulta, conexion);//consulta lo de arriba utilizando conexion
				da = new SqlDataAdapter(comando); // intermediario, se encarga de traer la info
				tabla = new DataTable();
				da.Fill(tabla); //poner la info en la tabla
				TempBomba_Min = float.Parse(tabla.Rows[0]["TempBomba_Min"].ToString());
				TempBomba_Max = float.Parse(tabla.Rows[0]["TempBomba_Max"].ToString());
				TempAmb_Min = float.Parse(tabla.Rows[0]["TempAmb_Min"].ToString());
				TempAmb_Max = float.Parse(tabla.Rows[0]["TempAmb_Max"].ToString());
				Caudal_Min = float.Parse(tabla.Rows[0]["Caudal_Min"].ToString());
                Caudal_Max = float.Parse(tabla.Rows[0]["Caudal_Max"].ToString());
				Presion_Min = float.Parse(tabla.Rows[0]["Presion_Min"].ToString());
				Presion_Max = float.Parse(tabla.Rows[0]["Presion_Max"].ToString());
				HoraFunc_Min = int.Parse(tabla.Rows[0]["HoraFunc_Min"].ToString()); //es entero
                HoraFunc_Max = int.Parse(tabla.Rows[0]["HoraFunc_Max"].ToString());
                Dictionary<string, float> insertAlarma = new Dictionary<string, float>();

				//comparamos para generar alarmas.

				if (TempBomba_Actual < TempBomba_Min || TempBomba_Actual > TempBomba_Max) // || = OR - Esto comparamos
				{
					if (TempBomba_Actual < TempBomba_Min)// si pasa esto
					{
						insertAlarma.Add("TempBomb_Lo", TempBomba_Actual); //hacemos esto, insertamos en TempBomba_Lo el valor Actual
					}
					else if (TempBomba_Actual > TempBomba_Max) // y sino verificamos
					{
						insertAlarma.Add("TempBomb_Hi", TempBomba_Actual); //y si es correcto hacemos esto
					}
					lbl_ebr101.Background = Brushes.Red;
				}
			

				if (TempAmb_Actual < TempAmb_Min || TempAmb_Actual > TempAmb_Max)
				{
					if (TempAmb_Actual < TempAmb_Min)
					{
						insertAlarma.Add("TempAmb_Lo", TempAmb_Actual);
					}
					else if (TempAmb_Actual > TempAmb_Max)
					{
						insertAlarma.Add("TempAmb_Hi", TempAmb_Actual);
					}
					lbl_ti_ebr102.Background = Brushes.Red;
				}
				if (PresionConducto_Actual < Presion_Min || PresionConducto_Actual > Presion_Max)
				{
					if (PresionConducto_Actual < Presion_Min)
					{
						insertAlarma.Add("Presion_Lo", PresionConducto_Actual);
					}
					else if (PresionConducto_Actual > Presion_Max)
					{
						insertAlarma.Add("Presion_Hi", PresionConducto_Actual);
					}
					lbl_pi_ebr101.Background = Brushes.Red;
				}
				if (Caudal_Actual < Caudal_Min || Caudal_Actual > Caudal_Max)
				{
					if (Caudal_Actual < Caudal_Min)
					{
						insertAlarma.Add("Caudal_Lo", Caudal_Actual);
					}
					else if (Caudal_Actual > Caudal_Max)
					{
						insertAlarma.Add("Caudal_Hi", Caudal_Actual);
					}

					lbl_fi_ebr101.Background = Brushes.Red;
				}
				//Hora max y min
				if (HoraFunc_Actual < HoraFunc_Min || HoraFunc_Actual > HoraFunc_Max)
				{
					if (HoraFunc_Actual < HoraFunc_Min)
					{
						
					}
					else if (HoraFunc_Actual > HoraFunc_Max)
					{
						insertAlarma.Add("HoraFunc_HiHi", HoraFunc_Actual);
					}
					lbl_kick_ebr101.Background = Brushes.Red;
				}
                    //

                    if (insertarAlarma) // si queremos insertar una alarma hacemos lo siguiente
				{
					// Si hubo una alarma, insertarlo en la tabla
					if (insertAlarma.Count > 0) // (cuantos valores tiene )
					{
						string ConsultaInsertAlarma = "INSERT INTO TablaAlarmas ("; //hacemos esto depende cuantos valores tenga

                        //con el foreach generamos que columnas se van a guardar
						//key generamos las columnas que estan fuera de rango
						// insertAlarma[key] accedemos al valor = que insertAlarma.Keys

                        foreach (string key in insertAlarma.Keys) 
						{
							ConsultaInsertAlarma += key + ",";
						}
						ConsultaInsertAlarma += "FechaHora)";
						ConsultaInsertAlarma += "VALUES (";

						foreach (string key in insertAlarma.Keys)
						{
							ConsultaInsertAlarma += insertAlarma[key] + ",";
						}
						ConsultaInsertAlarma += " GetDate());";//agregamos fecha actual a ConsultaInsertAlarma

						comando = new SqlCommand(ConsultaInsertAlarma, conexion);//consulta lo de arriba utilizando conexion
						comando.ExecuteNonQuery();
                    }
				}

			}

            //catch (Exception ex) { MessageBox.Show(ex.Message); }
            catch (SqlException)
			{	
                lbl_conectado.Visibility = Visibility.Hidden;
				lbl_desconectado.Visibility = Visibility.Visible;
				MessageBox.Show("Falla Alarmas");
			
                

        } 
			//si hay error. nos muestra el error con: ex exception, luego no me sirve porque me traba el programa

           // finally { conexion.Close(); } // Cerramos conexion - siempre
        }

      

        private void btnHistoricos_click(object sender, RoutedEventArgs e)
        {
			
            CargarDatos();
			CargarHistorico();
        }

		private void CargarHistorico()
		{
			SqlConnection conexion = new SqlConnection(conectionString); //para que sepamos a que base de datos nos vamos a conectar
			try
			{
				conexion.Open();
				// Consulto ultimos 5 con procedimiento almacenado 
				string consulta = "sp_InicioTOP5";
				SqlCommand comando = new SqlCommand(consulta, conexion);//consulta lo de arriba utilizando conexion
				SqlDataAdapter da = new SqlDataAdapter(comando); // intermediario, se encarga de traer la info
				DataTable tabla = new DataTable();
				da.Fill(tabla); //poner la info en la tabla
				dataGridHistorico.ItemsSource = tabla.DefaultView;
                CargarDatos();
            }
            catch (SqlException)
            {
                
                MessageBox.Show("Falla select top 5");
            }

            finally { conexion.Close(); }
		}
		//cuando cierro la pagina se detiene el reloj con stop y se anula con null.
		private void Page_Unloaded_1(object sender, RoutedEventArgs e) 
		{
			//dispatcherTimer.Stop();
			//dispatcherTimer = null;
		}
	}
}

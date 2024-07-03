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
            dispatcherTimer.Interval = new TimeSpan(0, 0, 5); // configuramos Cada 0 horas, 0 minutos, 5 segundos, hará la carga de datos de nuevo para ver si hay actualización.
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

        private void TI_EBR101_Click(object sender, EventArgs e) //boton Temp° Bomba
        {
            TI_EBR101 VentanaTIEBR101 = new TI_EBR101("TempBomba"); //  - Creo el objeto ventana a partir de la plantilla
            VentanaTIEBR101.ShowDialog(); // muestro el objeto
            CargarDatos();
            CargarHistorico();
        }

        private void btnTIEBR102_Click(object sender, EventArgs e)
        {
            TI_EBR101 VentanaTIEBR101 = new TI_EBR101("TempAmb"); //  nombre de ventana - Creo el objeto ventana
            VentanaTIEBR101.ShowDialog(); // muestro el objeto
            CargarDatos();
            CargarHistorico();
        }

        private void btnFIEBR101_Click(object sender, EventArgs e)
        {
            TI_EBR101 VentanaTIEBR101 = new TI_EBR101("Caudal"); // 
            VentanaTIEBR101.ShowDialog(); // muestro el objeto
            CargarDatos();
            CargarHistorico();
        }

        private void btnPIEBR101_Click(object sender, EventArgs e)
        {
            TI_EBR101 VentanaTIEBR101 = new TI_EBR101("Presion"); // TI_EBR101 nombre de ventana - Creo el objeto ventana
            VentanaTIEBR101.ShowDialog(); // muestro el objeto
            CargarDatos();
            CargarHistorico();
        }

        private void btnKICKEBR101_Click(object sender, EventArgs e)
        {
            TI_EBR101 VentanaTIEBR101 = new TI_EBR101("HoraFunc"); // 
            VentanaTIEBR101.ShowDialog(); // muestro el objeto
            CargarDatos();
            CargarHistorico();
        }

        private void Page_Unloaded_1(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Stop();
        }

        private void btnHistoricos_click(object sender, RoutedEventArgs e)
        {
            //abrir ventana de historicos
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            CargarDatos(true);// Acá generar alarma
            CargarHistorico();
        }

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
                float HoraFunc_Actual = float.Parse(tabla.Rows[0]["HoraFunc_Actual"].ToString());

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
                if (HoraFunc_Actual < HoraFunc_Min || HoraFunc_Actual > HoraFunc_Max)
                {
                    if (HoraFunc_Actual < HoraFunc_Min)
                    {
                        insertAlarma.Add("HoraFunc_Lo", HoraFunc_Actual);
                    }
                    else if (HoraFunc_Actual > HoraFunc_Max)
                    {
                        insertAlarma.Add("HoraFunc_Hi", HoraFunc_Actual);
                    }
                    lbl_kick_ebr101.Background = Brushes.Red;
                }

                if (insertarAlarma && insertAlarma.Count > 0) //si insertar alarma es true y ademas de que haya por lo menos una alarma
                {
                    InsertarAlarma(insertAlarma);
                }
            }
            catch (Exception ex)
            {
                lbl_conectado.Visibility = Visibility.Hidden;
                lbl_desconectado.Visibility = Visibility.Visible;
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conexion.Close();
            }
        }

        private void CargarHistorico()
        {
            SqlConnection conexion = new SqlConnection(conectionString);
            try
            {
                conexion.Open();
                string consulta = "sp_ObtenerHistorico";
                SqlCommand comando = new SqlCommand(consulta, conexion);
                SqlDataAdapter da = new SqlDataAdapter(comando);
                DataTable tabla = new DataTable();
                da.Fill(tabla);
                dataGridHistorico.ItemsSource = tabla.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conexion.Close();
            }
        }

        private void InsertarAlarma(Dictionary<string, float> alarmas)
        {
            using (SqlConnection conexion = new SqlConnection(conectionString))
            {
                try
                {
                    conexion.Open();
                    foreach (var alarma in alarmas)
                    {
                        using (SqlCommand comando = new SqlCommand("sp_InsertarAlarma", conexion))
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.Parameters.AddWithValue("@Alarma", alarma.Key);
                            comando.Parameters.AddWithValue("@Valor", alarma.Value);
                            comando.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al insertar alarma: " + ex.Message);
                }
                finally
                {
                    conexion.Close();
                }
            }
        }
    }
}

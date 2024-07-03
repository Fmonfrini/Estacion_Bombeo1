using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace WPF_EstacionBombeo.Ventanas
{
	/// <summary>
	/// Lógica de interacción para TI_EBR101.xaml
	/// </summary>
	public partial class TI_EBR101 : Window
    {
        private string _tipeconfiguration;
		private string tipeConfigMin;
		private string tipeConfigMax;
		string conectionString = "server=localhost ; database = Estacion_Bombeo ; integrated security = true";

		public TI_EBR101(string tipeconfiguration)
        {
            InitializeComponent();
            _tipeconfiguration = tipeconfiguration;
			tipeConfigMin = _tipeconfiguration + "_min";
			tipeConfigMax = _tipeconfiguration + "_max";
		}

        private void Window_Loaded(object sender, RoutedEventArgs e) //cuando la ventana se cargue vamos a hacer esto
        {
            switch (_tipeconfiguration)
            
            {
                
                case "TempBomba": // Si la ventana abierta es TempBomba entonces:
                    lbl_descripcion.Content = "Temperatura de bomba"; //Cargo descripción

                    lbl_tag.Content = "TI-EBR101"; // cargo el TAG
                    
                    /// Abro la conexión y Cargo los datos
                    
                    SqlConnection conexion = new SqlConnection(conectionString); //para que sepamos a que base de datos nos vamos a conectar
                    try
                    {
                        conexion.Open();
                        string consulta = "sp_ValorCfg_TempBomb";
                        SqlCommand comando = new SqlCommand(consulta, conexion);//consulta lo de arriba utilizando conexion
                        SqlDataAdapter da = new SqlDataAdapter(comando); // intermediario, se encarga de traer la info
                        //Especifico que el comando es un Procedimiento Almacenado
                        comando.CommandType = CommandType.StoredProcedure;
                        SqlDataReader registros = comando.ExecuteReader();
                        while (registros.Read())
                            {
                            txt_cfgmax.Text = (registros["TempBomba_Max"].ToString());
                            txt_cfgmin.Text = (registros["TempBomba_Min"].ToString());
                            txt_cfgmax_val.Text = (registros["TempBomba_Max"].ToString());
                            txt_cfgmin_val.Text = (registros["TempBomba_Min"].ToString());
                        }
                        registros.Close();

                        
                    }
                    catch (Exception ex) { }
                    finally
                    {
                        conexion.Close();
                    }

                    break;
                
                case "TempAmb":
                    lbl_descripcion.Content = "Temperatura ambiente";

                    lbl_tag.Content = "TI-EBR102";

                    /// Abro la conexión y Cargo los datos
                    SqlConnection conexion1 = new SqlConnection(conectionString); //para que sepamos a que base de datos nos vamos a conectar
                    try
                    {
                        conexion1.Open();
                        string consulta = "sp_ValorCfg_TempAmb";
                        SqlCommand comando = new SqlCommand(consulta, conexion1);//consulta lo de arriba utilizando conexion
                        SqlDataAdapter da = new SqlDataAdapter(comando); // intermediario, se encarga de traer la info
                        //Especifico que el comando es un Procedimiento Almacenado
                        comando.CommandType = CommandType.StoredProcedure;
                        SqlDataReader registros = comando.ExecuteReader();
                        while (registros.Read())
                        {
                            txt_cfgmax.Text = (registros["TempAmb_Max"].ToString());
                            txt_cfgmin.Text = (registros["TempAmb_Min"].ToString());
                            txt_cfgmax_val.Text = (registros["TempAmb_Max"].ToString());
                            txt_cfgmin_val.Text = (registros["TempAmb_Min"].ToString());
                        }
                        registros.Close();


                    }
                    catch (Exception ex) { }
                    finally
                    {
                        conexion1.Close();
                    }
                    break;


                case "Caudal":
                    lbl_descripcion.Content = "Caudal de bomba";
                    lbl_tag.Content = "FI-EBR101";
                    /// Abro la conexión y Cargo los datos
                    SqlConnection conexion3 = new SqlConnection(conectionString); //para que sepamos a que base de datos nos vamos a conectar
                    try
                    {
                        conexion3.Open();
                        string consulta = "sp_ValorCfg_Caudal";
                        SqlCommand comando = new SqlCommand(consulta, conexion3);//consulta lo de arriba utilizando conexion
                        SqlDataAdapter da = new SqlDataAdapter(comando); // intermediario, se encarga de traer la info
                        //Especifico que el comando es un Procedimiento Almacenado
                        comando.CommandType = CommandType.StoredProcedure;
                        SqlDataReader registros = comando.ExecuteReader();
                        while (registros.Read())
                        {
                            txt_cfgmax.Text = (registros["Caudal_Max"].ToString());
                            txt_cfgmin.Text = (registros["Caudal_Min"].ToString());
                            txt_cfgmax_val.Text = (registros["Caudal_Max"].ToString());
                            txt_cfgmin_val.Text = (registros["Caudal_Min"].ToString());
                        }
                        registros.Close();


                    }
                    catch (Exception ex) { }
                    finally
                    {
                        conexion3.Close();
                    }
                    break;



                case "Presion":
                    lbl_descripcion.Content = "Presion de conducto";
                    lbl_tag.Content = "PI-EBR101";
                    /// Abro la conexión y Cargo los datos
                    SqlConnection conexion4 = new SqlConnection(conectionString); //para que sepamos a que base de datos nos vamos a conectar
                    try
                    {
                        conexion4.Open();
                        string consulta = "sp_ValorCfg_Presion";
                        SqlCommand comando = new SqlCommand(consulta, conexion4);//consulta lo de arriba utilizando conexion
                        SqlDataAdapter da = new SqlDataAdapter(comando); // intermediario, se encarga de traer la info
                        //Especifico que el comando es un Procedimiento Almacenado
                        comando.CommandType = CommandType.StoredProcedure;
                        SqlDataReader registros = comando.ExecuteReader();
                        while (registros.Read())
                        {
                            txt_cfgmax.Text = (registros["Presion_Max"].ToString());
                            txt_cfgmin.Text = (registros["Presion_Min"].ToString());
                            txt_cfgmax_val.Text = (registros["Presion_Max"].ToString());
                            txt_cfgmin_val.Text = (registros["Presion_Min"].ToString());
                        }
                        registros.Close();


                    }
                    catch (Exception ex) { }
                    finally
                    {
                        conexion4.Close();
                    }
                    break;


                case "HoraFunc":
                    lbl_descripcion.Content = "Horas de funcionamiento";
                    lbl_tag.Content = "KICK-EBR101";
                    /// Abro la conexión y Cargo los datos
                    SqlConnection conexion5 = new SqlConnection(conectionString); //para que sepamos a que base de datos nos vamos a conectar
                    try
                    {
                        conexion5.Open();
                        string consulta = "sp_ValorCfg_HoraFunc";
                        SqlCommand comando = new SqlCommand(consulta, conexion5);//consulta lo de arriba utilizando conexion
                        SqlDataAdapter da = new SqlDataAdapter(comando); // intermediario, se encarga de traer la info
                        //Especifico que el comando es un Procedimiento Almacenado
                        comando.CommandType = CommandType.StoredProcedure;
                        SqlDataReader registros = comando.ExecuteReader();
                        while (registros.Read())
                        {
                            txt_cfgmax.Text = (registros["HoraFunc_Max"].ToString());
                            txt_cfgmin.Text = (registros["HoraFunc_Min"].ToString());
                            txt_cfgmax_val.Text = (registros["HoraFunc_Max"].ToString());
                            txt_cfgmin_val.Text = (registros["HoraFunc_Min"].ToString());
                        }
                        registros.Close();


                    }
                    catch (Exception ex) { }
                    finally
                    {
                        conexion5.Close();
                    }

                    break;

            }

           
           



        }

		private void btnActualizar_Click(object sender, RoutedEventArgs e)
		{
            bool minValValid = float.TryParse(this.txt_cfgmin.Text, out float minVal);
			bool maxValValid = float.TryParse(this.txt_cfgmax.Text, out float maxVal);

            if (minValValid && maxValValid)
            {
				SqlConnection conexion = new SqlConnection(conectionString); //para que sepamos a que base de datos nos vamos a conectar
				try
				{
					conexion.Open();
					string consulta = @"UPDATE TablaConfiguraciones SET " + tipeConfigMin + " = " + minVal + ", " + tipeConfigMax + " = " + maxVal;
					SqlCommand comando = new SqlCommand(consulta, conexion);//consulta lo de arriba utilizando conexion
					comando.ExecuteNonQuery();
                    txt_cfgmax_val.Text = maxVal.ToString();
                    txt_cfgmin_val.Text = minVal.ToString();
                    txt_cfgmax.Text = null;
                    txt_cfgmin.Text = null;
                    MessageBox.Show("Valores actualizados correctamente", "Confirmación");
				}
				catch (Exception ex) { }
				finally
				{
					conexion.Close();
				}
			}
            else
            {
                MessageBox.Show("Los valores de mínimo y máximo deben ser numéricos válidos.", "Confirmación");
            }

		}
	}
}

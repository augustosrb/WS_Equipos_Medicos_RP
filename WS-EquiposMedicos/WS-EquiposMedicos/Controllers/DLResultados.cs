using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace WS_EquiposMedicos.Controllers
{
    public class DLResultados : DBOBase
    {
        String strCon = ConfigurationManager.AppSettings["WS-Conexion"].ToString();

        SqlCommand cmd = new SqlCommand();

        public List<ELResultado> DL_ConsultarAmbiente()
        {

            List<ELResultado> objResultado = new List<ELResultado>();

            try
            {
                using (SqlConnection conn = new SqlConnection(strCon))
                {
                    conn.Open();

                    cmd.Connection = conn;
                    cmd.CommandText = "GENERAL.USP_Resultados";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Clear();

                    Random rnd = new Random();
                    int nAleatorio = rnd.Next(1, 5);

                    cmd.Parameters.AddWithValue("@nAleatorio", nAleatorio == 0 ? 0 : nAleatorio);

                    SqlDataReader drSQL = fLeer(cmd);

                    if (drSQL.HasRows)
                    {
                        objResultado = (List<ELResultado>)ConvertirDataReaderALista<ELResultado>(drSQL);
                    }
                }
            }
            catch (Exception e)
            {
                //ELog.Save(this, e);
            }
            finally
            {
                if (cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                }
            }

            return objResultado;
        }
    }
}
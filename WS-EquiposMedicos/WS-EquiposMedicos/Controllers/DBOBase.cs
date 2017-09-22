using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Reflection;
using System.Collections;

namespace WS_EquiposMedicos.Controllers
{
    public class DBOBase
    {
        #region Atributos
        protected String[] arrayLibraries;
        protected DataTable dataTable;
        #endregion

        #region Metodos
        protected void pAddParameter(SqlCommand command, String parameterName, Object value, SqlDbType dbTipo)
        {
            SqlParameter parameter = command.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = value;
            parameter.SqlDbType = dbTipo;
            parameter.Direction = ParameterDirection.Input;
            command.Parameters.Add(parameter);
        }

        public SqlConnection NewConnection(String strCan)
        {
            try
            {
                SqlConnection cn = new SqlConnection(strCan);

                return cn;

            }
            catch (Exception ex)
            {
                throw new Exception("Método Nueva Conexión: " + ex.Message, ex);
            }
            finally { }
        }

        protected String fEjecutar(SqlCommand cmd)
        {
            String Resultado = "";
            try
            {
                cmd.Connection.Open();
                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    Resultado = "OK";
                }
                else
                {
                    Resultado = "Fail";
                }
            }
            catch (Exception ex)
            {
                if (cmd.Transaction != null)
                {
                    cmd.Transaction.Rollback();
                }
                Resultado = ex.Message;
            }
            finally
            {
                if (cmd.Connection.State != ConnectionState.Closed)
                {
                    cmd.Connection.Close();
                }
            }
            return Resultado;
        }

        protected SqlDataReader fLeer(SqlCommand cmd)
        {
            try
            {
                if (cmd.Connection.State == ConnectionState.Closed)
                {
                    cmd.Connection.Open();
                }


                SqlDataReader reader = cmd.ExecuteReader();
                return reader;
            }
            catch (Exception ex)
            {
                if (cmd.Transaction != null)
                {
                    cmd.Transaction.Rollback();
                }
                if (cmd.Connection.State != ConnectionState.Closed)
                {
                    cmd.Connection.Close();
                }
                throw new Exception("Método Leer: " + ex.Message, ex);
            }
        }

        protected DataTable fLeerDT(SqlCommand cmd)
        {
            try
            {
                DataTable dt = new DataTable();
                DbDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                if (cmd.Transaction != null)
                {
                    cmd.Transaction.Rollback();
                }
                throw new Exception("Método Seleccionar: " + ex.Message, ex);
            }
            finally
            {
                if (cmd.Connection.State != ConnectionState.Closed)
                {
                    cmd.Connection.Close();
                }
            }
        }

        protected DataSet fLeerDS(SqlCommand cmd)
        {
            try
            {
                cmd.Connection.Open();
                DataSet dt = new DataSet();
                DbDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                if (cmd.Transaction != null)
                {
                    cmd.Transaction.Rollback();
                }
                throw new Exception("Método Seleccionar: " + ex.Message, ex);
            }
            finally
            {
                if (cmd.Connection.State != ConnectionState.Closed)
                {
                    cmd.Connection.Close();
                }
            }
        }

        protected Object fObtenerValor(IDbCommand cmd)
        {
            try
            {
                cmd.Connection.Open();
                return cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                if (cmd.Transaction != null)
                {
                    cmd.Transaction.Rollback();
                }
                throw new Exception("Método Obtener Valor: " + ex.Message, ex);
            }
            finally
            {
                if (cmd.Connection.State != ConnectionState.Closed)
                {
                    cmd.Connection.Close();
                }
            }
        }

        public static Object ConvertirDataReaderALista<T>(SqlDataReader myDataReader)
        {
            List<T> entidades = new List<T>();
            PropertyInfo[] propiedades = typeof(T).GetProperties();
            try
            {
                ArrayList columnasConsultadas = new ArrayList();
                for (int i = 0; i < myDataReader.FieldCount; i++) { columnasConsultadas.Add(myDataReader.GetName(i)); }
                while (myDataReader.Read())
                {
                    T entidad = Activator.CreateInstance<T>();
                    foreach (PropertyInfo propiedad in propiedades)
                    {
                        Attribute item = Attribute.GetCustomAttribute(propiedad, typeof(ELColumn));
                        if (item is ELColumn)
                        {
                            try
                            {
                                ELColumn column = (ELColumn)item;
                                if (columnasConsultadas.IndexOf(column.Name) > -1)
                                {
                                    propiedad.SetValue(entidad, myDataReader.GetValue(myDataReader.GetOrdinal(column.Name)), null);
                                }
                            }
                            catch (Exception ex) { /*throw new Exception(ex.Message);*/ }
                        }
                    }
                    entidades.Add(entidad);
                }
            }
            finally
            {
                myDataReader.Close();
            }
            return entidades;
        }

        #endregion
    }
}
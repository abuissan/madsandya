using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Reflection;

namespace notaModel.Model
{
    public class nota
    {
        //CONSTANT for class
        private const string _TABLE_ = "tblNota";
        private const string _KEY_ = "idNota";
        private const char _SPACE_ = (char)32;

        //Private variables
        private SqlConnection local;
        private string query  = @"";
        private string fields = @"()";
        private string quote  = @"''";
        //private string actions = @"VALUES (";
        
        //Class Properties
        public string C_idNota { set; get;}
        public string C_idBarang { set; get; }
        public int? C_idStatus { set; get; }
        public int? C_idCustomer { set; get; }
        public DateTime? C_tglMasuk { set; get; }
        public DateTime? C_tglDiambil { set; get; }
        private Boolean hasError { set; get; }



        public nota()
        {
            try
            {
                string ConnString = @"Data Source=DESKTOP-E5VJ4CP;Initial Catalog=bengkel;Integrated Security=True";
                local = new SqlConnection(ConnString);
                local.Open();
            }
            catch(Exception ex)
            {
                hasError = true;
            }

        }

        //method GetRow(string id) untuk mengambil Row dari tabel
        //public List<nota> GetRow(string idNota = null, string idBarang = null, string idStatus = null, string idCustomer = null)
        public List<nota> GetRow(nota notaku)
        {
            var listnota = new List<nota>();

            if (!hasError)
            {
                try
                {
                    string where = "";
                    query = @"SELECT * FROM" + _SPACE_ + _TABLE_ + _SPACE_;
                    /*if(id != null) 
                    {
                    }
                    ? "WHERE" + _SPACE_ + _KEY_ + "='" + id + "';":";";*/
                    PropertyInfo[] props = notaku.GetType().GetProperties();
                    foreach (PropertyInfo pi in props)
                    {
                        if (pi.GetValue(notaku) != null)
                        {
                            if (where.Length==0)
                            {
                                query+=where += "WHERE"+_SPACE_+pi.Name.Substring(2, pi.Name.Length - 2)+_SPACE_+"LIKE"+_SPACE_+"'%"+pi.GetValue(notaku)+"%'"+_SPACE_;
                            }
                            else
                            {
                                query+= "OR" + _SPACE_+pi.Name.Substring(2, pi.Name.Length - 2) + _SPACE_ + "LIKE" + _SPACE_ + "'%" + pi.GetValue(notaku) + "%'" + _SPACE_;
                            }
                        }
                            
                    }

                    SqlCommand cmd = new SqlCommand(query+";", local);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var notas = new nota();
                        PropertyInfo[] properties = notas.GetType().GetProperties();
                        foreach (PropertyInfo pi in properties)
                        {
                            if (pi.Name.Substring(0, 2) == "C_")
                                {
                                    if (pi.PropertyType == typeof(String))
                                    {
                                        pi.SetValue(notas, reader[pi.Name.Substring(2, pi.Name.Length - 2)].ToString());
                                    }
                                    else if (pi.PropertyType == typeof(DateTime))
                                    {
                                        pi.SetValue(notas, reader[pi.Name.Substring(2, pi.Name.Length - 2)]);
                                 }
                                    else
                                    {
                                        if (reader[pi.Name.Substring(2, pi.Name.Length - 2)].ToString().Length > 0)
                                        {
                                            pi.SetValue(notas, reader[pi.Name.Substring(2, pi.Name.Length - 2)]);
                                        }
                                        else
                                        {
                                            pi.SetValue(notas, null);
                                        }
                                    }
                                }                                
                        }
                        listnota.Add(notas);
                    }

                }
                catch (Exception ex)
                {
                    hasError = true;
                }
                finally
                {
                    local.Close();
                }
            }
            return listnota;

        } //method GetRow


        public Boolean InsertRow(nota notaku)
        {
            if (!hasError)
            {
                try
                {
                    query  = @"INSERT INTO"+_SPACE_+_TABLE_+_SPACE_;

                    string value = "";
                    string field = "";
                    
                    PropertyInfo[] properties = notaku.GetType().GetProperties();

                    foreach (PropertyInfo pi in properties)
                    {
                        string val = "";
                        string fld = "";

                        fld = pi.Name.Substring(2, pi.Name.Length - 2);
                        field += (field.Length == 0) ? fld : "," + fld;

                        if (pi.PropertyType == typeof(String) || pi.PropertyType == typeof(DateTime))
                        {
                            val = quote.Insert(1, pi.GetValue(notaku).ToString());
                        }
                        else
                        {
                            if (pi.GetValue(notaku) == null) { val = "null"; } else { val = quote.Insert(1, pi.GetValue(notaku).ToString()); }
                        }
                        value += (value.Length == 0) ? val : "," + val;
                    }

                    query += fields.Insert(1, field) + _SPACE_ + "VALUES" + _SPACE_ + fields.Insert(1, value)+";";
                    SqlCommand cmd = new SqlCommand(query, local);
                    int a = cmd.ExecuteNonQuery();
                    if (a < 1) hasError = true;
                   
                }
                catch (Exception ex)
                {
                    hasError = true;
                }
                finally
                {
                    local.Close();
                }
            }

            return !hasError;
        }

        public Boolean UpdateRow(nota notaku)
        {
            if (!hasError)
            {
                try
                {
                    query = @"UPDATE" + _SPACE_ + _TABLE_ + _SPACE_;

                    string set = "SET"+_SPACE_;
                    string where = "WHERE" + _SPACE_ + _KEY_ + "='";
                    string field = "";

                    PropertyInfo[] properties = notaku.GetType().GetProperties();

                    foreach (PropertyInfo pi in properties)
                    {
                        if (pi.GetValue(notaku) != null)
                        {
                            string val = "";
                            string fld = "";
                            fld = pi.Name.Substring(2, pi.Name.Length - 2);
                            if (fld == _KEY_) where += pi.GetValue(notaku).ToString() + "'";

                            if (pi.PropertyType == typeof(String) || pi.PropertyType == typeof(DateTime))
                            {
                                val = quote.Insert(1, pi.GetValue(notaku).ToString());
                            }
                            else
                            {
                                val = (pi.GetValue(notaku) == null) ? null : quote.Insert(1, pi.GetValue(notaku).ToString());
                            }
                            if (fld != _KEY_)field += (field.Length == 0) ? fld + "=" + val : "," + fld + "=" + val;

                        }
                        

                    }

                    query += set + field + _SPACE_ + where+ ";";
                    SqlCommand cmd = new SqlCommand(query, local);
                    int a = cmd.ExecuteNonQuery();
                    if (a < 1) hasError = true;

                }
                catch (Exception ex)
                {
                    hasError = true;
                }
                finally
                {
                    local.Close();
                }
            }

            return !hasError;
        }

        public Boolean DeleteRow(string id)
        {
            if (!hasError)
            {
                try
                {
                    query = @"DELETE FROM" + _SPACE_ + _TABLE_ + _SPACE_;

                    string where = "WHERE" + _SPACE_ + _KEY_ + "='" + id + "'";

                    query += where + ";";
                    SqlCommand cmd = new SqlCommand(query, local);
                    int a = cmd.ExecuteNonQuery();
                    if (a < 1) hasError = true;

                }
                catch (Exception ex)
                {
                    hasError = true;
                }
                finally
                {
                    local.Close();
                }
            }

            return !hasError;
        }				

    } //class
         
} //namespace
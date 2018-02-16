using SkywalkerAutoWash.DAO;
using SkywalkerAutoWash.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace SkywalkerAutoWash.BLL
{
    public class Servicos : Conexao
    {
        public DataTable GetVeiculo()
        {
            try
            {
                OpenCon();
                string str = string.Format(@"SELECT VeiculoId,Marca FROM dbo.Veiculos");
                using (cmd = new SqlCommand(str, con))
                {
                    using (Adp = new SqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            Adp.Fill(dt);
                            return dt;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {

                throw new Exception(ex.Message);
            }
            finally
            {
                CloseCon();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetTipoLavagem()
        {
            try
            {
                OpenCon();
                string str = string.Format(@"SELECT TipoId,TipoLavagem FROM dbo.TipoLavagem");
                using (cmd = new SqlCommand(str, con))
                {
                    using (Adp = new SqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            Adp.Fill(dt);
                            return dt;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {

                throw new Exception(ex.Message);
            }
            finally
            {
                CloseCon();
            }
        }

        public Usuarios GetPontos(int pontos)
        {
            try
            {
                OpenCon();
                string str = string.Format(@"SELECT Pontos FROM TipoLavagem WHERE TipoId =@id");
                using (cmd = new SqlCommand(str, con))
                {
                    cmd.Parameters.AddWithValue("@id", pontos);
                    using (Dr = cmd.ExecuteReader())
                    {
                        Usuarios mod = null;

                        while (Dr.Read())
                        {
                            mod = new Usuarios();
                            mod.MeusPontos = Convert.ToInt32(Dr["Pontos"]);
                        }

                        return mod;
                    }
                }
            }
            catch (SqlException ex)
            {

                throw new Exception(ex.Message);
            }
            finally
            {
                CloseCon();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void RegistrarServicos(Usuarios obj)
        {
            try
            {
                OpenCon();
                string str = string.Format(@"INSERT INTO dbo.LavagemVeiculo VALUES(@data,@hora,@cpf,@veiculo,@placa,@cor,@tipo,@pontos,NULL,@tipoPagamento)");
                using (cmd = new SqlCommand(str, con))
                {
                    string data = DateTime.Now.ToString("yyyy/MM/dd");
                    string hora = DateTime.Now.ToString("HH:mm:ss");
                    cmd.Parameters.AddWithValue("@data", data);
                    cmd.Parameters.AddWithValue("@hora", hora);
                    cmd.Parameters.AddWithValue("@cpf", obj.Cpf);
                    cmd.Parameters.AddWithValue("@veiculo", obj.Veiculo);
                    cmd.Parameters.AddWithValue("@placa", obj.Placa);
                    cmd.Parameters.AddWithValue("@cor", obj.Cor);
                    cmd.Parameters.AddWithValue("@tipo", obj.Tipo);
                    cmd.Parameters.AddWithValue("@pontos", obj.MeusPontos);
                    cmd.Parameters.AddWithValue("@tipoPagamento", obj.TipoPagamento);
                    cmd.ExecuteNonQuery();
                }


            }
            catch (SqlException ex)
            {

                throw new Exception(ex.Message);
            }
            finally
            {
                CloseCon();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        public Usuarios CheckCPF(string cpf)
        {
            try
            {
                OpenCon();
                string str = string.Format(@"SELECT CPF FROM dbo.Usuarios WHERE CPF = @cpf");
                using (cmd = new SqlCommand(str, con))
                {
                    cmd.Parameters.AddWithValue("@cpf", cpf);
                    using (Dr = cmd.ExecuteReader())
                    {
                        Usuarios mod = null;
                        while (Dr.Read())
                        {
                            mod = new Usuarios();
                            mod.Cpf = Dr["CPF"].ToString();
                        }
                        return mod;
                    }
                }
            }
            catch (SqlException ex)
            {

                throw new Exception(ex.Message);
            }
            finally
            {
                CloseCon();
            }
        }
        /// <summary>
        /// METODO PARA VERIFICAR OS PONTOS
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        public DataTable VerificarPontos(string cpf)
        {
            try
            {
                OpenCon();
                string str = string.Format(@"SELECT A.LavagemId,A.Data,D.NomeCompleto,D.Usuario,B.Marca,B.Modelo,B.Fabricante,C.TipoLavagem,A.Pontos FROM dbo.LavagemVeiculo A
                                             INNER JOIN dbo.Veiculos B ON A.Veiculo = B.VeiculoId
                                             INNER JOIN dbo.TipoLavagem C ON A.TipoLavagem = C.TipoId
                                             INNER JOIN dbo.Usuarios D ON A.CPF = D.CPF
                                             WHERE A.CPF=@cpf");
                using (cmd = new SqlCommand(str, con))
                {
                    cmd.Parameters.AddWithValue("@cpf", cpf);
                    using (Adp = new SqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            Adp.Fill(dt);
                            return dt;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {

                throw new Exception(ex.Message);
            }
            finally
            {
                CloseCon();
            }
        }

        public Usuarios SomaPontos(string cpf)
        {
            try
            {
                OpenCon();
                //string str = string.Format(@"SELECT ISNULL(SUM(C.Pontos),0)As Pontos FROM dbo.Usuarios A
                //                             INNER JOIN dbo.LavagemVeiculo B ON A.CPF = B.CPF
                //                             INNER JOIN dbo.TipoLavagem C ON B.LavagemId = C.TipoId 
                //                             INNER JOIN dbo.Veiculos D ON D.VeiculoId = B.Veiculo
                //                             WHERE A.CPF=@cpf");

                string str = string.Format(@"SELECT ISNULL(SUM(A.Pontos),0) As Pontos FROM dbo.LavagemVeiculo A
                                             INNER JOIN dbo.Veiculos B ON A.Veiculo = B.VeiculoId
                                             INNER JOIN dbo.TipoLavagem C ON A.TipoLavagem = C.TipoId
                                             INNER JOIN dbo.Usuarios D ON A.CPF = D.CPF WHERE A.CPF=@cpf");
                using (cmd = new SqlCommand(str, con))
                {
                    cmd.Parameters.AddWithValue("@cpf", cpf);
                    using (Dr = cmd.ExecuteReader())
                    {

                        Usuarios mod = null;
                        while (Dr.Read())
                        {
                            mod = new Usuarios();
                            mod.MeusPontos = Convert.ToInt32((Dr["Pontos"]).ToString());
                        }
                        return mod;
                    }
                }
            }
            catch (SqlException ex)
            {

                throw new Exception(ex.Message);
            }
            finally
            {
                CloseCon();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable Pesquisa()
        {
            try
            {
                OpenCon();
                string str = string.Format(@"SELECT A.Data,D.NomeCompleto,D.Usuario,A.CPF,B.Marca,B.Modelo,B.Fabricante,A.Placa,A.Cor,C.TipoLavagem,A.Pontos,C.Valores FROM dbo.LavagemVeiculo A
                                             INNER JOIN dbo.Veiculos B ON A.Veiculo = B.VeiculoId
                                             INNER JOIN dbo.TipoLavagem C ON A.TipoLavagem = C.TipoId
                                             INNER JOIN dbo.Usuarios D ON A.CPF = D.CPF");
                using (cmd = new SqlCommand(str, con))
                {
                    using (Adp = new SqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            Adp.Fill(dt);
                            return dt;

                        }
                    }
                }
            }
            catch (SqlException ex)
            {

                throw new Exception(ex.Message);
            }
            finally
            {
                CloseCon();
            }
        }

        public DataTable Pesquisa(string data1, string data2)
        {
            try
            {
                OpenCon();
                string str = string.Format(@"SELECT A.Data,D.NomeCompleto,D.Usuario,A.CPF,B.Marca,B.Modelo,B.Fabricante,A.Placa,A.Cor,C.TipoLavagem,C.Valores,A.Pontos FROM dbo.LavagemVeiculo A
                                             INNER JOIN dbo.Veiculos B ON A.Veiculo = B.VeiculoId
                                             INNER JOIN dbo.TipoLavagem C ON A.TipoLavagem = C.TipoId
                                             INNER JOIN dbo.Usuarios D ON A.CPF = D.CPF
                                             WHERE A.Data BETWEEN @data1 AND @data2");
                using (cmd = new SqlCommand(str, con))
                {
                    cmd.Parameters.AddWithValue("@data1", data1);
                    cmd.Parameters.AddWithValue("@data2", data2);
                    using (Adp = new SqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            Adp.Fill(dt);
                            return dt;

                        }
                    }
                }
            }
            catch (SqlException ex)
            {

                throw new Exception(ex.Message);
            }
            finally
            {
                CloseCon();
            }
        }
        public DataTable Pesquisa(string cpf)
        {
            try
            {
                OpenCon();
                string str = string.Format(@"SELECT A.Data,D.NomeCompleto,D.Usuario,A.CPF,B.Marca,B.Modelo,B.Fabricante,A.Placa,A.Cor,C.TipoLavagem,C.Valores,A.Pontos FROM dbo.LavagemVeiculo A
                                             INNER JOIN dbo.Veiculos B ON A.Veiculo = B.VeiculoId
                                             INNER JOIN dbo.TipoLavagem C ON A.TipoLavagem = C.TipoId
                                             INNER JOIN dbo.Usuarios D ON A.CPF = D.CPF
                                             WHERE A.CPF = @cpf");
                using (cmd = new SqlCommand(str, con))
                {
                    cmd.Parameters.AddWithValue("@cpf", cpf);
                    using (Adp = new SqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            Adp.Fill(dt);
                            return dt;

                        }
                    }
                }
            }
            catch (SqlException ex)
            {

                throw new Exception(ex.Message);
            }
            finally
            {
                CloseCon();
            }
        }

        public DataTable Pesquisa(string data1, string data2, string cpf)
        {
            try
            {
                OpenCon();
                string str = string.Format(@"SELECT A.Data,D.NomeCompleto,D.Usuario,A.CPF,B.Marca,B.Modelo,B.Fabricante,A.Placa,A.Cor,C.TipoLavagem,C.Valores,A.Pontos FROM dbo.LavagemVeiculo A
                                             INNER JOIN dbo.Veiculos B ON A.Veiculo = B.VeiculoId
                                             INNER JOIN dbo.TipoLavagem C ON A.TipoLavagem = C.TipoId
                                             INNER JOIN dbo.Usuarios D ON A.CPF = D.CPF
                                             WHERE A.Data BETWEEN @data1 AND @data2 AND A.CPF = @cpf");
                using (cmd = new SqlCommand(str, con))
                {
                    cmd.Parameters.AddWithValue("@data1", data1);
                    cmd.Parameters.AddWithValue("@data2", data2);
                    cmd.Parameters.AddWithValue("@cpf", cpf);
                    using (Adp = new SqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            Adp.Fill(dt);
                            return dt;

                        }
                    }
                }
            }
            catch (SqlException ex)
            {

                throw new Exception(ex.Message);
            }
            finally
            {
                CloseCon();
            }
        }

        #region Atualizar Pontos
        public void AtulizarPontos(Usuarios obj)
        {
            try
            {
                OpenCon();
                string str = string.Format(@"UPDATE LavagemVeiculo SET Pontos=@pontos,Descricao =@desc WHERE LavagemId =@id");
                using (cmd = new SqlCommand(str, con))
                {
                    cmd.Parameters.AddWithValue("@id", obj.TipoId);
                    cmd.Parameters.AddWithValue("@desc", obj.Descricao);
                    cmd.Parameters.AddWithValue("@pontos", obj.MeusPontos);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {

                throw new Exception(ex.Message);
            }
            finally
            {
                CloseCon();
            }
        }
        #endregion

        #region Finaceira

        public DataTable GetFinanceira(string data1, string data2)
        {
            try
            {
                OpenCon();
                string str = string.Format(@"SELECT CONVERT(varchar(3),(DATENAME(MONTH,A.Data)))As Mes,A.TipoPagamento,SUM(B.Valores)As Total FROM dbo.LavagemVeiculo A
                                             INNER JOIN TipoLavagem B ON A.TipoLavagem = B.TipoId WHERE Data BETWEEN @data1 AND @data2 GROUP BY A.TipoPagamento,A.Data");
                using (cmd = new SqlCommand(str, con))
                {
                    cmd.Parameters.AddWithValue("@data1", data1);
                    cmd.Parameters.AddWithValue("@data2", data2);
                    using (Adp = new SqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            Adp.Fill(dt);
                            return dt;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {

                throw new Exception(ex.Message);
            }
            finally
            {
                CloseCon();
            }
        }

        public DataTable GetFinanceira(string tipo)
        {
            try
            {
                OpenCon();
                string str = string.Format(@"SELECT CONVERT(varchar(3),(DATENAME(MONTH,A.Data)))As Mes,A.TipoPagamento,SUM(B.Valores)As Total FROM dbo.LavagemVeiculo A
                                             INNER JOIN TipoLavagem B ON A.TipoLavagem = B.TipoId WHERE A.TipoPagamento = @tipo GROUP BY A.TipoPagamento,A.Data");
                using (cmd = new SqlCommand(str, con))
                {
                    cmd.Parameters.AddWithValue("@tipo", tipo);

                    using (Adp = new SqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            Adp.Fill(dt);
                            return dt;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {

                throw new Exception(ex.Message);
            }
            finally
            {
                CloseCon();
            }
        }

        public DataTable GetFinanceira(string data1, string data2, string tipo)
        {
            try
            {
                OpenCon();
                string str = string.Format(@"SELECT CONVERT(varchar(3),(DATENAME(MONTH,A.Data)))As Mes,A.TipoPagamento,SUM(B.Valores)As Total FROM dbo.LavagemVeiculo A
                                             INNER JOIN TipoLavagem B ON A.TipoLavagem = B.TipoId WHERE Data BETWEEN @data1 AND @data2 AND A.TipoPagamento = @tipo GROUP BY A.TipoPagamento,A.Data");
                using (cmd = new SqlCommand(str, con))
                {
                    cmd.Parameters.AddWithValue("@data1", data1);
                    cmd.Parameters.AddWithValue("@data2", data2);
                    cmd.Parameters.AddWithValue("@tipo", tipo);

                    using (Adp = new SqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            Adp.Fill(dt);
                            return dt;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {

                throw new Exception(ex.Message);
            }
            finally
            {
                CloseCon();
            }
        }
        #endregion

        #region Meus Pontos
        public Usuarios MeusPontos(string cpf)
        {
            try
            {
                OpenCon();
                string str = string.Format(@"SELECT  ISNULL(SUM(A.Pontos),0)As Total FROM dbo.LavagemVeiculo A
                                             INNER JOIN Usuarios B ON A.CPF = B.CPF WHERE B.CPF = @cpf");
                using (cmd = new SqlCommand(str, con))
                {
                    cmd.Parameters.AddWithValue("@cpf", cpf);
                    using (Dr = cmd.ExecuteReader())
                    {
                        Usuarios mod = null;
                        while (Dr.Read())
                        {
                            mod = new Usuarios();
                            mod.MeusPontos = Convert.ToInt32(Dr["Total"]);
                        }
                        return mod;
                    }
                }
            }
            catch (SqlException ex)
            {

                throw new Exception(ex.Message);
            }
            finally
            {
                CloseCon();
            }
        }
        #endregion

        #region Tipo de Serviços
        public DataTable GetAllServices()
        {
            try
            {
                OpenCon();
                string str = string.Format(@"SELECT * FROM dbo.TipoLavagem ORDER BY TipoId DESC");
                using (cmd = new SqlCommand(str, con))
                {
                    using (Adp = new SqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            Adp.Fill(dt);
                            return dt;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {

                throw new Exception(ex.Message);
            }
            finally
            {
                CloseCon();
            }
        }

        public void NewService(Usuarios obj)
        {
            try
            {
                OpenCon();
                string str = string.Format(@"INSERT INTO dbo.TipoLavagem VALUES(@tipo,@desc,@valor,@pontos)");
                using (cmd = new SqlCommand(str, con))
                {
                    cmd.Parameters.AddWithValue("@tipo", obj.Tipo);
                    cmd.Parameters.AddWithValue("@desc", obj.Descricao);
                    cmd.Parameters.AddWithValue("@valor", obj.Valores);
                    cmd.Parameters.AddWithValue("@pontos", obj.MeusPontos);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {

                throw new Exception(ex.Message);
            }
            finally
            {
                CloseCon();
            }
        }

        public Usuarios ListaTipoServicos(int cod)
        {
            try
            {
                OpenCon();
                string str = string.Format(@"SELECT * FROM dbo.TipoLavagem WHERE TipoId = @cod");
                using (cmd = new SqlCommand(str, con))
                {
                    cmd.Parameters.AddWithValue("cod", cod);
                    using (Dr = cmd.ExecuteReader())
                    {
                        Usuarios mod = null;
                        while (Dr.Read())
                        {
                            mod = new Usuarios();
                            mod.Tipo = Convert.ToString(Dr["TipoLavagem"]);
                            mod.Descricao = Convert.ToString(Dr["Descricao"]);
                            mod.Valores = Convert.ToDouble(Dr["Valores"]);
                            mod.MeusPontos = Convert.ToInt32(Dr["Pontos"]);
                        }
                        return mod;
                    }
                }
            }
            catch (SqlException ex)
            {

                throw new Exception(ex.Message);
            }
            finally
            {
                CloseCon();
            }
        }

        public void UpdateServicos(Usuarios obj)
        {
            try
            {
                OpenCon();
                string str = string.Format(@"UPDATE dbo.TipoLavagem SET TipoLavagem =@tipo,Descricao =@desc,Valores=@valor,Pontos =@pontos WHERE TipoId = @cod");
                using (cmd = new SqlCommand(str, con))
                {
                    cmd.Parameters.AddWithValue("@cod", obj.TipoId);
                    cmd.Parameters.AddWithValue("@tipo", obj.Tipo);
                    cmd.Parameters.AddWithValue("@desc", obj.Descricao);
                    cmd.Parameters.AddWithValue("@valor", obj.Valores);
                    cmd.Parameters.AddWithValue("@pontos", obj.MeusPontos);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            finally
            {
                CloseCon();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public void ExluirServico(Usuarios obj)
        {
            try
            {
                OpenCon();
                string str = string.Format(@"DELETE FROM dbo.TipoLavagem WHERE TipoId =@id");
                using (cmd = new SqlCommand(str, con))
                {
                    cmd.Parameters.AddWithValue("@id", obj.TipoId);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {

                throw new Exception(ex.Message);
            }
            finally
            {
                CloseCon();
            }
        }

        #endregion

        #region Veiculos
        public List<Usuarios> GetAllVeiculo()
        {
            try
            {
                OpenCon();
                string str = string.Format(@"SELECT * FROM dbo.Veiculos ORDER BY VeiculoId DESC");
                using (cmd = new SqlCommand(str, con))
                {

                    using (Dr = cmd.ExecuteReader())
                    {
                        List<Usuarios> lista = new List<Usuarios>();
                        Usuarios mod = null;
                        while (Dr.Read())
                        {
                            mod = new Usuarios();
                            mod.VeiculoId = Convert.ToInt32(Dr["VeiculoId"]);
                            mod.Marca = Convert.ToString(Dr["Marca"]);
                            mod.Modelo = Convert.ToString(Dr["Modelo"]);
                            mod.Fabricante = Convert.ToString(Dr["Fabricante"]);
                            lista.Add(mod);
                        }
                        return lista;
                    }
                }
            }
            catch (SqlException ex)
            {

                throw new Exception(ex.Message);
            }
            finally
            {
                CloseCon();
            }
        }

        public List<Usuarios> GetAllVeiculo(int cod)
        {
            try
            {
                OpenCon();
                string str = string.Format(@"SELECT * FROM dbo.Veiculos WHERE VeiculoId =@id");
                using (cmd = new SqlCommand(str, con))
                {
                    cmd.Parameters.AddWithValue("@id", cod);
                    using (Dr = cmd.ExecuteReader())
                    {
                        List<Usuarios> lista = new List<Usuarios>();
                        Usuarios mod = null;
                        while (Dr.Read())
                        {
                            mod = new Usuarios();
                            mod.VeiculoId = Convert.ToInt32(Dr["VeiculoId"]);
                            mod.Marca = Convert.ToString(Dr["Marca"]);
                            mod.Modelo = Convert.ToString(Dr["Modelo"]);
                            mod.Fabricante = Convert.ToString(Dr["Fabricante"]);
                            lista.Add(mod);
                        }
                        return lista;
                    }
                }
            }
            catch (SqlException ex)
            {

                throw new Exception(ex.Message);
            }
            finally
            {
                CloseCon();
            }
        }

        public void UpdateVeiculo(Usuarios obj)
        {
            try
            {
                OpenCon();
                string str = string.Format(@"UPDATE dbo.Veiculos SET Marca=@marca,Modelo=@modelo,Fabricante=@fabricante WHERE VeiculoId=@id");
                using (cmd = new SqlCommand(str, con))
                {
                    cmd.Parameters.AddWithValue("@id", obj.VeiculoId);
                    cmd.Parameters.AddWithValue("@marca", obj.Marca);
                    cmd.Parameters.AddWithValue("@modelo", obj.Modelo);
                    cmd.Parameters.AddWithValue("@fabricante", obj.Fabricante);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {

                throw new Exception(ex.Message);
            }
            finally
            {
                CloseCon();
            }
        }
        public void CadastrarVeiculo(Usuarios obj)
        {
            try
            {
                OpenCon();
                string str = string.Format(@"INSERT INTO dbo.Veiculos VALUES(@marca,@modelo,@fabricante)");
                using (cmd = new SqlCommand(str, con))
                {
                    cmd.Parameters.AddWithValue("@marca", obj.Marca);
                    cmd.Parameters.AddWithValue("@modelo", obj.Modelo);
                    cmd.Parameters.AddWithValue("@fabricante", obj.Fabricante);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {

                throw new Exception(ex.Message);
            }
            finally
            {
                CloseCon();
            }
        }

        public void ExcluirVeiculo(Usuarios obj)
        {
            try
            {
                OpenCon();
                string str = string.Format(@"DELETE FROM dbo.Veiculos WHERE VeiculoId=@id");
                using (cmd = new SqlCommand(str, con))
                {
                    cmd.Parameters.AddWithValue("@id", obj.VeiculoId);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {

                throw new Exception(ex.Message);
            }
            finally
            {
                CloseCon();
            }
        }
        #endregion
    }
}

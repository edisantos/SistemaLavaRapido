using SkywalkerAutoWash.DAO;
using SkywalkerAutoWash.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace SkywalkerAutoWash.BLL
{
    public class UsuariosReg : Conexao
    {
        public void CadastrarUsuario(Usuarios objUser)
        {
            try
            {
                OpenCon();
                string strInsert = string.Format(@"INSERT INTO dbo.Usuarios VALUES(@cpf,@data,@nome,@email,@endereco,@numero,@bairro,@cidade,@uf,@usuario,@senha,@confirmaSenha)");
                using (cmd = new SqlCommand(strInsert, con))
                {
                    string data = DateTime.Now.ToString("yyyy/MM/dd");
                    string hora = DateTime.Now.ToString("HH:mm:ss");
                    cmd.Parameters.AddWithValue("@cpf", objUser.Cpf);
                    cmd.Parameters.AddWithValue("@data", data);
                    cmd.Parameters.AddWithValue("@nome", objUser.NomeCompleto);
                    cmd.Parameters.AddWithValue("@email", objUser.Email);
                    cmd.Parameters.AddWithValue("@endereco", objUser.Endereco);
                    cmd.Parameters.AddWithValue("@numero", objUser.Numero);
                    cmd.Parameters.AddWithValue("@bairro", objUser.Bairro);
                    cmd.Parameters.AddWithValue("@cidade", objUser.Cidade);
                    cmd.Parameters.AddWithValue("@uf", objUser.Uf);
                    cmd.Parameters.AddWithValue("@usuario", objUser.Usuario);
                    cmd.Parameters.AddWithValue("@senha", objUser.Senha);
                    cmd.Parameters.AddWithValue("@confirmaSenha", objUser.ConfirmaSenha);

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
        /// Aqui verifico se o usuário já existe cadastrado
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Usuarios VerificarUsuariosExistentes(string nome, string email, string cpf, string user)
        {
            try
            {
                OpenCon();
                string strSelect = string.Format(@"SELECT NomeCompleto,Email,CPF,Usuario FROM dbo.Usuarios WHERE (NomeCompleto = @nome) OR (Email = @email) OR (CPF =@cpf) OR (Usuario =@user)");
                using (cmd = new SqlCommand(strSelect, con))
                {
                    cmd.Parameters.AddWithValue("@nome", nome);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@cpf", cpf);
                    cmd.Parameters.AddWithValue("@user", user);
                    using (Dr = cmd.ExecuteReader())
                    {
                        Usuarios model = null;
                        while (Dr.Read())
                        {
                            model = new Usuarios();
                            model.NomeCompleto = Convert.ToString(Dr["NomeCompleto"]);
                        }
                        return model;
                    }

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
        /// Verifica o login
        /// </summary>
        /// <param name="user"></param>
        /// <param name="senha"></param>
        /// <returns></returns>
        public Usuarios Login(string user, string senha)
        {
            try
            {
                OpenCon();
                string strSelect = string.Format(@"SELECT Usuario,Senha FROM dbo.Usuarios WHERE Usuario =@user AND Senha =@senha");
                using (cmd = new SqlCommand(strSelect, con))
                {
                    cmd.Parameters.AddWithValue("@user", user);
                    cmd.Parameters.AddWithValue("@senha", senha);

                    using (Dr = cmd.ExecuteReader())
                    {
                        Usuarios mod = null;
                        while (Dr.Read())
                        {
                            mod = new Usuarios();
                            mod.Usuario = Dr["Usuario"].ToString();
                            mod.Senha = Dr["Senha"].ToString();
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
    }
}

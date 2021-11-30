using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;
using OutBackX.Config;
using OutBackX.Model;

namespace OutBackX.Repository
{
    public class FuncionarioRepository : IDisposable
    {

        private SQLite.SQLiteConnection connection;

        public FuncionarioRepository()
        {
            connection = SqlConnection.GetConnection();
            connection.CreateTable<FuncionarioModel>();
        }

        public FuncionarioModel GetValidationLogin( string email, string senha)
        {
            return connection.Table<Model.FuncionarioModel>().Where( x => x.EmailFuncionario.ToUpper().Equals(email.ToUpper() )  && x.SenhaFuncionario == senha).FirstOrDefault();
        }

        public void Insert(Model.FuncionarioModel _funcionarioModel)
        {
            connection.Insert(_funcionarioModel);
        }
        public void DropTable()
        {
            connection.DropTable<Model.FuncionarioModel>();
        }
        public void Dispose()
        {
            if (connection != null)
            {
                connection.Dispose(); 
            }
        }
    }
}

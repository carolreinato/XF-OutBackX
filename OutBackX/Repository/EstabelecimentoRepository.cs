using OutBackX.Config;
using OutBackX.Model;
using System;
using System.Collections.Generic;

namespace OutBackX.Repository
{
    public class EstabelecimentoRepository : IDisposable
    {

        private SQLite.SQLiteConnection connection;

        public EstabelecimentoRepository()
        {
            connection = SqlConnection.GetConnection();

            connection.CreateTable<EstabelecimentoModel>();
        }
        public List<EstabelecimentoModel> GetList()
        {
            return connection.Table<EstabelecimentoModel>().ToList();
        }
        public List<EstabelecimentoModel> GetList(int idFuncionario)
        {
            return connection.Table<EstabelecimentoModel>().Where(i => i.IdFuncionario == idFuncionario).ToList();
        }
        public EstabelecimentoModel Get(int id)
        {
            return connection.Table<EstabelecimentoModel>().Where(i => i.IdEstabelecimento == id).FirstOrDefault();
        }
        public void Insert(EstabelecimentoModel _estabelecimentoModel)
        {
            connection.Insert(_estabelecimentoModel);
        }
        public void Update(EstabelecimentoModel _estabelecimentoModel)
        {
            connection.Update(_estabelecimentoModel);
        }
        public void Delete(EstabelecimentoModel _estabelecimentoModel)
        {
            connection.Delete(_estabelecimentoModel);
        }
        public void DropTable()
        {
            connection.DropTable<EstabelecimentoModel>();
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

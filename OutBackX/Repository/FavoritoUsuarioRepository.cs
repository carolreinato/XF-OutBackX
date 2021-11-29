using OutBackX.Config;
using System;
using System.Collections.Generic;
using System.Text;
using OutBackX.Model;
using System.Linq;
using Xamarin.Forms.Internals;

namespace OutBackX.Repository
{
    public class FavoritoUsuarioRepository : IDisposable
    {
        private SQLite.SQLiteConnection connection;
        private static string sql = @"select F.IdFavoritoUsuario, F.DataCadastro, 
                E.IdEstabelecimento, E.NomeEstabelecimento, E.IdFuncionario, E.EnderecoEstabelecimento, E.BairroEstabelecimento, 
                E.CEPEstabelecimento, E.CidadeEstabelecimento, E.EstadoEstabelecimento, E.CoordenadaX, E.CoordenadaY, E.NivelLotacao 
                from FavoritoUsuarioModel F 
                inner join EstabelecimentoModel E on F.IdEstabelecimento = E.IdEstabelecimento ";

        public FavoritoUsuarioRepository()
        {
            connection = SqlConnection.GetConnection();
            connection.CreateTable<FavoritoUsuarioModel>();
        }

        public IList<FavoritoUsuarioModel> GetList()
        {
            var e = connection.Query<EstabelecimentoModel>(sql)
                .ToList();

            List<EstabelecimentoModel> listaEstab = e.ConvertAll(x => new EstabelecimentoModel
            {
                IdEstabelecimento = x.IdEstabelecimento,
                NomeEstabelecimento = x.NomeEstabelecimento,
                EnderecoEstabelecimento = x.EnderecoEstabelecimento,
                BairroEstabelecimento = x.BairroEstabelecimento,
                CEPEstabelecimento = x.CEPEstabelecimento,
                CidadeEstabelecimento = x.CidadeEstabelecimento,
                EstadoEstabelecimento = x.EstadoEstabelecimento,
                CoordenadaX = x.CoordenadaX,
                CoordenadaY = x.CoordenadaY,
                IdFuncionario = x.IdFuncionario,
                NivelLotacao = x.NivelLotacao
            });

            IList<FavoritoUsuarioModel> listaFav = connection.Table<FavoritoUsuarioModel>().ToList();
            listaFav.ForEach(x =>
                x.EstabelecimentoRef = listaEstab.Find(i => i.IdEstabelecimento == x.IdEstabelecimento)
            ); 
            return connection.Table<FavoritoUsuarioModel>().ToList();
        }

        public FavoritoUsuarioModel Get(int id)
        {
            return connection.Table<FavoritoUsuarioModel>().Where(i => i.IdFavoritoUsuario == id).FirstOrDefault();
        }
        public FavoritoUsuarioModel Get(EstabelecimentoModel e)
        {
            return connection.Query<FavoritoUsuarioModel>(
                sql + "where F.IdEstabelecimento = ? ", e.IdEstabelecimento)
                .ToList().FirstOrDefault();
        }
        public FavoritoUsuarioModel Get(string nome)
        {
            return connection.Query<FavoritoUsuarioModel>(
                sql + "where E.NomeEstabelecimento = ? ", nome)
                .ToList().FirstOrDefault();
        }
        public void Insert(FavoritoUsuarioModel _FavoritoUsuarioModel)
        {
            _FavoritoUsuarioModel.DataCadastro = DateTime.Now.ToString();
            connection.Insert(_FavoritoUsuarioModel);
        }

        public void Update(FavoritoUsuarioModel _FavoritoUsuarioModel)
        {
            connection.Update(_FavoritoUsuarioModel);
        }

        public void Delete(FavoritoUsuarioModel _FavoritoUsuarioModel)
        {
            connection.Delete(_FavoritoUsuarioModel);
        }

        public void DropTable()
        {
            connection.DropTable<FavoritoUsuarioModel>();
        }

        public void CreateTable()
        {
            connection.CreateTable<FavoritoUsuarioModel>();
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

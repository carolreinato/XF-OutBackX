using OutBackX.Model;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace OutBackX.Repository
{
    public class FavoritoUsuarioModel
    {

        private int idFavoritoUsuario;

        [PrimaryKey, AutoIncrement]
        public int IdFavoritoUsuario
        {
            get { return idFavoritoUsuario; }
            set
            {
                if (idFavoritoUsuario != value)
                {
                    idFavoritoUsuario = value;
                }
            }
        }

        private int idEstabelecimento;

        [ForeignKey(typeof(EstabelecimentoModel))]
        public int IdEstabelecimento
        {
            get { return idEstabelecimento; }
            set
            {
                if (idEstabelecimento != value)
                {
                    idEstabelecimento = value;
                }
            }
        }

        private EstabelecimentoModel estabelecimentoRef;

        [ManyToOne]
        public EstabelecimentoModel EstabelecimentoRef
        {
            get { return estabelecimentoRef; }
            set
            {
                if (estabelecimentoRef == null)
                {
                    estabelecimentoRef = value;
                }
                else if (value == null)
                {
                    estabelecimentoRef = value;
                }
                else if (estabelecimentoRef.IdEstabelecimento != value.IdEstabelecimento)
                {
                    estabelecimentoRef = value;
                    idEstabelecimento = value.IdEstabelecimento;
                }
            }
        }

        private string dataCadastro;
        public string DataCadastro
        {
            get { return dataCadastro; }
            set
            {
                if (dataCadastro != value)
                {
                    dataCadastro = value;
                }
            }
        }

    }
}

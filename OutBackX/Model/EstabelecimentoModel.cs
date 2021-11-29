using System;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace OutBackX.Model
{
    [Serializable()]
    public class EstabelecimentoModel
    {
        private int idEstabelecimento;

        [PrimaryKey, AutoIncrement]
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

        private int idFuncionario;      
        public int IdFuncionario
        {
            get { return idFuncionario; }
            set
            {
                if (idFuncionario != value)
                {
                    idFuncionario = value;
                }
            }
        }

        private string nomeEstabelecimento;
        public string NomeEstabelecimento
        {
            get { return nomeEstabelecimento; }
            set
            {
                if (nomeEstabelecimento != value)
                {
                    nomeEstabelecimento = value;
                }
            }
        }

        private string enderecoEstabelecimento;
        public string EnderecoEstabelecimento
        {
            get { return enderecoEstabelecimento; }
            set
            {
                if (enderecoEstabelecimento != value)
                {
                    enderecoEstabelecimento = value;
                }
            }
        }

        private string bairroEstabelecimento;
        public string BairroEstabelecimento
        {
            get { return bairroEstabelecimento; }
            set
            {
                if (bairroEstabelecimento != value)
                {
                    bairroEstabelecimento = value;
                }
            }
        }

        private string cidadeEstabelecimento;
        public string CidadeEstabelecimento
        {
            get { return cidadeEstabelecimento; }
            set
            {
                if (cidadeEstabelecimento != value)
                {
                    cidadeEstabelecimento = value;
                }
            }
        }

        private string estadoEstabelecimento;
        public string EstadoEstabelecimento
        {
            get { return estadoEstabelecimento; }
            set
            {
                if (estadoEstabelecimento != value)
                {
                    estadoEstabelecimento = value;
                }
            }
        }

        private string cepEstabelecimento;
        public string CEPEstabelecimento
        {
            get { return cepEstabelecimento; }
            set
            {
                if (cepEstabelecimento != value)
                {
                    cepEstabelecimento = value;
                }
            }
        }

        private double coordenadaX;
        public double CoordenadaX
        {
            get { return coordenadaX; }
            set
            {
                if (coordenadaX != value)
                {
                    coordenadaX = value;
                }
            }
        }

        private double coordenadaY;
        public double CoordenadaY
        {
            get { return coordenadaY; }
            set
            {
                if (coordenadaY != value)
                {
                    coordenadaY = value;
                }
            }
        }

        private string nivelLotacao;
        public string NivelLotacao
        {
            get { return nivelLotacao; }
            set
            {
                if (nivelLotacao != value)
                {
                    nivelLotacao = value;
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            return obj is EstabelecimentoModel model &&
                   IdEstabelecimento == model.IdEstabelecimento;
        }
    }
}

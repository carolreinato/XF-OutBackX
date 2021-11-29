using System;
using SQLite;

namespace OutBackX.Model
{
    public class FuncionarioModel
    {
        private int idFuncionario;

        [PrimaryKey, AutoIncrement]
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

        private string nomeFuncionario;
        public string NomeFuncionario
        {
            get { return nomeFuncionario; }
            set
            {
                if (nomeFuncionario != value)
                {
                    nomeFuncionario = value;
                }
            }
        }

        private string emailFuncionario;
        public string EmailFuncionario
        {
            get { return emailFuncionario; }
            set
            {
                if (emailFuncionario != value)
                {
                    emailFuncionario = value;
                }
            }
        }

        private string senhaFuncionario;
        public string SenhaFuncionario
        {
            get { return senhaFuncionario; }
            set
            {
                if (senhaFuncionario != value)
                {
                    senhaFuncionario = value;
                }
            }
        }

        private string cpfFuncionario;
        public string CpfFuncionario
        {
            get { return cpfFuncionario; }
            set
            {
                if (cpfFuncionario != value)
                {
                    cpfFuncionario = value;
                }
            }
        }
    }
}

using OutBackX.Model;
using OutBackX.Repository;
using OutBackX.View;
using System.Windows.Input;
using Xamarin.Forms;

namespace OutBackX.ViewModel
{
    public class FuncionarioViewModel
    {
        private readonly FuncionarioRepository _repository;

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
        public ICommand EntrarClickedCommand { get; private set; }
        public ICommand SalvarClickedCommand { get; private set; }
  
        public FuncionarioViewModel()
        {
            _repository = new FuncionarioRepository();
            var mensagem = string.Empty;

            EntrarClickedCommand = new Command(async () =>
            {
                FuncionarioModel model = new FuncionarioModel()
                {
                    EmailFuncionario = this.EmailFuncionario,
                    SenhaFuncionario = this.SenhaFuncionario
                };

                var resultModel = _repository.GetValidationLogin(model.EmailFuncionario, model.SenhaFuncionario);

                if (resultModel != null)
                {
                    await Application.Current.MainPage.Navigation.PushAsync(new EstabelecimentoPage());
                }
                else
                {
                    mensagem = "Verifique Seu Usuario e Senha!";
                    await Application.Current.MainPage.DisplayAlert("Atenção", mensagem, "OK");
                }
            });

            SalvarClickedCommand = new Command(() =>
            {
                if (this.EmailFuncionario != null &&
                    this.SenhaFuncionario != null &&
                    this.CpfFuncionario != null &&
                    this.NomeFuncionario != null)
                {
                    FuncionarioModel model = new FuncionarioModel()
                    {
                        EmailFuncionario = this.EmailFuncionario,
                        SenhaFuncionario = this.SenhaFuncionario,
                        CpfFuncionario = this.CpfFuncionario,
                        NomeFuncionario = this.NomeFuncionario
                    };

                    _repository.Insert(model);

                    mensagem = "Cadastrado com Sucesso!";
                }
                else
                {
                    mensagem = "Todos os campos são obrigatórios!";
                }
                Application.Current.MainPage.DisplayAlert("Atenção", mensagem, "OK");
                Application.Current.MainPage.Navigation.PushAsync(new FuncionarioInitialPage());
            });
        }
    }
}

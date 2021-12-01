using OutBackX.Model;
using OutBackX.Repository;
using OutBackX.View;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace OutBackX.ViewModel
{
    public class EstabelecimentoCrudViewModel : BaseViewModel
    {
        public FuncionarioModel funcionarioModel = new FuncionarioModel();

        private readonly FavoritoUsuarioRepository _favoritoRepository = new FavoritoUsuarioRepository();

        private readonly EstabelecimentoRepository _estabelecimentoRepository = new EstabelecimentoRepository();

        private EstabelecimentoModel estabelecimento = new EstabelecimentoModel();
        public EstabelecimentoModel Estabelecimento
        {
            get { return estabelecimento; }
            set
            {
                if (estabelecimento != value)
                {
                    estabelecimento = value;
                }
            }
        }

        private ObservableCollection<EstabelecimentoModel> estabelecimentoList = new ObservableCollection<EstabelecimentoModel>();
        public ObservableCollection<EstabelecimentoModel> EstabelecimentoList
        {
            get { return estabelecimentoList; }
            set
            {
                if (estabelecimentoList != value)
                {
                    estabelecimentoList = value;
                    OnPropertyChanged("EstabelecimentoList");
                }
            }
        }

        private EstabelecimentoModel _selectedEstabelecimento;
        public EstabelecimentoModel EstabelecimentoSelected
        {
            get => _selectedEstabelecimento;
            set
            {
                if (value != null)
                {
                    _selectedEstabelecimento = value;

                    NotifyPropertyChanged("EstabelecimentoSelected");

                    ExibirDetalhesExcluir(value);
                }
            }
        }

        public EstabelecimentoCrudViewModel()
        {

            SalvarClickedCommand = new Command(async () => await Salvar(estabelecimento));

            AtualizarClickedCommand = new Command(async () => await Atualizar(estabelecimento));

            ExcluirClickedCommand = new Command(async () => await Excluir(estabelecimento));
        }

        public EstabelecimentoCrudViewModel(EstabelecimentoModel estabelecimentoCurrent)
        {
            estabelecimento = estabelecimentoCurrent;

            SalvarClickedCommand = new Command(async () => await Salvar(estabelecimento));

            AtualizarClickedCommand = new Command(async () => await Atualizar(estabelecimento));

            ExcluirClickedCommand = new Command(async () => await Excluir(estabelecimento));
        }

        public ICommand ExcluirClickedCommand { get; private set; }
        public ICommand SalvarClickedCommand { get; private set; }
        public ICommand AtualizarClickedCommand { get; private set; }

        private ICommand _searchCommand;
        public ICommand SearchCommand
        {
            get
            {
                return _searchCommand ?? (_searchCommand = new Command<string>((text) =>
                {
                    funcionarioModel.IdFuncionario = estabelecimento.IdFuncionario;

                    var lista = _estabelecimentoRepository.GetList(estabelecimento.IdFuncionario).ToList<EstabelecimentoModel>();

                    if (text.Length >= 1)
                    {
                        EstabelecimentoList = new ObservableCollection<EstabelecimentoModel>(lista.Where(i => i.NomeEstabelecimento.ToUpper().Contains(text.ToUpper())).ToList());
                    }
                    else
                    {
                        EstabelecimentoList = new ObservableCollection<EstabelecimentoModel>(lista);
                    }
                }));
            }
        }
     
        public async Task Salvar(EstabelecimentoModel estabelecimento)
        {
            string message = estabelecimento.IdEstabelecimento > 0 ? "Deseja Salvar?" : "Incluir estabelecimento?";

            bool res = await _messageService.ShowAsyncBool("Salvar", message, "Sim", "Não");

            if (res)
            {
                var currentLocation = await Geolocation.GetLocationAsync();
                estabelecimento.CoordenadaX = currentLocation.Latitude;
                estabelecimento.CoordenadaY = currentLocation.Longitude;
                _estabelecimentoRepository.Insert(estabelecimento);

                await Application.Current.MainPage.Navigation.PushAsync(new EstabelecimentoPage());
            }
        }
        public async Task Atualizar(EstabelecimentoModel estabelecimento)
        {
            string message = "Deseja Salvar As Alterações?";

            bool res = await _messageService.ShowAsyncBool("Salvar", message, "Sim", "Não");

            if (res)
            {
                _estabelecimentoRepository.Update(estabelecimento);

                if (estabelecimento.IdFuncionario != 0)
                {
                    await Application.Current.MainPage.Navigation.PushAsync(new EstabelecimentoPage());
                }
                else
                {
                    await Application.Current.MainPage.Navigation.PushAsync(new UsuarioPage(estabelecimento.IdFuncionario));
                }
            }
        }
        public async Task Excluir(EstabelecimentoModel estabelecimento)
        {
            bool resposta = await _messageService.ShowAsyncBool("Excluir", "Confirma a exlusão?", "Sim", "Não");
            if (resposta)
            {
                var favorito = _favoritoRepository.Get(estabelecimento);
                if (favorito != null)
                {
                    _favoritoRepository.Delete(favorito);
                }
                _estabelecimentoRepository.Delete(estabelecimento);


                await Application.Current.MainPage.Navigation.PushAsync(new EstabelecimentoPage());
            }
        }
        private void ExibirDetalhesExcluir(EstabelecimentoModel estabelecimento)
        {
            Application.Current.MainPage.Navigation.PushAsync(new EstabelecimentoExcluirPage(estabelecimento));
        } 
    }
}

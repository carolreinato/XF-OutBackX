using OutBackX.Model;
using OutBackX.Repository;
using OutBackX.View;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace OutBackX.ViewModel
{
    public class EstabelecimentoListViewModel : BaseViewModel
    {
      
        public int IdFuncionarioModel { get; set; }
        public string PaginaOrigem { get; set; }

        private EstabelecimentoRepository _estabelecimentoRepository;

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

                    ExibirDetalhes(value);
                }
            }
        }
    
        public EstabelecimentoListViewModel(string paginaOrigemRecebida)
        {
            PaginaOrigem = paginaOrigemRecebida;
            ListarDados();
        }

        public EstabelecimentoListViewModel()
        {
            _estabelecimentoRepository = new EstabelecimentoRepository();
        }
        
        private ICommand _listarClickedCommand;
        public ICommand ListarClickedCommand
        {
            get
            {
                return _listarClickedCommand ?? (_listarClickedCommand = new Command(() => ListarEstabelecimentos()));
            }
        }

        private ICommand _searchCommand;
        public ICommand SearchCommand
        {
            get
            {
                return _searchCommand ?? (_searchCommand = new Command<string>((text) =>
                {
                    _estabelecimentoRepository = new EstabelecimentoRepository();

                    var lista = _estabelecimentoRepository.GetList(IdFuncionarioModel).ToList<EstabelecimentoModel>();

                    if (text.Length >= 1)
                    {
                        EstabelecimentoList = new ObservableCollection<EstabelecimentoModel>(lista
                            .Where(i => string.Concat(
                                i.NomeEstabelecimento,
                                i.EnderecoEstabelecimento,
                                i.BairroEstabelecimento,
                                i.EstadoEstabelecimento,
                                i.CidadeEstabelecimento
                                ).ToUpper().Contains(text.ToUpper())
                             ).ToList());
                    }
                    else
                    {
                        EstabelecimentoList = new ObservableCollection<EstabelecimentoModel>(lista);
                    }
                }));
            }
        }
        public ICommand _showMapCommand;
        public ICommand ShowMapCommand
        {
            get
            {
                return _showMapCommand ?? (_showMapCommand = new Command<EstabelecimentoModel>(async (estabelecimento) =>
                {
                    await Application.Current.MainPage.Navigation.PushAsync(new EstabelecimentoMapPage(estabelecimento));
                }));
            }
        }
               
        private void ListarEstabelecimentos()
        {
            ListarDados();

            Application.Current.MainPage.Navigation.PushAsync(new EstabelecimentoListPage("paginaPrincipal"));
        }
        private void ListarDados()
        {
            _estabelecimentoRepository = new EstabelecimentoRepository();

            var lista = _estabelecimentoRepository.GetList();

            estabelecimentoList.Clear();

            lista.OrderBy(f => f.NomeEstabelecimento).ForEach(x => estabelecimentoList.Add(x));

            NotifyPropertyChanged("EstabelecimentoList");
        }
        private async void ExibirDetalhes(EstabelecimentoModel estabelecimento)
        {
            switch (PaginaOrigem)
            {
                case "paginaAtualizar" :
                    await Application.Current.MainPage.Navigation.PushAsync(new EstabelecimentoUpdatePage(estabelecimento));
                    break;

                case "paginaExcluir":
                    await Application.Current.MainPage.Navigation.PushAsync(new EstabelecimentoDeletePage(estabelecimento));
                    break;

                case "UsuarioPage":
                    FavoritoUsuarioCrudViewModel favoritoUsuarioCrudViewModel = new FavoritoUsuarioCrudViewModel(estabelecimento);
                    await favoritoUsuarioCrudViewModel.Salvar(estabelecimento);
                    break;
                case "UsuarioEstabelecimentoFavoritoListarPage":
                    await Application.Current.MainPage.Navigation.PushAsync(new UsuarioFavoritListPage(estabelecimento));
                    break;
            }
        }
    }

    public class TextChangedBehavior : Behavior<SearchBar>
    {
        protected override void OnAttachedTo(SearchBar bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.TextChanged += Bindable_TextChanged;
        }

        protected override void OnDetachingFrom(SearchBar bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.TextChanged -= Bindable_TextChanged;
        }

        private void Bindable_TextChanged(object sender, TextChangedEventArgs e)
        {
            ((SearchBar)sender).SearchCommand?.Execute(e.NewTextValue);
        }
    }
}

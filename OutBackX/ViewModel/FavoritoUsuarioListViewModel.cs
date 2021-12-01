using OutBackX.Model;
using OutBackX.Repository;
using OutBackX.View;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace OutBackX.ViewModel
{
    public class FavoritoUsuarioListViewModel : BaseViewModel
    {
        #region Construtor
        public FavoritoUsuarioListViewModel()
        {
            _repository = new FavoritoUsuarioRepository();
            _repositoryEstabelecimento = new EstabelecimentoRepository();
            ListarDados();
        }

        #endregion

        #region Properties

        private FavoritoUsuarioModel _selectedFavorito;
        public FavoritoUsuarioModel SelectedFavorito
        {
            get => _selectedFavorito;
            set
            {
                if (value != null)
                {
                    _selectedFavorito = value;
                    NotifyPropertyChanged("SelectedFavorito");
                    ExibirDetalhes(value);
                }
            }
        }

        private ObservableCollection<FavoritoUsuarioModel> favoritoList = new ObservableCollection<FavoritoUsuarioModel>();
        public ObservableCollection<FavoritoUsuarioModel> FavoritoList
        {
            get { return favoritoList; }
            set
            {
                if (favoritoList != value)
                {
                    favoritoList = value;
                    OnPropertyChanged("FavoritoList");
                }
            }
        }

        protected FavoritoUsuarioRepository _repository;
        protected EstabelecimentoRepository _repositoryEstabelecimento;
        #endregion

        #region ICommand

        private ICommand _searchCommand;
        public ICommand SearchCommand
        {
            get
            {
                return _searchCommand ?? (_searchCommand = new Command<string>((text) =>
                {
                    var lista = _repository.GetList().ToList<FavoritoUsuarioModel>();
                    if (text.Length >= 1)
                    {
                        FavoritoList = new ObservableCollection<FavoritoUsuarioModel>(lista.ToList());
                    }
                    else
                    {
                        FavoritoList = new ObservableCollection<FavoritoUsuarioModel>(lista);
                    }
                }));
            }
        }
        public ICommand MeusFavoritosClickedCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await Application.Current.MainPage.Navigation.PushAsync(new UsuarioFavoritListPage());
                });

            }
        }
        private ICommand _compartilharFavoritoCommand;
        public ICommand CompartilharFavoritoCommand
        {
            get
            {
                return _compartilharFavoritoCommand ?? (_compartilharFavoritoCommand = new Command<FavoritoUsuarioModel>(async (favorito) =>
                {
                    EstabelecimentoModel e = favorito.EstabelecimentoRef ?? (_repositoryEstabelecimento.Get(favorito.IdEstabelecimento));
                    if (e != null)
                    {
                        string endereco = $"{e.NomeEstabelecimento}, {e.EnderecoEstabelecimento}, {e.BairroEstabelecimento}, " +
                            $"CEP {e.CEPEstabelecimento}, {e.CidadeEstabelecimento}, {e.EstadoEstabelecimento}";
                        await Share.RequestAsync(new ShareTextRequest
                        {
                            Subject = e.NomeEstabelecimento,
                            Title = e.NomeEstabelecimento,
                            Text = endereco,
                            Uri = "http://maps.google.com/maps?saddr=+" + endereco.Replace(" ", "+"),
                        });

                    }
                }));
            }
        }
        public ICommand _showMapCommand;
        public ICommand ShowMapCommand
        {
            get
            {
                return _showMapCommand ?? (_showMapCommand = new Command<FavoritoUsuarioModel>(async (favorito) =>
                {
                    EstabelecimentoModel e = favorito.EstabelecimentoRef ?? (_repositoryEstabelecimento.Get(favorito.IdEstabelecimento));
                    if (e != null)
                    {
                        await Application.Current.MainPage.Navigation.PushAsync(new EstabelecimentoMapPage(e));
                    }
                }));
            }
        }

        #endregion

        #region Metodos        

        private async void ExibirDetalhes(FavoritoUsuarioModel favoritoUsuarioModel)
        {
            var estabelecimento = _repositoryEstabelecimento.Get(favoritoUsuarioModel.IdEstabelecimento);
            await Application.Current.MainPage.Navigation.PushAsync(new EstabelecimentoUpdatePage(estabelecimento));
        }

        public void ListarDados()
        {
            var lista = _repository.GetList();

            favoritoList.Clear();

            lista.ForEach(x => favoritoList.Add(x));

            NotifyPropertyChanged("FavoritoList");
        }

        #endregion
    }
}
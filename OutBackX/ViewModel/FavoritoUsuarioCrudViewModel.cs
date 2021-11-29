using OutBackX.Model;
using OutBackX.Repository;
using OutBackX.Util;
using OutBackX.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace OutBackX.ViewModel
{
    public class FavoritoUsuarioCrudViewModel : EstabelecimentoBaseViewModel
    {
        private EstabelecimentoModel estabelecimento = new EstabelecimentoModel();
        private FavoritoUsuarioRepository _repository;
        private EstabelecimentoRepository _repositoryEstabelecimento;

        private ObservableCollection<EstabelecimentoModel> estabelecimentoList = new ObservableCollection<EstabelecimentoModel>();
        public ObservableCollection<EstabelecimentoModel> EstabelecimentoList
        {
            get { return estabelecimentoList; }
            set
            {
                if (estabelecimentoList != value)
                {
                    estabelecimentoList = value;
                    OnPropertyChanged("FavoritoList");
                }
            }
        }
        public FavoritoUsuarioCrudViewModel(EstabelecimentoModel estabelecimentoCurrent)
        {
            _messageService = DependencyService.Get<IMessageService>();

            _repository = new FavoritoUsuarioRepository();

            _repositoryEstabelecimento = new EstabelecimentoRepository();

            estabelecimento = estabelecimentoCurrent;

            SalvarClickedCommand = new Command(async () => await Salvar(estabelecimento));

            AtualizarClickedCommand = new Command(async () => await Atualizar(estabelecimento));
        }

        public FavoritoUsuarioCrudViewModel()
        {
            ListarDados();
        }

        public ICommand ExcluirClickedCommand { get; private set; }
        public ICommand SalvarClickedCommand { get; private set; }
        public ICommand AtualizarClickedCommand { get; private set; }
        public ICommand AddFavoritosClickedCommand { get; private set; }
        public ICommand ListarClickedCommand { get; private set; }

        public async Task Salvar(EstabelecimentoModel estabelecimento)
        {
            string message = estabelecimento.IdEstabelecimento > 0 ? "Adicionar" : "Adicionar aos favoritos?";

            bool res = await _messageService.ShowAsyncBool("Adicionar aos favoritos?", message, "Sim", "Não");

            if (res)
            {
                var currentLocation = await Geolocation.GetLocationAsync();
                estabelecimento.CoordenadaX = currentLocation.Latitude;
                estabelecimento.CoordenadaY = currentLocation.Longitude;

                var estabelecimentoFavorito = new FavoritoUsuarioModel()
                {
                    EstabelecimentoRef = estabelecimento,
                    IdEstabelecimento = estabelecimento.IdEstabelecimento,
                    IdFavoritoUsuario = 99,
                    DataCadastro = DateTime.Now.Date.ToString(),
                    NomeEstabelecimento = estabelecimento.NomeEstabelecimento
                };
                _repository.Insert(estabelecimentoFavorito);

                await Application.Current.MainPage.Navigation.PushAsync(new UsuarioPage(99));
            }
        }
        public async Task Atualizar(EstabelecimentoModel estabelecimento)
        {
            string message = "Deseja Salvar As Alterações?";

            bool res = await _messageService.ShowAsyncBool("Salvar", message, "Sim", "Não");

            if (res)
            {
                _repositoryEstabelecimento.Update(estabelecimento);

                if (estabelecimento.IdFuncionario != 99)
                {
                    await Application.Current.MainPage.Navigation.PushAsync(new EstabelecimentoPage());
                }
                else
                {
                    await Application.Current.MainPage.Navigation.PushAsync(new UsuarioPage(99));
                }
            }
        }

        private void ListarDados()
        {
            var lista = _repositoryEstabelecimento.GetList();

            estabelecimentoList.Clear();

            lista.OrderBy(f => f.NomeEstabelecimento).ForEach(x => estabelecimentoList.Add(x));

            NotifyPropertyChanged("FavoritoList");
        }
    }
}

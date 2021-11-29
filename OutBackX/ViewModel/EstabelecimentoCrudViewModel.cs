using OutBackX.Model;
using OutBackX.Repository;
using OutBackX.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace OutBackX.ViewModel
{
    public class EstabelecimentoCrudViewModel : EstabelecimentoBaseViewModel
    {
        #region Construtor 
        public EstabelecimentoCrudViewModel(int idFuncionarioLogado)
        {
            estabelecimento.IdFuncionario = 999999;

            funcionarioModel.IdFuncionario = idFuncionarioLogado;

            SalvarClickedCommand = new Command(async () => await Salvar(estabelecimento));

            AtualizarClickedCommand = new Command(async () => await Atualizar(estabelecimento));

            ExcluirClickedCommand = new Command(async () => await Excluir(estabelecimento));

            AddFavoritosClickedCommand = new Command(async () => await AddFavoritos(estabelecimento));
        }

        public EstabelecimentoCrudViewModel(EstabelecimentoModel estabelecimentoCurrent)
        {
            estabelecimento = estabelecimentoCurrent;

            SalvarClickedCommand = new Command(async () => await Salvar(estabelecimento));

            AtualizarClickedCommand = new Command(async () => await Atualizar(estabelecimento));

            ExcluirClickedCommand = new Command(async () => await Excluir(estabelecimento));

            AddFavoritosClickedCommand = new Command(async () => await AddFavoritos(estabelecimento));
        }

        public EstabelecimentoCrudViewModel()
        {           
        }
        #endregion

        #region Properties

        public FuncionarioModel funcionarioModel = new FuncionarioModel();

        private EstabelecimentoRepository _estabelecimentoRepository = new EstabelecimentoRepository();

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
        public EstabelecimentoModel SelectedEstabelecimento
        {
            get => _selectedEstabelecimento;
            set
            {
                if (value != null)
                {
                    _selectedEstabelecimento = value;

                    NotifyPropertyChanged("SelectedEstabelecimento");

                    ExibirDetalhesExcluir(value);
                }
            }
        }
        #endregion

        #region ICommand
        public ICommand ExcluirClickedCommand { get; private set; }
        public ICommand SalvarClickedCommand { get; private set; }
        public ICommand AtualizarClickedCommand { get; private set; }
        public ICommand AddFavoritosClickedCommand { get; private set; }
        public ICommand ListarClickedCommand { get; private set; }

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
        #endregion

        #region Métodos
        public async Task Salvar(EstabelecimentoModel estabelecimento)
        {
            _estabelecimentoRepository = new EstabelecimentoRepository();

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
            _estabelecimentoRepository = new EstabelecimentoRepository();

            string message = "Deseja Salvar As Alterações?";

            bool res = await _messageService.ShowAsyncBool("Salvar", message, "Sim", "Não");

            if (res)
            {
                _estabelecimentoRepository.Update(estabelecimento);

                if (estabelecimento.IdFuncionario != 99)
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
            _estabelecimentoRepository = new EstabelecimentoRepository();

            bool resposta = await _messageService.ShowAsyncBool("Excluir", "Confirma a exlusão?", "Sim", "Não");
            if (resposta)
            {
                _estabelecimentoRepository.Delete(estabelecimento);
                

                await Application.Current.MainPage.Navigation.PushAsync(new EstabelecimentoPage());
            }
        }
        public async Task AddFavoritos(EstabelecimentoModel estabelecimento)
        {
            bool resposta = await _messageService.ShowAsyncBool("Adicionar", "Adicionar aos favoritos?", "Sim", "Não");
            if (resposta)
            {
                estabelecimento.IdFuncionario = 99;
                _estabelecimentoRepository.Insert(estabelecimento);

                await Application.Current.MainPage.Navigation.PushAsync(new UsuarioPage(99));
            }
        }
        private void ExibirDetalhesExcluir(EstabelecimentoModel estabelecimento)
        {
            Application.Current.MainPage.Navigation.PushAsync(new EstabelecimentoExcluirPage(estabelecimento));
        }
        #endregion              
    }
}

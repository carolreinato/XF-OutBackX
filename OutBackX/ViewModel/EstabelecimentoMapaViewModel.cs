using OutBackX.Model;
using OutBackX.Repository;
using OutBackX.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;


namespace OutBackX.ViewModel
{
    public class EstabelecimentoMapaViewModel : INotifyPropertyChanged
    {
        private static Geocoder geocoder;
        public const double DistanciaListaEstabelecimentos = 10;
        protected IMessageService _messageService;

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged == null)
                return;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion


        #region Properties

        private EstabelecimentoRepository _estabelecimentoRepository;
        private FavoritoUsuarioRepository _favoritoUsuarioRepository;


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

        private Pin selectedPin = null;

        public Pin SelectedPin
        {
            get { return selectedPin; }
            set 
            { 
                selectedPin = value;
                OnPropertyChanged("SelectedPin");
            }
        }


        private bool editAproximacao = false;

        public bool EditAproximacao
        {
            get { return editAproximacao; }
            set { editAproximacao = value; }
        }

        private double distanciaKm = 2;

        public double DistanciaKm
        {
            get { return distanciaKm; }
            set
            {
                if (distanciaKm != value)
                {
                    distanciaKm = value;
                }
            }
        }

        private bool showShare = false;

        public bool ShowShare
        {
            get { return showShare; }
            set 
            {
                showShare = value;
                OnPropertyChanged("ShowShare");
            }
        }

        private Xamarin.Forms.GoogleMaps.Map _mapa = new Xamarin.Forms.GoogleMaps.Map();
        public Xamarin.Forms.GoogleMaps.Map Mapa
        {
            get { return _mapa; }
            set { _mapa = value; }
        }
        #endregion

        #region ICommand
        private ICommand _centralizarMapaCommand;
        public ICommand CentralizarMapaCommand 
        {
            get
            {
                return _centralizarMapaCommand ?? (_centralizarMapaCommand = new Command(async () => await CentralizarMap()));
            }
        }

        private ICommand _searchCommand;
        public ICommand SearchCommand
        {
            get
            {
                return _searchCommand ?? (_searchCommand = new Command<string>(async (endereco) => await LocalizarEndereco(endereco)));
            }
        }

        private ICommand _editAproximacaoCommand;
        public ICommand EditAproximacaoCommand
        {
            get
            {
                return _editAproximacaoCommand ?? (_editAproximacaoCommand = new Command(async () => await EditarDistanciaKm()));
            }
        }

        private ICommand _mostrarEstabelecimentosCommand;
        public ICommand MostrarEstabelecimentosCommand
        {
            get
            {
                return _mostrarEstabelecimentosCommand ?? (_mostrarEstabelecimentosCommand = new Command(async () => MostrarEstabelecimentos()));
            }
        }
        
        private ICommand _compartilharCommand;
        public ICommand CompartilharCommand
        {
            get
            {
                return _compartilharCommand ?? (_compartilharCommand  = new Command(async () => await CompartilharEstabelecimento()));
            }

        }

        private ICommand _adicionarFavoritosCommand;
        public ICommand AdicionarFavoritosCommand
        {
            get
            {
                return _adicionarFavoritosCommand ?? (_adicionarFavoritosCommand = new Command(async () => await AdicionarFavoritos()));
            }
        }

        #endregion

        #region Metodos
        private static Geocoder GetGeoCoder()
        {
            if (geocoder == null)
            {
                geocoder = new Geocoder();
            }
            return geocoder;
        }
        public EstabelecimentoMapaViewModel()
        {
            _messageService = DependencyService.Get<IMessageService>();
        }
        public EstabelecimentoMapaViewModel(Xamarin.Forms.GoogleMaps.Map mapa)
        {
            _messageService = DependencyService.Get<IMessageService>();
            _mapa = mapa;
            _mapa.PinClicked += Map_PinClicked; ;
            _mapa.InfoWindowClicked += InfoWindow_Clicked;
            CentralizarMapaCommand.Execute(null);
        }

        public EstabelecimentoMapaViewModel(Xamarin.Forms.GoogleMaps.Map mapa, EstabelecimentoModel estabelecimento)
        {
            _messageService = DependencyService.Get<IMessageService>();
            _mapa = mapa;
            _mapa.PinClicked += Map_PinClicked;
            _mapa.InfoWindowClicked += InfoWindow_Clicked;
            LocalizarEstabelecimento(estabelecimento);
        }

        private void ClearPins()
        {
            _mapa.Pins.Clear();
            selectedPin = null;
        }

        private void MostrarEstabelecimentos()
        {
           
            _estabelecimentoRepository = new EstabelecimentoRepository();

            List<EstabelecimentoModel> lista = _estabelecimentoRepository.GetList();
            ClearPins();

            foreach (EstabelecimentoModel e in lista)
            {
                LocalizarEstabelecimento(e);
            }
        }

        private async void LocalizarEstabelecimento(EstabelecimentoModel e)
        {
            double Latitude = e.CoordenadaX;
            double Longitude = e.CoordenadaY;
            if (Latitude != 0 && Longitude != 0)
            {
                Position position = new Position(Latitude, Longitude);
                await LocalizarEstabelecimentoByPosition(position, e);
            }

            if (_mapa.Pins.Count == 0)
            {
                string endereco = $"{e.EnderecoEstabelecimento}, {e.BairroEstabelecimento}, {e.CEPEstabelecimento}, {e.CidadeEstabelecimento}, {e.EstadoEstabelecimento}";
                await LocalizarEstabelecimentoByEndereco(endereco, e);
            }
        }

        private async Task LocalizarEstabelecimentoByPosition(Position position, EstabelecimentoModel estabelecimento)
        {
            IEnumerable<string> possibleAddresses = await GetGeoCoder().GetAddressesForPositionAsync(position);
            string address = possibleAddresses.FirstOrDefault();
            _mapa.Pins.Add(new Pin()
            {
                Type = PinType.Generic,
                Label = estabelecimento.NomeEstabelecimento,
                Position = position,
                Address = address,
                BindingContext = estabelecimento
            });
            if (_mapa.Pins.Count == 1)
            {
                _mapa.MoveToRegion(MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(DistanciaListaEstabelecimentos)));
            }
        }
        private async Task LocalizarEstabelecimentoByEndereco(string Endereco, EstabelecimentoModel estabelecimento)
        {
            Task<IEnumerable<Position>> resultado = GetGeoCoder().GetPositionsForAddressAsync(Endereco);
            IEnumerable<Position> posicoes = await resultado;

            foreach (Position item in posicoes)
            {
                IEnumerable<string> possibleAddresses = await GetGeoCoder().GetAddressesForPositionAsync(item);
                string address = possibleAddresses.FirstOrDefault();
                _mapa.Pins.Add(new Pin()
                {
                    Type = PinType.Generic,
                    Position = item,
                    Label = estabelecimento.NomeEstabelecimento,
                    Address = address,
                    BindingContext = estabelecimento
                });
                if (_mapa.Pins.Count == 1)
                {
                    _mapa.MoveToRegion(MapSpan.FromCenterAndRadius(item, Distance.FromKilometers(DistanciaListaEstabelecimentos)));
                }
            }
        }

        private async Task CentralizarMap()
        {
            var currentLocation = await Xamarin.Essentials.Geolocation.GetLastKnownLocationAsync();
            Position currentPosition = new Position(currentLocation.Latitude, currentLocation.Longitude);
            _mapa.MoveToRegion(MapSpan.FromCenterAndRadius(currentPosition, Distance.FromKilometers(DistanciaKm)));
            ClearPins();
            IEnumerable<string> possibleAddresses = await GetGeoCoder().GetAddressesForPositionAsync(currentPosition);
            _mapa.Pins.Add(new Pin()
            {
                Type = PinType.Generic,
                Position = currentPosition,
                Label = "Localização",
                Address = possibleAddresses.FirstOrDefault()
            });
        }
        private async Task LocalizarEndereco(string Endereco)
        {
            Task<IEnumerable<Position>> resultado = GetGeoCoder().GetPositionsForAddressAsync(Endereco);
            IEnumerable<Position> posicoes = await resultado;

            ClearPins();
            foreach (Position item in posicoes)
            {
                _mapa.MoveToRegion(MapSpan.FromCenterAndRadius(item, Distance.FromKilometers(DistanciaKm)));
                IEnumerable<string> possibleAddresses = await GetGeoCoder().GetAddressesForPositionAsync(item);
                string address = possibleAddresses.FirstOrDefault();
                _mapa.Pins.Add(new Pin()
                {
                    Type = PinType.Generic,
                    Position = item,
                    Label = Endereco,
                    Address = address
                });
            }
        }
        private async Task EditarDistanciaKm()
        {
            string result = await Application.Current.MainPage.DisplayPromptAsync(
                title: "Calibrar aproximação",
                message: "Distância em Km: ",
                accept: "OK",
                cancel: "Cancela",
                placeholder: "",
                maxLength: 6,
                keyboard: Keyboard.Numeric,
                initialValue: DistanciaKm.ToString());
            DistanciaKm = double.Parse(result);
            await CentralizarMap();
        }

        private async Task AdicionarFavoritos()
        {

            if (selectedPin != null)
            {
                EstabelecimentoModel e = (EstabelecimentoModel)selectedPin.BindingContext;
                if (e != null)
                {
                    bool resposta = await _messageService.ShowAsyncBool("Adicionar", $"Adicionar '{e.NomeEstabelecimento}' aos favoritos?", "Sim", "Não");
                    if (resposta)
                    {
                        if (_favoritoUsuarioRepository == null)
                            _favoritoUsuarioRepository = new FavoritoUsuarioRepository();

                        FavoritoUsuarioModel favorito = _favoritoUsuarioRepository.Get(e);
                        if (favorito == null)
                        {
                            favorito = new FavoritoUsuarioModel
                            {
                                EstabelecimentoRef = e,
                                IdEstabelecimento = e.IdEstabelecimento,
                                DataCadastro = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
                            };
                            _favoritoUsuarioRepository.Insert(favorito);

                            await _messageService.ShowAsync("Adicionado", "Adicionado aos favoritos", "OK");
                        }
                        else
                        {
                            await _messageService.ShowAsync("Atenção", "Estabelecimento já foi adicionado ", "OK");
                        }
                    }
                }
            }
            else
            {
                await _messageService.ShowAsync("Atenção", "Nenum local foi selecionado", "OK");
            }
        }

        private async Task CompartilharEstabelecimento()
        {
            if(selectedPin != null)
            {
                EstabelecimentoModel e = (EstabelecimentoModel)selectedPin.BindingContext;
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
                else
                {
                    await Share.RequestAsync(new ShareTextRequest
                    {
                        Subject = selectedPin.Label,
                        Text = selectedPin.Label,
                        Uri = "http://maps.google.com/maps?saddr=+" + selectedPin.Address.Replace(" ", "+"),
                        Title = selectedPin.Label
                    });
                }
            }
            else 
            {
                await _messageService.ShowAsync("Atenção", "Nenum local foi selecionado", "OK");
            }
        }

        private void Map_PinClicked(object sender, PinClickedEventArgs e)
        {
            e.Handled = false;
            selectedPin = e.Pin;
            showShare = true;
        }

        private void InfoWindow_Clicked(object sender, InfoWindowClickedEventArgs e)
        {
            showShare = true;
        }
        #endregion
    }
}
﻿using OutBackX.Model;
using OutBackX.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OutBackX.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UsuarioEstabelecimentoFavoritoListarPage : ContentPage
    {
        public UsuarioEstabelecimentoFavoritoListarPage(EstabelecimentoModel estabelecimento)
        {
            InitializeComponent();

            BindingContext = new EstabelecimentoCrudViewModel(estabelecimento);

            pesquisaEstabelecimento.IsVisible = false;

            dadosEstabelecimento.IsVisible = true;

            CarregaEstabelecimento(estabelecimento);
        }
        public UsuarioEstabelecimentoFavoritoListarPage(int idFuncionarioModel)
        {
            InitializeComponent();

            BindingContext = new EstabelecimentoListViewModel("UsuarioEstabelecimentoFavoritoListarPage");
        }

        private void CarregaEstabelecimento(EstabelecimentoModel estabelecimento)
        {
            nomeEstabelecimento.Text = estabelecimento.NomeEstabelecimento;
            cepEstabelecimento.Text = estabelecimento.CEPEstabelecimento;
            enderecoEstabelecimento.Text = estabelecimento.EnderecoEstabelecimento;
            estadoEstabelecimento.Text = estabelecimento.EstadoEstabelecimento;
            bairroEstabelecimento.Text = estabelecimento.BairroEstabelecimento;
            cidadeEstabelecimento.Text = estabelecimento.CidadeEstabelecimento;
            coordenadaXEstabelecimento.Text = estabelecimento.CoordenadaX.ToString();
            coordenadaYEstabelecimento.Text = estabelecimento.CoordenadaY.ToString();
            nivelLotacao.SelectedItem = estabelecimento.NivelLotacao;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
        }
    }
}
using OutBackX.ViewModel;
using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OutBackX.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EstabelecimentoUsuarioPage : ContentPage
    {
        public EstabelecimentoUsuarioPage(int IdEstabelecimento)
        {
            InitializeComponent();

            //BindingContext = new EstabelecimentoCrudViewModel(IdEstabelecimento);
        }
    }
}

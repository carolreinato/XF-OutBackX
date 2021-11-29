using OutBackX.Model;
using OutBackX.ViewModel;
using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OutBackX.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EstabelecimentoCadastrarPage : ContentPage
    {
        public EstabelecimentoCadastrarPage(int idFuncionarioModel)
        {
            InitializeComponent();

            BindingContext = new EstabelecimentoCrudViewModel(idFuncionarioModel);
        }
    }
}

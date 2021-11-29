using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OutBackX.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FuncionarioEntrarPage : ContentPage
    {
        public FuncionarioEntrarPage()
        {
            InitializeComponent();

            BindingContext = new ViewModel.FuncionarioViewModel();
        }
        public void PressedCadastrarFuncionario(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new FuncionarioCadastrarPage());
        }
    }
}

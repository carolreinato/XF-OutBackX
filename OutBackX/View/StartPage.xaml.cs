using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OutBackX.View
{
    public partial class StartPage : ContentPage
    {                
        public StartPage()
        {
            InitializeComponent();         
        }

        private async void PressedFuncionario(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new FuncionarioEntrarPage());
        }

        private async void PressedNaoFuncionario(object sender, System.EventArgs e)
        {            
            await Navigation.PushAsync(new UsuarioPage(99));
        }
    }
}

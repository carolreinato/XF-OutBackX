using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OutBackX.Util
{
    public class MessageService : IMessageService
    {
        public async Task ShowAsync(string message)
        {
            await Application.Current.MainPage.DisplayAlert("Clientes", message, "Ok");
        }
        public async Task ShowAsync(string title, string message, string text)
        {
            await Application.Current.MainPage.DisplayAlert(title, message, text);
        }
        public async Task ShowAsync(string title, string message, string text1, string text2)
        {
            await Application.Current.MainPage.DisplayAlert(title, message, text1, text2);
        }
        public async Task<bool> ShowAsyncBool(string title, string message, string text1, string text2)
        {
            return await Application.Current.MainPage.DisplayAlert(title, message, text1, text2);
        }
    }
}

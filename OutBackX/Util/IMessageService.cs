using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OutBackX.Util
{
    public interface IMessageService
    {
        Task ShowAsync(string message);
        Task ShowAsync(string title, string message, string text1);
        Task ShowAsync(string title, string message, string text1, string text2);
        Task<bool> ShowAsyncBool(string title, string message, string text1, string text2);
    }
}

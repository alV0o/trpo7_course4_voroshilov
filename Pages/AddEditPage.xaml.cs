using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using trpo7.Models;

namespace trpo7.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        public bool IsEdit { get; set; } = false;
        public Book Book { get; set; }
        HttpClient client;
        public AddEditPage(Book? selectedBook, HttpClient _client)
        {
            IsEdit = selectedBook != null;
            Book = new Book();
            if (IsEdit)
            {
                Book.Id = selectedBook.Id;
                Book.Title = selectedBook.Title;
                Book.Author = selectedBook.Author;
                Book.PublishYear = selectedBook.PublishYear;
                Book.Price = selectedBook.Price;
                Book.IsAvailable = selectedBook.IsAvailable;
            }
            client = _client;
            InitializeComponent();
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HttpResponseMessage response;
                string message = "";
                if (Book.Id != 0)
                {
                    response = await client.PutAsJsonAsync($"api/v1/Book/{Book.Id}", Book);
                    message = "Книга успешно обновлена";
                }
                else
                {
                    response = await client.PostAsJsonAsync($"api/v1/Book", Book);
                    message = "Книга успешно добавлена";
                }

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show(message);
                    NavigationService.GoBack();
                }
                else
                {
                    MessageBox.Show($"Ошибка: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private async void Patch_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                HttpResponseMessage response = await client.PatchAsJsonAsync($"api/v1/Book/{Book.Id}", Book);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Частичное изменение применено");
                    NavigationService.GoBack();
                }
                else
                {
                    MessageBox.Show($"Ошибка: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

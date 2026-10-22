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
        private bool isEdit = false;
        public Book Book { get; set; }
        HttpClient client;
        public AddEditPage(Book? selectedBook, HttpClient _client)
        {
            Book = new Book();
            isEdit = selectedBook != null;
            client = _client;
            InitializeComponent();
        }

        private async void Post()
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("/api/v1/Book", Book);
            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Книга успешно добавлена");
                NavigationService.GoBack();
            }
            else
            {
                MessageBox.Show($"Ошибка: {response.StatusCode} \n" +
                    $"{Book.Title} {Book.Author} {Book.PublishYear} {Book.Price} {Book.IsAvailable}");
            }
        }

        private async void Patch()
        {
            HttpResponseMessage response = await client.PatchAsJsonAsync();
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            Post();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}

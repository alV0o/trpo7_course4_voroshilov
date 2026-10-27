using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web;
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
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public ObservableCollection<Book> Books { get; set; } = new ObservableCollection<Book>();
        public string Author { get; set; }
        public int MinYear { get; set; } = 0;
        public int Index { get; set; } = 0;
        public Book SelectedBook { get; set; } = null;

        private HttpClient client = new HttpClient();
        public MainPage()
        {
            client.BaseAddress = new Uri("http://localhost:5000/"); //адрес api
            InitializeComponent();
        }

        private async void GetAll_Click(object sender, RoutedEventArgs e)
        {
            //https://stackoverflow.com/questions/17096201/build-query-string-for-system-net-httpclient-get
            var builder = new UriBuilder(client.BaseAddress);
            builder.Path = "/api/v1/Book";

            var query = HttpUtility.ParseQueryString(builder.Query);
            if (!String.IsNullOrWhiteSpace(Author)) query["author"] = Author;
            if (MinYear > 0) query["minYear"] = MinYear.ToString(); 
            if (query.Count > 0) builder.Query = query.ToString();

            try
            {
                var response = await client.GetFromJsonAsync<List<Book>>(builder.ToString());
                if (response != null) //??? почему нет StatusCode
                {
                    Books.Clear();
                    foreach (Book book in response)
                    {
                        Books.Add(book);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void GetById_Click(object sender, RoutedEventArgs e)
        {
            string path = $"/api/v1/Book/{Index}";
            try
            {
                var response = await client.GetFromJsonAsync<Book>(path);
                if (response != null)
                {
                    Books.Clear();
                    Books.Add(response);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Pages.AddEditPage(null, client));
            Books.Clear();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedBook != null)
            {
                NavigationService.Navigate(new Pages.AddEditPage(SelectedBook, client));
                Books.Clear();
            }
            else
            {
                MessageBox.Show("Выберите книгу");
            }
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedBook != null)
            {
                try
                {
                    var response = await client.DeleteAsync($"/api/v1/Book/{SelectedBook.Id}");
                    if (response.IsSuccessStatusCode)
                    {
                        Books.Remove(SelectedBook);
                        SelectedBook = null;
                    }
                    else
                    {
                        MessageBox.Show(response.ReasonPhrase);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Выберите книгу для удаления");
            }
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DatabaseConnection;

namespace Store
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        

        public MainWindow()
        {
            InitializeComponent();            
            State.Movies = API.GetMovieSlice(0, 30);
            HomeWindow.Children.Clear();
            PrintPosters(CreateMovieGrid());
            
        }

        // when hovering over a movie poster
        private void MovieBoxEnter(object sender, MouseEventArgs e)
        {
            var movieBox = (GroupBox)sender;
            movieBox.Height = 220;
            movieBox.Width = 160;
        }

        // when not hovering over a movie poster
        private void MovieBoxLeave(object sender, MouseEventArgs e)
        {
            var movieBox = (GroupBox)sender;
            movieBox.Height = 180;
            movieBox.Width = 120;
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //Debugging, den känner inte av vilken kolumn den är i.
            //Kanske känner av i fel grid? 
            var x = Grid.GetColumn(sender as UIElement);
            var y = Grid.GetRow(sender as UIElement);
            int i = y * State.currentGrid.ColumnDefinitions.Count() + x;

            State.Pick = State.Movies[i];

            var movieWindow = new MovieWindow();
            movieWindow.Show();
        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            HomeWindow.Children.Clear();

            var groupBox = new GroupBox
            {
                Name = "groupbox",
                Header = "Profile Information",
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(3),
                Height = 750,
                Width = 700,
                FontFamily = new FontFamily("Segoe Script"),
                
                
            };

            var border = new Border
            {
                Name = "replaceGroupBox",
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(3),
                Height = 750,
                Width = 700,
                CornerRadius = new CornerRadius(12, 12, 12, 12)                
            };
            
            HomeWindow.Children.Add(border);
            


            var stackpanel = new StackPanel
            {
                Orientation = Orientation.Vertical
            };

            border.Child = stackpanel;

            var welcomeMessage = new TextBlock
            {
                TextAlignment = TextAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Text = $"Hello {State.User.UserName}!",
                FontSize = 20,
                Margin = new Thickness(0, 20, 0, 0),
                FontFamily = new FontFamily("Segoe Script"),
            };

            stackpanel.Children.Add(welcomeMessage);

            var historyMessage = new TextBlock
            {
                TextAlignment = TextAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Text = $"Below you can see your history and currently rented movies.",
                FontSize = 20,
                Margin = new Thickness(0, 20, 0, 0),
                FontFamily = new FontFamily("Segoe Script"),
            };

            stackpanel.Children.Add(historyMessage);



            var dataGrid = new DataGrid
            {
                Height = 500,
                Width = 650,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 20, 0, 0),
                DataContext = State.Movies ////ändra sedan, bara för test
            };

            DataGridTextColumn titleColumn = new DataGridTextColumn();
            stackpanel.Children.Add(dataGrid);
            dataGrid.ItemsSource = API.rentalsHistory(State.User.Id);

        }


        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            HomeWindow.Children.Clear();

            State.Movies = API.GetMovieSlice(0, 30);

            PrintPosters(CreateMovieGrid());
        }

        private Grid CreateMovieGrid()
        {
            var updatedMovieGrid = new Grid()
            {
                Width = 900,
                Name = "updatedMovieGrid",
                ShowGridLines = false
            };


            var scrollViewer = new ScrollViewer()
            {
                Content = updatedMovieGrid,
                BorderThickness = new Thickness(1),
            };

            HomeWindow.Children.Add(scrollViewer);

            var column = 5;
            var row = (int)Math.Ceiling((double)State.Movies.Count() / (double)column);           


            for (int j = 0; j < column; j++)
            {
                var columnDefinition = new ColumnDefinition()
                {

                };
                updatedMovieGrid.ColumnDefinitions.Add(columnDefinition);
            }

            for (int i = 0; i < row; i++)
            {            
                
                
                var rowDefinition = new RowDefinition()
                {

                };

                rowDefinition.Height = new GridLength(230);

                updatedMovieGrid.RowDefinitions.Add(rowDefinition);
                              
            }

            State.currentGrid = updatedMovieGrid;

            return updatedMovieGrid;
        }

        /// <summary>
        /// Metod för att komma till varukorgen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CartButton_Click(object sender, RoutedEventArgs e)
        {
            HomeWindow.Children.Clear();

            var border = new Border
            {
                Name = "replaceGroupBoxInCart",
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(3),
                Height = 750,
                Width = 700,
                CornerRadius = new CornerRadius(12, 12, 12, 12)
            };


            var groupBox = new GroupBox
            {
                Name = "groupboxforcart",
                Header = "Cart",
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(3),
                Height = 750,
                Width = 700,
                FontFamily = new FontFamily("Segoe Script")
            };

            HomeWindow.Children.Add(border);





            var stackpanel = new StackPanel
            {
                Name = "CartStack",
                Orientation = Orientation.Vertical
            };

            border.Child = stackpanel;

            var welcomeMessage = new TextBlock
            {
                TextAlignment = TextAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Text = $"Hello {State.User.UserName}!",
                FontSize = 20,
                Margin = new Thickness(0, 20, 0, 0),
                FontFamily = new FontFamily("Segoe Script"),
            };

            stackpanel.Children.Add(welcomeMessage);

            var infoMessage = new TextBlock
            {
                TextAlignment = TextAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Text = $"Below you can see your current cart.",
                FontSize = 20,
                Margin = new Thickness(0, 20, 0, 0),
                FontFamily = new FontFamily("Segoe Script"),
            };

            stackpanel.Children.Add(infoMessage);


            var lview = new ListView()
            {
                Height = 400,
                Width = 600,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };
            stackpanel.Children.Add(lview);
            lview.SetValue(Grid.RowProperty, 2);

            GridView gv = new GridView();
            GridViewColumn gv1 = new GridViewColumn();
            gv1.DisplayMemberBinding = new Binding("Title");
            gv1.Header = "Title";
            gv.Columns.Add(gv1);

            GridViewColumn gv2 = new GridViewColumn();
            gv2.DisplayMemberBinding = new Binding("Price");
            gv2.Header = "Price";
            gv.Columns.Add(gv2);

            GridViewColumn gv3 = new GridViewColumn();
            gv3.Header = "Delete";
            gv.Columns.Add(gv3);            

            lview.ItemsSource = State.PickedMovies;
            lview.View = gv;

            var buyButton = new Button
            {
                Height = 100,
                Width = 100,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Name = "BuyButton",
                Content = "Buy"
            };

            buyButton.Click += buybtn_Click;
            stackpanel.Children.Add(buyButton);


            // Getting the total price of movies in cart
            double sum = State.PickedMovies.Sum(c => Convert.ToDouble(c.Price));

            TextBox totalPrice = new TextBox
            {
                Text = "Total: " + sum.ToString()
            };

            stackpanel.Children.Add(totalPrice);

        }


        /// <summary>
        /// BuyButton for the Cart
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buybtn_Click(object sender, RoutedEventArgs e)
        {
            //Använd kod i varukorg
            if (API.RegisterSale(State.User, State.PickedMovies))
                MessageBox.Show("Added to your basket.", "Sale Succeeded!", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show("An error happened while buying the movie, please try again at a later time.", "Sale Failed!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        // Printing movie posters in movie grid
        private void PrintPosters(Grid movieGrid)
        {
            //TODO 
            // create stackpanel to put groupbox and separate titletext in

            int i = 0;
            for (int y = 0; y < movieGrid.RowDefinitions.Count; y++)
            {
                for (int x = 0; x < movieGrid.ColumnDefinitions.Count; x++)
                {
                    if (i < State.Movies.Count)
                    {
                        var movie = State.Movies[i];
                        try
                        {
                            var image = new Image() { };
                            image.Cursor = Cursors.Hand;
                            image.MouseUp += Image_MouseUp;
                            image.HorizontalAlignment = HorizontalAlignment.Center;
                            image.VerticalAlignment = VerticalAlignment.Center;
                            image.Source = new BitmapImage(new Uri(movie.Poster));
                            //image.Height = 120;
                            image.Margin = new Thickness(20, 20, 20, 20);

                            movieGrid.Children.Add(image);
                            Grid.SetRow(image, y);
                            Grid.SetColumn(image, x);
                            i++;
                        }
                        catch (Exception e) when
                            (e is ArgumentNullException ||
                             e is System.IO.FileNotFoundException ||
                             e is UriFormatException)
                        {
                            
                            continue;
                        }
                    }
                    


                }
            }
        }

        private void search_Btn_Click(object sender, RoutedEventArgs e)
        {
            State.Movies = API.SearchFunction(searchTxt.Text);
            HomeWindow.Children.Clear();
            PrintPosters(CreateMovieGrid());
        }

        private void sortGenre_Btn_Click(object sender, RoutedEventArgs e)
        {
            State.Movies = API.SortGenre(SortByGenre.SelectedValue);
            HomeWindow.Children.Clear();
            PrintPosters(CreateMovieGrid());
        }


    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace _1._13_Irene_ArtWorld
{
    public partial class Autores : Window
    {
        private readonly string connectionString;

        public Autores()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["Conexion"]?.ConnectionString;
        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            Menu menu = new Menu();
            menu.Show();
            this.Close();
        }

        private void Autor_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Border border)
            {
                if (border.Child is Grid grid)
                {
                    grid.MinHeight = grid.ActualHeight;
                    grid.MinWidth = grid.ActualWidth;

                    var cuadrosPanel = FindChildByPartialName(grid, "CuadrosPanel") as UIElement;
                    var descripcion = FindChildByPartialName(grid, "Descripcion") as UIElement;

                    if (cuadrosPanel != null && descripcion != null)
                    {
                        cuadrosPanel.Visibility = Visibility.Collapsed;
                        descripcion.Visibility = Visibility.Visible;
                    }
                }
            }
        }

        private void Autor_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Border border)
            {
                if (border.Child is Grid grid)
                {
                    grid.ClearValue(Grid.MinHeightProperty);
                    grid.ClearValue(Grid.MinWidthProperty);

                    var cuadrosPanel = FindChildByPartialName(grid, "CuadrosPanel") as UIElement;
                    var descripcion = FindChildByPartialName(grid, "Descripcion") as UIElement;

                    if (cuadrosPanel != null && descripcion != null)
                    {
                        cuadrosPanel.Visibility = Visibility.Visible;
                        descripcion.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        private FrameworkElement FindChildByPartialName(FrameworkElement parent, string partialName)
        {
            if (parent == null) return null;

            foreach (object child in LogicalTreeHelper.GetChildren(parent))
            {
                if (child is FrameworkElement fe)
                {
                    if (fe.Name.Contains(partialName))
                        return fe;

                    var found = FindChildByPartialName(fe, partialName);
                    if (found != null)
                        return found;
                }
            }

            return null;
        }
    }
}

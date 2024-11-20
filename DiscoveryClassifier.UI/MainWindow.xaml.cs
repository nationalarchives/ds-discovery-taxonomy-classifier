using System;
using System.Collections.Generic;
using System.Linq;
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
using WPF.MDI;
using DiscoveryClassifier.UI.Views;

namespace DiscoveryClassifier.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ManageCategory(object sender, RoutedEventArgs e)
        {
            var opened = from child in Container.Children
                         where child.Title == "Manage Category"
                         select child;

            if (opened.Count() == 0)
                Container.Children.Add(new MdiChild
                                        {
                                            Title = "Manage Category",
                                            Content = new CategoriesView(),
                                            Width = 1260,
                                            Height = 920,
                                            MaximizeBox = true,
                                            Position = new Point(200, 30)
                                        });
            else
                opened.First().Focus();
        }

        private void ManageDocument(object sender, RoutedEventArgs e)
        {
            var opened = from child in Container.Children
                         where child.Title == "Manage Document"
                         select child;

            if (opened.Count() == 0)
                Container.Children.Add(new MdiChild
                                        {
                                            Title = "Manage Document",
                                            Content = new DocumentView(),
                                            Width = 1260,
                                            Height = 920,
                                            MaximizeBox = true,
                                            Position = new Point(200, 30)
                                        });
            else
                opened.First().Focus();
        }
    }
}

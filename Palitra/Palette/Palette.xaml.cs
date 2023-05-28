using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
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
using static System.Net.Mime.MediaTypeNames;

namespace Palitra
{
    /// <summary>
    /// Логика взаимодействия для Pallete.xaml
    /// </summary>
    public partial class Palette : UserControl
    {
        public Palette()
        {
            InitializeComponent();
        }

        private void RedBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Paste)
            {
                Regex r = new Regex("[^0-9]+");

                if (r.IsMatch(Clipboard.GetText()))
                {
                    e.Handled = true;
                    return;
                }

                if (Clipboard.GetText().Length + RedBox.Text.Length > 3 || Int32.Parse(RedBox.Text + Clipboard.GetText()) > 255)
                {
                    e.Handled = true;
                    RedBox.Text = "255";
                    RedBox.Select(3, 0);
                    return;
                }

                e.Handled = r.IsMatch(Clipboard.GetText());
            }
        }

        private void GreenBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Paste)
            {
                Regex r = new Regex("[^0-9]+");

                if (r.IsMatch(Clipboard.GetText()))
                {
                    e.Handled = true;
                    return;
                }

                if (Clipboard.GetText().Length + GreenBox.Text.Length > 3 || Int32.Parse(GreenBox.Text + Clipboard.GetText()) > 255)
                {
                    e.Handled = true;
                    GreenBox.Text = "255";
                    GreenBox.Select(3, 0);
                    return;
                }

                e.Handled = r.IsMatch(Clipboard.GetText());
            }
        }

        private void BlueBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Paste)
            {
                Regex r = new Regex("[^0-9]+");

                if (r.IsMatch(Clipboard.GetText()))
                {
                    e.Handled = true;
                    return;
                }

                if (Clipboard.GetText().Length + BlueBox.Text.Length > 3 || Int32.Parse(BlueBox.Text + Clipboard.GetText()) > 255)
                {
                    e.Handled = true;
                    BlueBox.Text = "255";
                    BlueBox.Select(3, 0);
                    return;
                }

                e.Handled = r.IsMatch(Clipboard.GetText());
            }
        }

        private void HexBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Paste)
            {
                Regex r = new Regex("[^#0-9A-Fa-f]+");
                
                if (Clipboard.GetText().Count(x => x == '#') > 1 || r.IsMatch(Clipboard.GetText()))
                {
                    e.Handled = true;
                    return;
                }

                if (Clipboard.GetText().Length + HexBox.Text.Length > 7)
                {
                    e.Handled = true;
                    return;
                }
            }
        }
    }
}

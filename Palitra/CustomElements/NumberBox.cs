using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Palitra.CustomElements
{
    public class NumberBox : TextBox
    {
        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            base.OnPreviewTextInput(e);
            Regex r = new Regex("[^0-9]+");

            if (!r.IsMatch(e.Text) && Int32.Parse(Text + e.Text) == 0)
            {
                Text = "0";
                e.Handled = true;
                Select(1, 0);
                return;
            }

            if (!r.IsMatch(e.Text) && int.Parse(Text + e.Text) > 255)
            {
                Text = "255";
                e.Handled = true;
                Select(3, 0);
                return;
            }

            e.Handled = r.IsMatch(e.Text);
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

    }
}

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
    public class HexBox : TextBox
    {
        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            base.OnPreviewTextInput(e);

            Regex r = new Regex("[^0-9A-Fa-f]+");
            if (Text.Length == 0)
            {
                Text = "#";
                Select(Text.Length, 1);
            }

            if (Text.Length >= 7)
            {
                e.Handled = true;
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

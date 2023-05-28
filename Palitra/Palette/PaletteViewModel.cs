using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Palitra
{
    public class PaletteViewModel : ViewModel
    {
        private string red, green, blue, hex;
        private Brush currentColor;
        
        public Brush CurrentColor
        {
            get => currentColor; 
            set
            {
                currentColor = value;
                OnPropertyChanged();
            }
        }

        public string Red
        {
            get => red;
            set
            {
                if (red != value)
                {
                    red = value;
                    SetUpHex();
                    SetUpColor();
                    OnPropertyChanged();
                }
                
            }
        }

        public string Green
        {
            get => green;
            set
            {
                if (green != value)
                {
                    green = value;
                    SetUpHex();
                    SetUpColor();
                    OnPropertyChanged();
                }
                
            }
        }

        public string Blue
        {
            get => blue; 
            set
            {
                if (blue != value)
                {
                    blue = value;
                    SetUpHex();
                    SetUpColor();
                    OnPropertyChanged();
                }
                
            }
        }

        public string Hex
        {
            get => hex;
            set
            {
                if (hex != value)
                {
                    hex = value;
                    SetUpRGB();
                    SetUpColor();
                    OnPropertyChanged();
                }
                
            }
        }

        private void SetUpHex()
        {
            string _red = "", _green = "", _blue = "";
            if (Red != null && Red != "") _red = $"{Byte.Parse(Red):X2}";
            if (Green != null && Green != "") _green = $"{Byte.Parse(Green):X2}";
            if (Blue != null && Blue != "") _blue = $"{Byte.Parse(Blue):X2}";

            Hex = $"#{_red}{_green}{_blue}";
        }

        private void SetUpRGB()
        {
            string hexValue = Hex;
            if (hexValue.Length == 7)
            {
                Red = Byte.Parse(hexValue.Substring(1, 2), NumberStyles.HexNumber).ToString();
                Green = Byte.Parse(hexValue.Substring(3, 2), NumberStyles.HexNumber).ToString();
                Blue = Byte.Parse(hexValue.Substring(5,2), NumberStyles.HexNumber).ToString();
            }
        }

        private void SetUpColor()
        {
            byte _red = 0, _green = 0, _blue = 0;

            if (Red != "" && Red != null) _red = Byte.Parse(Red);

            if (Green != "" && Green != null) _green = Byte.Parse(Green);

            if (Blue != "" && Blue != null) _blue = Byte.Parse(Blue);


            CurrentColor = new SolidColorBrush(Color.FromRgb(_red, _green, _blue));
        }
    }
}
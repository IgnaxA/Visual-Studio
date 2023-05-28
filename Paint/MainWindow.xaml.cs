using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Wpf.AvalonDock.Layout;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;


namespace Paint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        static List<LayoutAnchorablePane> ChildrenCvs = new List<LayoutAnchorablePane>();
        Point startCoordinate = new Point();
        Brush currentColor = Brushes.Black;
        Figures currentFigure = new Figures();
        enum Figures
        {
            Pen,
            Line,
            Ellipse,
            Eraser,
            Star
        }
        double thickness = 1;
        int anglesNum = 110;
        int width = 400;
        int height = 400;
        static int counter = 0;
        bool isDelete = false;
        bool isChanged = false;
        bool isSaved = false;
        bool DrawAvaliable = false; 
        public Double FixedCanvasWidth = new Double();
        public Double FixedCanvasHeight = new Double();
        static List<Canvas> CanvasEnumerable = new List<Canvas>();
        static List<LayoutAnchorable> ChildrenAnchorable = new List<LayoutAnchorable>();
        static Canvas lastChosenCanvas = new Canvas();
        string filePath = "";
        

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            MenuItemSave.IsHitTestVisible = false;
            MenuItemSaveAs.IsHitTestVisible = false;
        }

        private void IsChosen(object sender, EventArgs e)
        {
            LayoutAnchorable la = (LayoutAnchorable)sender;
            ScrollViewer content = (ScrollViewer)la.Content;
            Canvas c = (Canvas)content.Content;
            lastChosenCanvas = c;
            MenuItemSave.IsHitTestVisible = true;
        }

        public void CreateNewWindow(Image i = null)
        {
            MenuItem dynamicItem = new MenuItem();
            LayoutAnchorablePane pane = new LayoutAnchorablePane();
            LayoutAnchorable tab = new LayoutAnchorable();
            ScrollViewer scrollViewer = new ScrollViewer();
            Canvas c = new Canvas();
            Thumb thumb = new Thumb();

            ScaleTransform st = new ScaleTransform();

            if (i != null)  c.Children.Add(i);
            c.SetValue(Canvas.LeftProperty, 0.0);
            c.SetValue(Canvas.TopProperty, 0.0);
            c.VerticalAlignment = VerticalAlignment.Top;
            c.HorizontalAlignment = HorizontalAlignment.Left;
            c.Height = height;
            c.Width = width;
            
            thumb.Width = 20;
            thumb.Height = 20;
            thumb.DragDelta += (sender, e) => Thumb_DragDelta(sender, e, c);
            thumb.Cursor = Cursors.SizeAll;
            KeepThumbInRightBottomCorner(thumb, c);
            c.Loaded += (sender, e) => KeepThumbInRightBottomCorner(thumb, c);
            c.Children.Add(thumb);
            Canvas.SetLeft(thumb, c.ActualWidth - thumb.Width);
            Canvas.SetTop(thumb, c.ActualHeight - thumb.Height);
            
            tab.IsActiveChanged += IsChosen;
            
            tab.Title = "Tab " + (++counter).ToString();
            
            c.RenderTransform = st;
            c.MouseLeftButtonUp += Canvas_MouseLeftButtonDown;
            c.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;
            c.MouseEnter += Canvas_MouseEnter;
            c.MouseLeave += Canvas_MouseLeave;
            c.MouseMove += Canvas_MouseMove;
            
            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
            c.MouseWheel += (sender, e) =>
            {
                if (e.Delta > 0)
                {
                    st.ScaleX *= 1.5;
                    st.ScaleY *= 1.5;
                }
                else
                {
                    st.ScaleX /= 1.5;
                    st.ScaleY /= 1.5;
                }
            };
            FixedCanvasHeight = c.Height;
            FixedCanvasWidth = c.Width;
            c.Background = Brushes.White;
            scrollViewer.Content = c;
            tab.Content = scrollViewer;
            pane.Children.Add(tab);
            CanvasEnumerable.Add(c);
            
            dynamicItem.Header = tab.Title;
            dynamicItem.Click += (sender, e) => DynDirSave(sender, c);
            tab.Hiding += (sender, e) => Tab_Closure(e, pane, dynamicItem);
            //tab.Hiding += (sender, e) => Tab_Closure(pane, dynamicItem);
            ChildrenAnchorable.Add(tab);
            ChildrenCvs.Add(pane);
            FileDialogueSaver.Items.Add(dynamicItem);
            Panel.Children.Add(pane);
            MenuItemSaveAs.IsHitTestVisible = true;
            MenuItemSave.Click += (sender, e) => MenuItem_Save_Click(sender, e, c);
        }

        public Image CreateImage(string fileName)
        {
            Image con = new Image();
            BitmapImage com = new BitmapImage();
            Uri uri = new Uri(fileName);
            com.BeginInit();
            com.UriSource = uri;
            com.CacheOption = BitmapCacheOption.OnLoad;
            com.EndInit();
            width = (int)com.Width;
            height = (int)com.Height;
            con.Source = com;
            return con;
        }

        private static Line CreateLine(double x2, double y2, ref Point startCoordinate, ref Brush currentColor, ref double thickness)
        {
            Line line = new Line();
            line.X1 = startCoordinate.X;
            line.Y1 = startCoordinate.Y;
            line.X2 = x2;
            line.Y2 = y2;
            line.Stroke = currentColor;
            line.StrokeThickness = thickness;
            return line;
        }



        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Canvas es = (Canvas)sender;
            Rectangle thumbRect = new Rectangle();
            thumbRect.Width = 20;
            thumbRect.Height = 20;
            Canvas.SetTop(thumbRect, es.ActualHeight - 20);
            Canvas.SetLeft(thumbRect, es.ActualWidth - 20);
            Point chk = new Point(e.GetPosition(es).X, e.GetPosition(es).Y);
            if (currentFigure == Figures.Eraser)
            {
                DrawEraser(sender, e, es);
                isChanged = true;
            }
            if (chk.X <= es.ActualWidth - 15 && chk.Y <= es.ActualHeight - 15)
            {
                startCoordinate.X = e.GetPosition(es).X;
                startCoordinate.Y = e.GetPosition(es).Y;
                if (currentFigure == Figures.Eraser) DrawEraser(sender, e, es);
                isDelete = false;
                DrawAvaliable = true;
            }
            isDelete = false;
        }

        private void Canvas_MouseEnter(object sender, MouseEventArgs e)
        {
            Canvas es = (Canvas)sender;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DrawAvaliable = false;
            }
            else
            {
                DrawAvaliable = true;
            }
        }

        private void Canvas_MouseLeave(object sender, MouseEventArgs e)
        {
            DrawAvaliable = false;
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            Canvas es = (Canvas)sender;
            DrawAvaliable &= (e.GetPosition(es).X <= es.ActualWidth - 20 && e.GetPosition(es).Y <= es.ActualHeight - 20);
            if (e.LeftButton == MouseButtonState.Pressed && DrawAvaliable)
            {
                isChanged = true;
                switch (currentFigure)
                {
                    case Figures.Pen:
                        DrawPen(sender, e, es);
                        break;

                    case Figures.Line:
                        DrawLine(sender, e, es);
                        break;

                    case Figures.Ellipse:
                        DrawEllipse(sender, e, es);
                        break;

                    case Figures.Eraser:
                        DrawEraser(sender, e, es);
                        break;

                    case Figures.Star:
                        DrawStar(sender, e, es);
                        break;
                }
            }
        }

        private void DrawPen(object sender, MouseEventArgs e, Canvas es)
        {
            Line line1 = CreateLine(e.GetPosition(es).X, e.GetPosition(es).Y, ref startCoordinate, ref currentColor, ref thickness);

            es.Children.Add(line1);
            startCoordinate.X = e.GetPosition(es).X;
            startCoordinate.Y = e.GetPosition(es).Y;

            // рисуем кружочками
            Ellipse el1 = new Ellipse();
            el1.Width = thickness;
            el1.Height = thickness;
            el1.Stroke = currentColor;
            el1.Fill = currentColor;
            Canvas.SetTop(el1, e.GetPosition(es).Y - thickness / 2);
            Canvas.SetLeft(el1, e.GetPosition(es).X - thickness / 2);

            es.Children.Add(el1);
            if (e.LeftButton != MouseButtonState.Pressed)
            {
                es.Children.Remove(el1);
                es.Children.Remove(line1);
            }
        }

        private void DrawLine(object sender, MouseEventArgs e, Canvas es)
        {
            Line l = CreateLine(e.GetPosition(es).X, e.GetPosition(es).Y, ref startCoordinate, ref currentColor, ref thickness);
            if (isDelete) es.Children.RemoveAt(es.Children.Count - 1);
            es.Children.Add(l);
            isDelete = true;
        }

        private void DrawEllipse(object sender, MouseEventArgs e, Canvas es)
        {
            Ellipse el = new Ellipse();


            Canvas.SetLeft(el, startCoordinate.X);
            if (startCoordinate.X > e.GetPosition(es).X)
                Canvas.SetLeft(el, e.GetPosition(es).X);

            Canvas.SetTop(el, startCoordinate.Y);
            if (startCoordinate.Y > e.GetPosition(es).Y)
                Canvas.SetTop(el, e.GetPosition(es).Y);
            

            el.Width = Math.Abs(startCoordinate.X - e.GetPosition(es).X);
            el.Height= Math.Abs(startCoordinate.Y - e.GetPosition(es).Y);
            el.Stroke = currentColor;
            
            if (isDelete) es.Children.RemoveAt(es.Children.Count - 1);
            es.Children.Add(el);
            isDelete = true;
        }

        private void DrawEraser(object sender, MouseEventArgs e, Canvas es)
        {
            isChanged = true;
            Rectangle p = new Rectangle();
            p.Height = thickness;
            p.Width = thickness;
            p.Fill = es.Background;
            p.Stroke = es.Background;
            Canvas.SetLeft(p, e.GetPosition(es).X - thickness / 2);
            Canvas.SetTop(p, e.GetPosition(es).Y - thickness / 2);

            Brush backgroundColor = es.Background;
            Line line1 = CreateLine(e.GetPosition(es).X, e.GetPosition(es).Y, ref startCoordinate, ref backgroundColor, ref thickness);
            es.Children.Add(line1);
            startCoordinate.X = e.GetPosition(es).X;
            startCoordinate.Y = e.GetPosition(es).Y;

            es.Children.Add(p);
        }

        private void DrawStar(object sender, MouseEventArgs e, Canvas es)
        {
            if (TextBoxStarInput.Text != null && TextBoxStarInput.Text != "") anglesNum = Int32.Parse(TextBoxStarInput.Text);
            else anglesNum = 5;
            Point currentPosition = e.GetPosition(es);

            double width = Math.Abs(currentPosition.X - startCoordinate.X);
            double height = Math.Abs(currentPosition.Y - startCoordinate.Y);

            double x0 = width / 2;
            double y0 = height / 2;

            // R/r = ratio
            double ratio = 2;

            // радиусы
            double R = Math.Min(width, height) / 2;
            double r = R / ratio;

            Point[] points = new Point[2 * anglesNum + 1];
            double rotateValue = Math.PI / anglesNum;

            // текущий угол + определение начального
            double angle;
            if (anglesNum % 4 == 0)
                angle = 0;
            else if (anglesNum % 4 == 1)
                angle = 1.5 * rotateValue;
            else if (anglesNum % 4 == 2)
                angle = rotateValue;
            else
                angle = 1.5 * rotateValue - Math.PI;

            // растягиваем звезду
            double stretchX = 1;
            double stretchY = 1;
            if (width > height)
                stretchX = width / height;
            else if (width < height)
                stretchY = height / width;

            // маленький/большой круг
            double currRadius;

            for (int k = 0; k < 2 * anglesNum + 1; k++)
            {
                if (k % 2 == 0)
                    currRadius = R;
                else
                    currRadius = r;

                // посчитали правильную звезду
                double point_x = x0 + currRadius * Math.Cos(angle);
                double point_y = y0 + currRadius * Math.Sin(angle);

                // растянули по X/Y
                if (stretchX > 1)
                    point_x = (point_x - width / 2) * stretchX + width / 2;
                else if (stretchY > 1)
                    point_y = (point_y - height / 2) * stretchY + height / 2;

                points[k] = new Point(point_x, point_y);
                angle += rotateValue;
            }

            Polygon star = new Polygon();
            star.Stroke = currentColor;
            star.StrokeThickness = thickness;
            star.Points = new PointCollection(points);

            Canvas.SetTop(star, Math.Min(currentPosition.Y, startCoordinate.Y));
            Canvas.SetLeft(star, Math.Min(currentPosition.X, startCoordinate.X));
            if (isDelete) es.Children.RemoveAt(es.Children.Count - 1);

            es.Children.Add(star);

            isDelete = true;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            ComboBoxItem currentItem = (ComboBoxItem)comboBox.SelectedItem;
            TextBlock selectedText = (TextBlock)currentItem.Content;
            if (selectedText!= null) thickness = Convert.ToDouble(selectedText.Text);
        }

        private void Button_Pen_Click(object sender, RoutedEventArgs e)
        {
            currentFigure = Figures.Pen;
        }

        private void Button_Line_Click(object sender, RoutedEventArgs e)
        {
            currentFigure = Figures.Line;
        }

        private void Button_Ellipse_Click(object sender, RoutedEventArgs e)
        {
            currentFigure = Figures.Ellipse;
        }

        private void Button_Star_Click(object sender, RoutedEventArgs e)
        {
            currentFigure = Figures.Star;
        }

        private void Button_Eraser_Click(object sender, RoutedEventArgs e)
        {
            currentFigure = Figures.Eraser;
        }

        private void ColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (ChangeColor.SelectedColor != null) currentColor = new SolidColorBrush((Color)ChangeColor.SelectedColor);
        }


        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e, Canvas canv)
        {
            DrawAvaliable = false;
            if (canv.Width + e.HorizontalChange > 0 && canv.Height + e.VerticalChange > 0)
            {
                canv.Width += e.HorizontalChange;
                canv.Height += e.VerticalChange;
            }
            List<UIElement> childrenToRemove = new List<UIElement>();
            foreach (UIElement child in canv.Children)
            {
                double left = (double)child.GetValue(Canvas.LeftProperty);
                double top = (double)child.GetValue(Canvas.TopProperty);
                double width = child.RenderSize.Width;
                double height = child.RenderSize.Height;

                if (child is Line)
                {
                    Line line = (Line)child;
                    if (line.X1 > canv.ActualWidth || line.Y1 > canv.ActualHeight || line.X2 > canv.ActualWidth || line.Y2 > canv.ActualHeight)
                    {
                        childrenToRemove.Add(child);
                    }
                }
                else if (left + width < 0 || top + height < 0 || left + width > canv.ActualWidth || top + height > canv.ActualHeight)
                {
                    childrenToRemove.Add(child);
                }

            }
            foreach (UIElement child in childrenToRemove)
            {
                canv.Children.Remove(child);
            }
        }

        private void KeepThumbInRightBottomCorner(Thumb thumb, Canvas canvas)
        {
            thumb.SizeChanged += (sender, e) =>
            {
                Canvas.SetLeft(thumb, canvas.ActualWidth - thumb.ActualWidth);
                Canvas.SetTop(thumb, canvas.ActualHeight - thumb.ActualHeight);
            };

            canvas.SizeChanged += (sender, e) =>
            {
                Canvas.SetLeft(thumb, canvas.ActualWidth - thumb.ActualWidth);
                Canvas.SetTop(thumb, canvas.ActualHeight - thumb.ActualHeight);
            };
        }

        private void Tab_Closure(CancelEventArgs e, LayoutAnchorablePane pane, MenuItem dynamicItem)
        {
            
            MessageBoxButton b = MessageBoxButton.YesNoCancel;
            
            MessageBoxResult mb = MessageBox.Show("Вы уверены, что хотите закрыть вкладку?", "Закрытие вкладки", MessageBoxButton.YesNoCancel);
            
            if (MessageBoxResult.Yes == mb)
            {
                LayoutAnchorable la = pane.Children[0];
                ScrollViewer sv = (ScrollViewer)la.Content;
                Canvas c = (Canvas)sv.Content;
                DynDirSave(dynamicItem, c);
                FileDialogueSaver.Items.Remove(dynamicItem);
                Panel.Children.Remove(pane);
                if (FileDialogueSaver.Items.Count <= 0)
                {
                    MenuItemSave.IsHitTestVisible = false;
                    MenuItemSaveAs.IsHitTestVisible = false;
                }
                e.Cancel = false;
            }
            else if (MessageBoxResult.No == mb)
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }





            
        }

        private void MenuItem_NewFile_Click(object sender, RoutedEventArgs e)
        {
            CreateNewWindow();
        }

        private void MenuItem_OpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            ofd.Filter = "BMP or PNG(*.png or *.bmp)|*.png;*.bmp|PNG (*.png)|*.png|BMP (*.bmp)|*.bmp";

            if (ofd.ShowDialog() == true) 
            {
                foreach (string fileName in ofd.FileNames)
                {
                    Image con = CreateImage(fileName);
                    CreateNewWindow(con);
                    isSaved = true;
                    filePath = fileName;
                   /*if (System.IO.Path.GetFileName(fileName).Split(".")[System.IO.Path.GetFileName(fileName).Split(".").Length - 1].ToLower() == "png") Оставить на будующее, так как может пригодиться сортировка по файлам
                    {
                     

                    }
                    if (System.IO.Path.GetFileName(fileName).Split(".")[System.IO.Path.GetFileName(fileName).Split(".").Length - 1].ToLower() == "bmp")
                    {

                    }*/
                }
            }
        }

        private void DynDirSave(object sender, Canvas es)
        {
            isSaved = true;
            MenuItem MIT = (MenuItem)sender;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = MIT.Header.ToString(); // Default file name
            saveFileDialog.DefaultExt = ".jpg|.bmp"; // Default file extension
            saveFileDialog.Filter = "Image (.jpg)|*.jpg | (.bmp)|*.bmp"; // Filter files by extension

            ScaleTransform st = (ScaleTransform)es.RenderTransform;
            double x = st.ScaleX, y = st.ScaleY;
            st.ScaleX = 1;
            st.ScaleY = 1;
            es.RenderTransform = st;

            // Show save file dialog box
            Nullable<bool> result = saveFileDialog.ShowDialog();


            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                string filename = saveFileDialog.FileName;
                filePath = filename;
                //get the dimensions of the ink control

                int margin = (int)es.Margin.Left;
                int width = (int)es.ActualWidth - margin;
                int height = (int)es.ActualHeight - margin;


                //render ink to bitmap
                RenderTargetBitmap rtb =
                new RenderTargetBitmap(width, height, 96d, 96d, PixelFormats.Default);
                rtb.Render(es);

                using (FileStream fs = new FileStream(filename, FileMode.Create))
                {
                    BmpBitmapEncoder encoder = new BmpBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(rtb));
                    encoder.Save(fs);
                }
            }
            else
                isSaved = false;

            st.ScaleX = x;
            st.ScaleY = y;
            es.RenderTransform = st;
        }

        private void MenuItem_Save_Click(object sender, RoutedEventArgs e, Canvas st)
        {
            if (isSaved)
            {
                ScaleTransform st1 = (ScaleTransform)lastChosenCanvas.RenderTransform;
                double x = st1.ScaleX, y = st1.ScaleY;
                st1.ScaleY = 1;
                st1.ScaleX = 1;
                int margin = (int)lastChosenCanvas.Margin.Left;
                int width = (int)lastChosenCanvas.ActualWidth - margin;
                int height = (int)lastChosenCanvas.ActualHeight - margin;
                //render ink to bitmap
                RenderTargetBitmap rtb =
                new RenderTargetBitmap(width, height, 96d, 96d, PixelFormats.Default);
                rtb.Render(lastChosenCanvas);

                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {
                    BmpBitmapEncoder encoder = new BmpBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(rtb));
                    encoder.Save(fs);
                }
                lastChosenCanvas.RenderTransform = st1;
            }
            else
            {
                DynDirSave(sender, st);
            }
        }

        private void TextBox_NumsOnly_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (ChildrenAnchorable.Count > 0)
            {
                MessageBoxButton b = MessageBoxButton.YesNo;
                MessageBoxResult mb = MessageBox.Show("Вы уверены, что хотите закрыть приложение?", "Выход", b);
                if (MessageBoxResult.Yes == mb)
                {
                    e.Cancel = false;

                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Window1 aboutProgramm = new Window1();
            aboutProgramm.Owner = this;
            aboutProgramm.Show();

        }
    }
}

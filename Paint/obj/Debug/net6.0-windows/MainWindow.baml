   M S B A M L   `   `   `  ����@  <Paint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:PaintPaint  5   	       Paint.MainWindow   5   	   R NAvalonDock, Version=4.71.1.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f4W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ] YXceed.Wpf.Toolkit.NET5, Version=4.5.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  5   	   ` \Xceed.Wpf.AvalonDock.NET5, Version=4.5.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f4l	 hXceed.Wpf.AvalonDock.Themes.Aero.NET5, Version=4.5.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f4m
 iXceed.Wpf.AvalonDock.Themes.Metro.NET5, Version=4.5.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f4n jXceed.Wpf.AvalonDock.Themes.VS2010.NET5, Version=4.5.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f4C
avalondock,http://schemas.xceed.com/wpf/xaml/avalondock  	 
  5   	   %localclr-namespace:Paint  	 
  5   	   -   5          � Title$  Paint��5   	   $	��600��6   $
��1200��6$    � 	Resources 5         ButtonRadioButtonStyle �� 
TargetType �� BasedOn �� Triggers P� Property P� Value �� Property �� Value��%]  &	        7d��� 5   
   " 	�6/   8    5      P� 5       � {�$ False��6/   . �5      ��  � ��# � ��63   5      P� 5       �@d�	IsChecked$ FalseY�6/   . �5      ��  � ��# � ��63   5      P� 5       �@d�	IsChecked$
 TrueY�6/   . �5      ��  � �� �	#FF2F2FEA63   5      5      5      	 J� InputBindings	 5      �� 5   
   
 �� Gesture$
 CTRL+N��6B    �� Command 6   �� *MenuItem_NewFile_Gesture+6U   .��5#      �  � RowDefinitions 5$   
   �� 5%      $��30 �6   $��30��6(   �� 5&      $��30 �6   $��30��6(   �� 5'      $��* �6   �� 5(      $��20 �6   6*   .+�5-   
   )  #Xceed.Wpf.AvalonDock.DockingManager  $��2��6$      Theme 5/      -  'Xceed.Wpf.AvalonDock.Themes.VS2010Theme  51      6+      Layout. 55      ,  &Xceed.Wpf.AvalonDock.Layout.LayoutRoot     	RootPanel. 57      -  'Xceed.Wpf.AvalonDock.Layout.LayoutPanel  -   5        	Panel��59      5;      5=      �� 5?   
   $��1��6    �� Orientation$ 
Horizontal=�6"   
�������.+�5A      	� -   5        #���   5A       	� 	GroupName$ Instruments��6K   $��30��6c   $
��Auto��6n   
������ .��5B      �� $��\Images\Brush.png��6   $
��Fill=�63   5C      	� 5E      -   5        #���   5E      $ Instruments��6K   $��30��6c   $
��Auto��6n   
������ .��5F      �� $��\Images\Line.png��6   $
��Fill=�62   5G      	� 5I      -   5        #���   5I      $ Instruments��6K   $��30��6c   $
��Auto��6n   
������ .��5J      �� $��\Images\Ellipse.png��6   $
��Fill=�65   5K      	� 5M      -   5        #���   5M      $ Instruments��6K   $��30��6c   $
��Auto��6n   
������ .��5N      �� $��\Images\Star.png��6   $
��Fill=�62   5O      �� 5Q      -   5         ��Name$ TextBoxStarInput��5Q      $
��Auto��6e   $��40��6s   .��6   6�   	� 5S      -   5        #���   5S      $ Instruments��6K   $��30��6c   $
��Auto��6n   
������ .��5T      �� $��Images\Eraser.png��6   $
��Fill=�63   5U      �� 5W      -	   5        $��40��5W      $��Center=�6Q   .@�5X      ��  �� 
IsSelected$
 True��6   .��5Y      �� $��18(�6    161   5Z      �� 5\      .��5]      �� $��18(�6    361   5^      �� 5`      .��5a      �� $��18(�6    561   5b      �� 5d      .��5e      �� $��18(�6    861   5f      5g      #  Xceed.Wpf.Toolkit.ColorPicker  5i      -
   5        $��ChangeColor��5i      $	��120��6j      SelectedColor	 Black6v   6�   5j      �� 5l   
   ���#EFF4F9$��0��6*   $ 
Horizontal=�67   .+�5m      o� ���#EFF4F9.@�5n      m� $��Файл��6   $��30��6)   .@�5o      m� -   5        $��
Новый��5o      6.   m� 5p      -   5        $��Открыть��5p      60   m� 5q      -   5         m�Name$ MenuItemSave��5q      $��Сохранить��65   6J   m� 5r      -   5        $ MenuItemSaveAs��5r      $#��Сохранить как ...��67   .@�5s      o� -   5         o�Name$ FileDialogueSaver��5s      5u      5v      5w      5x      o� 5z      ���#EFF4F9.@�5{      m� $��Справка��6   $��30��6,   .@�5|      m� -   5        $��О программе...��5|      67   5}      5~      5�      �� 5�   
   
�������$��3��6,   .+�5�      �� ��6   ��  �� Path$ SynchronizedText�6A   5�      5�      5�      
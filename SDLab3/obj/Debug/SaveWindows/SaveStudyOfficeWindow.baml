   M S B A M L   `   `   `  ����A  =SDLab3, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:SDLab3.SaveWindowsSDLab3.SaveWindows  5   	   .    (SDLab3.SaveWindows.SaveStudyOfficeWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   2local clr-namespace:SDLab3.SaveWindows     5   	     � Title$]  WУправление записью о сотруднике учебного офиса��5   	   $	��175��6@   $	��320��6M    � 
ResizeMode$ NoResize=�5	   	   .��5
      �  � RowDefinitions 5   
   �� 5      $��25 �6   �� 5      $��35 �6   �� 5      $��35 �6   �� 5      .+�5   
   �� -   5         ��Name$ 	InfoLabel��5      $��0��6#   $V��PВведите данные о сотруднике учебного офиса:��60   ]� 5   
   $��1��6   $
��Auto��6!   .+�5      �� $,��&Инициалы сотрудника:��6   $��Center=�63   �� 5      -   5         ��Name$ WorkerInitials��5      $	��150��6.   $��Right=�6:   $��Center=�6V   $��
0, 0, 5, 0q�6x   5      ]� 5   
   $��2��6   $
��Auto��6!   .+�5      �� $!��Email сотрудника:��6   $��Center=�60   �� 5      -   5        $ Email��5      $	��150��6%   $��Right=�61   $��Center=�6M   $��
0, 0, 5, 0q�6o   5      �� 5   
   -   5        $��3��5      $��30��6   $��80��6*   $��Bottom=�65   $��Right=�6P   $��0, 0, 10, 10q�6l   $��Сохранить��6�   6�   5      5      
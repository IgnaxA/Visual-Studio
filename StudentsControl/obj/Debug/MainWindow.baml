   M S B A M L   `   `   `  ����J  FStudentsControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:StudentsControlStudentsControl  5   	   A%clr-namespace:StudentsControl.TeacherStudentsControl.Teacher  5   	        StudentsControl.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:StudentsControl     5   	   DlocalTeacherWindow%clr-namespace:StudentsControl.Teacher     5   	     � Title$%  Окно авторизации��5	   	   $	��400��6"   $	��600��6/    � 
ResizeMode$ NoResize=�6;    � 	Resources 5
      +   %StudentsControl.Teacher.TeacherWindow   TeacherWindwo��%   &	          5   
   6+   5      .��5      �  � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $
��1.8* �6   �� 5      $
��1.8* �6   �� 5      $
��1.4* �6   .+�5   
   �� $��0��6   .+�5      �� $��Авторизация��6   $��30(�6*   5      �� 5   
   $��1��6   $��Center=�6"   .+�5      �� -   5         ��Name$ LoginTextBox��5      $��40��6,   $��100 0 100 0q�68   $��Center=�6M   6�   5      �� 5   
   $��2��6   $��Center=�6"   .+�5      G� -   5         G�Name$ PasswordTextBox��5      $��40��63   $��100 0 100 0q�6?   $��Center=�6T   6v   5      �� 5    
   $��3��6   $��Center=�6"   .+�5!      �� -   5        $��
Войти��5!      $��40��6%   $��80��61   $��Center=�6=   6Z   5"      5$      5%      
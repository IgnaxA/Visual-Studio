   M S B A M L   `   `   `  ����J  FStudentsControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:StudentsControl.TeacherStudentsControl.Teacher  5   	   [2clr-namespace:StudentsControl.Teacher.UserControls$StudentsControl.Teacher.UserControls  5   	   +    %StudentsControl.Teacher.TeacherWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   7local%clr-namespace:StudentsControl.Teacher     5   	   OlocalUserControl2clr-namespace:StudentsControl.Teacher.UserControls     5   	     � Title$)  #Окно преподавателя��5	   	   $	��450��6$   $	��800��61   $	��450��6=   $	��800��6M    � 	Resources 5
      A   ;StudentsControl.Teacher.UserControls.StudentDatabaseControl   StudentsDatabaseControl?   9StudentsControl.Teacher.UserControls.ThemeDatabaseControl  ThemeDatabaseControlB   <StudentsControl.Teacher.UserControls.CalendarDatabaseControl  CalendarDatabaseControl��%W   &	        &	      &	 &       5   
   62     5   
   60     5   
   63   5      .��5      � -   5         �Name$ TeacherWindowGrid��5       � ColumnDefinitions 5   
   �� 5      $��89 �6   �� 5      $��8* �6    � RowDefinitions 5   
   �� 5      $��40 �6   �� 5      .+�5   
   ]� $��0��6   $��1��6!   
������ .+�5      ]� $
��Left=�6   .+�5      �� $ ��Размер шрифта:��6   $��Center=�61   �� 5      -   5         ��Name$ SetFontSize��5      $��Center=�60   $��40��6�   .@�5       �� $��11��6#   �� 5!       �� 
IsSelected . $��12��65   �� 5"      $��14��6#   �� 5#      $��16��6#   �� 5$      $��18��6#   �� 5%      $��20��6#   �� 5&      $��22��6#   �� 5'      $��26��6#   �� 5(      $��28��6#   �� 5)      $��36��6#   �� 5*      $��48��6#   �� 5+      $��56��6#   �� 5,      $��72��6#   5-      5.      ]� 5/      $��Right=�6   .+�50      �� -   5         ��Name$ LabelInitials��50      $��Center=�6/   �� 51      -   5        $��35��51      $��35��6$   
������ $��Center=�6\   $��0 0 10 0q�6w   .��52      �� $'��!\Images\QuitTeacherWindowView.png��6   $
��Fill=�6G   53      54      55      �� 57   
    �� Orientation$ Vertical=�6   $��1��6,   $��0��69   .+�58      �� -   5        $	��124��58      
������ .��59      �� $��\Images\BrigadesView.png��6   $
��Fill=�6:   5:      �� 5;      -   5        $	��124��5;      
������ .��5<      �� $��\Images\ThemeView.png��6   $
��Fill=�67   5=      �� 5?      -   5        $	��124��5?      
������ .��5@      �� $��\Images\CalendarView.png��6   $
��Fill=�6:   5A      5B      5G      5H      
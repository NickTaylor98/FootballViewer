   M S B A M L   `   `   `  ����F  BFootballApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:FootballAppFootballApp  5   	       FootballApp.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:FootballApp     5   	   -   5          � Icon$  /img/football.png��5   	    � Title$ 
Войти��5	   	   $��205.017��6   $��275.809��6(   
���P�� � 
ResizeMode$ NoResize=�5   	   .��5      � .+�5   
   �� -   5        $��Login��5      $
��Left=�6   $��23��6:   $��
109,36,0,0q�6F    �� TextWrapping$
 Wrap=�6Z   $	��Top=�6n   $��Center=�6�   $	��120��6�   G� 5   
   -   5        $��Password��5      $
��Left=�6&   $��23��6A   $��
109,73,0,0q�6M   $	��Top=�6a   $��Center=�6y   $	��120��6�   �� 5   
   $��
Логин��6   $
��Left=�6    $	��Top=�6;   $��	43,34,0,0q�6S   �� 5   
   $��Пароль��6   $
��Left=�6!   $	��Top=�6<   $��	43,71,0,0q�6T   �� 5   
   $��Center=�6   $��26��6-   $��	1,138,1,0q�69   $	��Top=�6L   $	��268��6d   $��Center=�6p   $��Center=�6�   .��5      �� -   5        $��	Underline{�5      $��Регистрация��64   ��5      �� 5       �� 
TargetType" ��6   .�5      ��  �� Property � �� �� Value
 ��  �67    �� Triggers 5      ]� 5       ]� 
Conditions 5   "   �� 5   &   	 �� Property	 �@J�IsMouseOver
 �� Value
 . 6O   /�5   "   �� 5   &    �@�Cursor$
 Hand��6G   �� 5   &    � ��
 �  ��6C   5       5"      5$      5%      �� 5&   
   -   5        $��Enter��5&      $��
Войти��6   $��Center=�6.   $��98,122,98,0q�6K   $	��Top=�6`   $��74��6x   6�   �� 5'   
   -   5        $��Capslock��5'      $�� ��6    $��Center=�6+   $	��Top=�6H   $��0,96,0,0q�6`   5(      5)      
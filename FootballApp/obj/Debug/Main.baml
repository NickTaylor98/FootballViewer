   M S B A M L   `   `   `  ����F  BFootballApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:FootballAppFootballApp  5   	       FootballApp.Main   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:FootballApp     5   	   -   5        $
��wind��5   	     � Icon$  /img/football.png��5	   	   ��5
   	   �� *&$SystemParameters.PrimaryScreenHeight+��5   	   �� *%#SystemParameters.PrimaryScreenWidth+.��5      �  � ColumnDefinitions 5   
   �� 5      $
��157* �6   �� 5      $
��279* �6   �� 5      $
��162* �6   �� 5      $
��162* �6    � RowDefinitions 5   
   �� 5      $	��31* �6   �� 5      $	��81* �6   �� 5      $	��67* �6   �� 5      $
��325* �6   .+�5   
   � $��4��6    5      �� 5      �� 5      �� 5      .+�5      �  5       �� 5!      $	��31* �6$   �� 5"      $
��137* �6$   .+�5$      �� $��Сегодня��6   $��0��6*   $��0��6:   $��Center=�6G   $
��Bold"�6k   $��15(�6}   �� 5%      -   5        $��Today��5%   '   $��1��6    �� SelectionMode$ Single=�64   6K   5&      � 5'      $��0��6   $��1��6#    5(      �� 5)      $	��31* �6$   �� 5*      $
��137* �6$   .+�5,      �� $��
Вчера��6   $��0��6(   $��0��68   $��Center=�6E   $
��Bold"�6i   $��15(�6{   �� 5-      -   5        $��	Yesterday��5-      $��1��6+   $ Single=�68   6O   5.      � 5/      $��0��6   $��2��6#    50      �� 51      $	��31* �6$   �� 52      $
��137* �6$   .+�54      �� $��Завтра��6   $��0��6)   $��0��69   $��Center=�6F   $
��Bold"�6j   $��15(�6|   �� 55      -   5        $��Tomorrow��55      $��1��6*   $ Single=�67   6N   56      57      �� 58   
   $��1��6   $��0��6    $+��%Обозреватель матчей��6-   $��Center=�6K   $
��Bold"�6o   $��15(�6�   �� 59   
   $��1��6   $��1��6%   $��3��62   .+�5:      �� -   5        $��Matches��5:      $y�Hidden=�6%   69   5;      �� 5<   
   -   5        $��Squad1��5<      $��2��6   $��15(�6.   $
��Bold"�6<   �� 5=   
   -   5        $��Squad2��5=      $��3��6   $��15(�6.   $
��Bold"�6<   � 5>   
   $��1��6   $��2��6   $��2��6,    5?      �� 5@      �� 5A      �� 5B      .+�5D      �� -   5        $��ImageFirstTeam��5D      $��0��6*   �� 5E      -	   5        $��ImageSecondTeam��5E      $��2��6+   �� 5F      -
   5        $��
FirstGoals��5F      $��0��6&   $
��Bold"�66   $��12(�5G      $
��Left=�5H      � 5I      $��1��6    5J      �� 5K      $	��25* �6$   �� 5L      $	��42* �6$   .+�5N      (� -   5        $��Viewbox��5N   '   $��1��6   .��5O      �� -   5        $��Goals��5O      5P      �� 5Q      -   5        $��Status��5Q      $
��Bold"�6&   $��Times New Roman)�68   $��12(�5R      $	��Top=�5S      $��Center=�67   5T      �� 5U      -   5        $��SecondGoals��5U      $��2��6'   $
��Bold"�67   $��12(�5V      $
��Left=�5W      5X      � 5Y   
   $��2��6   $��2��6   $��2��6-   $��2��6>    5Z      �� 5[      �� 5\       5^      �� 5_      $	��28* �6    �� 5`      $	��28* �6    �� 5a      $
��222* �6    �� 5b      $	��28* �6    �� 5c      $
��101* �6    .+�5e      �� -   5        $��Trainer1��5e      $��13(�6$   (� 5f      -   5        $��	FirstView��5f   3   $��2��6   $��0��6#   .��5g      �� -   5        $��
FirstSquad��5g      $y�Hidden=�6,   $
��Auto=�6@   $��Disabled=�5h      5i      �� 5j      -   5        $��	BestSquad��5j      $��1��6%   $��2��62   $��Center=�6F   $��Center=�6j   �� 5k      -   5        $��AnotherPlayers��5k      $��3��6*   $��2��67   $��Center=�6K   $��Center=�6o   �� 5l      -   5        $��Trainer2��5l      $��13(�6$   $��1��62   (� 5m      -   5        $��
SecondView��5m   3   $��1��6   $��2��6&   .��5n      �� -   5        $��SecondSquad��5n      $y�Hidden=�6-   $
��Auto=�6A   $��Disabled=�5o      5w      5x      (� 5y      $��4��6   .��5z      �� -   5        $��AnotherFirstSquad��5z      $y�Hidden=�63   $
��Auto=�6G   $��Disabled=�5{      5|      (� 5}      $��4��6   $��1��6#   .��5~      �� -   5        $��AnotherSecondSquad��5~      $y�Hidden=�64   $
��Auto=�6H   $��Disabled=�5      5�      5�      5�      5�      
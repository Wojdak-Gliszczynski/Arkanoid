   M S B A M L   `   `   `  ����C  ?Arkanoid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:ArkanoidArkanoid  5   	       Arkanoid.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:Arkanoid     5   	     � Title$  Arkanoid��5   	   $	��813��6   $	��631��6&    � 
ResizeMode$ NoResize=�63   .��5	      � -   5         �Name$ menuGrid��5	      $	��600��6   $	��800��6*   .+�5   
   �� -   5         ��Name$ menuBackground��5      $��0,0,0,0q�6(   $	��Top=�69   $$��./Graphics/background_menu.png��6Q    J� Opacity$
 0.65Q�6y   �� 5   
   -   5        $ 
imageTitle��5      $��91��6$   $��262,64,263,0q�60   $	��Top=�6F   $��./Graphics/title.png��6^   �� 5   
   -   5         ��Name$ buttonStartGame��5      $��
Start Game��6*   $��Center=�6?   $��350,220,350,348q�6Z   $��32��6s   6   �� 5   
   -   5        $ buttonScoreboard��5      $��High scores��6+   $��Center=�6A   $��Center=�6^   $	��100��6y   $��0q�6�   $��32��6�   6�   �� 5   
   -   5        $ 
buttonQuit��5      $��	Exit game��6%   $��Center=�69   $��Center=�6V   $	��100��6q   $��350,128,350,0q�6}   $��32��6�   6�   � 5   
   -   5        $ gameGrid��5      $	��600��6!   $	��800��6.   � 5   
   -   5        $ menuInGameGrid��5      $	��150��6'   $	��250��64   $��Center=�6@   $��Center=�6[   $y�Hidden=�6x   .+�5      �� ���	#FFF4F4F5
�������$
��Left=�67   $	��150��6R   $	��Top=�6_   $	��250��6w   �� 5      -	   5        $ buttonIGGBackToGame��5      $��Back to game��62   $��Center=�6I   $��Center=�6f   $	��100��6�   $��0,0,0,64q�6�   $��32��6�   6�   �� 5      -
   5        $ buttonIGGBackToMainMenu��5      $��	Main Menu��66   $��Center=�6J   $��Center=�6g   $	��100��6�   $��0,64,0,0q�6�   $��32��6�   6�   5      5      5      
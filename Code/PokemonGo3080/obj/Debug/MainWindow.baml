   M S B A M L   `   `   `  ����H  DPokemonGo3080, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:PokemonGo3080PokemonGo3080  5   	       PokemonGo3080.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:PokemonGo3080     5   	     � Title$  
MainWindow��5   	   $	��450��6   $	��800��6)   .��5	      �  � ColumnDefinitions 5
   
   �� 5      $
��152* �6   �� 5      $	��408 �6   �� 5      $	��79* �6   �� 5      $
��153* �6   ��5   
   �� 5       �� EndPoint$ 0.5,1&�6"    �� 
StartPoint$ 0.5,0&�63   .��5      �  � Color$ 	#FF00D1FF��6    � Offset$ 0Q�61   � 5      $ 	#FFE0A5F5��6   $ 1Q�61   5      .+�5   
   �� -   5         ��Name$ pokemonBlock��5      $
��Left=�6*   $
��Wrap=�6E   $	��Top=�6Y   $��367.333,13,0,0q�6q    J� RenderTransformOrigin$ 5.131,0.922&�6�   $��32��6�   $	��122��6�   $��24(�6�   $��Segoe Script)�6�   $��Pokemon��6�   $��1��6�   $��3��6  �� 5   
   -   5         ��Name$ 
pokemonBox��5      $
��Left=�6'   $��Bottom=�6B   $	��120��6]   $��367.333,0,0,331.667q�6i   $��1��6�   $��2��6�   6�   �� 5   
   -   5        $ 	itemBlock��5      $
��Left=�6'   $
��Wrap=�6B   $	��Top=�6V   $��	38,13,0,0q�6n   $ 5.131,0.922&�6�   $��32��6�   $��63��6�   $��24(�6�   $��Segoe Script)�6�   $
��Item��6�   $��3��6�   �� 5   
   -   5        $ itemBox��5      $
��Left=�6$   $��Bottom=�6?   $	��120��6Z   $��10,0,0,331.667q�6f   $��3��6~   6�   �� 5   
   -   5        $ message��5      $
��Left=�6%   $
��Wrap=�6@   $	��Top=�6T   $��
41,199,0,0q�6l   $ 5.131,0.922&�6�   $	��152��6�   $	��477��6�   $��24(�6�   $��Segoe Script)�6�   $��2��6�   �� 6�   $�This is a text block��6�   �� 6  $� ��6   �� 6+  �� 67  $#�that print out battle message��6;  6c  �� 5   
   -   5        	 ��Name$	 move0_button��5      $��Move 0��6'   $
��Left=�68   $	��Top=�6S   $	��140��6k   $��
41,356,0,0q�6w   $��2��6�   $��39��6�   �� 5   
   -   5        $	 move1_button��5      $��Move 1��6'   $
��Left=�68   $	��Top=�6S   $	��140��6k   $��46.333,356,0,0q�6w   $��1��6�   $��39��6�   �� 5   
   -   5        $	 item_button��5      $
��Heal��6&   $
��Left=�65   $	��Top=�6P   $	��140��6h   $��206.333,356,0,0q�6t   $��1��6�   $��39��6�   �� 5   
   -	   5        $	 start_button��5      $��Start��6'   $
��Left=�67   $	��Top=�6R   $��75��6j   $��2��6u   $��12.333,361,0,0q�6�   $��2��6�   �� 5   
   -
   5        $ selectedPokemonBlock��5      $
��Left=�62   $
��Wrap=�6M   $	��Top=�6a   $��387.333,121,0,0q�6y   $ 5.131,0.922&�6�   $��32��6�   $	��230��6�   $��24(�6�   $��Segoe Script)�6�   $��1��6�   $��3��6  �� 6  $�	Selected ��6  �� 62  $�Pokemon��66  6H  �� 5    
   -   5        
 ��Name$
 selectedPokemonBox��5       $
��Left=�6.   $	��156��6I   $	��Top=�6V   $	��170��6n   $��2��6z   $��2��6�   $��10.333,158,0,0q�6�   �� 5!   
   -   5        $	 
add_button��5!      $	��Add��6%   $
��Left=�63   $	��Top=�6N   $��75��6f   $��2��6q   $��12.333,334,0,0q�6�   $��2��6�   �� 5"   
   -   5        $	 remove_button��5"      $��Remove��6(   $
��Left=�69   $	��Top=�6T   $��75��6l   $��3��6w   $��
23,334,0,0q�6�   6�   �� 5#   
   -   5        $	 
run_button��5#      $	��Run��6%   $
��Left=�63   $	��Top=�6N   $��75��6f   $��3��6q   $��
23,361,0,0q�6�   6�   �� 5$   
   -   5         ��Name$ player_pkm_image��5$      $
��Left=�6*   $	��100��6E   $	��Top=�6R   $	��100��6j   $��	81,43,0,0q�6v   $��2��6�   �� 5%   
   -   5        $ master_pkm_image��5%      $
��Left=�6*   $	��100��6E   $	��Top=�6R   $	��100��6j   $��1��6v   $��176.333,43,0,0q�6�   � 5&   
   -   5         �Name$ player_pkm_hp��5&      $
��Left=�6-   $��10��6H   $	��Top=�6T   $	��100��6l   $��2��6x   $��
81,158,0,0q�6�   � 5'   
   -   5        $ master_pkm_hp��5'      $
��Left=�6-   $��10��6H   $	��Top=�6T   $	��100��6l   $��176.333,158,0,0q�6x   $��1��6�   �� 5(   
   -   5        $ gym_master_pokemon��5(      $
��Left=�60   $
��Wrap=�6K   $	��Top=�6_   $��	86,57,0,0q�6w   $ 5.131,0.922&�6�   $��32��6�   $	��310��6�   $��24(�6�   $��Segoe Script)�6�   $��2��6�   �� 6  $
�Gym ��6  �� 6  $
�Lead��6  �� 6'  $�er's Pokemon:��6+  6C  �� 5)   
   -   5        $ master_pkm_1��5)      $
��Left=�6&   $	��100��6A   $	��Top=�6N   $	��100��6f   $��	86,94,0,0q�6r   $��2��6�   �� 5*   
   -   5        $ master_pkm_2��5*      $
��Left=�6&   $	��100��6A   $	��Top=�6N   $	��100��6f   $��46.333,94,0,0q�6r   $��1��6�   �� 5+   
   -   5        $ master_pkm_3��5+      $
��Left=�6&   $	��100��6A   $	��Top=�6N   $	��100��6f   $��176.333,94,0,0q�6r   $��1��6�   �� 5,   
   -   5         ��Name$ 
mainCanvas��5,      $
��Left=�6%   $	��385��6@   $	��Top=�6M   $	��385��6e   $ 0.5,0.5&�6q   $��	39,33,0,0q�6�   $��2��6�   z�5-      V� 5.      .~�5/      �� �� 50      �� 51       �� Angle$ -0.205Q�6&   T� 52      53      55      �� 56   
   -   5         ��Name$ 	indicator��56      
�������$
��Left=�6;   $��43��6V   
���   �$	��Top=�6q   $��7��6�   $��	56,22,0,0q�6�   $��1��6�   �� 57   
   -   5        $	 spin_button��57      $��Throw Ball!��6&   $
��Left=�6<   $��57��6W   $��329.333,189,0,0q�6c   $	��Top=�6|   $	��181��6�   ���	#FFC581FD$��28(�6�   $��Monotype Corsiva)�6�   $��3��6�   $��1��6  �� 58   
   -   5        $	 stop_button��58      $��Stop!!!��6&   $
��Left=�68   $��56��6S   $��329.333,258,0,0q�6_   $	��Top=�6x   $	��181��6�   ���	#FFC581FD$��28(�6�   $��Monotype Corsiva)�6�   $��1��6�   $ -0.519,0.631&�6	  $��3��6.  �� 59   
   -   5        $ status��59      $
��Left=�6$   $
��Wrap=�6?   $	��Top=�6S   $��303.333,101,0,0q�6k   $ 5.131,0.922&�6�   $��32��6�   $	��314��6�   $��24(�6�   $��Segoe Script)�6�   $��This is a text block��6�   $��1��6  $��3��6  �� 5:   
   -   5        $ 	image_box��5:      $
��Left=�6#   $	��100��6>   $	��Top=�6K   $	��100��6c   $��3��6o   $��
30,142,0,0q�6   �� 5;   
   -   5        $ ColorExplanation��5;      $
��Left=�6/   $
��Wrap=�6J   $	��Top=�6^   $��3��6v   $��
38,260,0,0q�6�   $��56��6�   $��92��6�   �� 6�   $�Purple: Success��6�   �� 6�   �� 6�   $�Blue: Leave ball��6�   �� 6�   �� 6  $�Grey: Escaped��6	  6!  �� 5<   
   -   5         ��Name$ Pokemon_Name��5<      $
��Left=�6(   $��43��6C    �� TextWrapping$
 Wrap=�6O   $��Pokemon Name��6c   $	��Top=�6w   $	��217��6�   $��1��6�   $��92.333,22,0,0q�6�   $��24(�6�   $��Segoe Script)�6�   .��6  ��5=      �� 5>       �� Color$ White��6"    �� Opacity$ 0Q�60   .��5?   "   5@      �� 5A   
   -   5        $ 	basicInfo��5A      $
��Left=�6'   $
��Wrap=�6B   $	��Top=�6V   $��92.333,74,0,0q�6n   $ 5.131,0.922&�6�   $	��115��6�   $	��251��6�   $��24(�6�   $��Segoe Script)�6�   $��1��6�   �� 6�   $�Species: Metamon��6�   �� 6  �� 6%  $�
Level: 100��6)  �� 6=  �� 6I  $�Type: Normal��6M  6d  �� 5B   
   -    5        $ pokemonStat��5B      $
��Left=�6)   $
��Wrap=�6D   $	��Top=�6X   $��
41,199,0,0q�6p   $ 5.131,0.922&�6�   $	��152��6�   $	��199��6�   $��24(�6�   $��Segoe Script)�6�   $��2��6�   �� 6�   $	�HP:��6  �� 6  $� ��6  �� 6  $�123/123��6"  �� 63  �� 6?  $�
Attack: 56��6C  �� 6W  �� 6c  $�Defence: 15��6g  �� 6|  �� 6�  $�	Speed: 90��6�  6�  �� 5C   
   -!   5        $ pokemonMove��5C      $
��Left=�6)   $
��Wrap=�6D   $	��Top=�6X   $��92.333,199,0,0q�6p   $ 5.131,0.922&�6�   $	��152��6�   $	��385��6�   $��24(�6�   $��Segoe Script)�6�   $��1��6�   $��2��6�   �� 6  $�Move 1: XXX��6  �� 6+  �� 67  $�Power: 100 Accuracy: 100��6;  �� 6]  �� 6i  $�Move 2: YYY��6m  �� 6�  �� 6�  $�Power: 120 Accuracy: 80��6�  6�  �� 5D   
   -"   5        $	 lv_up_Button��5D      $��Level up��6'   $
��Left=�6:   $	��Top=�6U   $	��140��6m   $��
41,356,0,0q�6y   $��2��6�   $��39��6�   6�   �� 5E   
   -#   5        $	 evolve_Button��5E      $��Evolve��6(   $
��Left=�69   $	��Top=�6T   $	��140��6l   $��46.333,356,0,0q�6x   $��39��6�   $��1��6�   �� 5F   
   -$   5        $	 release_button��5F      $��Release��6)   $
��Left=�6;   $	��Top=�6V   $��75��6n   $��3��6y   $��
23,336,0,0q�6�   6�   �� 5G   
   -%   5        $ TrainerCanvas��5G      $��4��6(   $��0,-1,0.333,0.667q�6<   ��5H      �� 5I       �� ImageSource$ map.png��6   .+�5K      �� $
��Left=�6   $	��100��6/   $	��Top=�6<   $	��100��6T   �� 5L      -&   5        $ GymBattleBlock��5L      $
��Left=�61   $��35��6L   $
��Wrap=�6X   $	��Top=�6l   $��35��6�    �� TextAlignment$ Center=�6�    �� Left$	 185��6�   
 �� Top$	 234��6�   �� 6�   $	�Gym��6�   �� 6�   �� 6�   $�Battle��6�   6�   �� 5M      -'   5        $ GetItemBlock��5M      $
��Left=�6/   $��33��6J   $
��Wrap=�6V   $��Get Item��6j   $	��Top=�6z   $��54��6�   $ Center=�6�   $	 187��6�   $	 360��6�   �� 5N      -(   5        $ CatchPokemonBlock��5N      $��Center=�64   $��42��6Q   $
��Wrap=�6]   $��Catch Pokemon��6q   $	��Top=�6�   $��67��6�   $ Center=�6�   $	 153��6�   $	 112��6�   �� 5O      -)   5        $	 ViewPokemonButton��5O      $��View My 
Pokemon��61   $
��Left=�6U   $��72��6p   $	��Top=�6|   $��95��6�   ���	#FF996BEA��. $	 676��6�   $	 335��6  $��Segoe Script)�6   $��15(�6:  �� 5P      -*   5        $ Trainer��5P      $��25��6*   $	 603��66   $	 345��6H   $��25��6Y   $ 1.246,0.509&�6d   ��5Q      �� 5R      $ player_icon.png��6!   5T      5U      5V      5W      
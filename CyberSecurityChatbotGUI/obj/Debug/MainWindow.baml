   M S B A M L   `   `   `  ����R  NCyberSecurityChatbotGUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:CyberSecurityChatbotGUICyberSecurityChatbotGUI  5   	   (    "CyberSecurityChatbotGUI.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   7local%clr-namespace:CyberSecurityChatbotGUI     5   	     � Title$  
MainWindow��5   	   $	��450��6   $	��800��6)   .��5	      � $��15q�6    � RowDefinitions 5
   
   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� -   5         ��Name$ ConversationDisplay��5      $��0��63    �� 
IsReadOnly . " �� VerticalScrollBarVisibility$
 Auto=�6R   $��14(�6u   $��Segoe UI)�6�   ���	#FFEAEAEA$��5q�6�   � 5   
   $��1��6   $��0,10,0,0q�6    � ColumnDefinitions 5      �� 5      $��* �6#   �� 5      $
��Auto �6#   .+�5      �� -   5         ��Name$ UserInputBox��5      $��0��6,   $��14(�6<   $��Center=�6i   $��5q�6�   �� 5      -   5         ��Name$ 
SendButton��5      $��1��6)   $
��Send��69   $��10,0,0,0q�6H   $
��20,8q�6Z   ���	#FF007ACC
�������$
��Bold"�6�   5      5      5      
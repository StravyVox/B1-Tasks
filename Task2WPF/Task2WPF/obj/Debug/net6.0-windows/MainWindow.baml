   M S B A M L   `   `   `  ����C  ?Task2WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:Task2WPFTask2WPF  5   	       Task2WPF.MainWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:Task2WPF     5   	     � Title$  
MainWindow��5   	   $	��450��6   $	��800��6)   .��5	      �  � ColumnDefinitions 5
   
   �� 5      $	��62* �6   �� 5      $	��79* �6   �� 5      $
��335* �6   �� 5      $
��137* �6   �� 5      $
��125* �6   �� 5      $	��62* �6    � RowDefinitions 5   
   �� 5      $��40 �6   �� 5      $��40 �6   �� 5      $��40 �6   �� 5      $	��250 �6   �� 5      $��50 �6   .+�5   
   �� $
��Left=�6   $��Center=�6/   $
��Bold"�6J   $��16��6\   $	��116��6h   $��3��6t   $��29,0,0,0q�6�   Database string:6�   �� 5   
   -   5         ��Name$ SQLConString��5      $��0��6(   $��2��65    �� TextWrapping$
 Wrap=�6E   $��20��6Y   $��10(�6�   ��6e   �� *SQLConnectionString+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�6�   �� 5   
   -   5         ��Name$ Connect��5   .   $��0��6   $��3��6   $��20��6?   $	��120��6K   ��6W   �� *ConnectToDataBase+��6}   ��  �� ElementName$ SQLConString��	 �� Path$
	 Text�.��6�   Connect to DB6�   �� 5    
   $��1��6   $��1��6!   $
��Left=�61   $��Center=�6L   $
��Bold"�6g   $��16��6y   $��17,0,0,0q�6�   $��68��6�   $��2��6�   File:6�   �� 5!   
   -   5        $
 Path��5!      $��1��6    $��2��6-   $
 Wrap=�6=   $��20��6Q   ��6]   �� *
FilePath+$ TwoWay=�$ PropertyChanged=��� 5"   
   $��2��6   $��2��6!   $
��Left=�61   $	��Top=�6L   $
��Bold"�6d   $��16��6v   $	��131��6�   $��0,15,0,0q�6�   Table header row start:6�   �� 5#   
   -   5        $ TableHeader��5#      $��2��6'   $��2��67   $��30��6D   $��30��6O   $��135,0,150,0q�6[   $��20(�6�   ��6p   �� *TableHeader+$ TwoWay=�$ PropertyChanged=�6�   &   System.Windows.Controls.DataGrid  5%   
   -   5        
  Name$
 DataGridInfo��5%         AutoGenerateColumns$
 True��6*   $��3��6�   $��1��6�   $��4��6�   ��6E   �� *	DataTable+$ PropertyChanged=�6�   �� 5'   
   $��4��6   $��1��6"   $��50��62   $
��Bold"�6=   $��15(�6O   $��Center=�6]   $
��Left=�6x   $��20��6�   	Result:6�   �� 5(   
   -   5         ��Name$ GeneratedLine��5(      $��2��6+   $��4��6;   $��24,15,24,15q�6�   ��6H   �� *Result+$ PropertyChanged=�6�   �� 5*   
   -   5        $ GenerateFiles��5*   .   $��1��6   $��3��6   $��20��6E   $	��120��6Q   ��6]   �� *
SelectFile+{�6|   �� *ConnectedToDataBase+$ PropertyChanged=�.��6�   Select File6�   �� 5+   
   -   5        $ ConvertFile��5+   .   $��2��6   $��3��6   $��20��6D   $	��120��6P   ��6\   �� *TestCommand+{�6|   �� *
LoadedFile+$ PropertyChanged=�.��6�   Load File to DB6�   �� 5,   
   -	   5        $ 	CopyToSql��5,   .   $��4��6   $��3��6   $��	2,15,3,15q�6B   ��6U   �� *GetAmountOfFiles+{�6z   �� *ConnectedToDataBase+$ PropertyChanged=�.��6�   Get amount of files6�   �� 5-   
   -
   5        $ 
GetFromSQL��5-   .   $��4��6   $��4��6   $��20��6C   $	��100��6O   ��6[   �� *ShowDataFromDBByFile+��6�   �� $
 Path��$
	 Text�{�6�   �� *ConnectedToDataBase+$ PropertyChanged=�.��6  Get File from DB6  �� 5.   
   -   5        $ GenerateFiles_Copy��5.   .   $��1��6   $��4��6   $��20��6J   $	��100��6V   $��	Load File��6�   ��6b   �� *LoadInfoFromFile+6�   �� 5/   
   -   5        $ CopyToSql_Copy��5/   .   $��4��6   $��3��6   $��	2,15,3,15q�6G   $��Get amount of files��6   ��6Z   �� *GetAmountOfFiles+6   51      52      
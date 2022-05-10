# WinUI3_CustomCaption

Test custom Caption in WinUI 3 on Windows 10 21H1 (with a Manifest)
by copying and updating styles from generic.xaml :
 
 >  <Style x:Key="WindowCaptionButton" TargetType="Button">
  
 > <Style TargetType="ContentControl" x:Key="WindowChromeStyle">
 

   
 (colors, size, CornerRadius to make rounded System Buttons)
   
 
and a scrolling text for testing, but not very smooth like with Direct2D...
   
   
   ![WinUI3_CustomTitleBar](https://user-images.githubusercontent.com/22345506/167635812-8b930fea-66ee-4e35-9575-5fa87a9b6d08.jpg)

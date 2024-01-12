## About 

Enhanced controls for Wpf. The Border has BoxShadow like as Web css3. Some controls that can adjust the spacing such as WrapPanel and StackPanel.

Supporting .NET Framework 4.0 and greater, .NET Core 3.0 and greater(on Windows)

## How to use
1. Add nuget package to your project:
    > if you use .net cli, execute this:
    ```shell
    dotnet add package Yumikou.Wpf.Controls
    ```
    > if you use vs package-manager, execute this:
    ```shell
    Install-Package Yumikou.Wpf.Controls
    ```
2. Add namespace to your  `.xaml` file:
   ```xaml
   xmlns:ymk="clr-namespace:Yumikou.Wpf.Controls;assembly=Yumikou.Wpf.Controls"
   ```
3. The control use cases:
   #### Border with BoxShadow
   ```xaml
    <ymk:Border Grid.Row="1" Width="100" Height="100" Background="Transparent" BorderThickness="1" BorderBrush="DarkGreen" CornerRadius="30">
        <ymk:Border.BoxShadows>
            <ymk:BoxShadow BlurRadius="0" SpreadRadius="10" Brush="#6600ff00" OffsetX="0" OffsetY="0"></ymk:BoxShadow>
            <ymk:BoxShadow BlurRadius="20" SpreadRadius="5" Brush="#88ff0000" OffsetX="15" OffsetY="15"></ymk:BoxShadow>
        </ymk:Border.BoxShadows>
    </ymk:Border>
   ```
   ![Border with BoxShadow](https://raw.githubusercontent.com/yumikou/Yumikou.Wpf.Controls/master/docs/Border.png)

   #### WrapPanel can adjust the spacing
   ```xaml
    <ymk:WrapPanel HorizontalSpacing="20" VerticalSpacing="10">
        <Button>Button</Button>
        <Button>Button</Button>
        ...
    </ymk:WrapPanel>
   ```
   ![WrapPanel can adjust the spacing](https://raw.githubusercontent.com/yumikou/Yumikou.Wpf.Controls/master/docs/WrapPanel.png)

   #### StackPanel can adjust the spacing
   ```xaml
    <ymk:StackPanel Spacing="10" Orientation="Horizontal" VerticalAlignment="Top">
        <Button>Button</Button>
        <Button>Button</Button>
        ...            
    </ymk:StackPanel>
   ```
   ![StackPanel can adjust the spacing](https://raw.githubusercontent.com/yumikou/Yumikou.Wpf.Controls/master/docs/StackPanel.png)
## TODO
   - Blur effect of inset BoxShadow
   - Grid has lines
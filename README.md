# How to create and dynamically update target line for WPF Chart
This article provides a detailed walkthrough on how to create and dynamically update target line in [WPF Chart](https://www.syncfusion.com/wpf-controls/charts).

The [SfChart](https://help.syncfusion.com/cr/wpf/Syncfusion.UI.Xaml.Charts.SfChart.html) includes support for [Annotations](https://help.syncfusion.com/cr/wpf/Syncfusion.UI.Xaml.Charts.SfChart.html#Syncfusion_UI_Xaml_Charts_SfChart_Annotations), enabling the addition of various types of annotations to enhance chart visualization. Using [HorizontalLineAnnotation](https://help.syncfusion.com/cr/wpf/Syncfusion.UI.Xaml.Charts.HorizontalLineAnnotation.html), you can create and dynamically adjust the target line.

The Horizontal Line Annotation includes following property:
* [Y1](https://help.syncfusion.com/cr/wpf/Syncfusion.UI.Xaml.Charts.Annotation.html#Syncfusion_UI_Xaml_Charts_Annotation_Y1) - Represents the Y1 Coordinate of the horizontal line Annotation.
* [Stroke](https://help.syncfusion.com/cr/wpf/Syncfusion.UI.Xaml.Charts.ShapeAnnotation.html#Syncfusion_UI_Xaml_Charts_ShapeAnnotation_Stroke) - Represents the brush for the horizontal line annotation outline.
* [StrokeThickness](https://help.syncfusion.com/cr/wpf/Syncfusion.UI.Xaml.Charts.ShapeAnnotation.html#Syncfusion_UI_Xaml_Charts_ShapeAnnotation_StrokeThickness) - Represents the thickness of the horizontal line annotation outline.
* [StrokeDashArray](https://help.syncfusion.com/cr/wpf/Syncfusion.UI.Xaml.Charts.ShapeAnnotation.html#Syncfusion_UI_Xaml_Charts_ShapeAnnotation_StrokeDashArray) - Represents the DashArray of the horizontal line annotation stroke.
* [Text](https://help.syncfusion.com/cr/wpf/Syncfusion.UI.Xaml.Charts.Annotation.html#Syncfusion_UI_Xaml_Charts_Annotation_Text) - Gets or sets the description text for horizontal line Annotation.

Learn step-by-step instructions and gain insights to create and dynamically update the target line.

**Step 1:** The layout is created using a grid with two columns.

**XAML**

 ```xml
<Grid>

    <Grid.ColumnDefinitions>
        <ColumnDefinition Width="*"></ColumnDefinition>
        <ColumnDefinition Width="200"></ColumnDefinition>
    </Grid.ColumnDefinitions>

</Grid> 
 ```
 
**Step 2:** In first column of grid layout, initialize the [SfChart](https://help.syncfusion.com/wpf/charts/getting-started)and add the axes and series as shown below.

**XAML**
 
 ```xml
<chart:SfChart Grid.Column="0">

    <chart:SfChart.PrimaryAxis>
        <chart:CategoryAxis EdgeLabelsDrawingMode="Fit" ShowGridLines="False" Header="Months"/>
    </chart:SfChart.PrimaryAxis>

    <chart:SfChart.SecondaryAxis>
        <chart:NumericalAxis x:Name="Y_Axis" Minimum="0" Maximum="20000" Interval="5000" ShowGridLines="False" Header="Revenue" LabelFormat="'$'0" PlotOffsetEnd="30"/>
    </chart:SfChart.SecondaryAxis>

    <chart:ColumnSeries ItemsSource="{Binding Data}"
                    XBindingPath="Months"
                    YBindingPath="Revenue"
                    Palette="Custom"
                    Opacity="0.7">
        <chart:ColumnSeries.ColorModel>
            <chart:ChartColorModel>
                <chart:ChartColorModel.CustomBrushes>
                    ......
                </chart:ChartColorModel.CustomBrushes>
            </chart:ChartColorModel>
        </chart:ColumnSeries.ColorModel>
    </chart:ColumnSeries>

</chart:SfChart> 
 ```
 
**Step 3:** The [HorizontalLineAnnotation](https://help.syncfusion.com/wpf/charts/annotations#vertical-and-horizontal-line-annotation) is initialized within the [Annotations](https://help.syncfusion.com/wpf/charts/annotations) collection of the [SfChart](https://help.syncfusion.com/wpf/charts/getting-started) to mark a dynamic target value on the Y-axis. The Y1 property is data-bound to the ViewModel, allowing the target line to adjust dynamically when the value changes.


**XAML**
 
 ```xml
<chart:SfChart Grid.Column="0">
   .....
    <chart:SfChart.Annotations>
        <chart:HorizontalLineAnnotation Y1="{Binding Y1}"
                                    ....>
        </chart:HorizontalLineAnnotation>
    </chart:SfChart.Annotations>
   .....
</chart:SfChart> 
 ```
 
**C#**
 
 ```csharp
internal class ViewModel : INotifyPropertyChanged
{
    private double y1;
    public double Y1
    {
        get => y1;
        set
        {
            if(y1 != value)
            {
                y1 = value;
                OnPropertyChanged(nameof(Y1));
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string name)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    .....
    
    public ViewModel()
    {
        Y1 = 12000;
        .....
    }
} 
 ```
 
**Step 4:** The second column of the grid layout contains a StackPanel with a Slider, TextBox and TextBlock, allowing the user to change the annotation value dynamically. The TextBox_TextChanged event ensures valid input by clamping values between 0 and the maximum of the Y_Axis.

**XAML**
  
 ```xml
<StackPanel Orientation="Vertical" Margin="10" Grid.Column="1">

    <TextBlock Text="Adjust Target Line" FontSize="16" FontWeight="Bold" TextAlignment="Center" HorizontalAlignment="Center" Margin="0,0,0,20"/>
    <TextBox Text="{Binding Y1}" HorizontalAlignment="Stretch" VerticalAlignment="Center" TextChanged="TextBox_TextChanged" Margin="0,0,0,20" Padding="10"/>
    <Slider Minimum="{Binding Minimum, Source={x:Reference Y_Axis}}" 
            Maximum="{Binding Maximum, Source={x:Reference Y_Axis}}" 
            Value="{Binding Y1}" HorizontalAlignment="Stretch"/>

</StackPanel> 
 ```
This code handles the TextChanged event for a TextBox, dynamically updating the Y1 property in the ViewModel while ensuring the value stays within the axis’s maximum and minimum bounds. It also manages text formatting and prevents recursive event triggers.

**C#**
 
 ```csharp
private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
{
    if (Y_Axis == null) return;
    var maxValue = Y_Axis.Maximum;

    if (sender is TextBox textBox)
    {
        textBox.TextChanged -= TextBox_TextChanged;

        if (string.IsNullOrWhiteSpace(textBox.Text))
        {
            viewModel.Y1 = double.MinValue;
            textBox.Text = string.Empty;
        }
        else
        {
            if (int.TryParse(textBox.Text, out int newValue))
            {
                if (newValue > maxValue)
                    newValue = (int)maxValue;
                else if (newValue < 0)
                    newValue = 0;

                viewModel.Y1 = newValue;

                textBox.Text = newValue.ToString();
                textBox.CaretIndex = textBox.Text.Length;
            }
            else
            {
                textBox.Text = ((int)viewModel.Y1).ToString();
                textBox.CaretIndex = textBox.Text.Length;
            }
        }

        textBox.TextChanged += TextBox_TextChanged;
    }
} 
 ```
**Step 5:** This code defines a [HorizontalLineAnnotation](https://help.syncfusion.com/wpf/charts/annotations#vertical-and-horizontal-line-annotation) for a [SfChart](https://help.syncfusion.com/wpf/charts/getting-started), representing a horizontal line at a specified Y-axis value. It includes custom styling such as dashed stroke, text with font formatting, and text alignment settings.

**XAML**
  
 ```
<chart:SfChart Grid.Column="0">

        .....

        <chart:SfChart.Annotations>
            <chart:HorizontalLineAnnotation Y1="{Binding Y1}"
                                        Stroke="Black"
                                        StrokeThickness="2"
                                        StrokeDashArray="5,2,2"
                                        Text="Target"
                                        FontSize="14"
                                        FontWeight="Bold" 
                                        HorizontalTextAlignment="Left"
                                        VerticalTextAlignment="Top">
            </chart:HorizontalLineAnnotation>
        </chart:SfChart.Annotations>

        .....

</chart:SfChart>
 ```

**Output:**

![DynamicTargetLine1](https://github.com/user-attachments/assets/aa0e643e-f62e-4d95-a596-7cd981484d47)

**Troubleshooting**

Path too long exception

If you are facing a path too long exception when building this example project, close Visual Studio and rename the repository to a shorter name before building the project.

For more details, refer to the KB on [how to create and dynamically update target line for WPF Chart](https://support.syncfusion.com/agent/kb/18542).

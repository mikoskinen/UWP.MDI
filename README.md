# UWP.MDI

UWP.MDI provides Multiple document interface (MDI) support for UWP applications.

## Background

MDI (Multiple Document Interface) was popular user interface paradigma in Windows Forms era. MDI allows one window to host multiple child windows. Each window can be resized and moved around.

When WPF was released, it didn't contain support for MDI interfaces and the situation didn't change when WinRT and UWP were released.

UWP.MDI has two targets: 

1. To provide comprehensive MDI support for UWP applications. 
2. To provide MDI support in such a way that those familiar with Windows Forms' MDI support feel at home.

## Getting started

1. Create a blank Universal Windows Application
2. Reference UWP.MDI
3. Add MDIContainer to MainPage:
```xml
<Page
    x:Class="UWP.MDI.Sample.MainPage"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:local="using:UWP.MDI.Sample"
    xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
    xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
    xmlns:controls="using:Microsoft.Toolkit.Uwp.UI.Controls"
    xmlns:mdi="using:UWP.MDI.Controls"
    mc:Ignorable="d">

    <Grid Background="{ThemeResource ApplicationPageBackgroundThemeBrush}">
        <mdi:MDIContainer />
    </Grid>
</Page>
```

4. Create UserControl with the content you like.
5. In MainApplication, create a new instance of the UserControl and use Show-method to display it:

            var frm = new CustomerForm();
            frm.Show();
			
## Sample ##

The repository contains UWP.MDI.Sample, which contains two different child windows.
			
## Basics ##

Each application should have single MDIContainer. MDIContainer is used to host the windows. 

Each window should be a UserControl.

Use attached properties provided by FormProperties to configure the basic settings of your child windows. For example: Title, Starting position.

## MVVM Support ##

https://github.com/mikoskinen/UWP.MDI/issues/2

## Customizations ##

### Title ###

To add title for your UserControl, use AttachedProperty FormProperties.Text. For example:

<UserControl
    x:Class="UWP.MDI.Sample.InvoiceForm"
    xmlns:controls="using:UWP.MDI.Controls"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:local="using:UWP.MDI.Sample"
    xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
    xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
    controls:FormProperties.Text="Invoices">

### Width and height ###

To set starting size for your UserControl, set UserControl's Width and Height properties.

### Visual style ###

To change the styling, use MDIChild.xaml and MDIContainer.xaml as basis.

## Supported settings ##

The settings/configuration for child windows are provided using attached properties. 

Title: FormProperties.Text
Starting position: FormProperties.FormStartPosition
Resizable or not: FormProperties.FormBorderStyle

## Dependencies

UWP.MDI doesn't have external dependencies.

## Acknowledgements

UWP.MDI contains code from the following projects:

* UWP Community Toolkit: Mouse cursor change support and VisualTreeHelper. https://github.com/Microsoft/UWPCommunityToolkit
* WPFMDI: Original inspiration and theming. https://github.com/dutts/wpfmdi


 

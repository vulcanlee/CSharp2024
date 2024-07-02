# Expanding and Customizing the MAUI Project Template

Commands to be used:

* Switch to the working directory
* Use this project template to generate a new project

# Create a new empty project of Prism.Maui (With Prism Library)
dotnet new Vulcan-Maui-Prism -o d:\Temp\MyApp

# Create a new full project of Prism.Maui (With Prism Library and has flyout menu)
dotnet new Full-Maui -o d:\Temp\MyApp

# Create a new full project of Native Maui (No Prism Library)
dotnet new Vulcan-Maui -o d:\Temp\MyApp

* Create View / ViewModel items

# Create a new View and ViewModel for Native MAUI Project (don't append Page to view-name parameter)

dotnet new MVVMItem --namespace mauiSoftKeyboardVisibility --view-name MyFirst

# Create a new View and ViewModel for Prism MAUI Project (don't append Page to view-name parameter)

dotnet new MVVMPrismItem --namespace mauiSoftKeyboardVisibility --view-name MyFirst

# Remove this project template
dotnet new uninstall Vulcan.Maui.Template

# Font file lookup webpage
https://pictogrammers.com/library/mdi/
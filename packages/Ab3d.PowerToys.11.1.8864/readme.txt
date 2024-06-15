-------------------------------------------------
-----         Ab3d.PowerToys Readme         -----
-------------------------------------------------

Ab3d.PowerToys is an ultimate WPF and WinForms 3D toolkit library 
that greatly simplifies developing desktop applications with 
scientific, technical, CAD or other 3D graphics.

Ab3d.PowerToys is using WPF 3D rendering engine (DirectX 9).
Check Ab3d.DXEngine for super fast DirectX 11 rendering engine
that can render the existing WPF 3D scene much faster and 
with better visual quality.

Main features of the library:
- The easiest to use 3D API with many great code samples in C#
- Cameras (TargetPositionCamera, FreeCamera, FirstPersonCamera, ThirdPersonCamera, etc.)
- Camera Controllers (MouseCameraController, CameraControlPanel, CameraNavigationCircles)
- 3D Models and Visuals (Sphere, Box, Cylinder, etc.)
- Generate extruded or lathe 3D objects
- Use Boolean operations on 3D objects or slice them with a 3D plane
- 3D Lines (the fastest implementation for connected, disconnected and lines with arrows)
- 3D Text
- Event Manager 3D (simplified event handling on 3D objects)
- Many helper classes to ease work with WPF 3D
- Support for touch and multi-touch to rotate, move and zoom the camera
- Import 3D models from obj files (integrated in Ab3d.PowerToys) or almost all other 3D files with Assimp importer
- Play keyframe and skeletal animations from many 3D files with Assimp importer
- Export 3D models to many file types files (using Assimp exporter)
- Fully optimized to achieve best possible performance
- When faster rendering is required, it is very easy to switch to DirectX 11 rendering with Ab3d.DXEngine
- Premium support for all questions regarding WPF 3D and our libraries


Sample projects:
https://github.com/ab4d/Ab3d.PowerToys.Wpf.Samples      (.Net 4.5, Core3 and .Net 5.0 solutions)
https://github.com/ab4d/Ab3d.PowerToys.WinForms.Samples (.Net 4.5 solution)

https://github.com/ab4d/Ab3d.DXEngine.Wpf.Samples


Homepage:
https://www.ab4d.com/PowerToys.aspx

Online reference help:
https://www.ab4d.com/help/PowerToys/html/R_Project_Ab3d_PowerToys.htm

Change log:
https://www.ab4d.com/PowerToys-history.aspx


This version of Ab3d.PowerToys can be used as an evaluation and as a commercial version.

Evaluation usage:
On the first usage of the library, a dialog to start a 60-days evaluation is shown.
The evaluation version offers full functionality of the library but displays an evaluation
info dialog once a day and occasionally shows a "Ab3d.PowerToys evaluation" watermark text.
When the evaluation is expired, you can ask for evaluation extension or restart 
the evaluation period when a new version of the library is available.

You can see the prices of the library and purchase it on 
https://www.ab4d.com/Purchase.aspx#PowerToys

Commercial usage:
In case you have purchased a license, you can get the license parameters
from your User Account web page (https://www.ab4d.com/UserLogIn.aspx).
Then set the parametes with adding the following code before the library is used:

Ab3d.Licensing.PowerToys.LicenseHelper.SetLicense(licenseOwner: "[CompanyName]", 
                                                  licenseType: "[LicenseType]", 
                                                  license: "[LicenseText]");

Note that the version that is distributed as NuGet package uses a different licensing
mechanism then the commercial version that is distributed with a windows installer. 
Also the LicenseText that is used as a parameter to the SetLicense method is different 
then the license key used in the installer.
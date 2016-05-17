Boolean Ops - Perform CSG Operations on Meshes
v1.0.3 - Released 29/04/2016 by Alan Baylis
----------------------------------------


Foreword
----------------------------------------
Thank you for downloading Boolean Ops. Boolean operations allow you to create new meshes by performing Subtraction, Union and Intersection between objects. The creation is done in the editor and support for run time operations is in the works. The resulting geometry preserves the texture UV coordinates of the original meshes. The program also handles rotated and scaled objects as expected and is suitable for objects with multiple materials and submeshes. 


Notes
----------------------------------------
It is not recommended to do more that a few operations on the same target object or the geometry may become corrupted.

While the program works well on simple objects it may fail on very complicated objects or objects with underlying problems like holes in their geometry. It is highly recommended that you make a copy of the target object and save your scene before using this software.


Attributions
----------------------------------------
Boolean Ops uses parts of the open source software created by Evan Wallace and released under the MIT license. To get the original source code and license details follow the links below. 

Direct port of https://github.com/timknip/csg.as (Actionscript 3) to C# / Unity.

Copyright (c) 2011 Evan Wallace (http://madebyevan.com/), under the MIT license (original Javascript version, https://github.com/evanw/csg.js/).

Copyright (c) 2012 Tim Knip (http://floorplanner.com/), under the MIT license (AS3 port, https://github.com/evanw/csg.js/).

Copyright (c) 2013 Andrew Perry (http://omgwtfgames.com), under MIT license (C#/Unity port here at https://github.com/omgwtfgames/csg.cs).


To-do List.
----------------------------------------
Add an easy to use run time API for scripting.
Mesh optimization to remove excess triangles after each operation.
Option to group triangles that share the same materials.
Recalculate bounding boxes and other obvious fixes.


Common Issues / FAQ
----------------------------------------
Please visit the home page at http://www.meshmaker.com for the latest news and help forum.


Contact
----------------------------------------
Alan Baylis
www.meshmaker.com
support@meshmaker.com


Update Log
-----------------------------------------
v1.0.0 released 04/06/14
First release of Boolean Ops.

v1.0.1 released 03/09/14
New GUI layout and better integration with Mesh Maker.

v1.0.2 released 17/06/15
Updated for Unity 4.6.2 and 5.0

v1.0.3 released 29/04/16
Improved support for Unity 5.x
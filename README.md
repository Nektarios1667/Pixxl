## Overview
Pixxl is a simulation written in C# can create unique interactions between materials.
Different materials have a unique density, thermal conductivity, strength, falling behavior, and properties.
![PixxlSnapshot](https://github.com/user-attachments/assets/a660fb43-2d32-474a-b14f-1300ecff1446)


## Controls
`Primary Mouse` - Draw current material selection onto the canvas.  
`Middle mouse` - Set pixel to air.  
`s`- Quickly save current canvas.  
`l`- Load quicksave to current canvas.  
`v`- Cycle view mode between regular, colored thermal, grayscale thermal, and monotexture.  
`e`- Set tool to air to allow erasing of pixels on the canvas.  
`p`- Toggle pausing the pixels on the canvas.  
`r`- Runs a single update on the canvas.  
`d`- Cycles the simulation speed between 0.5x, 1x, 2x, and 3x.  

## Tools
 - `Reset`- Clears the canvas and fills it with air pixels at the room temperature.
 - `Reset Temp`- Sets the temperature of all pixels to the room temperature.
 - `Save`- Quickly save current canvas.
 - `Load`- Load quicksave to current canvas.
 - `View Mode`- Cycle view mode between regular, colored thermal, grayscale thermal, and monotexture.
 - `Erase`- Set tool to air to allow erasing of pixels on the canvas.
 - `Pause`- Toggle pausing the pixels on the canvas.
 - `Run Frame`- Runs a single update on the canvas.
 - `Speed`- Cycles the simulation speed between 0.5x, 1x, 2x, and 3x.

## Interactions
These are some of the cool interactions between materials:  
__Fire__ + __Copper__ - will allow you to see heat transfering quickly.  
__Acid__ + __Concrete__ - acid will slowly burn through.  
__Sodium__ + __Water__ - chemical reaction that will burn.  
__Potassium__ + __Water__ - chemical reaction that will cause a sudden explosion.  
__Coal__ + __Fire__ - coal will burn and create smoke and ash.  
__BlueFire__ + __Gold__ - superheated fire will melt the gold.  
__Fire__ + __Coolant__ - the coolant will take heat, evaporate, release heat, then condense.  
__BlueFire__ + __Insulation__ - insulation will block heat from transfering, even at extreme temperatures.  
__Propane__ + __Fire__ - the propane will instantly burn.  
__Nuke__ - explosion will destroy materials and create plasma.  



## Modifying
### Adding a material
1. To add materials to the game, first download the source code and open it in a C# IDE.
2. The code for each material is in their respective file located in the 'Materials' folder.
3. When adding a new material, create a new C# file and add its code and properties.
4. The C# file and class for the material should be the same and will appear with the same name in game.
5. After creating the code for the material, open 'Filed.cs' under the 'Registry' folder.
6. Add a line to the string variable 'Materials' inside of the 'Filed' class.
7. The line for the material should be the following: `name` | `RGB color` | `max RGB color variation` | `description`
8. Adding a '.' at the beginning of the name in the registry will cause it to be hidden from the tool bar.
9. It is recommended to add spaces so that the bars line up with the other lines, and that the materials are sorted alphabetically.
10. *__Note__*: the material name should be the same as the material file and class.
11. *__Warning__*: ensure the color variation will not cause the RGB values to be less than 0 or greater than 255, as this will cause undesired colors.
12. Once all these steps are complete, run the program to see the new material added/

### Modifying a material
1. To add materials to the game, first download the source code and open it in a C# IDE.
2. The code for each material is in their respective file located in the 'Materials' folder.
3. Locate the material you want to modify.
4. Open it up and change the properties and behavior.
5. If you want to change the color open the `Filed.cs` file in the `Registry` folder, and locate the line in the `Materials` string with the materials name.
6. The line for the material should be the following: `name` | `RGB color` | `max RGB color variation` | `description`
7. Change the color and/or variation as save the file.
8. *__Warning__*: ensure the color variation will not cause the RGB values to be less than 0 or greater than 255, as this will cause undesired colors.
9. Save the files and run the program to see the changes.
   
### Adding a tool button
1. To add materials to the game, first download the source code and open it in a C# IDE.
2. Then open the `Tools.cs` file in the `Registry` folder.
3. Add the name of the tool in the list called `Names`.
4. Add the Xna Color object of the tool button as it will appear in the toolbar.
5. Create a static function that will run when the button is clicked.
6. Add a reference to the created function.
7. *__Note__*: the button function will accept either a Window object or a Canvas object as specified in the next list called `Args`.
8. Add a string 'Window' or 'Canvas' specifying what argument should be passed to the function when the button is clicked.
9. Save the files and run the program to see the changes.

### Modifying a tool button
1. To add materials to the game, first download the source code and open it in a C# IDE.
2. Then open the `Tools.cs` file in the `Registry` folder.
3. You can change the name, color, function, or arguments of the button in the respective lists.
4. To alter the function code, find the funtion and modify the code.
5. Save the files and run the program to see the changes.

### Adding and modifying a hotkey
1. To add hotkeys to the game, first download the source code and open it in a C# IDE.
2. Then open the `Window.cs` file.
3. Find the `Update` method for the `Window` class and find the comment called 'Hotkeys'.
4. Create a new if statement that will check if the key is pressed, and on the same line call the function for the hotkey inside the if statement.
5. It should look similar to the following:
> if (KeyPress(Keys.`Keyname`)) { `function`(`args`); }
6. To modify a hotkey instead, modify the if statement to change the key, or modify the code of the function it runs.
7. Save the files and run the program to see the changes.

### Modifying the code
1. To modify any other part of the code, first download the source code and open it in a C# IDE.
2. Modify the code, save, and run the program to see the changes.

## Logs
To log something to the logs file, use the Logger.Log() function and pass in the messages as strings.
After each run a text file will be saved containing the logs of the program.  
The logs are located in `/pixxl/Logs/` and are named based on their date and time.

## Saves
The save files are located in `/pixxl/Saves/`.  
There can be up to ten save files but if nothing has been saved then there will be none.  
The files are compressed and unreadable unless unzipped using gzip.  
The first 2 lines contains metadata about gridsize and pixel size, and the rest of the lines are individual pixels and its data.  

## [License](https://creativecommons.org/licenses/by-nc-sa/4.0/deed.en  )
Creative Commons Attribution-NonCommercial-ShareAlike (CC BY-NC-SA) license. Distributing and changing this code is allowed if you give appropriate credit, provide a link to the license, and indicate if changes were made. You may not use the material for commercial purposes. If you remix, transform, or build upon this code, you must distribute your contributions under the same license as the original. You may not apply legal terms or technological measures that legally restrict others from doing anything the license permits.

This tool is created to view heat map data in the editor and in your game.  Simply drop the "HeatmapGenerator" prefab into your scene, fill out the fields, and it will do the rest.

First, you will need a set of data with a 2 point location (x/y, x/z, y,z) and a color value with red, green, blue, and alpha.  This should be in a CSV format and can be imported via URL or TXT file.  Please note the program uses an x/y value as a setup.  You will have to rotate the HeatmapGenerator's x value 90 degrees for x/z tracking, and rotate x and y 90 degrees for y/z tracking.

The program is set to use a TXT file as a default.  If one is present in the "Text Set" field the "URL" field will be ignored.

The URL should be a direct link (Without HTTP://) to the location of a CSV file.  The "Text Set" field takes precedence over this field.

The particle size is used to set the size each data-point is displayed.

"ZPositionOfHeatMap" is used to set the value you are not tracking.  For example, in the Angry Bots demo the value had to be set to -3 to be visible though the level.

"EmitterPrefab" is the particle emitter used to create your heat map.  This should be left to the default for most people.

"Off" is a check-box to turn off your heat map.

X, Y, R, G, B, A "column" properties are the location and color values for your heat map.  This is an integer value of the column number starting with 1.

If you are using a different value than x/y please put the first value in X and the second in Y.  
R = red 
G = green
B = blue 
A = alpha

Please note due to the large data-sets sometimes used for heat maps the points are only drawn once and not updated.  If you wish to update data please press "Play" then "Stop" or to update movement/rotation of the generator toggle "off" then back on.

Thank you, and enjoy your heat maps!

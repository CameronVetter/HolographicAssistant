# Introduction 
This repo is part of a Workshop and may not be very useful if you are not currently in that workshop! 

# Prerequisites
- You should have completed the [Holographic Assistant Server](https://github.com/CameronVetter/HolographicAssistantServer) exercises and have the server running locally in the background.

# Getting Started
1. Clone this repo. 
2. This folder contains the completed solution, this time **YOU WILL** start here.

# Setup Unity + VS Code integration
1. Open Unity Hub.
2. In Unity Hub Click Add and select the "HolographicAssistant" folder.
3. Click once on this entry in the list to open it.  (NOTE: If you did not install the exact version in the prerequisites this will take much longer to load but will PROBABLY work fine.)
4. Select the edit menu and preferences.
5. Select External Tools and select visual studio code.
6. Close the preferences window.

# Looking Glass users ONLY
1. Make sure to run Looking Glass Bridge before opening the scene.

# Exercise 1
1. Open the scene for this project.  In the Project Pane, find the scenes folder and select Assistant.

![image](https://github.com/CameronVetter/HolographicAssistant/blob/main/images/selectscene.png?raw=true)

2. This scene opens and is ready to go press play!

# Looking Glass users ONLY

3. If you see the elephant display on your looking glass and look 3D you are ready to go.
4. If you do not see him on your looking glass adjust the settings on the Holoplay Capture Object in the scene.  They should match these settings, but your target display may be different.

![image](https://github.com/CameronVetter/HolographicAssistant/blob/main/images/lookingglasssettings.png?raw=true)

# Non Looking Glass users ONLY

3. We will adjust the settings to display in 2d mode on your normal screen.
4. Open the Holoplay Capture Object as shown above. Select the Check box for Preview 2d.
5. Play the scene.
6. If the scene is streteched which it likely is, select the resultion dropdown and select the Looking Glass Portrait resolution here:

![image](https://github.com/CameronVetter/HolographicAssistant/blob/main/images/resolution.png?raw=true)

7. Based on your install / configuration you may not have this resolution, but you can create a custom resolution of 1536 x 2048.
8. Make sure the scene is clear and has the right aspect ratio before continuing.

# Connecting to Azure Speech
1. In the project pane open the scripts folder.
2. Double click Speech.cs from this pane and VS Code should open.
3. There is a place for your subscription key and region near the top of this file.  Replace these with your values that we will create in the next steps.
4. To create your own speech subscription, go to the Azure portal. https://portal.azure.com
5. Select Create a Resource and search for Speech.
6. Click create and follow the wizard, East Us and Free tier are recommended for this exercise.
7. Open the created resource and look under "Keys and Endpoint" and you will find the key and region to use in speech.cs.
8. Open recognize.cs and set the key and region there as well.
9. Make sure to save your files and close vs code.

# Try your assistant
1. Play the scene.
2. With the preview in focus, press the space bar and say hello to your assistant!
3. If you have been successful she will spin ridiculously while she is thinking and verbally reply.

# Extra
1. Have a conversation with her.
2. Explore the limitations of her short term memory.
3. Don't forget to ask questions that use her tools.
4. Watch the debug window of the service as she is thinking!
﻿# Fernweh
Fernweh is an Engine/Emulator for text adventure RPG Games and turn based combat. Under the hood it is built on Monogame.

We use Lua mods to create campaigns/games and add more functionality to Fernweh.
Currently, we have a "Frostglade Tundra" Lua game you can try, 
with all the code open for you to edit and play with.

# Short Term Goals
- [ ] Remove MGCB for core game and mods
- [ ] Cleanup code base and application flow
- [ ] Complete example campaign "Frostglade Tundra"
- [ ] Polish menus
- [ ] Add settings for volume, text speed and window mode
- [ ] Introduce alternative font for improved readability settings
- [ ] Improve documentation

# Future Plans
We have a giant list of issues on our to-do board... 
First, we will finish Frostglade Tundra, adding more content, and making the world 
feel more alive and add character to the currently lifeless party members.

Soon we want to get in contact with some fellow game devs who could help take 
this project to the next level with upgraded art and sounds.
Of course along the way fixing bugs, increasing Lua support + libraries + documentation, 
and eventually, a creating a map editor.

At that point, you should be able to play Fernweh and get a close-to-finish experience.
And once we're there, ideally, we should have a small community of people interested in 
development, and we should have a team ready to write, design, and create more 
campaigns/games for Fernweh.

This is quite a hopeful plan. It'll be lots of work, receiving feedback and 
masterful writing before we get close to that. 
But this demo is step one, gotta start some where.

Thanks for playing,
Devs

# Set up dev environment

Tested IDEs are:
 - VS Code
 - Visual Studio 2019 or later
 - Jetbrains Rider

Requirements:
 - [dotnet SDK 3.1](https://dotnet.microsoft.com/en-us/download/dotnet/3.1)
 - [dotnet SDK 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)

Additional tools. For some content it is required to have the MGCB tool installed. See this [guide](https://docs.monogame.net/articles/getting_started/1_setting_up_your_development_environment_windows.html#install-mgcb-editor).

# Running
Before debugging the game, you need to manually build the sub-project: "PipelineExtension".

Then you can start debugging the "CrossPlatformDesktop" project.

# Packaging

``dotnet publish -c Release -r win-x64 /p:PublishReadyToRun=false /p:TieredCompilation=false --self-contained``

You can change ``win-x64`` to ``linux-x64`` or ``osx-x64`` depending on your target platform.

Make sure to create a "mods" folder as well, otherwise, the game won't run properly.

More info here if needed: https://docs.monogame.net/articles/packaging_games.html

# License
- The code and binaries are licensed under the [EULA](./EULA.txt)
- Mods located under Core/Mods can have different Licenses in their respective folders!
- Third party libraries e.g. in ./PipelineAssemblies are distributed under their own license
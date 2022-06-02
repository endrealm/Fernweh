# Fernweh
Fernweh is a turn-based RPG combined with elements of text adventures. 
Though on its own, Fernweh is only a framework. 

We use Lua mods to create campaigns/games and add more functionality to Fernweh.
Currently, we have a "Frostglade Tundra" Lua game you can try, 
with all of the code open for you to edit and play with.

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

# Running
Before debugging the game, you need to manually build the sub-project: "PipelineExtension".

Then you can start debugging the "CrossPlatformDesktop" project.

# Packaging

``dotnet publish -c Release -r win-x64 /p:PublishReadyToRun=false /p:TieredCompilation=false --self-contained``

You can change ``win-x64`` to ``linux-x64`` or ``osx-x64`` depending on your target platform.

Make sure to create a "mods" folder as well, otherwise, the game won't run properly.

More info here if needed: https://docs.monogame.net/articles/packaging_games.html
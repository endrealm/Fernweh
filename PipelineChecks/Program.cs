// See https://aka.ms/new-console-template for more information

using Core.Scenes.Ingame.Battle;
using Core.States;
using Core.States.ScriptApi;
using PipelineExtensionLibrary;

new XmlDialogParser().Parse("Some example <color=\"rgba(1,1,1,.4)\">text</color> here!");

var manager = new ScriptLoader(new StateRegistry(), new BattleRegistry());

manager.LoadScript(@"
stateBuilder('my_other_state')
    :render(
        function(renderer, context)
            renderer.addText('example.message.4')
            renderer.addAction(StateChangeAction('example.button.1', 'my_state'))
            renderer.addAction(CustomAction('example.button.3',
                function()
                    context.exit();
                end
            ))
        end
    )
    :build()
", new ScriptContext(new NamespacedKey("", ""), ""));

Console.WriteLine("Done");
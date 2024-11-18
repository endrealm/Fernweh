// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using PipelineExtensionLibrary;

var watch = new Stopwatch();
var iterations = 1000;
var parser = new TranslationTextParser();
watch.Start();
for (var i = 0; i < iterations; i++)
    parser.Parse(
        "Some example <color=\"rgba(1,1,1,.4)\">text</color> here! <color=\"rgba(1,1,1,.4)\">text <color=\"rgba(1,1,1,.4)\">text</color> asd</color> asd <color=\"rgba(1,1,1,.4)\">text</color>");
watch.Stop();

Console.WriteLine("Took: " + watch.ElapsedMilliseconds / 1000f);
Console.WriteLine("Per: " + watch.ElapsedMilliseconds / 1000f / iterations);

// var manager = new ScriptLoader(new StateRegistry(), new BattleRegistry());
// 
// manager.LoadScript(@"
// stateBuilder('my_other_state')
//     :render(
//         function(renderer, context)
//             renderer.addText('example.message.4')
//             renderer.addAction(StateChangeAction('example.button.1', 'my_state'))
//             renderer.addAction(CustomAction('example.button.3',
//                 function()
//                     context.exit();
//                 end
//             ))
//         end
//     )
//     :build()
// ", new ScriptContext(new NamespacedKey("", ""), ""));

Console.WriteLine("Done");
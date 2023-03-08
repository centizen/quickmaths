using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Game.Gui;
using System.Data;
using System;

namespace QuickMaths.QuickMaths
{
    public sealed class Plugin : IDalamudPlugin
    {
        public string Name => "Quick Maths";
        private const string CommandName = "/pcalc";

        private DalamudPluginInterface PluginInterface { get; init; }
        private CommandManager CommandManager { get; init; }
        private ChatGui ChatGui { get; init; }


        public Plugin(
            [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
            [RequiredVersion("1.0")] CommandManager commandManager,
            [RequiredVersion("1.0")] ChatGui chatGui)
        {
            PluginInterface = pluginInterface;
            CommandManager = commandManager;
            ChatGui = chatGui;

            CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
            {
                HelpMessage = "Quick and dirty chatlog calculator. No need for any extra windows! \n\nSigns \n +  =  Addition \n -  =  Subtraction \n *  =  Multiplication \n /  =  Division \n\nExamples: \n/pcalc 2 + 2 \n/pcalc 1 + 2 * 3 / 4 \n/pcalc 10 * (5 * 2) - 2\n\n\n"
            });
        }

        public void Dispose()
        {
            CommandManager.RemoveHandler(CommandName);
        }

        private void OnCommand(string command, string args)
        {
            try
            {
                var result = new DataTable().Compute(args, null);
                ChatGui.Print("" + args + " = " + result);
                ChatGui.UpdateQueue();
            }
            catch (Exception)
            {
                ChatGui.PrintError("QuickMaths: Error in expression");
                ChatGui.UpdateQueue();
            }
        }
    }
}

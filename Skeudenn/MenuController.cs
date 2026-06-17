using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Skeudenn
{
   public sealed record MenuItem(string Id, string Text);

   public sealed class MenuController
   {
      private const string Main = "Main";
      private const string File = "File";
      private const string Plugins = "Plugins";
      private const string Help = "Help";
      private const string FileUp = "FileUp";
      private const string FileOpen = "FileOpen";
      private const string FileExit = "FileExit";
      private const string PluginsUp = "PluginsUp";
      private const string PluginsBinarize = "PluginsBinarize";
      private const string HelpUp = "HelpUp";
      private const string HelpAbout = "HelpAbout";
      private const string TwoDots = "..";
      private const string Open = "Open...";
      private const string Exit = "Exit";
      private const string Binarize = "Binarize";
      private const string About = "About";

      private readonly Func<string, IReadOnlyList<MenuItem>, MenuItem> promptMenu;
      private readonly Func<string, string, string> askText;
      private readonly Func<string, byte> askByte;
      private readonly Action clear;
      private readonly Action<string> writeLine;
      private readonly Action<string> openFile;
      private readonly Func<string> aboutText;

      public MenuController(
         Func<string, IReadOnlyList<MenuItem>, MenuItem> promptMenu,
         Func<string, string, string> askText,
         Func<string, byte> askByte,
         Action clear,
         Action<string> writeLine,
         Action<string> openFile,
         Func<string> aboutText)
      {
         this.promptMenu = promptMenu;
         this.askText = askText;
         this.askByte = askByte;
         this.clear = clear;
         this.writeLine = writeLine;
         this.openFile = openFile;
         this.aboutText = aboutText;
      }

      public void Run()
      {
         bool exitMenu = false;
         string menu = Main;
         ImmutableDictionary<string, string> menuPrompts = ImmutableDictionary<string, string>.Empty;
         ImmutableDictionary<string, ImmutableList<string>> menuChoices = ImmutableDictionary<string, ImmutableList<string>>.Empty;
         ImmutableDictionary<string, string> menuConversion = ImmutableDictionary<string, string>.Empty;
         ImmutableDictionary<string, Action> menuAction = ImmutableDictionary<string, Action>.Empty;

         menuPrompts = menuPrompts
            .Add(Main, "MainMenu")
            .Add(File, "FileMenu")
            .Add(Plugins, "PluginsMenu")
            .Add(Help, "HelpMenu");

         menuChoices = menuChoices
            .Add(Main, [File, Plugins, Help])
            .Add(File, [FileUp, FileOpen, FileExit])
            .Add(Plugins, [PluginsUp, PluginsBinarize])
            .Add(Help, [HelpUp, HelpAbout]);

         menuConversion = menuConversion
            .Add(FileUp, TwoDots)
            .Add(FileOpen, Open)
            .Add(FileExit, Exit)
            .Add(PluginsUp, TwoDots)
            .Add(PluginsBinarize, Binarize)
            .Add(HelpUp, TwoDots)
            .Add(HelpAbout, About);

         menuAction = menuAction
            .Add(FileUp, () => menu = Main)
            .Add(FileOpen, () =>
            {
               string filePath = askText("Enter file path", string.Empty);
               clear();
               openFile(filePath);
               menu = Main;
            })
            .Add(FileExit, () => exitMenu = true)
            .Add(PluginsUp, () => menu = Main)
            .Add(PluginsBinarize, () =>
            {
               _ = askByte("Input binarization threshold to apply");
               menu = Main;
            })
            .Add(HelpUp, () => menu = Main)
            .Add(HelpAbout, () =>
            {
               writeLine(aboutText());
               menu = Main;
            });

         while (!exitMenu)
         {
            if (menuPrompts.TryGetValue(menu, out string? menuTitle))
            {
               if (menuChoices.TryGetValue(menu, out ImmutableList<string>? choices))
               {
                  var menuItems = choices
                     .Select(choice => new MenuItem(
                        choice,
                        menuConversion.TryGetValue(choice, out string? converted) ? converted : choice))
                     .ToImmutableArray();

                  MenuItem selected = promptMenu(menuTitle, menuItems);
                  menu = selected.Id;
               }
               else
               {
                  System.Diagnostics.Debug.Fail("Unknown menu choices");
                  exitMenu = true;
               }
            }
            else if (menuAction.TryGetValue(menu, out Action? action))
            {
               action.Invoke();
            }
            else
            {
               System.Diagnostics.Debug.Fail("Unknown menu");
               exitMenu = true;
            }
         }
      }
   }
}

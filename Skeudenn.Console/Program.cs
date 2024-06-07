using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Skeudenn.UI;
using Spectre.Console;
using System;
using System.Collections.Immutable;
using System.IO;

// UNDONE Fix all the Intellisense messages to improve the code
namespace Skeudenn.Console
{
   class Program
   {
      static void Main(string[] args)
      {
         bool exitMenu = false;
         const string main = "Main";
         const string file = "File";
         const string help = "Help";
         const string fileUp = "FileUp";
         const string fileOpen = "FileOpen";
         const string fileExit = "FileExit";
         const string helpUp = "HelpUp";
         const string helpAbout = "HelpAbout";
         const string twoDots = "..";
         const string open = "Open...";
         const string exit = "Exit";
         const string about = "About";
         string menu = main;
         ImmutableDictionary<string, string> menuPrompts = ImmutableDictionary<string, string>.Empty;
         ImmutableDictionary<string, ImmutableList<string>> menuChoices = ImmutableDictionary<string, ImmutableList<string>>.Empty;
         ImmutableDictionary<string, string> menuConversion = ImmutableDictionary<string, string>.Empty;
         ImmutableDictionary<string, Action> menuAction = ImmutableDictionary<string, Action>.Empty;
         MainView mainView = new MainView();

         menuPrompts = menuPrompts.Add(main, "MainMenu").Add(file, "FileMenu").Add(help, "HelpMenu");

         menuChoices = menuChoices.Add(menu, ImmutableList<string>.Empty.Add(file).Add(help));
         menuChoices = menuChoices.Add(file, ImmutableList<string>.Empty.Add(fileUp).Add(fileOpen).Add(fileExit));
         menuChoices = menuChoices.Add(help, ImmutableList<string>.Empty.Add(helpUp).Add(helpAbout));

         menuConversion = menuConversion.Add(fileUp, twoDots).Add(fileOpen, open).Add(fileExit, exit);
         menuConversion = menuConversion.Add(helpUp, twoDots).Add(helpAbout, about);

         menuAction = menuAction.Add(fileUp, () => menu = main);
         menuAction = menuAction.Add(fileOpen, () =>
         {
            string filePath = AnsiConsole.Ask<string>("Enter file path", string.Empty);

            AnsiConsole.Clear();

            if (filePath != null)
            {
               filePath = filePath.Replace("\"", string.Empty);

               try
               {
                  UI.Image imageUI = mainView.OpenFile(filePath);

                  if (imageUI.Valid)
                  {
                     Image<L8> image = SixLabors.ImageSharp.Image.LoadPixelData<L8>(imageUI.ImageData(), imageUI.Size.Width, imageUI.Size.Height);
                     CanvasImage canvasImage;

                     // HACK I think this can now be simplified without passing by a BMP
                     using (MemoryStream memoryStream = new MemoryStream())
                     {
                        image.SaveAsBmp(memoryStream);
                        memoryStream.Seek(0, SeekOrigin.Begin);

                        canvasImage = new CanvasImage(memoryStream);
                     }

                     AnsiConsole.Write(canvasImage);
                  }
                  else
                  {
                     AnsiConsole.WriteLine("Unable to load file.");
                  }
               }
               catch (FileNotFoundException)
               {
                  AnsiConsole.WriteLine("Cannot find or open this image file.");
                  AnsiConsole.WriteLine(filePath);
               }
               catch
               {
                  AnsiConsole.WriteLine("Unkown error while opening image file.");
                  AnsiConsole.WriteLine(filePath);
               }
            }

            menu = main;
         });
         menuAction = menuAction.Add(fileExit, () => exitMenu = true);
         menuAction = menuAction.Add(helpUp, () => menu = main);
         menuAction = menuAction.Add(helpAbout, () =>
         {
            AnsiConsole.WriteLine(mainView.AboutText());

            menu = main;
         });

         // HACK Add support for zoom tool with AnsiConsole.Console.Input.ReadKey(), AnsiConsole.Live() and canvasImage.MaxWidth
         // HACK Add all the same UI functionalities as with the Godot UI
         while (!exitMenu)
         {
            string? menuTitle;
            Action? action;

            if (menuPrompts.TryGetValue(menu, out menuTitle))
            {
               ImmutableList<string>? choices;

               if (menuChoices.TryGetValue(menu, out choices))
               {
                  menu = AnsiConsole.Prompt(
                             new SelectionPrompt<string>()
                                 .Title(menuTitle)
                                 .AddChoices(choices)
                                 .UseConverter(menuItem =>
                                 {
                                    string? convertedMenu;

                                    if (menuConversion.TryGetValue(menuItem, out convertedMenu))
                                    {
                                       return convertedMenu;
                                    }

                                    return menuItem;
                                 }));
               }
               else
               {
                  System.Diagnostics.Debug.Fail("Unknown menu choices");
                  exitMenu = true;
               }
            }
            else if (menuAction.TryGetValue(menu, out action))
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

using Shouldly;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Skeudenn.UI;
using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Collections.Immutable;
using System.ComponentModel;
using System.IO;

namespace Skeudenn.Console
{
   // TODO Apply the command-line parser logic to the Godot UI
   public class FileOpenCommandSettings : CommandSettings
   {
      [Description("Image to open")]
      [CommandArgument(0, "[image file]")]
      public required string ImageFilePath { get; init; }
   }

   public class FileOpenCommand : Command<FileOpenCommandSettings>
   {
      private void OpenFile(string filePath)
      {
         if (filePath != null)
         {
            filePath = filePath.Replace("\"", string.Empty);

            try
            {
               UI.Image imageUI = MainView.OpenFile(filePath);

               if (imageUI.Valid)
               {
                  // TODO The Console UI should not depend on Skeudenn, only on Skeudenn.UI.
                  ImageProcessors imageProcessors = new();

                  Image<L8> image = SixLabors.ImageSharp.Image.LoadPixelData<L8>(imageUI.ImageData(imageProcessors), imageUI.Size.Width, imageUI.Size.Height);
                  CanvasImage canvasImage;

                  // HACK I think this can now be simplified without passing by a BMP
                  using (MemoryStream memoryStream = new())
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
      }

      // UNDONE Move this to the Skeudenn namespace in order to leave a simple Main() in this project and unit test the extracted code, using Spectre.Console.Testing
      public override int Execute(CommandContext context, FileOpenCommandSettings settings)
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
         MainView mainView = new();
         // TODO The Console UI should not depend on Skeudenn, only on Skeudenn.UI.
         ImageProcessors imageProcessors = new();

         menuPrompts = menuPrompts.Add(main, "MainMenu").Add(file, "FileMenu").Add(help, "HelpMenu");

         menuChoices = menuChoices.Add(menu, [file, help]);
         menuChoices = menuChoices.Add(file, [fileUp, fileOpen, fileExit]);
         menuChoices = menuChoices.Add(help, [helpUp, helpAbout]);

         menuConversion = menuConversion.Add(fileUp, twoDots).Add(fileOpen, open).Add(fileExit, exit);
         menuConversion = menuConversion.Add(helpUp, twoDots).Add(helpAbout, about);

         menuAction = menuAction.Add(fileUp, () => menu = main);
         menuAction = menuAction.Add(fileOpen, () =>
         {
            string filePath = AnsiConsole.Ask<string>("Enter file path", string.Empty);

            AnsiConsole.Clear();

            OpenFile(filePath);

            menu = main;
         });
         menuAction = menuAction.Add(fileExit, () => exitMenu = true);
         menuAction = menuAction.Add(helpUp, () => menu = main);
         menuAction = menuAction.Add(helpAbout, () =>
         {
            AnsiConsole.WriteLine(MainView.AboutText());

            menu = main;
         });

         if (settings.Validate().Successful)
         {
            settings.ImageFilePath.ShouldNotBeNull();
            OpenFile(settings.ImageFilePath);
         }

         // HACK Add support for zoom tool with AnsiConsole.Console.Input.ReadKey(), AnsiConsole.Live() and canvasImage.MaxWidth
         // HACK Add all the same UI functionalities as with the Godot UI
         while (!exitMenu)
         {
            if (menuPrompts.TryGetValue(menu, out string? menuTitle))
            {
               if (menuChoices.TryGetValue(menu, out ImmutableList<string>? choices))
               {
                  menu = AnsiConsole.Prompt(
                             new SelectionPrompt<string>()
                                 .Title(menuTitle)
                                 .AddChoices(choices)
                                 .UseConverter(menuItem =>
                                 {
                                    if (menuConversion.TryGetValue(menuItem, out string? convertedMenu))
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

         return 0;
      }
   }

   class Program
   {
      // UNDONE Add plugin menu to console
      static int Main(string[] args)
      {
         var app = new CommandApp<FileOpenCommand>();
         app.Configure(config =>
         {
            config.SetApplicationName("skeudenn");

            config.AddExample("example.jpg");

#if DEBUG
            config.PropagateExceptions();
            config.ValidateExamples();
#endif
         });

         try
         {
            return app.Run(args);
         }
         catch (Exception ex)
         {
            AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
            return -1;
         }
      }
   }
}

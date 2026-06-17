using Shouldly;
using SixLabors.ImageSharp.PixelFormats;
using Skeudenn;
using Skeudenn.UI;
using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using static SixLabors.ImageSharp.ImageExtensions;

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

                  SixLabors.ImageSharp.Image<L8> image = SixLabors.ImageSharp.Image.LoadPixelData<L8>(imageUI.ImageData(imageProcessors), imageUI.Size.Width, imageUI.Size.Height);
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

      // TODO Unit test this code, using Spectre.Console.Testing
      protected override int Execute(CommandContext context, FileOpenCommandSettings settings, CancellationToken cancellationToken)
      {
         if (settings.Validate().Successful)
         {
            settings.ImageFilePath.ShouldNotBeNull();
            OpenFile(settings.ImageFilePath);
         }

         MenuController controller = new(
            (title, items) => AnsiConsole.Prompt(
               new SelectionPrompt<MenuItem>()
                  .Title(title)
                  .AddChoices(items)
                  .UseConverter(item => item.Text)),
            (prompt, defaultValue) => AnsiConsole.Ask(prompt, defaultValue),
            prompt => AnsiConsole.Prompt(new TextPrompt<byte>(prompt)),
            () => AnsiConsole.Clear(),
            text => AnsiConsole.WriteLine(text),
            path => OpenFile(path),
            () => MainView.AboutText());

         controller.Run();

         return 0;
      }
   }

   class Program
   {
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

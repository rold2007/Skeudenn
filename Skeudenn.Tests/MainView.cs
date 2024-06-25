using System.Collections.Generic;
using System.IO;
using Shouldly;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Xunit;

namespace Skeudenn.Tests
{
   public sealed class MainView
   {
      [Fact]
      public void Constructor()
      {
         UI.MainView fileMenu = new();
      }

      [Fact]
      public void CanExit()
      {
         UI.MainView fileMenu = new();

         UI.MainView.CanExit().ShouldBeTrue();
      }

      [Fact]
      public void Exit()
      {
         UI.MainView fileMenu = new();

         UI.MainView.Exit();
      }

      [Fact]
      public void OpenFilePath()
      {
         using Image<L8> tempImage = new(3, 3);
         UI.MainView fileMenu = new();

         string tempFilename = string.Empty;

         try
         {
            tempFilename = Path.GetTempFileName() + ".bmp";

            tempImage.SaveAsBmp(tempFilename);

            UI.Image image = UI.MainView.OpenFile(tempFilename);

            image.Valid.ShouldBeTrue();
         }
         finally
         {
            if (tempFilename != string.Empty)
            {
               File.Delete(tempFilename);
            }
         }
      }

      [Fact]
      public void OpenFile()
      {
         using Image<L8> tempImage = new(3, 3);
         using MemoryStream memoryStream = new();
         UI.MainView fileMenu = new();

         tempImage.SaveAsBmp(memoryStream);
         memoryStream.Seek(0, SeekOrigin.Begin);

         UI.Image image = UI.MainView.OpenFile(memoryStream);

         image.Valid.ShouldBeTrue();
      }

      [Fact]
      public void OpenFilePathFail()
      {
         UI.MainView fileMenu = new();

         string tempFilename = string.Empty;

         try
         {
            tempFilename = Path.GetTempFileName();

            UI.MainView.OpenFile(tempFilename).Valid.ShouldBeFalse();
         }
         finally
         {
            if (tempFilename != string.Empty)
            {
               File.Delete(tempFilename);
            }
         }
      }

      [Fact]
      public void OpenFileFail()
      {
         using MemoryStream memoryStream = new();
         UI.MainView fileMenu = new();

         UI.MainView.OpenFile(memoryStream).Valid.ShouldBeFalse();
      }

      [Fact]
      public void OpenFilesPath()
      {
         List<UI.Image> images;

         using Image<L8> tempImage = new(3, 3);
         UI.MainView fileMenu = new();

         List<string> tempFilenames = [];

         try
         {
            for (int i = 0; i < 10; i++)
            {
               string tempFilename = Path.GetTempFileName() + ".bmp";

               tempFilenames.Add(tempFilename);
               tempImage.SaveAsBmp(tempFilename);
            }


            images = UI.MainView.OpenFiles([.. tempFilenames], out bool error);
         }
         finally
         {
            foreach (string tempFilename in tempFilenames)
            {
               File.Delete(tempFilename);
            }
         }
      }

      [Fact]
      public void OpenFilesPathFail()
      {
         List<UI.Image> images;

         using Image<L8> tempImage = new(3, 3);
         UI.MainView fileMenu = new();

         List<string> tempFilenames = [];

         try
         {
            for (int i = 0; i < 10; i++)
            {
               string tempFilename;

               if (i == 3)
               {
                  tempFilename = Path.GetTempFileName();
               }
               else
               {
                  tempFilename = Path.GetTempFileName() + ".bmp";
               }

               tempFilenames.Add(tempFilename);

               if ((i != 3) && (i != 5))
               {
                  tempImage.SaveAsBmp(tempFilename);
               }
            }


            images = UI.MainView.OpenFiles([.. tempFilenames], out bool error);
         }
         finally
         {
            foreach (string tempFilename in tempFilenames)
            {
               File.Delete(tempFilename);
            }
         }
      }

      [Fact]
      public void AboutText()
      {
         UI.MainView fileMenu = new();

         string aboutText = UI.MainView.AboutText();

         aboutText.ShouldBe("v1.0");
      }
   }
}

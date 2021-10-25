using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;
using Xunit;

namespace Skeudenn.Tests
{
   public sealed class MainMenu
   {
      [Fact]
      public void Constructor()
      {
         Skeudenn.UI.MainMenu fileMenu = new UI.MainMenu();
      }

      [Fact]
      public void CanExit()
      {
         Skeudenn.UI.MainMenu fileMenu = new UI.MainMenu();

         fileMenu.CanExit().ShouldBeTrue();
      }

      [Fact]
      public void Exit()
      {
         Skeudenn.UI.MainMenu fileMenu = new UI.MainMenu();

         fileMenu.Exit();
      }

      [Fact]
      public void OpenFilePath()
      {
         Skeudenn.UI.Image image;

         using (SixLabors.ImageSharp.Image<L8> tempImage = new SixLabors.ImageSharp.Image<L8>(3, 3))
         {
            Skeudenn.UI.MainMenu fileMenu = new UI.MainMenu();

            string tempFilename = null;

            try
            {
               tempFilename = Path.GetTempFileName() + ".bmp";

               tempImage.SaveAsBmp(tempFilename);

               image = fileMenu.OpenFile(tempFilename);
            }
            finally
            {
               File.Delete(tempFilename);
            }
         }
      }

      [Fact]
      public void OpenFile()
      {
         Skeudenn.UI.Image image;

         using (SixLabors.ImageSharp.Image<L8> tempImage = new SixLabors.ImageSharp.Image<L8>(3, 3))
         {
            using (MemoryStream memoryStream = new MemoryStream())
            {
               Skeudenn.UI.MainMenu fileMenu = new UI.MainMenu();

               tempImage.SaveAsBmp(memoryStream);
               memoryStream.Seek(0, SeekOrigin.Begin);

               image = fileMenu.OpenFile(memoryStream);
            }
         }
      }

      [Fact]
      public void AboutText()
      {
         Skeudenn.UI.MainMenu fileMenu = new UI.MainMenu();

         string aboutText = fileMenu.AboutText();

         aboutText.ShouldBe("v1.0");
      }

      [Fact]
      public void ImageClone()
      {
         Skeudenn.UI.Image imageUI;

         using (SixLabors.ImageSharp.Image<L8> tempImage = new SixLabors.ImageSharp.Image<L8>(3, 3))
         {
            using (MemoryStream memoryStream = new MemoryStream())
            {
               Skeudenn.UI.MainMenu fileMenu = new UI.MainMenu();

               tempImage.SaveAsBmp(memoryStream);
               memoryStream.Seek(0, SeekOrigin.Begin);

               imageUI = fileMenu.OpenFile(memoryStream);

               using (MemoryStream imageClonerMemoryStream = new MemoryStream())
               {
                  Image<L8> image = imageUI.ImageClone;

                  image.SaveAsBmp(imageClonerMemoryStream);

                  imageClonerMemoryStream.ToArray().ShouldBe(memoryStream.ToArray());
               }
            }
         }
      }
   }
}

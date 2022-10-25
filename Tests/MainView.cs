﻿using System.IO;
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
         Skeudenn.UI.MainView fileMenu = new UI.MainView();
      }

      [Fact]
      public void CanExit()
      {
         Skeudenn.UI.MainView fileMenu = new UI.MainView();

         fileMenu.CanExit().ShouldBeTrue();
      }

      [Fact]
      public void Exit()
      {
         Skeudenn.UI.MainView fileMenu = new UI.MainView();

         fileMenu.Exit();
      }

      [Fact]
      public void OpenFilePath()
      {
         Skeudenn.UI.Image image;

         using (SixLabors.ImageSharp.Image<L8> tempImage = new SixLabors.ImageSharp.Image<L8>(3, 3))
         {
            Skeudenn.UI.MainView fileMenu = new UI.MainView();

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
               Skeudenn.UI.MainView fileMenu = new UI.MainView();

               tempImage.SaveAsBmp(memoryStream);
               memoryStream.Seek(0, SeekOrigin.Begin);

               image = fileMenu.OpenFile(memoryStream);
            }
         }
      }

      [Fact]
      public void AboutText()
      {
         Skeudenn.UI.MainView fileMenu = new UI.MainView();

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
               Skeudenn.UI.MainView fileMenu = new UI.MainView();

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
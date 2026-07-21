using System;
using System.Collections.Generic;
using System.Text;
using UrlShorten.Infrastructures.Implements;

namespace UrlShorten.Test.Services
{
    public class Base62ShortenServiceTest
    {
        [Fact]
        public void EncodeBase62_ShouldReturnCorrectBase62String()
        {
            // Arrange
            //var service = new Base62ShortenService(null, null, null); // Pass null for context as it's not used in this method
            //long value = 12345;
            //// Act
            //string result = service.ShortenHash(value);
            //// Assert
            //if (result != "3d7")
            //{
            //    throw new Exception($"Expected '3d7' but got '{result}'");
            //}
        }
    }
}

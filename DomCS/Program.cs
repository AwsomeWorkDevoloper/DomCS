using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomCS;

namespace Testing_DomCS
{
    class Program
    {
        static void Main(string[] args)
        {
            string outputFolder = Directory.GetCurrentDirectory() + @"\ExampleWebsite\";
            
            Server server = new Server(
                5500, 
                outputFolder
            );

            server.HTMLDocs = new List<HTMLDoc>()
            {
                Index(),
                About()
            };

            server.StartServer();
        }

        static HTMLDoc Index()
        {
            HTMLDoc index = new HTMLDoc("Home");
            index.Route = "/";

            DomElement header = new DomElement("h1");
            header.Value = "Hello, World!";

            DomElement aboutLink = new DomElement("a");
            aboutLink.Value = "About Page";
            aboutLink.Attributes = "href = 'about'";

            index.DomElements = new List<DomElement>()
            {
                header,
                aboutLink
            };

            return index;
        }

        static HTMLDoc About()
        {
            HTMLDoc about = new HTMLDoc("About");
            about.Route = "/about";

            DomElement header = new DomElement("h1");
            header.Value = "This is my website!";

            about.DomElements = new List<DomElement>()
            {
                header
            };

            return about;
        }
    }
}

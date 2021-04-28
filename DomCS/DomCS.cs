using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomCS
{
    class Server
    {
        private string _code;
        public string OutputPath { get; set; }
        public List<HTMLDoc> HTMLDocs { get; set; }

        public Server(int port, string outputPath)
        {
            _code +=
                "const express = require('express');\n" +
                "const app = express();\n" +
                $"const server = app.listen(" +
                $"{port}, " +
                $"()=>console.log('Running server on http://localhost:{port}')" +
                $");" +
                "app.use(express.static('public'));\n" +
                "app.set('view engine', 'ejs');";
            OutputPath = outputPath;
        }

        public void StartServer()
        {
            Console.WriteLine("Compiling Website...");

            if (!Directory.Exists(OutputPath))
            {
                Directory.CreateDirectory(OutputPath);
            }

            if(!File.Exists(OutputPath + "\\package.json"))
            {
                ExecuteCommand($"cd {OutputPath} & npm init -y");
            }


            if (!Directory.Exists(OutputPath + "\\" + "node_modules"))
            {
                ExecuteCommand(
                    $"cd {OutputPath} &" +
                    $"npm install --save express ejs"
                );
            }

            if(!Directory.Exists(OutputPath + "\\" + "public"))
            {
                Directory.CreateDirectory(OutputPath + "\\" + "public");
            }

            if (!Directory.Exists(OutputPath + "\\" + "views"))
            {
                Directory.CreateDirectory(OutputPath + "\\" + "views");
            }

            File.Create($"{OutputPath}/server.js").Close();

            File.Create($"{OutputPath}/Run.bat").Close();
            File.WriteAllText($"{OutputPath}/Run.bat", "npm start\npause");

            ExportHTMLDocs();

            File.WriteAllText($"{OutputPath}/server.js", _code);

            Console.WriteLine($"Compiled Website.\nRun 'Run.bat' to start the server.");
        }

        public void ExportHTMLDocs()
        {
            if(HTMLDocs != null)
            {
                foreach (var html in HTMLDocs)
                {
                    File.Create($"{OutputPath}/views/{html.Title}.ejs").Close();
                    File.WriteAllText(
                        $"{OutputPath}/views/{html.Title}.ejs",
                        html.GenerateDOC()
                    );

                    _code += $"\napp.get('{html.Route}', (req, res)=>res.render('{html.Title}.ejs'));";
                }
            }
        }

        public void ExecuteCommand(string Command)
        {
            ProcessStartInfo ProcessInfo;
            Process Process;

            ProcessInfo = new ProcessStartInfo("cmd.exe", "/K " + Command);
            ProcessInfo.CreateNoWindow = true;
            ProcessInfo.UseShellExecute = true;
            //ProcessInfo.WindowStyle = ProcessWindowStyle.Hidden;

            Process = Process.Start(ProcessInfo);

            Process.CloseMainWindow();
        }
    }

    class HTMLDoc
    {
        public List<DomElement> DomElements { get; set; }
        public List<HeadElement> HeadElements { get; set; }
        public string Route { get; set; }
        public string Title;

        public HTMLDoc(string title)
        {
            Title = title;
            Route = title;
        }

        public string GenerateDOC()
        {
            var BodyCode = "";
            var HeadCode = "";

            if(DomElements != null)
            {
                foreach (var d in DomElements)
                {
                    BodyCode += d.GenerateHTML() + "\n";
                }
            }

            if (HeadElements != null)
            {
                foreach (var h in HeadElements)
                {
                    HeadCode += h.GenerateHTML() + "\n";
                }
            }

            return $"<html>" +
                $"<head>" +
                $"<title>{Title}</title>" +
                $"{HeadCode}" +
                $"</head>" +
                $"<body>" +
                $"{BodyCode}" +
                $"</body>" +
                $"</html>";
        }
    }

    class DomElement
    {
        public string Type;
        public string Value = "";
        public string Attributes = "";

        public List<DomElement> DomElements { get; set; }

        public DomElement(string type)
        {
            Type = type.ToLower();
        }

        public string GenerateHTML()
        {
            var html = $"<{Type} {Attributes}>{Value}";

            if(DomElements != null)
            {
                foreach (var d in DomElements)
                {
                    html += d.GenerateHTML();
                }
            }

            html += "</" + Type + ">";

            return html;
        }
    }

    class HeadElement
    {
        public string Type;
        public string Attributes = "";

        public HeadElement(string type)
        {
            Type = type.ToLower();
        }

        public string GenerateHTML()
        {
            var html = $"<{Type} {Attributes}>";

            return html;
        }
    }
}

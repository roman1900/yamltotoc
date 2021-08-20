using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace yamltotoc
{
    class Program
    {
        enum parameters {
            append,
            output,
            path,
            undefined
        }
        static void Main(string[] args)
        {
            bool append = false;
            string output = "readme.md";
            string path = @".\";
            parameters currentArg = parameters.undefined;
            if (args.Length >0)
            {
                foreach (string arg in args)
                {
                    if (arg.Substring(0,1) == "-")
                    {
                        //Parameter
                        switch (arg)
                        {
                            case "-a":
                            case "--append":
                                append = true;
                                break;
                            case "-p":
                            case "--path":
                                currentArg = parameters.path;
                                break;
                            case "-o":
                            case "--output":
                                currentArg = parameters.output;
                                break;
                            case "-h":
                            case "--help":
                                Console.WriteLine("Usage: yamltotoc.exe [options]");
                                Console.WriteLine(" options:");
                                Console.WriteLine("     -a, --append                   Append the TOC to the end of the output file. Without append the output file is overwritten");
                                Console.WriteLine("     -h, --help                     Print this help message");
                                Console.WriteLine("     -o, --output {filename}        Provide the file name to output the TOC. Defaults to readme.md");
                                Console.WriteLine("     -p, --path {directory path}    Provide the path to documents to create the TOC from");
                                Environment.Exit(0);
                                break;
                            default:
                                throw(new System.Exception($"Unknown parameter supplied {arg}"));
                        } 
                        
                    }
                    else 
                    {
                        //Value
                        switch (currentArg) 
                        {
                            case parameters.output:
                                output = arg;
                                break;
                            case parameters.path:
                                path = arg;
                                break;
                            case parameters.undefined:
                            default:
                                throw(new System.Exception($"Unknown bare word parameter supplied {arg} are you missing a switch?"));
                        }
                        currentArg = parameters.undefined;
                    }
                }
            }
            var deserializer = new DeserializerBuilder()
                .IgnoreUnmatchedProperties()
                .Build();
            List<Document> documents = new List<Document>();
            var mdfiles = Directory.GetFiles(path,"*.md");
            foreach (string doc in mdfiles)
            {
                if (doc != path+output) { //do not include the output file
                    var content = File.ReadAllLines(doc);
                    string yaml = "";
                    if (content[0].Replace(" ","")  == "---") { //yaml content in doc
                        for(int i = 1; i < content.Length && content[i].Replace(" ","") != "---"; i++)
                        {
                            yaml += content[i] + System.Environment.NewLine; 
                        }
                        Document currentdoc = deserializer.Deserialize<Document>(yaml);
                        currentdoc.docpath = doc.Replace(" ","%20").Replace(".\\","");
                        documents.Add(currentdoc);
                        yaml = "";
                    }
                }
            }

            if (documents.Count > 0) {
                string toc = "";
                toc +="|Title|Desription|"
                    + System.Environment.NewLine
                    + "|---|---|"
                    + System.Environment.NewLine;
                foreach (Document d in documents)
                {
                    toc += $"|[{d.title}]({d.docpath})|{d.shortdescription}|"
                            + System.Environment.NewLine;
                }
                if (append)
                    File.AppendAllText(Path.Combine(path,output),toc);
                else
                    File.WriteAllText(Path.Combine(path,output),toc);
            }
        }   
            

    }
}

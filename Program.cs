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
            undefined
        }
        static void Main(string[] args)
        {
            bool append = false;
            string output = "readme.md";
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
                            case "-o":
                            case "--output":
                                currentArg = parameters.output;
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
            //TODO: #1 Implement command line parameters to specify directories to process
            var mdfiles = Directory.GetFiles(".","*.md");
            foreach (string doc in mdfiles)
            {
                if (doc != @".\"+output) { //do not include the output file
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
                //TODO: #2 Implement command line parameter for alternate output file
                if (append)
                    File.AppendAllText(output,toc);
                else
                    File.WriteAllText(output,toc);
            }
        }   
            

    }
}
